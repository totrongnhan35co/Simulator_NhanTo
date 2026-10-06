using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSC
{
    public partial class Form1 : Form
    {
        #region Camera Models & Fields
        private TcpListener _camTcpListener;
        private CancellationTokenSource _camTcpCts;
        private readonly List<ConnectedClient> _camConnectedClients = new List<ConnectedClient>();
        private readonly object _camClientsLock = new object();

        private SimpleFtpServer _ftpServer;

        private long _camTotalSent = 0;
        private long _camTotalTriggers = 0;
        private long _camTotalErrors = 0;
        private int _camActiveProgramNo = 0;

        private int _camConsecutiveErrorsRemaining = 0;
        private readonly Random _random = new Random();

        private string _ftpRootDirectory;
        private readonly List<ProgramModel> _programList = new List<ProgramModel>
        {
            new ProgramModel { Number = 0, Name = "0000 Default", Description = "Default Program" },
            new ProgramModel { Number = 1, Name = "0001 TH_Milk_180ml", Description = "TH True Milk 180ml" },
            new ProgramModel { Number = 2, Name = "0002 TH_Milk_110ml", Description = "TH True Milk 110ml" },
            new ProgramModel { Number = 3, Name = "0003 TH_Milk_Topkid", Description = "TH Topkid 110ml" },
            new ProgramModel { Number = 4, Name = "0004 TH_Milk_Yogurt", Description = "TH Yogurt 100g" },
            new ProgramModel { Number = 5, Name = "0005 TH_Sterilized_220ml", Description = "TH Milk Bag 220ml" },
            new ProgramModel { Number = 10, Name = "0010 TH_TrueWater_500ml", Description = "TH True Water 500ml" }
        };
        #endregion

        #region Printer Models & Fields
        public class PrintBufferItem
        {
            public int Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string Command { get; set; }
            public string RawData { get; set; }
            public string Payload { get; set; }
            public string Status { get; set; } // "Waiting", "Printed", "Rejected"
        }

        private TcpListener _prnTcpListener;
        private CancellationTokenSource _prnTcpCts;
        private readonly List<ConnectedClient> _prnConnectedClients = new List<ConnectedClient>();
        private readonly object _prnClientsLock = new object();

        private readonly List<PrintBufferItem> _prnBufferList = new List<PrintBufferItem>();
        private readonly ConcurrentQueue<PrintBufferItem> _prnQueue = new ConcurrentQueue<PrintBufferItem>();
        private readonly object _prnBufferLock = new object();

        private int _prnTotalReceived = 0;
        private int _prnTotalPrinted = 0;
        private string _prnState = "Ready";
        private int _prnSpeedPpm = 300;
        private string _prnTemplate = "TH_MILK_180ML_STD";
        private bool _prnAutoAck = true;
        private bool _prnAutoPrint = true;
        private bool _prnInterlockCamera = true;
        private string _prnStarResponse = "STAR;OK";

        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        #endregion

        #region Interlock Fields
        private long _linePrintedCount = 0;
        private long _lineVerifiedCount = 0;
        private long _lineNgCount = 0;
        public bool IsFullLineRunning { get { return _isFullLineRunning; } }
        private bool _isFullLineRunning = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeFtpDirectories();
            LoadProgramList();
            UpdateDateFields();
            GenerateBarcodeString();
            UpdatePreviewFrame();

            // Init Timer for continuous trigger
            timerContinuousTrigger.Interval = (int)nudInterval.Value;
            timerContinuousTrigger.Tick += (s, ev) => SendCameraTriggerResult();

            // Init Timer for Printer Auto Print Loop
            UpdatePrinterTimerInterval();
            timerPrinterPrint.Tick += (s, ev) => ProcessPrinterAutoPrintCycle();

            // UI Refresh Timer for stats and progress
            timerUiRefresh.Interval = 500;
            timerUiRefresh.Tick += (s, ev) => UpdateUiCounters();
            timerUiRefresh.Start();

            AppendCameraLog("SYS", "Camera Keyence VS-C Simulator sẵn sàng (Port 8500, FTP Port 21).", Color.Cyan);
            AppendPrinterLog("SYS", "Máy In POD / TIJ Simulator sẵn sàng (Port 1997/2030, Framing [STX]...[ETX]).", Color.Cyan);
            AppendInterlockLog("Hệ thống liên hợp Camera & Máy In đã khởi tạo thành công.", Color.LightSkyBlue);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamServer();
            StopPrnServer();
            _ftpServer?.Stop();
        }

        #region Camera Logic & Events
        private void btnCamServerToggle_Click(object sender, EventArgs e)
        {
            if (_camTcpListener == null)
            {
                StartCamServer();
            }
            else
            {
                StopCamServer();
            }
        }

        private void StartCamServer()
        {
            try
            {
                int port = (int)nudCamPort.Value;
                IPAddress ip = IPAddress.Any;
                if (!string.IsNullOrWhiteSpace(txtCamServerIp.Text) && txtCamServerIp.Text != "0.0.0.0")
                {
                    IPAddress.TryParse(txtCamServerIp.Text, out ip);
                }

                _camTcpCts = new CancellationTokenSource();
                _camTcpListener = new TcpListener(ip, port);
                _camTcpListener.Start();

                // Start FTP server
                int ftpPort = (int)nudCamFtpPort.Value;
                _ftpServer = new SimpleFtpServer(ip, ftpPort, _ftpRootDirectory, (src, msg, col) => AppendCameraLog(src, msg, col));
                _ftpServer.Start();

                Task.Run(() => CamListenForClientsAsync(_camTcpCts.Token));

                btnCamServerToggle.Text = "DỪNG CAMERA SERVER";
                btnCamServerToggle.BackColor = Color.FromArgb(191, 97, 106);
                pnlCamStatus.BackColor = Color.LimeGreen;
                lblCamStatus.Text = $"Server: Đang chạy ({port})";
                lblCamStatus.ForeColor = Color.FromArgb(163, 190, 140);

                AppendCameraLog("SYS", $"Camera TCP Server khởi chạy tại port {port}. FTP Server port {ftpPort}.", Color.LimeGreen);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi động Camera Server: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendCameraLog("ERR", "Lỗi start Camera Server: " + ex.Message, Color.Red);
            }
        }

        private void StopCamServer()
        {
            try
            {
                _camTcpCts?.Cancel();
                _camTcpListener?.Stop();
                _camTcpListener = null;

                _ftpServer?.Stop();
                _ftpServer = null;

                lock (_camClientsLock)
                {
                    foreach (var client in _camConnectedClients)
                    {
                        client.Dispose();
                    }
                    _camConnectedClients.Clear();
                }

                btnCamServerToggle.Text = "KHỞI ĐỘNG CAMERA SERVER";
                btnCamServerToggle.BackColor = Color.FromArgb(46, 139, 87);
                pnlCamStatus.BackColor = Color.Red;
                lblCamStatus.Text = "Server: Đã dừng";
                lblCamStatus.ForeColor = Color.FromArgb(191, 97, 106);
                lblCamClient.Text = "Client: 0 kết nối";

                timerContinuousTrigger.Stop();
                rbContinuousTrigger.Checked = false;

                AppendCameraLog("SYS", "Camera TCP Server & FTP Server đã dừng.", Color.OrangeRed);
            }
            catch (Exception ex)
            {
                AppendCameraLog("ERR", "Lỗi stop Camera Server: " + ex.Message, Color.Red);
            }
        }

        private async Task CamListenForClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _camTcpListener != null)
            {
                try
                {
                    TcpClient client = await _camTcpListener.AcceptTcpClientAsync();
                    var connectedClient = new ConnectedClient(client);

                    lock (_camClientsLock)
                    {
                        _camConnectedClients.Add(connectedClient);
                    }

                    UpdateCamClientCount();
                    string ep = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
                    AppendCameraLog("CONNECT", $"R-Link kết nối đến Camera từ {ep}", Color.FromArgb(163, 190, 140));

                    _ = Task.Run(() => HandleCamClientAsync(connectedClient, token));
                }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        AppendCameraLog("ERR", "Lỗi Accept Camera Client: " + ex.Message, Color.Red);
                    }
                }
            }
        }

        private async Task HandleCamClientAsync(ConnectedClient client, CancellationToken token)
        {
            byte[] buffer = new byte[4096];
            StringBuilder sb = new StringBuilder();

            try
            {
                while (!token.IsCancellationRequested && client.IsConnected)
                {
                    int bytesRead = await client.Stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0) break;

                    string text = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    sb.Append(text);

                    string content = sb.ToString();
                    int crlfIndex;
                    while ((crlfIndex = content.IndexOfAny(new char[] { '\r', '\n' })) >= 0)
                    {
                        string line = content.Substring(0, crlfIndex).Trim();
                        content = content.Substring(crlfIndex + 1);

                        if (!string.IsNullOrEmpty(line))
                        {
                            AppendCameraLog("RECV", line, Color.LightGray);
                            ProcessCamCommand(client, line);
                        }
                    }
                    sb.Clear();
                    sb.Append(content);
                }
            }
            catch { }
            finally
            {
                lock (_camClientsLock)
                {
                    _camConnectedClients.Remove(client);
                }
                client.Dispose();
                UpdateCamClientCount();
                AppendCameraLog("DISCONNECT", "R-Link đã ngắt kết nối Camera.", Color.Orange);
            }
        }

        private void ProcessCamCommand(ConnectedClient client, string cmd)
        {
            string upper = cmd.Trim().ToUpperInvariant();
            Interlocked.Increment(ref _camTotalTriggers);

            if (upper == "T1" || upper == "T2" || upper == "TRG" || upper == "T2/C" || upper.StartsWith("SW") || upper.StartsWith("PW"))
            {
                SendCameraTriggerResult();
            }
            else if (upper == "VR" || upper.StartsWith("VR,"))
            {
                SendCamResponse(client, "VR,VS-C,V1.20,KEYENCE");
            }
            else if (upper == "SR" || upper.StartsWith("SR,"))
            {
                SendCamResponse(client, $"SR,{_camActiveProgramNo:D4},OK");
            }
            else if (upper.StartsWith("PW,") || upper.StartsWith("PR,"))
            {
                string[] parts = upper.Split(',');
                if (parts.Length > 1 && int.TryParse(parts[1], out int progNo))
                {
                    _camActiveProgramNo = progNo;
                    SendCamResponse(client, $"PW,{_camActiveProgramNo:D4},OK");
                }
                else
                {
                    SendCamResponse(client, "PW,OK");
                }
            }
            else if (upper == "RD" || upper.StartsWith("RD,"))
            {
                SendCamResponse(client, "RD,0000,OK");
            }
            else
            {
                SendCamResponse(client, $"{cmd},OK");
            }
        }

        private void SendCamResponse(ConnectedClient client, string response)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(response + "\r\n");
                client.Stream.Write(bytes, 0, bytes.Length);
                client.Stream.Flush();
                AppendCameraLog("SEND", response, Color.FromArgb(136, 192, 208));
            }
            catch { }
        }

        public void SendCameraTriggerResult()
        {
            string payload = GenerateCurrentPayload();
            bool isError = IsErrorCondition(payload);

            string packet = FormatKeyencePacket(payload);
            byte[] data = Encoding.ASCII.GetBytes(packet);

            lock (_camClientsLock)
            {
                foreach (var client in _camConnectedClients.ToList())
                {
                    try
                    {
                        if (client.IsConnected)
                        {
                            client.Stream.Write(data, 0, data.Length);
                            client.Stream.Flush();
                        }
                    }
                    catch { }
                }
            }

            Interlocked.Increment(ref _camTotalSent);
            if (isError)
            {
                Interlocked.Increment(ref _camTotalErrors);
                Interlocked.Increment(ref _lineNgCount);
            }
            else
            {
                Interlocked.Increment(ref _lineVerifiedCount);
            }

            BeginInvoke(new Action(() =>
            {
                AppendCameraLog("SEND", packet.Replace("\x02", "<STX>").Replace("\x03", "<ETX>").Replace("\r", "\\r").Replace("\n", "\\n"), isError ? Color.Salmon : Color.LightGreen);
                UpdatePreviewStatus(isError, payload);
            }));
        }

        private string GenerateCurrentPayload()
        {
            if (chkInjectCustomString.Checked && !string.IsNullOrWhiteSpace(txtCustomErrorString.Text))
            {
                return txtCustomErrorString.Text.Trim();
            }

            if (chkInjectNoRead.Checked || _camConsecutiveErrorsRemaining > 0)
            {
                if (_camConsecutiveErrorsRemaining > 0) _camConsecutiveErrorsRemaining--;
                return "ERROR";
            }

            string code = txtBarcodeData.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                GenerateBarcodeString();
                code = txtBarcodeData.Text.Trim();
            }

            if (chkInjectBadFormat.Checked)
            {
                return code + "_BAD_CHECKSUM";
            }
            if (chkInjectWrongDate.Checked)
            {
                return code.Replace(dtpMfgDate.Value.ToString("ddMMyy"), "010199");
            }
            if (chkInjectExpiredDate.Checked)
            {
                return code.Replace(dtpExpDate.Value.ToString("ddMMyy"), "010120");
            }
            if (chkInjectPartialRead.Checked)
            {
                return code.Length > 8 ? code.Substring(0, code.Length - 5) : "893500";
            }

            return code;
        }

        private bool IsErrorCondition(string payload)
        {
            return payload.Contains("ERROR") || payload.Contains("BAD") || payload.Contains("9999") || chkInjectNoRead.Checked || chkInjectBadFormat.Checked || chkInjectExpiredDate.Checked || chkInjectWrongDate.Checked;
        }

        private string FormatKeyencePacket(string payload)
        {
            return $"{((char)STX)}{payload}\r\n{((char)ETX)}";
        }

        private void btnSendSingleTrigger_Click(object sender, EventArgs e)
        {
            SendCameraTriggerResult();
        }

        private void rbContinuousTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (rbContinuousTrigger.Checked)
            {
                timerContinuousTrigger.Interval = (int)nudInterval.Value;
                timerContinuousTrigger.Start();
                AppendCameraLog("SYS", $"Bắt đầu Continuous Trigger ({nudInterval.Value} ms).", Color.Yellow);
            }
            else
            {
                timerContinuousTrigger.Stop();
            }
        }

        private void btnInjectOnce_Click(object sender, EventArgs e)
        {
            chkInjectNoRead.Checked = true;
            SendCameraTriggerResult();
            chkInjectNoRead.Checked = false;
        }

        private void btnRunScenario_Click(object sender, EventArgs e)
        {
            int scenarioIndex = cboScenario.SelectedIndex;
            Task.Run(async () =>
            {
                BeginInvoke(new Action(() => { prgScenario.Value = 0; prgScenario.Maximum = 100; lblScenarioStatus.Text = "Đang chạy kịch bản..."; }));

                int count = 100;
                for (int i = 1; i <= count; i++)
                {
                    if (scenarioIndex == 1 && i % 10 == 0)
                    {
                        chkInjectBadFormat.Checked = true;
                        SendCameraTriggerResult();
                        chkInjectBadFormat.Checked = false;
                    }
                    else if (scenarioIndex == 2 && i >= 10 && i <= 14)
                    {
                        chkInjectNoRead.Checked = true;
                        SendCameraTriggerResult();
                        chkInjectNoRead.Checked = false;
                    }
                    else
                    {
                        SendCameraTriggerResult();
                    }

                    int pct = (int)((i / (double)count) * 100);
                    BeginInvoke(new Action(() => { prgScenario.Value = pct; }));
                    await Task.Delay(scenarioIndex == 3 ? 20 : 100);
                }

                BeginInvoke(new Action(() => { lblScenarioStatus.Text = "Kịch bản hoàn tất 100%!"; }));
            });
        }

        private void UpdateCamClientCount()
        {
            int count = 0;
            lock (_camClientsLock)
            {
                count = _camConnectedClients.Count;
            }
            BeginInvoke(new Action(() =>
            {
                lblCamClient.Text = $"Client R-Link: {count} kết nối";
            }));
        }

        private void GenerateBarcodeString()
        {
            string productPrefix = "8935001234567";
            switch (cboProductType.SelectedIndex)
            {
                case 0: productPrefix = "8935001234567"; break;
                case 1: productPrefix = "8935001234581"; break;
                case 2: productPrefix = "8935001234604"; break;
                case 3: productPrefix = "8935001234628"; break;
                case 4: productPrefix = "8935001234659"; break;
                case 5: productPrefix = "8935001234888"; break;
            }

            string mfg = dtpMfgDate.Value.ToString("ddMMyy");
            string exp = dtpExpDate.Value.ToString("ddMMyy");
            string time = txtShiftTime.Text.Replace(":", "").Trim();
            string line = txtLineInfo.Text.Replace(" ", "").Trim();

            txtBarcodeData.Text = $"{productPrefix} {mfg} {exp} {time} {line}";
            UpdatePreviewFrame();
        }

        private void cboProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateBarcodeString();
        }

        private void UpdateDateFields()
        {
            dtpExpDate.Value = dtpMfgDate.Value.AddMonths(6);
            GenerateBarcodeString();
        }

        private void UpdatePreviewFrame()
        {
            try
            {
                int w = picPreview.Width > 0 ? picPreview.Width : 400;
                int h = picPreview.Height > 0 ? picPreview.Height : 180;
                Bitmap bmp = new Bitmap(w, h);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(24, 24, 37));
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Draw Camera Viewfinder Bounding Box
                    using (Pen penBox = new Pen(Color.FromArgb(136, 192, 208), 2))
                    {
                        g.DrawRectangle(penBox, 20, 20, w - 40, h - 40);
                    }

                    // Draw DataMatrix / QR simulated grid
                    int qrX = 35;
                    int qrY = 35;
                    int qrSize = h - 70;
                    g.FillRectangle(Brushes.White, qrX, qrY, qrSize, qrSize);
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            if ((r + c) % 2 == 0 || r == 0 || c == 0 || r == 7 || c == 7)
                            {
                                g.FillRectangle(Brushes.Black, qrX + (c * (qrSize / 8)), qrY + (r * (qrSize / 8)), qrSize / 8, qrSize / 8);
                            }
                        }
                    }

                    // Text Info
                    string code = txtBarcodeData.Text;
                    using (Font fontTitle = new Font("Segoe UI", 10F, FontStyle.Bold))
                    using (Font fontCode = new Font("Consolas", 9F, FontStyle.Bold))
                    {
                        g.DrawString("KEYENCE VS-C VISION LIVE OCR", fontTitle, Brushes.LightSkyBlue, qrX + qrSize + 15, 35);
                        g.DrawString($"Data: {code}", fontCode, Brushes.Yellow, qrX + qrSize + 15, 65);
                        g.DrawString($"MFG: {dtpMfgDate.Value:dd/MM/yyyy}  EXP: {dtpExpDate.Value:dd/MM/yyyy}", fontCode, Brushes.Gainsboro, qrX + qrSize + 15, 90);
                        g.DrawString($"Time: {txtShiftTime.Text}  Line: {txtLineInfo.Text}", fontCode, Brushes.Gainsboro, qrX + qrSize + 15, 110);
                    }
                }

                var old = picPreview.Image;
                picPreview.Image = bmp;
                old?.Dispose();
            }
            catch { }
        }

        private void UpdatePreviewStatus(bool isError, string payload)
        {
            if (isError)
            {
                lblPreviewStatus.Text = $"CAMERA TRẠNG THÁI: NG (LỖI ĐỌC MÃ) - [{payload}]";
                lblPreviewStatus.BackColor = Color.FromArgb(191, 97, 106);
            }
            else
            {
                lblPreviewStatus.Text = $"CAMERA TRẠNG THÁI: PASS (OK) - [{payload}]";
                lblPreviewStatus.BackColor = Color.FromArgb(46, 139, 87);
            }
        }

        private void AppendCameraLog(string source, string msg, Color color)
        {
            if (rtbCameraLog.IsDisposed) return;
            if (rtbCameraLog.InvokeRequired)
            {
                rtbCameraLog.BeginInvoke(new Action(() => AppendCameraLog(source, msg, color)));
                return;
            }

            if (rtbCameraLog.Lines.Length > 1000) rtbCameraLog.Clear();

            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string line = $"[{timestamp}] [{source}] {msg}\n";

            rtbCameraLog.SelectionStart = rtbCameraLog.TextLength;
            rtbCameraLog.SelectionLength = 0;
            rtbCameraLog.SelectionColor = color;
            rtbCameraLog.AppendText(line);
            rtbCameraLog.SelectionColor = rtbCameraLog.ForeColor;

            if (chkCamAutoScroll.Checked)
            {
                rtbCameraLog.ScrollToCaret();
            }
        }

        private void SaveCameraLogToFile()
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text file (*.txt)|*.txt|Log file (*.log)|*.log", FileName = $"Camera_Traffic_{DateTime.Now:yyyyMMdd_HHmmss}.log" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, rtbCameraLog.Text, Encoding.UTF8);
                    MessageBox.Show("Đã lưu log Camera thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void InitializeFtpDirectories()
        {
            try
            {
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                _ftpRootDirectory = Path.Combine(appDir, "FTP_Root");
                string programsDir = Path.Combine(_ftpRootDirectory, "VS", "Camera", "Programs");
                string imagesDir = Path.Combine(_ftpRootDirectory, "VS", "Camera", "Images");

                Directory.CreateDirectory(programsDir);
                Directory.CreateDirectory(imagesDir);

                txtFtpRootPath.Text = _ftpRootDirectory;

                foreach (var prog in _programList)
                {
                    string pDir = Path.Combine(programsDir, prog.Name);
                    Directory.CreateDirectory(pDir);
                }
            }
            catch { }
        }

        private void LoadProgramList()
        {
            lstPrograms.Items.Clear();
            foreach (var prog in _programList)
            {
                lstPrograms.Items.Add($"[{prog.Number:D4}] {prog.Name} - {prog.Description}");
            }
            if (lstPrograms.Items.Count > 0) lstPrograms.SelectedIndex = 0;
        }

        private void OpenFtpFolder()
        {
            if (Directory.Exists(_ftpRootDirectory))
            {
                Process.Start("explorer.exe", _ftpRootDirectory);
            }
        }
        #endregion

        #region Printer Logic & Events
        private void btnPrnServerToggle_Click(object sender, EventArgs e)
        {
            if (_prnTcpListener == null)
            {
                StartPrnServer();
            }
            else
            {
                StopPrnServer();
            }
        }

        private void StartPrnServer()
        {
            try
            {
                int port = (int)nudPrnPort.Value;
                IPAddress ip = IPAddress.Any;
                if (!string.IsNullOrWhiteSpace(txtPrnServerIp.Text) && txtPrnServerIp.Text != "0.0.0.0")
                {
                    IPAddress.TryParse(txtPrnServerIp.Text, out ip);
                }

                _prnTcpCts = new CancellationTokenSource();
                _prnTcpListener = new TcpListener(ip, port);
                _prnTcpListener.Start();

                Task.Run(() => PrnListenForClientsAsync(_prnTcpCts.Token));

                btnPrnServerToggle.Text = "DỪNG SERVER MÁY IN";
                btnPrnServerToggle.BackColor = Color.FromArgb(191, 97, 106);
                pnlPrnStatus.BackColor = Color.LimeGreen;
                lblPrnStatus.Text = $"Server: Đang chạy ({port})";
                lblPrnStatus.ForeColor = Color.FromArgb(163, 190, 140);

                AppendPrinterLog("SYS", $"Server Máy In khởi chạy thành công tại port {port}.", Color.LimeGreen);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi động Server Máy In: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendPrinterLog("ERR", "Lỗi start Printer Server: " + ex.Message, Color.Red);
            }
        }

        private void StopPrnServer()
        {
            try
            {
                _prnTcpCts?.Cancel();
                _prnTcpListener?.Stop();
                _prnTcpListener = null;

                lock (_prnClientsLock)
                {
                    foreach (var client in _prnConnectedClients)
                    {
                        client.Dispose();
                    }
                    _prnConnectedClients.Clear();
                }

                btnPrnServerToggle.Text = "KHỞI ĐỘNG SERVER MÁY IN";
                btnPrnServerToggle.BackColor = Color.FromArgb(46, 139, 87);
                pnlPrnStatus.BackColor = Color.Red;
                lblPrnStatus.Text = "Server: Đã dừng";
                lblPrnStatus.ForeColor = Color.FromArgb(191, 97, 106);
                lblPrnClient.Text = "Client R-Link: 0 kết nối";

                timerPrinterPrint.Stop();

                AppendPrinterLog("SYS", "Server Máy In đã dừng.", Color.OrangeRed);
            }
            catch (Exception ex)
            {
                AppendPrinterLog("ERR", "Lỗi stop Printer Server: " + ex.Message, Color.Red);
            }
        }

        private async Task PrnListenForClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _prnTcpListener != null)
            {
                try
                {
                    TcpClient client = await _prnTcpListener.AcceptTcpClientAsync();
                    var connectedClient = new ConnectedClient(client);

                    lock (_prnClientsLock)
                    {
                        _prnConnectedClients.Add(connectedClient);
                    }

                    UpdatePrnClientCount();
                    string ep = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
                    AppendPrinterLog("CONNECT", $"R-Link kết nối đến Máy In từ {ep}", Color.FromArgb(163, 190, 140));

                    _ = Task.Run(() => HandlePrnClientAsync(connectedClient, token));
                }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        AppendPrinterLog("ERR", "Lỗi Accept Printer Client: " + ex.Message, Color.Red);
                    }
                }
            }
        }

        private async Task HandlePrnClientAsync(ConnectedClient client, CancellationToken token)
        {
            byte[] buffer = new byte[4096];
            StringBuilder sb = new StringBuilder();

            try
            {
                while (!token.IsCancellationRequested && client.IsConnected)
                {
                    int bytesRead = await client.Stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0) break;

                    for (int i = 0; i < bytesRead; i++)
                    {
                        byte b = buffer[i];
                        if (b == STX)
                        {
                            sb.Clear();
                        }
                        else if (b == ETX || b == (byte)'\r' || b == (byte)'\n')
                        {
                            if (sb.Length > 0)
                            {
                                string msg = sb.ToString();
                                sb.Clear();
                                ProcessPrnCommand(client, msg);
                            }
                        }
                        else
                        {
                            sb.Append((char)b);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                lock (_prnClientsLock)
                {
                    _prnConnectedClients.Remove(client);
                }
                client.Dispose();
                UpdatePrnClientCount();
                AppendPrinterLog("DISCONNECT", "R-Link đã ngắt kết nối Máy In.", Color.Orange);
            }
        }

        private void ProcessPrnCommand(ConnectedClient client, string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;

            AppendPrinterLog("RECV", $"<STX>{msg}<ETX>", Color.LightGray);
            string[] parts = msg.Split(';');
            string cmd = parts[0].Trim().ToUpperInvariant();

            switch (cmd)
            {
                case "DATA":
                case "RDATA":
                    HandlePrnDataCommand(client, cmd, msg, parts);
                    break;

                case "STAR":
                    HandlePrnStarCommand(client, parts);
                    break;

                case "STOP":
                    HandlePrnStopCommand(client);
                    break;

                case "MON":
                    HandlePrnMonCommand(client);
                    break;

                case "SET":
                    SendPrnResponse(client, "SET;RYES");
                    break;

                case "RTIME":
                    SendPrnResponse(client, msg + ";OK");
                    break;

                case "CLPB":
                    ClearPrintBuffer();
                    SendPrnResponse(client, "CLPB;OK");
                    break;

                case "RQLI":
                    SendPrnResponse(client, "RQLI;TH_MILK_180ML_STD;TH_MILK_110ML_STD;TH_TOPKID_110ML;TH_YOGURT_100G");
                    break;

                case "SETP":
                    if (parts.Length > 1)
                    {
                        _prnTemplate = parts[1];
                        BeginInvoke(new Action(() => txtPrnTemplate.Text = _prnTemplate));
                    }
                    SendPrnResponse(client, "SETP;RYES");
                    break;

                default:
                    SendPrnResponse(client, $"{cmd};OK");
                    break;
            }
        }

        private void HandlePrnDataCommand(ConnectedClient client, string cmd, string rawMsg, string[] parts)
        {
            if (_prnAutoAck)
            {
                SendPrnResponse(client, $"{cmd};RYES");
            }

            int id = Interlocked.Increment(ref _prnTotalReceived);
            string payload = parts.Length > 1 ? string.Join(";", parts.Skip(1)) : rawMsg;

            var item = new PrintBufferItem
            {
                Id = id,
                Timestamp = DateTime.Now,
                Command = cmd,
                RawData = rawMsg,
                Payload = payload,
                Status = "Waiting"
            };

            lock (_prnBufferLock)
            {
                _prnBufferList.Add(item);
                _prnQueue.Enqueue(item);
            }

            BeginInvoke(new Action(() =>
            {
                dgvPrintBuffer.Rows.Add(item.Id, item.Timestamp.ToString("HH:mm:ss.fff"), item.Command, item.Payload, item.Status);
                if (dgvPrintBuffer.Rows.Count > 100)
                {
                    dgvPrintBuffer.Rows.RemoveAt(0);
                }
                dgvPrintBuffer.FirstDisplayedScrollingRowIndex = dgvPrintBuffer.RowCount - 1;
            }));
        }

        private void HandlePrnStarCommand(ConnectedClient client, string[] parts)
        {
            string starSetting = _prnStarResponse;
            if (starSetting.StartsWith("STAR;ERR;"))
            {
                SendPrnResponse(client, starSetting);
                SetPrinterState("Error");
            }
            else if (starSetting == "STAR;READY")
            {
                SendPrnResponse(client, "STAR;READY");
                SetPrinterState("Ready");
            }
            else
            {
                SendPrnResponse(client, "STAR;OK");
                SetPrinterState("Printing");
                if (_prnAutoPrint)
                {
                    timerPrinterPrint.Start();
                }
            }
        }

        private void HandlePrnStopCommand(ConnectedClient client)
        {
            SendPrnResponse(client, "STOP;OK");
            SetPrinterState("Stop");
            timerPrinterPrint.Stop();
        }

        private void HandlePrnMonCommand(ConnectedClient client)
        {
            string monPacket = $"MON;{_prnState};{_prnSpeedPpm};{_prnTotalPrinted};0;OK;0;0;0;0;{_prnTemplate}";
            SendPrnResponse(client, monPacket);
        }

        private void SendPrnResponse(ConnectedClient client, string message)
        {
            try
            {
                string packet = $"{((char)STX)}{message}{((char)ETX)}";
                byte[] bytes = Encoding.UTF8.GetBytes(packet);
                client.Stream.Write(bytes, 0, bytes.Length);
                client.Stream.Flush();

                AppendPrinterLog("SEND", $"<STX>{message}<ETX>", Color.FromArgb(136, 192, 208));
            }
            catch { }
        }

        public void SendPrnBroadcast(string message)
        {
            string packet = $"{((char)STX)}{message}{((char)ETX)}";
            byte[] bytes = Encoding.UTF8.GetBytes(packet);

            lock (_prnClientsLock)
            {
                foreach (var client in _prnConnectedClients.ToList())
                {
                    try
                    {
                        if (client.IsConnected)
                        {
                            client.Stream.Write(bytes, 0, bytes.Length);
                            client.Stream.Flush();
                        }
                    }
                    catch { }
                }
            }

            AppendPrinterLog("SEND", $"<STX>{message}<ETX>", Color.LightGreen);
        }

        private void ProcessPrinterAutoPrintCycle()
        {
            if (_prnState != "Printing" && _prnState != "Processing" && _prnState != "Ready") return;

            PrintBufferItem item = null;
            if (_prnQueue.TryDequeue(out item))
            {
                int printed = Interlocked.Increment(ref _prnTotalPrinted);
                Interlocked.Increment(ref _linePrintedCount);

                item.Status = "Printed";

                string rsfp = $"RSFP;{printed}/{_prnTotalReceived};DATA;{item.Payload}";
                SendPrnBroadcast(rsfp);

                BeginInvoke(new Action(() =>
                {
                    foreach (DataGridViewRow row in dgvPrintBuffer.Rows)
                    {
                        if (row.Cells["colId"].Value != null && (int)row.Cells["colId"].Value == item.Id)
                        {
                            row.Cells["colStatus"].Value = "Printed";
                            row.Cells["colStatus"].Style.ForeColor = Color.LightGreen;
                            break;
                        }
                    }
                }));

                if (_prnInterlockCamera && chkPrnInterlockCamera.Checked)
                {
                    FeedToCameraSimulator(item.Payload);
                }
            }
        }

        private void FeedToCameraSimulator(string payload)
        {
            Task.Run(async () =>
            {
                await Task.Delay(150);

                string[] fields = payload.Split(';');
                string barcode = fields[0];
                if (fields.Length > 1 && !string.IsNullOrWhiteSpace(fields[1]))
                {
                    barcode = fields[0] + " " + fields[1];
                }

                BeginInvoke(new Action(() =>
                {
                    txtBarcodeData.Text = barcode;
                    SendCameraTriggerResult();
                }));
            });
        }

        private void btnPrnPrintNext_Click(object sender, EventArgs e)
        {
            ProcessPrinterAutoPrintCycle();
        }

        private void btnPrnClearBuffer_Click(object sender, EventArgs e)
        {
            ClearPrintBuffer();
            SendPrnBroadcast("CLPB;OK");
        }

        private void ClearPrintBuffer()
        {
            lock (_prnBufferLock)
            {
                _prnBufferList.Clear();
                while (_prnQueue.TryDequeue(out _)) { }
            }
            BeginInvoke(new Action(() =>
            {
                dgvPrintBuffer.Rows.Clear();
                lblPrnBufferCount.Text = "Chờ trong Buffer: 0";
            }));
            AppendPrinterLog("SYS", "Đã xóa sạch bộ đệm in (CLPB).", Color.Yellow);
        }

        private void ResetPrinterCounters()
        {
            Interlocked.Exchange(ref _prnTotalReceived, 0);
            Interlocked.Exchange(ref _prnTotalPrinted, 0);
            ClearPrintBuffer();
            UpdateUiCounters();
            AppendPrinterLog("SYS", "Đã reset bộ đếm máy in.", Color.Yellow);
        }

        private void btnSendRsal_Click(object sender, EventArgs e)
        {
            string selected = cboRsalCode.SelectedItem?.ToString() ?? "007";
            string code = selected.Substring(0, 3);
            string rsalPacket = $"RSAL;{code};1";

            SendPrnBroadcast(rsalPacket);
            AppendPrinterLog("ALARM", $"Tiêm cảnh báo: {rsalPacket} ({selected})", Color.Salmon);
        }

        private void btnSendPlc001_Click(object sender, EventArgs e)
        {
            SendPrnBroadcast("PLC001");
            AppendPrinterLog("PULSE", "Gửi tín hiệu xung index PLC001 sang R-Link", Color.LightSkyBlue);
        }

        private void btnPrnSimulateDrop_Click(object sender, EventArgs e)
        {
            lock (_prnClientsLock)
            {
                foreach (var client in _prnConnectedClients)
                {
                    client.Dispose();
                }
                _prnConnectedClients.Clear();
            }
            UpdatePrnClientCount();
            AppendPrinterLog("DROP", "Mô phỏng ngắt kết nối đột ngột với R-Link!", Color.Red);
        }

        private void btnPrnTestSyncTime_Click(object sender, EventArgs e)
        {
            SendPrnBroadcast("SET;RYES");
        }

        private void btnPrnTestRollTime_Click(object sender, EventArgs e)
        {
            SendPrnBroadcast("RTIME;1;2355;10;OK");
        }

        private void cboPrnState_SelectedIndexChanged(object sender, EventArgs e)
        {
            _prnState = cboPrnState.SelectedItem?.ToString() ?? "Ready";
        }

        private void SetPrinterState(string state)
        {
            _prnState = state;
            BeginInvoke(new Action(() =>
            {
                cboPrnState.SelectedItem = state;
            }));
        }

        private void UpdatePrinterTimerInterval()
        {
            _prnSpeedPpm = (int)nudPrnSpeed.Value;
            if (_prnSpeedPpm <= 0) _prnSpeedPpm = 300;
            int intervalMs = Math.Max(10, 60000 / _prnSpeedPpm);
            timerPrinterPrint.Interval = intervalMs;
        }

        private void chkPrnAutoPrint_CheckedChanged(object sender, EventArgs e)
        {
            _prnAutoPrint = chkPrnAutoPrint.Checked;
            if (_prnAutoPrint && (_prnState == "Printing" || _prnState == "Processing"))
            {
                timerPrinterPrint.Start();
            }
            else
            {
                timerPrinterPrint.Stop();
            }
        }

        private void UpdatePrnClientCount()
        {
            int count = 0;
            lock (_prnClientsLock)
            {
                count = _prnConnectedClients.Count;
            }
            BeginInvoke(new Action(() =>
            {
                lblPrnClient.Text = $"Client R-Link: {count} kết nối";
            }));
        }

        private void AppendPrinterLog(string source, string msg, Color color)
        {
            if (rtbPrinterLog.IsDisposed) return;
            if (rtbPrinterLog.InvokeRequired)
            {
                rtbPrinterLog.BeginInvoke(new Action(() => AppendPrinterLog(source, msg, color)));
                return;
            }

            if (rtbPrinterLog.Lines.Length > 1000) rtbPrinterLog.Clear();

            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string line = $"[{timestamp}] [{source}] {msg}\n";

            rtbPrinterLog.SelectionStart = rtbPrinterLog.TextLength;
            rtbPrinterLog.SelectionLength = 0;
            rtbPrinterLog.SelectionColor = color;
            rtbPrinterLog.AppendText(line);
            rtbPrinterLog.SelectionColor = rtbPrinterLog.ForeColor;

            if (chkPrnAutoScroll.Checked)
            {
                rtbPrinterLog.ScrollToCaret();
            }
        }

        private void SavePrinterLogToFile()
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Text file (*.txt)|*.txt|Log file (*.log)|*.log", FileName = $"Printer_Traffic_{DateTime.Now:yyyyMMdd_HHmmss}.log" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, rtbPrinterLog.Text, Encoding.UTF8);
                    MessageBox.Show("Đã lưu log Máy In thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region Interlock Simulation Logic
        private void StartFullLineSimulation()
        {
            if (_camTcpListener == null) StartCamServer();
            if (_prnTcpListener == null) StartPrnServer();

            _isFullLineRunning = true;
            chkPrnAutoPrint.Checked = true;
            chkPrnInterlockCamera.Checked = true;
            SetPrinterState("Printing");

            lblLineSimStatus.Text = "⚡ DÂY CHUYỀN ĐANG HOẠT ĐỘNG: In + So khớp Realtime!";
            lblLineSimStatus.ForeColor = Color.LimeGreen;
            AppendInterlockLog("Đã kích hoạt toàn bộ dây chuyền mô phỏng liên hợp.", Color.LimeGreen);
        }

        private void StopAllServers()
        {
            StopCamServer();
            StopPrnServer();
            _isFullLineRunning = false;

            lblLineSimStatus.Text = "Trạng thái liên hợp: Đã dừng tất cả server.";
            lblLineSimStatus.ForeColor = Color.Salmon;
            AppendInterlockLog("Đã dừng tất cả các server mô phỏng.", Color.Salmon);
        }

        private void AppendInterlockLog(string msg, Color color)
        {
            if (rtbInterlockLog.IsDisposed) return;
            if (rtbInterlockLog.InvokeRequired)
            {
                rtbInterlockLog.BeginInvoke(new Action(() => AppendInterlockLog(msg, color)));
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string line = $"[{timestamp}] {msg}\n";

            rtbInterlockLog.SelectionStart = rtbInterlockLog.TextLength;
            rtbInterlockLog.SelectionLength = 0;
            rtbInterlockLog.SelectionColor = color;
            rtbInterlockLog.AppendText(line);
            rtbInterlockLog.SelectionColor = rtbInterlockLog.ForeColor;
            rtbInterlockLog.ScrollToCaret();
        }

        private void UpdateUiCounters()
        {
            lblCamSentCount.Text = $"Gửi: {_camTotalSent} gói";
            lblCamTriggerCount.Text = $"Trigger: {_camTotalTriggers}";
            lblCamErrorCount.Text = $"Lỗi NG: {_camTotalErrors}";

            int pending = _prnQueue.Count;
            lblPrnTotalRecv.Text = $"Tổng nhận: {_prnTotalReceived}";
            lblPrnTotalPrinted.Text = $"Đã in: {_prnTotalPrinted}";
            lblPrnBufferCount.Text = $"Chờ trong Buffer: {pending}";

            int pct = _prnTotalReceived > 0 ? (int)((_prnTotalPrinted / (double)_prnTotalReceived) * 100) : 0;
            prgPrnBuffer.Value = Math.Min(100, Math.Max(0, pct));

            lblLinePrintedCount.Text = $"Máy In Đã Xả: {_linePrintedCount} sản phẩm";
            lblLineVerifiedCount.Text = $"Camera Đã Đọc: {_lineVerifiedCount} sản phẩm";
            lblLineNgCount.Text = $"Lỗi NG In/Đọc: {_lineNgCount}";

            double matchRate = _lineVerifiedCount > 0 ? ((_lineVerifiedCount - _lineNgCount) / (double)_lineVerifiedCount) * 100.0 : 100.0;
            lblLineMatchRate.Text = $"Tỷ Lệ Khớp: {matchRate:F1}%";
        }
        #endregion
    }

    #region Helper Classes
    public class ConnectedClient : IDisposable
    {
        public TcpClient Client { get; private set; }
        public NetworkStream Stream { get; private set; }
        public string RemoteEndPoint { get; private set; }

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (Client?.Client == null) return false;
                    return Client.Client.Connected && !(Client.Client.Poll(0, SelectMode.SelectRead) && Client.Client.Available == 0);
                }
                catch { return false; }
            }
        }

        public ConnectedClient(TcpClient client)
        {
            Client = client;
            Client.NoDelay = true;
            Stream = client.GetStream();
            RemoteEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
        }

        public void Dispose()
        {
            try { Stream?.Close(); } catch { }
            try { Client?.Close(); } catch { }
        }
    }

    public class ProgramModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SimpleFtpServer
    {
        private readonly IPAddress _ip;
        private readonly int _port;
        private readonly string _rootDir;
        private readonly Action<string, string, Color> _logger;
        private TcpListener _listener;
        private CancellationTokenSource _cts;

        public SimpleFtpServer(IPAddress ip, int port, string rootDir, Action<string, string, Color> logger)
        {
            _ip = ip;
            _port = port;
            _rootDir = rootDir;
            _logger = logger;
        }

        public void Start()
        {
            try
            {
                _cts = new CancellationTokenSource();
                _listener = new TcpListener(_ip, _port);
                _listener.Start();
                _logger?.Invoke("FTP", $"FTP Server đang lắng nghe tại port {_port}, Root: {_rootDir}", Color.LimeGreen);

                Task.Run(() => ListenAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                _logger?.Invoke("FTP_ERR", "Lỗi start FTP Server: " + ex.Message, Color.Red);
            }
        }

        public void Stop()
        {
            try
            {
                _cts?.Cancel();
                _listener?.Stop();
                _listener = null;
            }
            catch { }
        }

        private async Task ListenAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _listener != null)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleFtpClientAsync(client, token));
                }
                catch { break; }
            }
        }

        private async Task HandleFtpClientAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            using (var writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true })
            {
                try
                {
                    await writer.WriteLineAsync("220 Keyence VS-C Simulated FTP Service Ready.");
                    string currentDir = _rootDir;
                    TcpListener pasvListener = null;

                    while (!token.IsCancellationRequested && client.Connected)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line == null) break;

                        string[] parts = line.Split(' ');
                        string cmd = parts[0].ToUpperInvariant();
                        string arg = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";

                        switch (cmd)
                        {
                            case "USER":
                                await writer.WriteLineAsync("331 Anonymous access allowed.");
                                break;
                            case "PASS":
                                await writer.WriteLineAsync("230 User logged in successfully.");
                                break;
                            case "PWD":
                                string rel = currentDir.Replace(_rootDir, "").Replace("\\", "/");
                                if (string.IsNullOrEmpty(rel)) rel = "/";
                                await writer.WriteLineAsync($"257 \"{rel}\" is current directory.");
                                break;
                            case "CWD":
                                string target = Path.Combine(currentDir, arg.TrimStart('/'));
                                if (Directory.Exists(target))
                                {
                                    currentDir = target;
                                    await writer.WriteLineAsync("250 Directory successfully changed.");
                                }
                                else
                                {
                                    await writer.WriteLineAsync("250 Directory changed.");
                                }
                                break;
                            case "TYPE":
                                await writer.WriteLineAsync("200 Type set to I.");
                                break;
                            case "PASV":
                                int pPort = new Random().Next(20000, 30000);
                                pasvListener = new TcpListener(IPAddress.Any, pPort);
                                pasvListener.Start();
                                byte[] ipBytes = IPAddress.Loopback.GetAddressBytes();
                                await writer.WriteLineAsync($"227 Entering Passive Mode ({ipBytes[0]},{ipBytes[1]},{ipBytes[2]},{ipBytes[3]},{pPort / 256},{pPort % 256}).");
                                break;
                            case "LIST":
                            case "NLST":
                                await writer.WriteLineAsync("150 Here comes the directory listing.");
                                if (pasvListener != null)
                                {
                                    using (var dataClient = await pasvListener.AcceptTcpClientAsync())
                                    using (var dataStream = dataClient.GetStream())
                                    using (var dataWriter = new StreamWriter(dataStream, Encoding.ASCII) { AutoFlush = true })
                                    {
                                        if (Directory.Exists(currentDir))
                                        {
                                            foreach (var d in Directory.GetDirectories(currentDir))
                                            {
                                                var di = new DirectoryInfo(d);
                                                await dataWriter.WriteLineAsync($"drwxr-xr-x 1 owner group          0 Jan 01 00:00 {di.Name}");
                                            }
                                            foreach (var f in Directory.GetFiles(currentDir))
                                            {
                                                var fi = new FileInfo(f);
                                                await dataWriter.WriteLineAsync($"-rw-r--r-- 1 owner group {fi.Length,10} Jan 01 00:00 {fi.Name}");
                                            }
                                        }
                                    }
                                    pasvListener.Stop();
                                    pasvListener = null;
                                }
                                await writer.WriteLineAsync("226 Directory send OK.");
                                break;
                            case "QUIT":
                                await writer.WriteLineAsync("221 Goodbye.");
                                return;
                            default:
                                await writer.WriteLineAsync("200 Command OK.");
                                break;
                        }
                    }
                }
                catch { }
            }
        }
    }
    #endregion
}
