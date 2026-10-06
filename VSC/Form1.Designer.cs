using System;
using System.Drawing;
using System.Windows.Forms;

namespace VSC
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Main Tab Control
        private TabControl tabMain;
        private TabPage tabCamera;
        private TabPage tabPrinter;
        private TabPage tabInterlock;

        #region Camera Tab Controls
        private Panel pnlCamLeft;
        private Panel pnlCamRight;
        private Panel pnlCamRightTop;
        private GroupBox grpCameraServer;
        private Label lblCamIp;
        private TextBox txtCamServerIp;
        private Label lblCamPort;
        private NumericUpDown nudCamPort;
        private Label lblCamFtpPort;
        private NumericUpDown nudCamFtpPort;
        private Button btnCamServerToggle;
        private Panel pnlCamStatus;
        private Label lblCamStatus;
        private Label lblCamClient;

        private GroupBox grpTrigger;
        private RadioButton rbManualTrigger;
        private RadioButton rbContinuousTrigger;
        private RadioButton rbOnDemandTrigger;
        private Button btnSendSingleTrigger;
        private Label lblInterval;
        private TrackBar trackInterval;
        private NumericUpDown nudInterval;

        private GroupBox grpProductData;
        private Label lblBarcodeData;
        private TextBox txtBarcodeData;
        private Label lblProductType;
        private ComboBox cboProductType;
        private Label lblMfgDate;
        private DateTimePicker dtpMfgDate;
        private Label lblExpDate;
        private DateTimePicker dtpExpDate;
        private Label lblShiftTime;
        private TextBox txtShiftTime;
        private Label lblLineInfo;
        private TextBox txtLineInfo;
        private Button btnGenerateBarcode;

        private GroupBox grpErrorInjection;
        private CheckBox chkInjectBadFormat;
        private CheckBox chkInjectWrongDate;
        private CheckBox chkInjectExpiredDate;
        private CheckBox chkInjectNoRead;
        private CheckBox chkInjectPartialRead;
        private CheckBox chkInjectCustomString;
        private TextBox txtCustomErrorString;
        private Button btnInjectOnce;

        private GroupBox grpScenarios;
        private ComboBox cboScenario;
        private Button btnRunScenario;
        private ProgressBar prgScenario;
        private Label lblScenarioStatus;

        private GroupBox grpFtp;
        private TextBox txtFtpRootPath;
        private Button btnBrowseFtp;
        private ListBox lstPrograms;
        private Button btnRefreshPrograms;

        private GroupBox grpCameraLog;
        private Panel pnlCamLogTools;
        private RichTextBox rtbCameraLog;
        private Button btnCamClearLog;
        private CheckBox chkCamAutoScroll;
        private Button btnCamSaveLog;
        private Label lblCamSentCount;
        private Label lblCamTriggerCount;
        private Label lblCamErrorCount;

        private GroupBox grpPreview;
        private PictureBox picPreview;
        private Label lblPreviewStatus;
        #endregion

        #region Printer Tab Controls
        private Panel pnlPrnLeft;
        private Panel pnlPrnRight;
        private GroupBox grpPrinterServer;
        private Label lblPrnIp;
        private TextBox txtPrnServerIp;
        private Label lblPrnPort;
        private NumericUpDown nudPrnPort;
        private Button btnPrnServerToggle;
        private Panel pnlPrnStatus;
        private Label lblPrnStatus;
        private Label lblPrnClient;

        private GroupBox grpPrinterControl;
        private Label lblPrnState;
        private ComboBox cboPrnState;
        private Label lblPrnSpeed;
        private TrackBar trackPrnSpeed;
        private NumericUpDown nudPrnSpeed;
        private Label lblPrnTemplate;
        private TextBox txtPrnTemplate;
        private CheckBox chkPrnAutoAck;
        private CheckBox chkPrnAutoPrint;
        private CheckBox chkPrnInterlockCamera;

        private GroupBox grpPrintBuffer;
        private Panel pnlBufferStats;
        private DataGridView dgvPrintBuffer;
        private Label lblPrnTotalRecv;
        private Label lblPrnTotalPrinted;
        private Label lblPrnBufferCount;
        private Button btnPrnPrintNext;
        private Button btnPrnClearBuffer;
        private Button btnPrnResetStats;
        private ProgressBar prgPrnBuffer;

        private GroupBox grpPrinterAlarms;
        private Label lblRsalCode;
        private ComboBox cboRsalCode;
        private Button btnSendRsal;
        private Label lblStarResponse;
        private ComboBox cboStarResponse;
        private Button btnSendPlc001;
        private Button btnPrnSimulateDrop;
        private Button btnPrnTestSyncTime;
        private Button btnPrnTestRollTime;

        private GroupBox grpPrinterLog;
        private Panel pnlPrnLogTools;
        private RichTextBox rtbPrinterLog;
        private Button btnPrnClearLog;
        private CheckBox chkPrnAutoScroll;
        private Button btnPrnSaveLog;
        #endregion

        #region Interlock Tab Controls
        private GroupBox grpInterlockOverview;
        private Label lblInterlockTitle;
        private Label lblInterlockDesc;
        private Button btnStartFullLineSim;
        private Button btnStopFullLineSim;
        private Label lblLineSimStatus;
        private ProgressBar prgLineSim;
        private GroupBox grpInterlockStats;
        private Panel pnlStatsBar;
        private Label lblLinePrintedCount;
        private Label lblLineVerifiedCount;
        private Label lblLineMatchRate;
        private Label lblLineNgCount;
        private RichTextBox rtbInterlockLog;
        #endregion

        private System.Windows.Forms.Timer timerContinuousTrigger;
        private System.Windows.Forms.Timer timerScenario;
        private System.Windows.Forms.Timer timerPrinterPrint;
        private System.Windows.Forms.Timer timerUiRefresh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabCamera = new System.Windows.Forms.TabPage();
            this.tabPrinter = new System.Windows.Forms.TabPage();
            this.tabInterlock = new System.Windows.Forms.TabPage();

            // Camera components
            this.pnlCamLeft = new System.Windows.Forms.Panel();
            this.pnlCamRight = new System.Windows.Forms.Panel();
            this.pnlCamRightTop = new System.Windows.Forms.Panel();
            this.grpCameraServer = new System.Windows.Forms.GroupBox();
            this.lblCamIp = new System.Windows.Forms.Label();
            this.txtCamServerIp = new System.Windows.Forms.TextBox();
            this.lblCamPort = new System.Windows.Forms.Label();
            this.nudCamPort = new System.Windows.Forms.NumericUpDown();
            this.lblCamFtpPort = new System.Windows.Forms.Label();
            this.nudCamFtpPort = new System.Windows.Forms.NumericUpDown();
            this.btnCamServerToggle = new System.Windows.Forms.Button();
            this.pnlCamStatus = new System.Windows.Forms.Panel();
            this.lblCamStatus = new System.Windows.Forms.Label();
            this.lblCamClient = new System.Windows.Forms.Label();
            this.grpTrigger = new System.Windows.Forms.GroupBox();
            this.rbManualTrigger = new System.Windows.Forms.RadioButton();
            this.rbContinuousTrigger = new System.Windows.Forms.RadioButton();
            this.rbOnDemandTrigger = new System.Windows.Forms.RadioButton();
            this.btnSendSingleTrigger = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.trackInterval = new System.Windows.Forms.TrackBar();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.grpProductData = new System.Windows.Forms.GroupBox();
            this.lblProductType = new System.Windows.Forms.Label();
            this.cboProductType = new System.Windows.Forms.ComboBox();
            this.lblMfgDate = new System.Windows.Forms.Label();
            this.dtpMfgDate = new System.Windows.Forms.DateTimePicker();
            this.lblExpDate = new System.Windows.Forms.Label();
            this.dtpExpDate = new System.Windows.Forms.DateTimePicker();
            this.lblShiftTime = new System.Windows.Forms.Label();
            this.txtShiftTime = new System.Windows.Forms.TextBox();
            this.lblLineInfo = new System.Windows.Forms.Label();
            this.txtLineInfo = new System.Windows.Forms.TextBox();
            this.lblBarcodeData = new System.Windows.Forms.Label();
            this.txtBarcodeData = new System.Windows.Forms.TextBox();
            this.btnGenerateBarcode = new System.Windows.Forms.Button();
            this.grpErrorInjection = new System.Windows.Forms.GroupBox();
            this.chkInjectBadFormat = new System.Windows.Forms.CheckBox();
            this.chkInjectWrongDate = new System.Windows.Forms.CheckBox();
            this.chkInjectExpiredDate = new System.Windows.Forms.CheckBox();
            this.chkInjectNoRead = new System.Windows.Forms.CheckBox();
            this.chkInjectPartialRead = new System.Windows.Forms.CheckBox();
            this.chkInjectCustomString = new System.Windows.Forms.CheckBox();
            this.txtCustomErrorString = new System.Windows.Forms.TextBox();
            this.btnInjectOnce = new System.Windows.Forms.Button();
            this.grpScenarios = new System.Windows.Forms.GroupBox();
            this.cboScenario = new System.Windows.Forms.ComboBox();
            this.btnRunScenario = new System.Windows.Forms.Button();
            this.prgScenario = new System.Windows.Forms.ProgressBar();
            this.lblScenarioStatus = new System.Windows.Forms.Label();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.lblPreviewStatus = new System.Windows.Forms.Label();
            this.grpFtp = new System.Windows.Forms.GroupBox();
            this.txtFtpRootPath = new System.Windows.Forms.TextBox();
            this.btnBrowseFtp = new System.Windows.Forms.Button();
            this.lstPrograms = new System.Windows.Forms.ListBox();
            this.btnRefreshPrograms = new System.Windows.Forms.Button();
            this.grpCameraLog = new System.Windows.Forms.GroupBox();
            this.pnlCamLogTools = new System.Windows.Forms.Panel();
            this.lblCamSentCount = new System.Windows.Forms.Label();
            this.lblCamTriggerCount = new System.Windows.Forms.Label();
            this.lblCamErrorCount = new System.Windows.Forms.Label();
            this.chkCamAutoScroll = new System.Windows.Forms.CheckBox();
            this.btnCamClearLog = new System.Windows.Forms.Button();
            this.btnCamSaveLog = new System.Windows.Forms.Button();
            this.rtbCameraLog = new System.Windows.Forms.RichTextBox();

            // Printer components
            this.pnlPrnLeft = new System.Windows.Forms.Panel();
            this.pnlPrnRight = new System.Windows.Forms.Panel();
            this.grpPrinterServer = new System.Windows.Forms.GroupBox();
            this.lblPrnIp = new System.Windows.Forms.Label();
            this.txtPrnServerIp = new System.Windows.Forms.TextBox();
            this.lblPrnPort = new System.Windows.Forms.Label();
            this.nudPrnPort = new System.Windows.Forms.NumericUpDown();
            this.btnPrnServerToggle = new System.Windows.Forms.Button();
            this.pnlPrnStatus = new System.Windows.Forms.Panel();
            this.lblPrnStatus = new System.Windows.Forms.Label();
            this.lblPrnClient = new System.Windows.Forms.Label();
            this.grpPrinterControl = new System.Windows.Forms.GroupBox();
            this.lblPrnState = new System.Windows.Forms.Label();
            this.cboPrnState = new System.Windows.Forms.ComboBox();
            this.lblPrnSpeed = new System.Windows.Forms.Label();
            this.trackPrnSpeed = new System.Windows.Forms.TrackBar();
            this.nudPrnSpeed = new System.Windows.Forms.NumericUpDown();
            this.lblPrnTemplate = new System.Windows.Forms.Label();
            this.txtPrnTemplate = new System.Windows.Forms.TextBox();
            this.chkPrnAutoAck = new System.Windows.Forms.CheckBox();
            this.chkPrnAutoPrint = new System.Windows.Forms.CheckBox();
            this.chkPrnInterlockCamera = new System.Windows.Forms.CheckBox();
            this.grpPrinterAlarms = new System.Windows.Forms.GroupBox();
            this.lblRsalCode = new System.Windows.Forms.Label();
            this.cboRsalCode = new System.Windows.Forms.ComboBox();
            this.btnSendRsal = new System.Windows.Forms.Button();
            this.lblStarResponse = new System.Windows.Forms.Label();
            this.cboStarResponse = new System.Windows.Forms.ComboBox();
            this.btnSendPlc001 = new System.Windows.Forms.Button();
            this.btnPrnSimulateDrop = new System.Windows.Forms.Button();
            this.btnPrnTestSyncTime = new System.Windows.Forms.Button();
            this.btnPrnTestRollTime = new System.Windows.Forms.Button();
            this.grpPrintBuffer = new System.Windows.Forms.GroupBox();
            this.pnlBufferStats = new System.Windows.Forms.Panel();
            this.lblPrnTotalRecv = new System.Windows.Forms.Label();
            this.lblPrnTotalPrinted = new System.Windows.Forms.Label();
            this.lblPrnBufferCount = new System.Windows.Forms.Label();
            this.btnPrnPrintNext = new System.Windows.Forms.Button();
            this.btnPrnClearBuffer = new System.Windows.Forms.Button();
            this.btnPrnResetStats = new System.Windows.Forms.Button();
            this.prgPrnBuffer = new System.Windows.Forms.ProgressBar();
            this.dgvPrintBuffer = new System.Windows.Forms.DataGridView();
            this.grpPrinterLog = new System.Windows.Forms.GroupBox();
            this.pnlPrnLogTools = new System.Windows.Forms.Panel();
            this.chkPrnAutoScroll = new System.Windows.Forms.CheckBox();
            this.btnPrnClearLog = new System.Windows.Forms.Button();
            this.btnPrnSaveLog = new System.Windows.Forms.Button();
            this.rtbPrinterLog = new System.Windows.Forms.RichTextBox();

            // Interlock components
            this.grpInterlockOverview = new System.Windows.Forms.GroupBox();
            this.lblInterlockTitle = new System.Windows.Forms.Label();
            this.lblInterlockDesc = new System.Windows.Forms.Label();
            this.btnStartFullLineSim = new System.Windows.Forms.Button();
            this.btnStopFullLineSim = new System.Windows.Forms.Button();
            this.lblLineSimStatus = new System.Windows.Forms.Label();
            this.prgLineSim = new System.Windows.Forms.ProgressBar();
            this.grpInterlockStats = new System.Windows.Forms.GroupBox();
            this.pnlStatsBar = new System.Windows.Forms.Panel();
            this.lblLinePrintedCount = new System.Windows.Forms.Label();
            this.lblLineVerifiedCount = new System.Windows.Forms.Label();
            this.lblLineMatchRate = new System.Windows.Forms.Label();
            this.lblLineNgCount = new System.Windows.Forms.Label();
            this.rtbInterlockLog = new System.Windows.Forms.RichTextBox();

            // Timers
            this.timerContinuousTrigger = new System.Windows.Forms.Timer(this.components);
            this.timerScenario = new System.Windows.Forms.Timer(this.components);
            this.timerPrinterPrint = new System.Windows.Forms.Timer(this.components);
            this.timerUiRefresh = new System.Windows.Forms.Timer(this.components);

            this.tabMain.SuspendLayout();
            this.tabCamera.SuspendLayout();
            this.tabPrinter.SuspendLayout();
            this.tabInterlock.SuspendLayout();
            this.pnlCamLeft.SuspendLayout();
            this.pnlCamRight.SuspendLayout();
            this.pnlCamRightTop.SuspendLayout();
            this.grpCameraServer.SuspendLayout();
            this.grpTrigger.SuspendLayout();
            this.grpProductData.SuspendLayout();
            this.grpErrorInjection.SuspendLayout();
            this.grpScenarios.SuspendLayout();
            this.grpPreview.SuspendLayout();
            this.grpFtp.SuspendLayout();
            this.grpCameraLog.SuspendLayout();
            this.pnlCamLogTools.SuspendLayout();
            this.pnlPrnLeft.SuspendLayout();
            this.pnlPrnRight.SuspendLayout();
            this.grpPrinterServer.SuspendLayout();
            this.grpPrinterControl.SuspendLayout();
            this.grpPrinterAlarms.SuspendLayout();
            this.grpPrintBuffer.SuspendLayout();
            this.pnlBufferStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintBuffer)).BeginInit();
            this.grpPrinterLog.SuspendLayout();
            this.pnlPrnLogTools.SuspendLayout();
            this.grpInterlockOverview.SuspendLayout();
            this.grpInterlockStats.SuspendLayout();
            this.pnlStatsBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamFtpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrnPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPrnSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrnSpeed)).BeginInit();
            this.SuspendLayout();

            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabCamera);
            this.tabMain.Controls.Add(this.tabPrinter);
            this.tabMain.Controls.Add(this.tabInterlock);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tabMain.ItemSize = new System.Drawing.Size(260, 36);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1384, 861);
            this.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMain.TabIndex = 0;

            // 
            // tabCamera
            // 
            this.tabCamera.BackColor = System.Drawing.Color.FromArgb(30, 30, 46);
            this.tabCamera.Controls.Add(this.pnlCamRight);
            this.tabCamera.Controls.Add(this.pnlCamLeft);
            this.tabCamera.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabCamera.Location = new System.Drawing.Point(4, 40);
            this.tabCamera.Name = "tabCamera";
            this.tabCamera.Padding = new System.Windows.Forms.Padding(10);
            this.tabCamera.Size = new System.Drawing.Size(1376, 817);
            this.tabCamera.TabIndex = 0;
            this.tabCamera.Text = "đŸ“· Camera Keyence VS-C (Port 8500)";

            // 
            // pnlCamLeft
            // 
            this.pnlCamLeft.AutoScroll = true;
            this.pnlCamLeft.Controls.Add(this.grpScenarios);
            this.pnlCamLeft.Controls.Add(this.grpErrorInjection);
            this.pnlCamLeft.Controls.Add(this.grpProductData);
            this.pnlCamLeft.Controls.Add(this.grpTrigger);
            this.pnlCamLeft.Controls.Add(this.grpCameraServer);
            this.pnlCamLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCamLeft.Location = new System.Drawing.Point(10, 10);
            this.pnlCamLeft.Name = "pnlCamLeft";
            this.pnlCamLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlCamLeft.Size = new System.Drawing.Size(460, 797);
            this.pnlCamLeft.TabIndex = 0;

            // 
            // grpCameraServer
            // 
            this.grpCameraServer.Controls.Add(this.lblCamIp);
            this.grpCameraServer.Controls.Add(this.txtCamServerIp);
            this.grpCameraServer.Controls.Add(this.lblCamPort);
            this.grpCameraServer.Controls.Add(this.nudCamPort);
            this.grpCameraServer.Controls.Add(this.lblCamFtpPort);
            this.grpCameraServer.Controls.Add(this.nudCamFtpPort);
            this.grpCameraServer.Controls.Add(this.btnCamServerToggle);
            this.grpCameraServer.Controls.Add(this.pnlCamStatus);
            this.grpCameraServer.Controls.Add(this.lblCamStatus);
            this.grpCameraServer.Controls.Add(this.lblCamClient);
            this.grpCameraServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCameraServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCameraServer.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpCameraServer.Location = new System.Drawing.Point(0, 0);
            this.grpCameraServer.Name = "grpCameraServer";
            this.grpCameraServer.Padding = new System.Windows.Forms.Padding(8);
            this.grpCameraServer.Size = new System.Drawing.Size(450, 115);
            this.grpCameraServer.TabIndex = 0;
            this.grpCameraServer.TabStop = false;
            this.grpCameraServer.Text = "Cáº¤U HĂŒNH TCP/FTP SERVER (CAMERA KEYENCE)";

            this.lblCamIp.AutoSize = true;
            this.lblCamIp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCamIp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCamIp.Location = new System.Drawing.Point(12, 25);
            this.lblCamIp.Name = "lblCamIp";
            this.lblCamIp.Size = new System.Drawing.Size(81, 15);
            this.lblCamIp.Text = "IP Láº¯ng Nghe:";

            this.txtCamServerIp.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtCamServerIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCamServerIp.ForeColor = System.Drawing.Color.White;
            this.txtCamServerIp.Location = new System.Drawing.Point(95, 22);
            this.txtCamServerIp.Name = "txtCamServerIp";
            this.txtCamServerIp.Size = new System.Drawing.Size(90, 23);
            this.txtCamServerIp.TabIndex = 1;
            this.txtCamServerIp.Text = "0.0.0.0";

            this.lblCamPort.AutoSize = true;
            this.lblCamPort.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCamPort.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCamPort.Location = new System.Drawing.Point(195, 25);
            this.lblCamPort.Name = "lblCamPort";
            this.lblCamPort.Size = new System.Drawing.Size(56, 15);
            this.lblCamPort.TabIndex = 2;
            this.lblCamPort.Text = "TCP Port:";

            this.nudCamPort.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.nudCamPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudCamPort.ForeColor = System.Drawing.Color.White;
            this.nudCamPort.Location = new System.Drawing.Point(255, 22);
            this.nudCamPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.nudCamPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCamPort.Name = "nudCamPort";
            this.nudCamPort.Size = new System.Drawing.Size(65, 23);
            this.nudCamPort.TabIndex = 3;
            this.nudCamPort.Value = new decimal(new int[] { 8500, 0, 0, 0 });

            this.lblCamFtpPort.AutoSize = true;
            this.lblCamFtpPort.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCamFtpPort.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCamFtpPort.Location = new System.Drawing.Point(330, 25);
            this.lblCamFtpPort.Name = "lblCamFtpPort";
            this.lblCamFtpPort.Size = new System.Drawing.Size(30, 15);
            this.lblCamFtpPort.TabIndex = 4;
            this.lblCamFtpPort.Text = "FTP:";

            this.nudCamFtpPort.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.nudCamFtpPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudCamFtpPort.ForeColor = System.Drawing.Color.White;
            this.nudCamFtpPort.Location = new System.Drawing.Point(365, 22);
            this.nudCamFtpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.nudCamFtpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCamFtpPort.Name = "nudCamFtpPort";
            this.nudCamFtpPort.Size = new System.Drawing.Size(65, 23);
            this.nudCamFtpPort.TabIndex = 5;
            this.nudCamFtpPort.Value = new decimal(new int[] { 21, 0, 0, 0 });

            this.btnCamServerToggle.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.btnCamServerToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamServerToggle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnCamServerToggle.ForeColor = System.Drawing.Color.White;
            this.btnCamServerToggle.Location = new System.Drawing.Point(12, 52);
            this.btnCamServerToggle.Name = "btnCamServerToggle";
            this.btnCamServerToggle.Size = new System.Drawing.Size(230, 30);
            this.btnCamServerToggle.TabIndex = 6;
            this.btnCamServerToggle.Text = "KHá»I Äá»˜NG CAMERA SERVER";
            this.btnCamServerToggle.UseVisualStyleBackColor = false;
            this.btnCamServerToggle.Click += new System.EventHandler(this.btnCamServerToggle_Click);

            this.pnlCamStatus.BackColor = System.Drawing.Color.Red;
            this.pnlCamStatus.Location = new System.Drawing.Point(250, 58);
            this.pnlCamStatus.Name = "pnlCamStatus";
            this.pnlCamStatus.Size = new System.Drawing.Size(16, 16);
            this.pnlCamStatus.TabIndex = 7;

            this.lblCamStatus.AutoSize = true;
            this.lblCamStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCamStatus.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.lblCamStatus.Location = new System.Drawing.Point(272, 58);
            this.lblCamStatus.Name = "lblCamStatus";
            this.lblCamStatus.Size = new System.Drawing.Size(107, 15);
            this.lblCamStatus.TabIndex = 8;
            this.lblCamStatus.Text = "Server: ChÆ°a cháº¡y";

            this.lblCamClient.AutoSize = true;
            this.lblCamClient.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCamClient.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblCamClient.Location = new System.Drawing.Point(12, 88);
            this.lblCamClient.Name = "lblCamClient";
            this.lblCamClient.Size = new System.Drawing.Size(89, 13);
            this.lblCamClient.TabIndex = 9;
            this.lblCamClient.Text = "Client: 0 káº¿t ná»‘i";

            // 
            // grpTrigger
            // 
            this.grpTrigger.Controls.Add(this.rbManualTrigger);
            this.grpTrigger.Controls.Add(this.rbContinuousTrigger);
            this.grpTrigger.Controls.Add(this.rbOnDemandTrigger);
            this.grpTrigger.Controls.Add(this.btnSendSingleTrigger);
            this.grpTrigger.Controls.Add(this.lblInterval);
            this.grpTrigger.Controls.Add(this.trackInterval);
            this.grpTrigger.Controls.Add(this.nudInterval);
            this.grpTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTrigger.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTrigger.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpTrigger.Location = new System.Drawing.Point(0, 115);
            this.grpTrigger.Name = "grpTrigger";
            this.grpTrigger.Padding = new System.Windows.Forms.Padding(8);
            this.grpTrigger.Size = new System.Drawing.Size(450, 110);
            this.grpTrigger.TabIndex = 1;
            this.grpTrigger.TabStop = false;
            this.grpTrigger.Text = "CHáº¾ Äá»˜ TRIGGER & NHá»P Äá»˜ CHá»¤P";

            this.rbManualTrigger.AutoSize = true;
            this.rbManualTrigger.Checked = true;
            this.rbManualTrigger.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbManualTrigger.ForeColor = System.Drawing.Color.White;
            this.rbManualTrigger.Location = new System.Drawing.Point(12, 22);
            this.rbManualTrigger.Name = "rbManualTrigger";
            this.rbManualTrigger.Size = new System.Drawing.Size(133, 19);
            this.rbManualTrigger.TabIndex = 0;
            this.rbManualTrigger.TabStop = true;
            this.rbManualTrigger.Text = "Thá»§ cĂ´ng (NĂºt báº¥m)";
            this.rbManualTrigger.UseVisualStyleBackColor = true;

            this.rbContinuousTrigger.AutoSize = true;
            this.rbContinuousTrigger.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbContinuousTrigger.ForeColor = System.Drawing.Color.White;
            this.rbContinuousTrigger.Location = new System.Drawing.Point(155, 22);
            this.rbContinuousTrigger.Name = "rbContinuousTrigger";
            this.rbContinuousTrigger.Size = new System.Drawing.Size(176, 19);
            this.rbContinuousTrigger.TabIndex = 1;
            this.rbContinuousTrigger.Text = "Tá»± Ä‘á»™ng (Continuous Loop)";
            this.rbContinuousTrigger.UseVisualStyleBackColor = true;
            this.rbContinuousTrigger.CheckedChanged += new System.EventHandler(this.rbContinuousTrigger_CheckedChanged);

            this.rbOnDemandTrigger.AutoSize = true;
            this.rbOnDemandTrigger.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.rbOnDemandTrigger.ForeColor = System.Drawing.Color.White;
            this.rbOnDemandTrigger.Location = new System.Drawing.Point(12, 45);
            this.rbOnDemandTrigger.Name = "rbOnDemandTrigger";
            this.rbOnDemandTrigger.Size = new System.Drawing.Size(168, 19);
            this.rbOnDemandTrigger.TabIndex = 2;
            this.rbOnDemandTrigger.Text = "Theo lá»‡nh (T1/T2/TRG/SW)";
            this.rbOnDemandTrigger.UseVisualStyleBackColor = true;

            this.btnSendSingleTrigger.BackColor = System.Drawing.Color.FromArgb(94, 129, 172);
            this.btnSendSingleTrigger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendSingleTrigger.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnSendSingleTrigger.ForeColor = System.Drawing.Color.White;
            this.btnSendSingleTrigger.Location = new System.Drawing.Point(230, 45);
            this.btnSendSingleTrigger.Name = "btnSendSingleTrigger";
            this.btnSendSingleTrigger.Size = new System.Drawing.Size(205, 28);
            this.btnSendSingleTrigger.TabIndex = 3;
            this.btnSendSingleTrigger.Text = "đŸ“¸ Báº®N 1 TRIGGER (Gá»¬I Dá»® LIá»†U)";
            this.btnSendSingleTrigger.UseVisualStyleBackColor = false;
            this.btnSendSingleTrigger.Click += new System.EventHandler(this.btnSendSingleTrigger_Click);

            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInterval.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblInterval.Location = new System.Drawing.Point(12, 78);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(73, 15);
            this.lblInterval.TabIndex = 4;
            this.lblInterval.Text = "Tá»‘c Ä‘á»™ (ms):";

            this.trackInterval.Location = new System.Drawing.Point(90, 74);
            this.trackInterval.Maximum = 2000;
            this.trackInterval.Minimum = 20;
            this.trackInterval.Name = "trackInterval";
            this.trackInterval.Size = new System.Drawing.Size(260, 45);
            this.trackInterval.TabIndex = 5;
            this.trackInterval.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackInterval.Value = 200;
            this.trackInterval.Scroll += (s, e) => { this.nudInterval.Value = this.trackInterval.Value; this.timerContinuousTrigger.Interval = this.trackInterval.Value; };

            this.nudInterval.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.nudInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudInterval.ForeColor = System.Drawing.Color.White;
            this.nudInterval.Location = new System.Drawing.Point(360, 76);
            this.nudInterval.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            this.nudInterval.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(65, 23);
            this.nudInterval.TabIndex = 6;
            this.nudInterval.Value = new decimal(new int[] { 200, 0, 0, 0 });
            this.nudInterval.ValueChanged += (s, e) => { this.trackInterval.Value = (int)this.nudInterval.Value; this.timerContinuousTrigger.Interval = (int)this.nudInterval.Value; };

            // 
            // grpProductData
            // 
            this.grpProductData.Controls.Add(this.lblProductType);
            this.grpProductData.Controls.Add(this.cboProductType);
            this.grpProductData.Controls.Add(this.lblMfgDate);
            this.grpProductData.Controls.Add(this.dtpMfgDate);
            this.grpProductData.Controls.Add(this.lblExpDate);
            this.grpProductData.Controls.Add(this.dtpExpDate);
            this.grpProductData.Controls.Add(this.lblShiftTime);
            this.grpProductData.Controls.Add(this.txtShiftTime);
            this.grpProductData.Controls.Add(this.lblLineInfo);
            this.grpProductData.Controls.Add(this.txtLineInfo);
            this.grpProductData.Controls.Add(this.lblBarcodeData);
            this.grpProductData.Controls.Add(this.txtBarcodeData);
            this.grpProductData.Controls.Add(this.btnGenerateBarcode);
            this.grpProductData.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProductData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpProductData.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpProductData.Location = new System.Drawing.Point(0, 225);
            this.grpProductData.Name = "grpProductData";
            this.grpProductData.Padding = new System.Windows.Forms.Padding(8);
            this.grpProductData.Size = new System.Drawing.Size(450, 205);
            this.grpProductData.TabIndex = 2;
            this.grpProductData.TabStop = false;
            this.grpProductData.Text = "Dá»® LIá»†U MĂƒ Váº CH / QR TH TRUE MILK";

            this.lblProductType.AutoSize = true;
            this.lblProductType.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblProductType.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblProductType.Location = new System.Drawing.Point(12, 24);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(89, 15);
            this.lblProductType.TabIndex = 0;
            this.lblProductType.Text = "Loáº¡i Sáº£n Pháº©m:";

            this.cboProductType.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.cboProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboProductType.ForeColor = System.Drawing.Color.White;
            this.cboProductType.FormattingEnabled = true;
            this.cboProductType.Items.AddRange(new object[] {
            "Sá»¯a tÆ°Æ¡i tiá»‡t trĂ¹ng 180ml (8935001234567)",
            "Sá»¯a tÆ°Æ¡i tiá»‡t trĂ¹ng 110ml (8935001234581)",
            "Sá»¯a chua Äƒn tá»± nhiĂªn 100g (8935001234604)",
            "Sá»¯a chua uá»‘ng Topkid 110ml (8935001234628)",
            "Sá»¯a bá»‹ch tiá»‡t trĂ¹ng 220ml (8935001234659)",
            "NÆ°á»›c tinh khiáº¿t TH True WATER (8935001234888)"});
            this.cboProductType.Location = new System.Drawing.Point(115, 21);
            this.cboProductType.Name = "cboProductType";
            this.cboProductType.Size = new System.Drawing.Size(315, 23);
            this.cboProductType.TabIndex = 1;
            this.cboProductType.SelectedIndexChanged += new System.EventHandler(this.cboProductType_SelectedIndexChanged);

            this.lblMfgDate.AutoSize = true;
            this.lblMfgDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMfgDate.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblMfgDate.Location = new System.Drawing.Point(12, 54);
            this.lblMfgDate.Name = "lblMfgDate";
            this.lblMfgDate.Size = new System.Drawing.Size(91, 15);
            this.lblMfgDate.TabIndex = 2;
            this.lblMfgDate.Text = "NgĂ y SX (MFG):";

            this.dtpMfgDate.CustomFormat = "dd/MM/yyyy";
            this.dtpMfgDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMfgDate.Location = new System.Drawing.Point(115, 51);
            this.dtpMfgDate.Name = "dtpMfgDate";
            this.dtpMfgDate.Size = new System.Drawing.Size(110, 23);
            this.dtpMfgDate.TabIndex = 3;

            this.lblExpDate.AutoSize = true;
            this.lblExpDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblExpDate.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblExpDate.Location = new System.Drawing.Point(235, 54);
            this.lblExpDate.Name = "lblExpDate";
            this.lblExpDate.Size = new System.Drawing.Size(83, 15);
            this.lblExpDate.TabIndex = 4;
            this.lblExpDate.Text = "Háº¡n SD (EXP):";

            this.dtpExpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpExpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpDate.Location = new System.Drawing.Point(320, 51);
            this.dtpExpDate.Name = "dtpExpDate";
            this.dtpExpDate.Size = new System.Drawing.Size(110, 23);
            this.dtpExpDate.TabIndex = 5;

            this.lblShiftTime.AutoSize = true;
            this.lblShiftTime.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblShiftTime.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblShiftTime.Location = new System.Drawing.Point(12, 84);
            this.lblShiftTime.Name = "lblShiftTime";
            this.lblShiftTime.Size = new System.Drawing.Size(73, 15);
            this.lblShiftTime.TabIndex = 6;
            this.lblShiftTime.Text = "Giá» / Ca SX:";

            this.txtShiftTime.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtShiftTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShiftTime.ForeColor = System.Drawing.Color.White;
            this.txtShiftTime.Location = new System.Drawing.Point(115, 81);
            this.txtShiftTime.Name = "txtShiftTime";
            this.txtShiftTime.Size = new System.Drawing.Size(110, 23);
            this.txtShiftTime.TabIndex = 7;
            this.txtShiftTime.Text = "08:00";

            this.lblLineInfo.AutoSize = true;
            this.lblLineInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLineInfo.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblLineInfo.Location = new System.Drawing.Point(235, 84);
            this.lblLineInfo.Name = "lblLineInfo";
            this.lblLineInfo.Size = new System.Drawing.Size(86, 15);
            this.lblLineInfo.TabIndex = 8;
            this.lblLineInfo.Text = "Line / Ca / Roll:";

            this.txtLineInfo.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtLineInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLineInfo.ForeColor = System.Drawing.Color.White;
            this.txtLineInfo.Location = new System.Drawing.Point(320, 81);
            this.txtLineInfo.Name = "txtLineInfo";
            this.txtLineInfo.Size = new System.Drawing.Size(110, 23);
            this.txtLineInfo.TabIndex = 9;
            this.txtLineInfo.Text = "A01 C1 R01";

            this.lblBarcodeData.AutoSize = true;
            this.lblBarcodeData.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBarcodeData.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblBarcodeData.Location = new System.Drawing.Point(12, 114);
            this.lblBarcodeData.Name = "lblBarcodeData";
            this.lblBarcodeData.Size = new System.Drawing.Size(86, 15);
            this.lblBarcodeData.TabIndex = 10;
            this.lblBarcodeData.Text = "Chuá»—i mĂ£ gá»­i:";

            this.txtBarcodeData.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtBarcodeData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcodeData.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtBarcodeData.ForeColor = System.Drawing.Color.Yellow;
            this.txtBarcodeData.Location = new System.Drawing.Point(115, 111);
            this.txtBarcodeData.Name = "txtBarcodeData";
            this.txtBarcodeData.Size = new System.Drawing.Size(315, 22);
            this.txtBarcodeData.TabIndex = 11;

            this.btnGenerateBarcode.BackColor = System.Drawing.Color.FromArgb(76, 86, 106);
            this.btnGenerateBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateBarcode.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnGenerateBarcode.ForeColor = System.Drawing.Color.White;
            this.btnGenerateBarcode.Location = new System.Drawing.Point(115, 142);
            this.btnGenerateBarcode.Name = "btnGenerateBarcode";
            this.btnGenerateBarcode.Size = new System.Drawing.Size(315, 28);
            this.btnGenerateBarcode.TabIndex = 12;
            this.btnGenerateBarcode.Text = "â¡ TĂI Táº O MĂƒ THEO THá»œI GIAN THá»°C";
            this.btnGenerateBarcode.UseVisualStyleBackColor = false;

            // 
            // grpErrorInjection
            // 
            this.grpErrorInjection.Controls.Add(this.chkInjectBadFormat);
            this.grpErrorInjection.Controls.Add(this.chkInjectWrongDate);
            this.grpErrorInjection.Controls.Add(this.chkInjectExpiredDate);
            this.grpErrorInjection.Controls.Add(this.chkInjectNoRead);
            this.grpErrorInjection.Controls.Add(this.chkInjectPartialRead);
            this.grpErrorInjection.Controls.Add(this.chkInjectCustomString);
            this.grpErrorInjection.Controls.Add(this.txtCustomErrorString);
            this.grpErrorInjection.Controls.Add(this.btnInjectOnce);
            this.grpErrorInjection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpErrorInjection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpErrorInjection.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.grpErrorInjection.Location = new System.Drawing.Point(0, 430);
            this.grpErrorInjection.Name = "grpErrorInjection";
            this.grpErrorInjection.Padding = new System.Windows.Forms.Padding(8);
            this.grpErrorInjection.Size = new System.Drawing.Size(450, 160);
            this.grpErrorInjection.TabIndex = 3;
            this.grpErrorInjection.TabStop = false;
            this.grpErrorInjection.Text = "TIĂM Lá»–I KIá»‚M THá»¬ (ERROR INJECTION - NG TEST)";

            this.chkInjectBadFormat.AutoSize = true;
            this.chkInjectBadFormat.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectBadFormat.ForeColor = System.Drawing.Color.White;
            this.chkInjectBadFormat.Location = new System.Drawing.Point(12, 22);
            this.chkInjectBadFormat.Name = "chkInjectBadFormat";
            this.chkInjectBadFormat.Size = new System.Drawing.Size(215, 19);
            this.chkInjectBadFormat.TabIndex = 0;
            this.chkInjectBadFormat.Text = "Sai Ä‘á»‹nh dáº¡ng mĂ£ (Bad Checksum)";
            this.chkInjectBadFormat.UseVisualStyleBackColor = true;

            this.chkInjectWrongDate.AutoSize = true;
            this.chkInjectWrongDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectWrongDate.ForeColor = System.Drawing.Color.White;
            this.chkInjectWrongDate.Location = new System.Drawing.Point(230, 22);
            this.chkInjectWrongDate.Name = "chkInjectWrongDate";
            this.chkInjectWrongDate.Size = new System.Drawing.Size(201, 19);
            this.chkInjectWrongDate.TabIndex = 1;
            this.chkInjectWrongDate.Text = "Sai ngĂ y sáº£n xuáº¥t (Wrong MFG)";
            this.chkInjectWrongDate.UseVisualStyleBackColor = true;

            this.chkInjectExpiredDate.AutoSize = true;
            this.chkInjectExpiredDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectExpiredDate.ForeColor = System.Drawing.Color.White;
            this.chkInjectExpiredDate.Location = new System.Drawing.Point(12, 45);
            this.chkInjectExpiredDate.Name = "chkInjectExpiredDate";
            this.chkInjectExpiredDate.Size = new System.Drawing.Size(167, 19);
            this.chkInjectExpiredDate.TabIndex = 2;
            this.chkInjectExpiredDate.Text = "QuĂ¡ háº¡n sá»­ dá»¥ng (Expired)";
            this.chkInjectExpiredDate.UseVisualStyleBackColor = true;

            this.chkInjectNoRead.AutoSize = true;
            this.chkInjectNoRead.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectNoRead.ForeColor = System.Drawing.Color.White;
            this.chkInjectNoRead.Location = new System.Drawing.Point(230, 45);
            this.chkInjectNoRead.Name = "chkInjectNoRead";
            this.chkInjectNoRead.Size = new System.Drawing.Size(227, 19);
            this.chkInjectNoRead.TabIndex = 3;
            this.chkInjectNoRead.Text = "KhĂ´ng Ä‘á»c Ä‘Æ°á»£c (ERROR / NOREAD)";
            this.chkInjectNoRead.UseVisualStyleBackColor = true;

            this.chkInjectPartialRead.AutoSize = true;
            this.chkInjectPartialRead.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectPartialRead.ForeColor = System.Drawing.Color.White;
            this.chkInjectPartialRead.Location = new System.Drawing.Point(12, 68);
            this.chkInjectPartialRead.Name = "chkInjectPartialRead";
            this.chkInjectPartialRead.Size = new System.Drawing.Size(193, 19);
            this.chkInjectPartialRead.TabIndex = 4;
            this.chkInjectPartialRead.Text = "MĂ£ bá»‹ máº¥t kĂ½ tá»± (Partial Read)";
            this.chkInjectPartialRead.UseVisualStyleBackColor = true;

            this.chkInjectCustomString.AutoSize = true;
            this.chkInjectCustomString.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkInjectCustomString.ForeColor = System.Drawing.Color.White;
            this.chkInjectCustomString.Location = new System.Drawing.Point(12, 92);
            this.chkInjectCustomString.Name = "chkInjectCustomString";
            this.chkInjectCustomString.Size = new System.Drawing.Size(117, 19);
            this.chkInjectCustomString.TabIndex = 5;
            this.chkInjectCustomString.Text = "Chuá»—i tĂ¹y chá»‰nh:";
            this.chkInjectCustomString.UseVisualStyleBackColor = true;

            this.txtCustomErrorString.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtCustomErrorString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomErrorString.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.txtCustomErrorString.ForeColor = System.Drawing.Color.Salmon;
            this.txtCustomErrorString.Location = new System.Drawing.Point(135, 90);
            this.txtCustomErrorString.Name = "txtCustomErrorString";
            this.txtCustomErrorString.Size = new System.Drawing.Size(295, 21);
            this.txtCustomErrorString.TabIndex = 6;
            this.txtCustomErrorString.Text = "TH_MILK_WRONG_CODE_9999";

            this.btnInjectOnce.BackColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.btnInjectOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInjectOnce.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnInjectOnce.ForeColor = System.Drawing.Color.White;
            this.btnInjectOnce.Location = new System.Drawing.Point(12, 118);
            this.btnInjectOnce.Name = "btnInjectOnce";
            this.btnInjectOnce.Size = new System.Drawing.Size(418, 28);
            this.btnInjectOnce.TabIndex = 7;
            this.btnInjectOnce.Text = "đŸ¨ TIĂM 1 Lá»–I NGAY BĂ‚Y GIá»œ";
            this.btnInjectOnce.UseVisualStyleBackColor = false;
            this.btnInjectOnce.Click += new System.EventHandler(this.btnInjectOnce_Click);

            // 
            // grpScenarios
            // 
            this.grpScenarios.Controls.Add(this.cboScenario);
            this.grpScenarios.Controls.Add(this.btnRunScenario);
            this.grpScenarios.Controls.Add(this.prgScenario);
            this.grpScenarios.Controls.Add(this.lblScenarioStatus);
            this.grpScenarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpScenarios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpScenarios.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpScenarios.Location = new System.Drawing.Point(0, 590);
            this.grpScenarios.Name = "grpScenarios";
            this.grpScenarios.Padding = new System.Windows.Forms.Padding(8);
            this.grpScenarios.Size = new System.Drawing.Size(450, 110);
            this.grpScenarios.TabIndex = 4;
            this.grpScenarios.TabStop = false;
            this.grpScenarios.Text = "Ká»CH Báº¢N CHáº Y Tá»° Äá»˜NG (TEST SCENARIOS)";

            this.cboScenario.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.cboScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScenario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboScenario.ForeColor = System.Drawing.Color.White;
            this.cboScenario.FormattingEnabled = true;
            this.cboScenario.Items.AddRange(new object[] {
            "1. Cháº¡y 100% PASS liĂªn tá»¥c (500 sáº£n pháº©m)",
            "2. 10 PASS thĂ¬ 1 NG xen káº½ (Intermittent NG)",
            "3. Bá»‹ 5 lá»—i NG liĂªn tiáº¿p (Test CĂ²i/ÄĂ¨n PLC AutoStop)",
            "4. Stress Test tá»‘c Ä‘á»™ cao (20ms/mĂ£, 2000 mĂ£)"});
            this.cboScenario.Location = new System.Drawing.Point(12, 24);
            this.cboScenario.Name = "cboScenario";
            this.cboScenario.Size = new System.Drawing.Size(280, 23);
            this.cboScenario.TabIndex = 0;

            this.btnRunScenario.BackColor = System.Drawing.Color.FromArgb(143, 188, 187);
            this.btnRunScenario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunScenario.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnRunScenario.ForeColor = System.Drawing.Color.Black;
            this.btnRunScenario.Location = new System.Drawing.Point(300, 22);
            this.btnRunScenario.Name = "btnRunScenario";
            this.btnRunScenario.Size = new System.Drawing.Size(130, 27);
            this.btnRunScenario.TabIndex = 1;
            this.btnRunScenario.Text = "CHáº Y Ká»CH Báº¢N";
            this.btnRunScenario.UseVisualStyleBackColor = false;
            this.btnRunScenario.Click += new System.EventHandler(this.btnRunScenario_Click);

            this.prgScenario.Location = new System.Drawing.Point(12, 58);
            this.prgScenario.Name = "prgScenario";
            this.prgScenario.Size = new System.Drawing.Size(418, 16);
            this.prgScenario.TabIndex = 2;

            this.lblScenarioStatus.AutoSize = true;
            this.lblScenarioStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblScenarioStatus.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblScenarioStatus.Location = new System.Drawing.Point(12, 80);
            this.lblScenarioStatus.Name = "lblScenarioStatus";
            this.lblScenarioStatus.Size = new System.Drawing.Size(183, 13);
            this.lblScenarioStatus.TabIndex = 3;
            this.lblScenarioStatus.Text = "Sáºµn sĂ ng cháº¡y ká»‹ch báº£n kiá»ƒm thá»­.";

            // 
            // pnlCamRight
            // 
            this.pnlCamRight.Controls.Add(this.grpCameraLog);
            this.pnlCamRight.Controls.Add(this.pnlCamRightTop);
            this.pnlCamRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCamRight.Location = new System.Drawing.Point(470, 10);
            this.pnlCamRight.Name = "pnlCamRight";
            this.pnlCamRight.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlCamRight.Size = new System.Drawing.Size(896, 797);
            this.pnlCamRight.TabIndex = 1;

            // 
            // pnlCamRightTop
            // 
            this.pnlCamRightTop.Controls.Add(this.grpFtp);
            this.pnlCamRightTop.Controls.Add(this.grpPreview);
            this.pnlCamRightTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCamRightTop.Location = new System.Drawing.Point(5, 0);
            this.pnlCamRightTop.Name = "pnlCamRightTop";
            this.pnlCamRightTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlCamRightTop.Size = new System.Drawing.Size(891, 220);
            this.pnlCamRightTop.TabIndex = 0;

            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.picPreview);
            this.grpPreview.Controls.Add(this.lblPreviewStatus);
            this.grpPreview.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpPreview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPreview.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpPreview.Location = new System.Drawing.Point(0, 0);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(420, 212);
            this.grpPreview.TabIndex = 0;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "MĂ” PHá»NG CAMERA CHá»¤P THá»°C Táº¾ (LIVE OCR / BARCODE)";

            this.picPreview.BackColor = System.Drawing.Color.FromArgb(20, 20, 30);
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(3, 19);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(414, 166);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;

            this.lblPreviewStatus.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.lblPreviewStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPreviewStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPreviewStatus.ForeColor = System.Drawing.Color.White;
            this.lblPreviewStatus.Location = new System.Drawing.Point(3, 185);
            this.lblPreviewStatus.Name = "lblPreviewStatus";
            this.lblPreviewStatus.Size = new System.Drawing.Size(414, 24);
            this.lblPreviewStatus.TabIndex = 1;
            this.lblPreviewStatus.Text = "CAMERA TRáº NG THĂI: PASS (OK)";
            this.lblPreviewStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // grpFtp
            // 
            this.grpFtp.Controls.Add(this.txtFtpRootPath);
            this.grpFtp.Controls.Add(this.btnBrowseFtp);
            this.grpFtp.Controls.Add(this.lstPrograms);
            this.grpFtp.Controls.Add(this.btnRefreshPrograms);
            this.grpFtp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFtp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpFtp.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpFtp.Location = new System.Drawing.Point(420, 0);
            this.grpFtp.Name = "grpFtp";
            this.grpFtp.Size = new System.Drawing.Size(471, 212);
            this.grpFtp.TabIndex = 1;
            this.grpFtp.TabStop = false;
            this.grpFtp.Text = "FTP SERVER (PROGRAMS / IMAGES)";

            this.txtFtpRootPath.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtFtpRootPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFtpRootPath.Font = new System.Drawing.Font("Consolas", 8F);
            this.txtFtpRootPath.ForeColor = System.Drawing.Color.LightGray;
            this.txtFtpRootPath.Location = new System.Drawing.Point(10, 22);
            this.txtFtpRootPath.Name = "txtFtpRootPath";
            this.txtFtpRootPath.ReadOnly = true;
            this.txtFtpRootPath.Size = new System.Drawing.Size(340, 20);
            this.txtFtpRootPath.TabIndex = 0;

            this.btnBrowseFtp.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnBrowseFtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseFtp.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBrowseFtp.ForeColor = System.Drawing.Color.White;
            this.btnBrowseFtp.Location = new System.Drawing.Point(355, 20);
            this.btnBrowseFtp.Name = "btnBrowseFtp";
            this.btnBrowseFtp.Size = new System.Drawing.Size(95, 24);
            this.btnBrowseFtp.TabIndex = 1;
            this.btnBrowseFtp.Text = "Má»Ÿ ThÆ° Má»¥c";
            this.btnBrowseFtp.UseVisualStyleBackColor = false;

            this.lstPrograms.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.lstPrograms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstPrograms.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.lstPrograms.ForeColor = System.Drawing.Color.White;
            this.lstPrograms.FormattingEnabled = true;
            this.lstPrograms.ItemHeight = 13;
            this.lstPrograms.Location = new System.Drawing.Point(10, 50);
            this.lstPrograms.Name = "lstPrograms";
            this.lstPrograms.Size = new System.Drawing.Size(340, 145);
            this.lstPrograms.TabIndex = 2;

            this.btnRefreshPrograms.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnRefreshPrograms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshPrograms.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshPrograms.ForeColor = System.Drawing.Color.White;
            this.btnRefreshPrograms.Location = new System.Drawing.Point(355, 50);
            this.btnRefreshPrograms.Name = "btnRefreshPrograms";
            this.btnRefreshPrograms.Size = new System.Drawing.Size(95, 26);
            this.btnRefreshPrograms.TabIndex = 3;
            this.btnRefreshPrograms.Text = "LĂ m Má»›i";
            this.btnRefreshPrograms.UseVisualStyleBackColor = false;

            // 
            // grpCameraLog
            // 
            this.grpCameraLog.Controls.Add(this.rtbCameraLog);
            this.grpCameraLog.Controls.Add(this.pnlCamLogTools);
            this.grpCameraLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCameraLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCameraLog.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpCameraLog.Location = new System.Drawing.Point(5, 220);
            this.grpCameraLog.Name = "grpCameraLog";
            this.grpCameraLog.Size = new System.Drawing.Size(891, 577);
            this.grpCameraLog.TabIndex = 1;
            this.grpCameraLog.TabStop = false;
            this.grpCameraLog.Text = "NHáº¬T KĂ TRAFFIC CAMERA KEYENCE (RAW DATA MONITOR)";

            this.pnlCamLogTools.Controls.Add(this.lblCamSentCount);
            this.pnlCamLogTools.Controls.Add(this.lblCamTriggerCount);
            this.pnlCamLogTools.Controls.Add(this.lblCamErrorCount);
            this.pnlCamLogTools.Controls.Add(this.chkCamAutoScroll);
            this.pnlCamLogTools.Controls.Add(this.btnCamClearLog);
            this.pnlCamLogTools.Controls.Add(this.btnCamSaveLog);
            this.pnlCamLogTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCamLogTools.Location = new System.Drawing.Point(3, 19);
            this.pnlCamLogTools.Name = "pnlCamLogTools";
            this.pnlCamLogTools.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCamLogTools.Size = new System.Drawing.Size(885, 32);
            this.pnlCamLogTools.TabIndex = 0;

            this.lblCamSentCount.AutoSize = true;
            this.lblCamSentCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCamSentCount.ForeColor = System.Drawing.Color.FromArgb(163, 190, 140);
            this.lblCamSentCount.Location = new System.Drawing.Point(10, 8);
            this.lblCamSentCount.Name = "lblCamSentCount";
            this.lblCamSentCount.Size = new System.Drawing.Size(61, 15);
            this.lblCamSentCount.TabIndex = 0;
            this.lblCamSentCount.Text = "Gá»­i: 0 gĂ³i";

            this.lblCamTriggerCount.AutoSize = true;
            this.lblCamTriggerCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCamTriggerCount.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.lblCamTriggerCount.Location = new System.Drawing.Point(110, 8);
            this.lblCamTriggerCount.Name = "lblCamTriggerCount";
            this.lblCamTriggerCount.Size = new System.Drawing.Size(59, 15);
            this.lblCamTriggerCount.TabIndex = 1;
            this.lblCamTriggerCount.Text = "Trigger: 0";

            this.lblCamErrorCount.AutoSize = true;
            this.lblCamErrorCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCamErrorCount.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.lblCamErrorCount.Location = new System.Drawing.Point(200, 8);
            this.lblCamErrorCount.Name = "lblCamErrorCount";
            this.lblCamErrorCount.Size = new System.Drawing.Size(59, 15);
            this.lblCamErrorCount.TabIndex = 2;
            this.lblCamErrorCount.Text = "Lá»—i NG: 0";

            this.chkCamAutoScroll.AutoSize = true;
            this.chkCamAutoScroll.Checked = true;
            this.chkCamAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCamAutoScroll.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkCamAutoScroll.ForeColor = System.Drawing.Color.White;
            this.chkCamAutoScroll.Location = new System.Drawing.Point(300, 7);
            this.chkCamAutoScroll.Name = "chkCamAutoScroll";
            this.chkCamAutoScroll.Size = new System.Drawing.Size(73, 19);
            this.chkCamAutoScroll.TabIndex = 3;
            this.chkCamAutoScroll.Text = "Tá»± cuá»™n";
            this.chkCamAutoScroll.UseVisualStyleBackColor = true;

            this.btnCamClearLog.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnCamClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamClearLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCamClearLog.ForeColor = System.Drawing.Color.White;
            this.btnCamClearLog.Location = new System.Drawing.Point(380, 4);
            this.btnCamClearLog.Name = "btnCamClearLog";
            this.btnCamClearLog.Size = new System.Drawing.Size(90, 24);
            this.btnCamClearLog.TabIndex = 4;
            this.btnCamClearLog.Text = "XĂ³a Nháº­t KĂ½";
            this.btnCamClearLog.UseVisualStyleBackColor = false;

            this.btnCamSaveLog.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnCamSaveLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamSaveLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCamSaveLog.ForeColor = System.Drawing.Color.White;
            this.btnCamSaveLog.Location = new System.Drawing.Point(475, 4);
            this.btnCamSaveLog.Name = "btnCamSaveLog";
            this.btnCamSaveLog.Size = new System.Drawing.Size(90, 24);
            this.btnCamSaveLog.TabIndex = 5;
            this.btnCamSaveLog.Text = "LÆ°u File Log";
            this.btnCamSaveLog.UseVisualStyleBackColor = false;

            this.rtbCameraLog.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.rtbCameraLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCameraLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCameraLog.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.rtbCameraLog.ForeColor = System.Drawing.Color.LightGreen;
            this.rtbCameraLog.Location = new System.Drawing.Point(3, 51);
            this.rtbCameraLog.Name = "rtbCameraLog";
            this.rtbCameraLog.ReadOnly = true;
            this.rtbCameraLog.Size = new System.Drawing.Size(885, 523);
            this.rtbCameraLog.TabIndex = 1;
            this.rtbCameraLog.Text = "";

            // 
            // tabPrinter
            // 
            this.tabPrinter.BackColor = System.Drawing.Color.FromArgb(30, 30, 46);
            this.tabPrinter.Controls.Add(this.pnlPrnRight);
            this.tabPrinter.Controls.Add(this.pnlPrnLeft);
            this.tabPrinter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPrinter.Location = new System.Drawing.Point(4, 40);
            this.tabPrinter.Name = "tabPrinter";
            this.tabPrinter.Padding = new System.Windows.Forms.Padding(10);
            this.tabPrinter.Size = new System.Drawing.Size(1376, 817);
            this.tabPrinter.TabIndex = 1;
            this.tabPrinter.Text = "đŸ–¨ï¸ MĂ¡y In POD / TIJ (Port 1997 / 2030)";

            // 
            // pnlPrnLeft
            // 
            this.pnlPrnLeft.AutoScroll = true;
            this.pnlPrnLeft.Controls.Add(this.grpPrinterAlarms);
            this.pnlPrnLeft.Controls.Add(this.grpPrinterControl);
            this.pnlPrnLeft.Controls.Add(this.grpPrinterServer);
            this.pnlPrnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPrnLeft.Location = new System.Drawing.Point(10, 10);
            this.pnlPrnLeft.Name = "pnlPrnLeft";
            this.pnlPrnLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlPrnLeft.Size = new System.Drawing.Size(430, 797);
            this.pnlPrnLeft.TabIndex = 0;

            // 
            // grpPrinterServer
            // 
            this.grpPrinterServer.Controls.Add(this.lblPrnIp);
            this.grpPrinterServer.Controls.Add(this.txtPrnServerIp);
            this.grpPrinterServer.Controls.Add(this.lblPrnPort);
            this.grpPrinterServer.Controls.Add(this.nudPrnPort);
            this.grpPrinterServer.Controls.Add(this.btnPrnServerToggle);
            this.grpPrinterServer.Controls.Add(this.pnlPrnStatus);
            this.grpPrinterServer.Controls.Add(this.lblPrnStatus);
            this.grpPrinterServer.Controls.Add(this.lblPrnClient);
            this.grpPrinterServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPrinterServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPrinterServer.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpPrinterServer.Location = new System.Drawing.Point(0, 0);
            this.grpPrinterServer.Name = "grpPrinterServer";
            this.grpPrinterServer.Padding = new System.Windows.Forms.Padding(8);
            this.grpPrinterServer.Size = new System.Drawing.Size(420, 115);
            this.grpPrinterServer.TabIndex = 0;
            this.grpPrinterServer.TabStop = false;
            this.grpPrinterServer.Text = "Cáº¤U HĂŒNH TCP SERVER MĂY IN (POD / TIJ)";

            this.lblPrnIp.AutoSize = true;
            this.lblPrnIp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPrnIp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPrnIp.Location = new System.Drawing.Point(12, 25);
            this.lblPrnIp.Name = "lblPrnIp";
            this.lblPrnIp.Size = new System.Drawing.Size(81, 15);
            this.lblPrnIp.TabIndex = 0;
            this.lblPrnIp.Text = "IP Láº¯ng Nghe:";

            this.txtPrnServerIp.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtPrnServerIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrnServerIp.ForeColor = System.Drawing.Color.White;
            this.txtPrnServerIp.Location = new System.Drawing.Point(95, 22);
            this.txtPrnServerIp.Name = "txtPrnServerIp";
            this.txtPrnServerIp.Size = new System.Drawing.Size(110, 23);
            this.txtPrnServerIp.TabIndex = 1;
            this.txtPrnServerIp.Text = "0.0.0.0";

            this.lblPrnPort.AutoSize = true;
            this.lblPrnPort.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPrnPort.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPrnPort.Location = new System.Drawing.Point(220, 25);
            this.lblPrnPort.Name = "lblPrnPort";
            this.lblPrnPort.Size = new System.Drawing.Size(56, 15);
            this.lblPrnPort.TabIndex = 2;
            this.lblPrnPort.Text = "TCP Port:";

            this.nudPrnPort.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.nudPrnPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudPrnPort.ForeColor = System.Drawing.Color.White;
            this.nudPrnPort.Location = new System.Drawing.Point(285, 22);
            this.nudPrnPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.nudPrnPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudPrnPort.Name = "nudPrnPort";
            this.nudPrnPort.Size = new System.Drawing.Size(80, 23);
            this.nudPrnPort.TabIndex = 3;
            this.nudPrnPort.Value = new decimal(new int[] { 1997, 0, 0, 0 });

            this.btnPrnServerToggle.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.btnPrnServerToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnServerToggle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPrnServerToggle.ForeColor = System.Drawing.Color.White;
            this.btnPrnServerToggle.Location = new System.Drawing.Point(12, 52);
            this.btnPrnServerToggle.Name = "btnPrnServerToggle";
            this.btnPrnServerToggle.Size = new System.Drawing.Size(220, 30);
            this.btnPrnServerToggle.TabIndex = 4;
            this.btnPrnServerToggle.Text = "KHá»I Äá»˜NG SERVER MĂY IN";
            this.btnPrnServerToggle.UseVisualStyleBackColor = false;
            this.btnPrnServerToggle.Click += new System.EventHandler(this.btnPrnServerToggle_Click);

            this.pnlPrnStatus.BackColor = System.Drawing.Color.Red;
            this.pnlPrnStatus.Location = new System.Drawing.Point(242, 58);
            this.pnlPrnStatus.Name = "pnlPrnStatus";
            this.pnlPrnStatus.Size = new System.Drawing.Size(16, 16);
            this.pnlPrnStatus.TabIndex = 5;

            this.lblPrnStatus.AutoSize = true;
            this.lblPrnStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPrnStatus.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.lblPrnStatus.Location = new System.Drawing.Point(265, 58);
            this.lblPrnStatus.Name = "lblPrnStatus";
            this.lblPrnStatus.Size = new System.Drawing.Size(107, 15);
            this.lblPrnStatus.TabIndex = 6;
            this.lblPrnStatus.Text = "Server: ChÆ°a cháº¡y";

            this.lblPrnClient.AutoSize = true;
            this.lblPrnClient.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPrnClient.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblPrnClient.Location = new System.Drawing.Point(12, 88);
            this.lblPrnClient.Name = "lblPrnClient";
            this.lblPrnClient.Size = new System.Drawing.Size(126, 13);
            this.lblPrnClient.TabIndex = 7;
            this.lblPrnClient.Text = "Client R-Link: 0 káº¿t ná»‘i";

            // 
            // grpPrinterControl
            // 
            this.grpPrinterControl.Controls.Add(this.lblPrnState);
            this.grpPrinterControl.Controls.Add(this.cboPrnState);
            this.grpPrinterControl.Controls.Add(this.lblPrnSpeed);
            this.grpPrinterControl.Controls.Add(this.trackPrnSpeed);
            this.grpPrinterControl.Controls.Add(this.nudPrnSpeed);
            this.grpPrinterControl.Controls.Add(this.lblPrnTemplate);
            this.grpPrinterControl.Controls.Add(this.txtPrnTemplate);
            this.grpPrinterControl.Controls.Add(this.chkPrnAutoAck);
            this.grpPrinterControl.Controls.Add(this.chkPrnAutoPrint);
            this.grpPrinterControl.Controls.Add(this.chkPrnInterlockCamera);
            this.grpPrinterControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPrinterControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPrinterControl.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpPrinterControl.Location = new System.Drawing.Point(0, 115);
            this.grpPrinterControl.Name = "grpPrinterControl";
            this.grpPrinterControl.Padding = new System.Windows.Forms.Padding(8);
            this.grpPrinterControl.Size = new System.Drawing.Size(420, 230);
            this.grpPrinterControl.TabIndex = 1;
            this.grpPrinterControl.TabStop = false;
            this.grpPrinterControl.Text = "ÄIá»€U KHIá»‚N & TRáº NG THĂI MĂY IN (MON/STAR/STOP)";

            this.lblPrnState.AutoSize = true;
            this.lblPrnState.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPrnState.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPrnState.Location = new System.Drawing.Point(12, 25);
            this.lblPrnState.Name = "lblPrnState";
            this.lblPrnState.Size = new System.Drawing.Size(107, 15);
            this.lblPrnState.TabIndex = 0;
            this.lblPrnState.Text = "Tráº¡ng thĂ¡i mĂ¡y in:";

            this.cboPrnState.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.cboPrnState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrnState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPrnState.ForeColor = System.Drawing.Color.White;
            this.cboPrnState.FormattingEnabled = true;
            this.cboPrnState.Items.AddRange(new object[] {
            "Ready",
            "Printing",
            "Stop",
            "Processing",
            "Error",
            "Disable"});
            this.cboPrnState.Location = new System.Drawing.Point(130, 22);
            this.cboPrnState.Name = "cboPrnState";
            this.cboPrnState.Size = new System.Drawing.Size(150, 23);
            this.cboPrnState.TabIndex = 1;
            this.cboPrnState.SelectedIndexChanged += new System.EventHandler(this.cboPrnState_SelectedIndexChanged);

            this.lblPrnSpeed.AutoSize = true;
            this.lblPrnSpeed.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPrnSpeed.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPrnSpeed.Location = new System.Drawing.Point(12, 56);
            this.lblPrnSpeed.Name = "lblPrnSpeed";
            this.lblPrnSpeed.Size = new System.Drawing.Size(95, 15);
            this.lblPrnSpeed.TabIndex = 2;
            this.lblPrnSpeed.Text = "Tá»‘c Ä‘á»™ in (PPM):";

            this.trackPrnSpeed.Location = new System.Drawing.Point(125, 52);
            this.trackPrnSpeed.Maximum = 800;
            this.trackPrnSpeed.Minimum = 20;
            this.trackPrnSpeed.Name = "trackPrnSpeed";
            this.trackPrnSpeed.Size = new System.Drawing.Size(200, 45);
            this.trackPrnSpeed.TabIndex = 3;
            this.trackPrnSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackPrnSpeed.Value = 300;

            this.nudPrnSpeed.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.nudPrnSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudPrnSpeed.ForeColor = System.Drawing.Color.White;
            this.nudPrnSpeed.Location = new System.Drawing.Point(330, 54);
            this.nudPrnSpeed.Maximum = new decimal(new int[] { 800, 0, 0, 0 });
            this.nudPrnSpeed.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudPrnSpeed.Name = "nudPrnSpeed";
            this.nudPrnSpeed.Size = new System.Drawing.Size(65, 23);
            this.nudPrnSpeed.TabIndex = 4;
            this.nudPrnSpeed.Value = new decimal(new int[] { 300, 0, 0, 0 });

            this.lblPrnTemplate.AutoSize = true;
            this.lblPrnTemplate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPrnTemplate.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPrnTemplate.Location = new System.Drawing.Point(12, 88);
            this.lblPrnTemplate.Name = "lblPrnTemplate";
            this.lblPrnTemplate.Size = new System.Drawing.Size(107, 15);
            this.lblPrnTemplate.TabIndex = 5;
            this.lblPrnTemplate.Text = "Template hiá»‡n táº¡i:";

            this.txtPrnTemplate.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.txtPrnTemplate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrnTemplate.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.txtPrnTemplate.ForeColor = System.Drawing.Color.White;
            this.txtPrnTemplate.Location = new System.Drawing.Point(130, 85);
            this.txtPrnTemplate.Name = "txtPrnTemplate";
            this.txtPrnTemplate.Size = new System.Drawing.Size(265, 21);
            this.txtPrnTemplate.TabIndex = 6;
            this.txtPrnTemplate.Text = "TH_MILK_180ML_STD";

            this.chkPrnAutoAck.AutoSize = true;
            this.chkPrnAutoAck.Checked = true;
            this.chkPrnAutoAck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrnAutoAck.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkPrnAutoAck.ForeColor = System.Drawing.Color.White;
            this.chkPrnAutoAck.Location = new System.Drawing.Point(12, 120);
            this.chkPrnAutoAck.Name = "chkPrnAutoAck";
            this.chkPrnAutoAck.Size = new System.Drawing.Size(315, 19);
            this.chkPrnAutoAck.TabIndex = 7;
            this.chkPrnAutoAck.Text = "Tá»± Ä‘á»™ng pháº£n há»“i ACK khi nháº­n DATA (DATA;RYES)";
            this.chkPrnAutoAck.UseVisualStyleBackColor = true;

            this.chkPrnAutoPrint.AutoSize = true;
            this.chkPrnAutoPrint.Checked = true;
            this.chkPrnAutoPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrnAutoPrint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.chkPrnAutoPrint.ForeColor = System.Drawing.Color.FromArgb(163, 190, 140);
            this.chkPrnAutoPrint.Location = new System.Drawing.Point(12, 145);
            this.chkPrnAutoPrint.Name = "chkPrnAutoPrint";
            this.chkPrnAutoPrint.Size = new System.Drawing.Size(325, 19);
            this.chkPrnAutoPrint.TabIndex = 8;
            this.chkPrnAutoPrint.Text = "Tá»± Ä‘á»™ng in theo chu ká»³ tá»‘c Ä‘á»™ (Auto Stream RSFP)";
            this.chkPrnAutoPrint.UseVisualStyleBackColor = true;

            this.chkPrnInterlockCamera.AutoSize = true;
            this.chkPrnInterlockCamera.Checked = true;
            this.chkPrnInterlockCamera.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrnInterlockCamera.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.chkPrnInterlockCamera.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.chkPrnInterlockCamera.Location = new System.Drawing.Point(12, 170);
            this.chkPrnInterlockCamera.Name = "chkPrnInterlockCamera";
            this.chkPrnInterlockCamera.Size = new System.Drawing.Size(387, 19);
            this.chkPrnInterlockCamera.TabIndex = 9;
            this.chkPrnInterlockCamera.Text = "â¡ Tá»± Ä‘á»™ng liĂªn káº¿t Ä‘áº©y mĂ£ Ä‘Ă£ in sang Camera Keyence";
            this.chkPrnInterlockCamera.UseVisualStyleBackColor = true;

            // 
            // grpPrinterAlarms
            // 
            this.grpPrinterAlarms.Controls.Add(this.lblRsalCode);
            this.grpPrinterAlarms.Controls.Add(this.cboRsalCode);
            this.grpPrinterAlarms.Controls.Add(this.btnSendRsal);
            this.grpPrinterAlarms.Controls.Add(this.lblStarResponse);
            this.grpPrinterAlarms.Controls.Add(this.cboStarResponse);
            this.grpPrinterAlarms.Controls.Add(this.btnSendPlc001);
            this.grpPrinterAlarms.Controls.Add(this.btnPrnSimulateDrop);
            this.grpPrinterAlarms.Controls.Add(this.btnPrnTestSyncTime);
            this.grpPrinterAlarms.Controls.Add(this.btnPrnTestRollTime);
            this.grpPrinterAlarms.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPrinterAlarms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPrinterAlarms.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.grpPrinterAlarms.Location = new System.Drawing.Point(0, 345);
            this.grpPrinterAlarms.Name = "grpPrinterAlarms";
            this.grpPrinterAlarms.Padding = new System.Windows.Forms.Padding(8);
            this.grpPrinterAlarms.Size = new System.Drawing.Size(420, 240);
            this.grpPrinterAlarms.TabIndex = 2;
            this.grpPrinterAlarms.TabStop = false;
            this.grpPrinterAlarms.Text = "Báº¢NG TIĂM Lá»–I MĂY IN (RSAL / STAR / TIMEOUT)";

            this.lblRsalCode.AutoSize = true;
            this.lblRsalCode.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRsalCode.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblRsalCode.Location = new System.Drawing.Point(12, 25);
            this.lblRsalCode.Name = "lblRsalCode";
            this.lblRsalCode.Size = new System.Drawing.Size(140, 15);
            this.lblRsalCode.TabIndex = 0;
            this.lblRsalCode.Text = "Chá»n mĂ£ cáº£nh bĂ¡o RSAL:";

            this.cboRsalCode.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.cboRsalCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRsalCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRsalCode.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cboRsalCode.ForeColor = System.Drawing.Color.White;
            this.cboRsalCode.FormattingEnabled = true;
            this.cboRsalCode.Items.AddRange(new object[] {
            "001 - Over Speed Error (QuĂ¡ tá»‘c Ä‘á»™)",
            "002 - Receive Data Timeout (Háº¿t giá» nháº­n)",
            "003 - Delay Data Error (Trá»… dá»¯ liá»‡u)",
            "004 - No Cartridge (KhĂ´ng cĂ³ há»™p má»±c)",
            "005 - Invalid Cartridge (Há»™p má»±c khĂ´ng há»£p lá»‡)",
            "006 - Lock Cartridge (Há»™p má»±c bá»‹ khĂ³a)",
            "007 - Ink Out (Háº¿t má»±c)",
            "008 - Ink Low (Má»±c sáº¯p háº¿t - Warning)",
            "009 - Empty Data (Dá»¯ liá»‡u in trá»‘ng)",
            "010 - Empty Buffer (Buffer mĂ¡y in trá»‘ng)",
            "011 - Waiting Data (MĂ¡y in Ä‘ang chá» dá»¯ liá»‡u)",
            "100 - Air Tank Overflow (Má»±c trĂ n bĂ¬nh khĂ­)",
            "101 - Sub Tank Ink High Level (Má»±c sub tank cao)",
            "104 - Pressure Sensor Error (Lá»—i Ă¡p suáº¥t ISS)",
            "106 - Main Ink Tank Low (BĂ¬nh má»±c chĂ­nh sáº¯p háº¿t)",
            "201 - Emergency Stop (Dá»«ng kháº©n cáº¥p)",
            "203 - Safety Interlock Open (Cá»­a an toĂ n má»Ÿ)",
            "205 - Power Failure (Máº¥t nguá»“n Ä‘iá»‡n)"});
            this.cboRsalCode.Location = new System.Drawing.Point(12, 45);
            this.cboRsalCode.Name = "cboRsalCode";
            this.cboRsalCode.Size = new System.Drawing.Size(280, 21);
            this.cboRsalCode.TabIndex = 1;

            this.btnSendRsal.BackColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.btnSendRsal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendRsal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnSendRsal.ForeColor = System.Drawing.Color.White;
            this.btnSendRsal.Location = new System.Drawing.Point(298, 43);
            this.btnSendRsal.Name = "btnSendRsal";
            this.btnSendRsal.Size = new System.Drawing.Size(115, 26);
            this.btnSendRsal.TabIndex = 2;
            this.btnSendRsal.Text = "đŸ¨ TIĂM Lá»–I RSAL";
            this.btnSendRsal.UseVisualStyleBackColor = false;
            this.btnSendRsal.Click += new System.EventHandler(this.btnSendRsal_Click);

            this.lblStarResponse.AutoSize = true;
            this.lblStarResponse.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStarResponse.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblStarResponse.Location = new System.Drawing.Point(12, 80);
            this.lblStarResponse.Name = "lblStarResponse";
            this.lblStarResponse.Size = new System.Drawing.Size(119, 15);
            this.lblStarResponse.TabIndex = 3;
            this.lblStarResponse.Text = "Pháº£n há»“i lá»‡nh STAR:";

            this.cboStarResponse.BackColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.cboStarResponse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStarResponse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStarResponse.ForeColor = System.Drawing.Color.White;
            this.cboStarResponse.FormattingEnabled = true;
            this.cboStarResponse.Items.AddRange(new object[] {
            "STAR;OK (BĂ¬nh thÆ°á»ng)",
            "STAR;READY",
            "STAR;ERR;005 (Äáº§u in máº¥t káº¿t ná»‘i)",
            "STAR;ERR;007 (KhĂ´ng cĂ³ há»™p má»±c)",
            "STAR;ERR;009 (Háº¿t má»±c)",
            "STAR;ERR;015 (Má»±c tháº¥p)",
            "STAR;ERR;018 (Xung Ä‘á»™t há»™p má»±c)"});
            this.cboStarResponse.Location = new System.Drawing.Point(140, 77);
            this.cboStarResponse.Name = "cboStarResponse";
            this.cboStarResponse.Size = new System.Drawing.Size(152, 23);
            this.cboStarResponse.TabIndex = 4;

            this.btnSendPlc001.BackColor = System.Drawing.Color.FromArgb(94, 129, 172);
            this.btnSendPlc001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendPlc001.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnSendPlc001.ForeColor = System.Drawing.Color.White;
            this.btnSendPlc001.Location = new System.Drawing.Point(12, 115);
            this.btnSendPlc001.Name = "btnSendPlc001";
            this.btnSendPlc001.Size = new System.Drawing.Size(195, 28);
            this.btnSendPlc001.TabIndex = 5;
            this.btnSendPlc001.Text = "â¡ Báº®N XUNG PLC001 TRIGGER";
            this.btnSendPlc001.UseVisualStyleBackColor = false;
            this.btnSendPlc001.Click += new System.EventHandler(this.btnSendPlc001_Click);

            this.btnPrnSimulateDrop.BackColor = System.Drawing.Color.FromArgb(180, 70, 70);
            this.btnPrnSimulateDrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnSimulateDrop.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnPrnSimulateDrop.ForeColor = System.Drawing.Color.White;
            this.btnPrnSimulateDrop.Location = new System.Drawing.Point(215, 115);
            this.btnPrnSimulateDrop.Name = "btnPrnSimulateDrop";
            this.btnPrnSimulateDrop.Size = new System.Drawing.Size(198, 28);
            this.btnPrnSimulateDrop.TabIndex = 6;
            this.btnPrnSimulateDrop.Text = "đŸ”Œ MĂ” PHá»NG Rá»T Máº NG TCP";
            this.btnPrnSimulateDrop.UseVisualStyleBackColor = false;
            this.btnPrnSimulateDrop.Click += new System.EventHandler(this.btnPrnSimulateDrop_Click);

            this.btnPrnTestSyncTime.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnPrnTestSyncTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnTestSyncTime.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnTestSyncTime.ForeColor = System.Drawing.Color.White;
            this.btnPrnTestSyncTime.Location = new System.Drawing.Point(12, 150);
            this.btnPrnTestSyncTime.Name = "btnPrnTestSyncTime";
            this.btnPrnTestSyncTime.Size = new System.Drawing.Size(195, 26);
            this.btnPrnTestSyncTime.TabIndex = 7;
            this.btnPrnTestSyncTime.Text = "â±ï¸ Test Gá»­i SET;RYES";
            this.btnPrnTestSyncTime.UseVisualStyleBackColor = false;
            this.btnPrnTestSyncTime.Click += new System.EventHandler(this.btnPrnTestSyncTime_Click);

            this.btnPrnTestRollTime.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnPrnTestRollTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnTestRollTime.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnTestRollTime.ForeColor = System.Drawing.Color.White;
            this.btnPrnTestRollTime.Location = new System.Drawing.Point(215, 150);
            this.btnPrnTestRollTime.Name = "btnPrnTestRollTime";
            this.btnPrnTestRollTime.Size = new System.Drawing.Size(198, 26);
            this.btnPrnTestRollTime.TabIndex = 8;
            this.btnPrnTestRollTime.Text = "đŸ”„ Test Gá»­i RTIME;OK";
            this.btnPrnTestRollTime.UseVisualStyleBackColor = false;
            this.btnPrnTestRollTime.Click += new System.EventHandler(this.btnPrnTestRollTime_Click);

            // 
            // pnlPrnRight
            // 
            this.pnlPrnRight.Controls.Add(this.grpPrinterLog);
            this.pnlPrnRight.Controls.Add(this.grpPrintBuffer);
            this.pnlPrnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPrnRight.Location = new System.Drawing.Point(440, 10);
            this.pnlPrnRight.Name = "pnlPrnRight";
            this.pnlPrnRight.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlPrnRight.Size = new System.Drawing.Size(926, 797);
            this.pnlPrnRight.TabIndex = 1;

            // 
            // grpPrintBuffer
            // 
            this.grpPrintBuffer.Controls.Add(this.dgvPrintBuffer);
            this.grpPrintBuffer.Controls.Add(this.prgPrnBuffer);
            this.grpPrintBuffer.Controls.Add(this.pnlBufferStats);
            this.grpPrintBuffer.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPrintBuffer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPrintBuffer.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpPrintBuffer.Location = new System.Drawing.Point(5, 0);
            this.grpPrintBuffer.Name = "grpPrintBuffer";
            this.grpPrintBuffer.Padding = new System.Windows.Forms.Padding(8);
            this.grpPrintBuffer.Size = new System.Drawing.Size(921, 270);
            this.grpPrintBuffer.TabIndex = 0;
            this.grpPrintBuffer.TabStop = false;
            this.grpPrintBuffer.Text = "Bá»˜ Äá»†M Dá»® LIá»†U IN Tá»ª R-LINK (PRINT BUFFER & FIFO QUEUE)";

            this.pnlBufferStats.Controls.Add(this.lblPrnTotalRecv);
            this.pnlBufferStats.Controls.Add(this.lblPrnTotalPrinted);
            this.pnlBufferStats.Controls.Add(this.lblPrnBufferCount);
            this.pnlBufferStats.Controls.Add(this.btnPrnPrintNext);
            this.pnlBufferStats.Controls.Add(this.btnPrnClearBuffer);
            this.pnlBufferStats.Controls.Add(this.btnPrnResetStats);
            this.pnlBufferStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBufferStats.Location = new System.Drawing.Point(8, 24);
            this.pnlBufferStats.Name = "pnlBufferStats";
            this.pnlBufferStats.Padding = new System.Windows.Forms.Padding(4);
            this.pnlBufferStats.Size = new System.Drawing.Size(905, 34);
            this.pnlBufferStats.TabIndex = 0;

            this.lblPrnTotalRecv.AutoSize = true;
            this.lblPrnTotalRecv.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPrnTotalRecv.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.lblPrnTotalRecv.Location = new System.Drawing.Point(8, 8);
            this.lblPrnTotalRecv.Name = "lblPrnTotalRecv";
            this.lblPrnTotalRecv.Size = new System.Drawing.Size(81, 15);
            this.lblPrnTotalRecv.TabIndex = 0;
            this.lblPrnTotalRecv.Text = "Tá»•ng nháº­n: 0";

            this.lblPrnTotalPrinted.AutoSize = true;
            this.lblPrnTotalPrinted.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPrnTotalPrinted.ForeColor = System.Drawing.Color.FromArgb(163, 190, 140);
            this.lblPrnTotalPrinted.Location = new System.Drawing.Point(130, 8);
            this.lblPrnTotalPrinted.Name = "lblPrnTotalPrinted";
            this.lblPrnTotalPrinted.Size = new System.Drawing.Size(51, 15);
            this.lblPrnTotalPrinted.TabIndex = 1;
            this.lblPrnTotalPrinted.Text = "ÄĂ£ in: 0";

            this.lblPrnBufferCount.AutoSize = true;
            this.lblPrnBufferCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPrnBufferCount.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblPrnBufferCount.Location = new System.Drawing.Point(230, 8);
            this.lblPrnBufferCount.Name = "lblPrnBufferCount";
            this.lblPrnBufferCount.Size = new System.Drawing.Size(117, 15);
            this.lblPrnBufferCount.TabIndex = 2;
            this.lblPrnBufferCount.Text = "Chá» trong Buffer: 0";

            this.btnPrnPrintNext.BackColor = System.Drawing.Color.FromArgb(94, 129, 172);
            this.btnPrnPrintNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnPrintNext.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnPrnPrintNext.ForeColor = System.Drawing.Color.White;
            this.btnPrnPrintNext.Location = new System.Drawing.Point(410, 4);
            this.btnPrnPrintNext.Name = "btnPrnPrintNext";
            this.btnPrnPrintNext.Size = new System.Drawing.Size(150, 26);
            this.btnPrnPrintNext.TabIndex = 3;
            this.btnPrnPrintNext.Text = "â–¶ Báº¯n 1 MĂ£ (Print Next)";
            this.btnPrnPrintNext.UseVisualStyleBackColor = false;
            this.btnPrnPrintNext.Click += new System.EventHandler(this.btnPrnPrintNext_Click);

            this.btnPrnClearBuffer.BackColor = System.Drawing.Color.FromArgb(76, 86, 106);
            this.btnPrnClearBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnClearBuffer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnClearBuffer.ForeColor = System.Drawing.Color.White;
            this.btnPrnClearBuffer.Location = new System.Drawing.Point(570, 4);
            this.btnPrnClearBuffer.Name = "btnPrnClearBuffer";
            this.btnPrnClearBuffer.Size = new System.Drawing.Size(140, 26);
            this.btnPrnClearBuffer.TabIndex = 4;
            this.btnPrnClearBuffer.Text = "XĂ³a HĂ ng Äá»£i (CLPB)";
            this.btnPrnClearBuffer.UseVisualStyleBackColor = false;
            this.btnPrnClearBuffer.Click += new System.EventHandler(this.btnPrnClearBuffer_Click);

            this.btnPrnResetStats.BackColor = System.Drawing.Color.FromArgb(76, 86, 106);
            this.btnPrnResetStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnResetStats.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnResetStats.ForeColor = System.Drawing.Color.White;
            this.btnPrnResetStats.Location = new System.Drawing.Point(720, 4);
            this.btnPrnResetStats.Name = "btnPrnResetStats";
            this.btnPrnResetStats.Size = new System.Drawing.Size(90, 26);
            this.btnPrnResetStats.TabIndex = 5;
            this.btnPrnResetStats.Text = "Reset Äáº¿m";
            this.btnPrnResetStats.UseVisualStyleBackColor = false;

            this.prgPrnBuffer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prgPrnBuffer.Location = new System.Drawing.Point(8, 252);
            this.prgPrnBuffer.Name = "prgPrnBuffer";
            this.prgPrnBuffer.Size = new System.Drawing.Size(905, 10);
            this.prgPrnBuffer.TabIndex = 1;

            this.dgvPrintBuffer.AllowUserToAddRows = false;
            this.dgvPrintBuffer.AllowUserToDeleteRows = false;
            this.dgvPrintBuffer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrintBuffer.BackgroundColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.dgvPrintBuffer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrintBuffer.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPrintBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrintBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrintBuffer.EnableHeadersVisualStyles = false;
            this.dgvPrintBuffer.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.dgvPrintBuffer.GridColor = System.Drawing.Color.FromArgb(40, 40, 56);
            this.dgvPrintBuffer.Location = new System.Drawing.Point(8, 58);
            this.dgvPrintBuffer.Name = "dgvPrintBuffer";
            this.dgvPrintBuffer.ReadOnly = true;
            this.dgvPrintBuffer.RowHeadersVisible = false;
            this.dgvPrintBuffer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrintBuffer.Size = new System.Drawing.Size(905, 194);
            this.dgvPrintBuffer.TabIndex = 2;

            // 
            // grpPrinterLog
            // 
            this.grpPrinterLog.Controls.Add(this.rtbPrinterLog);
            this.grpPrinterLog.Controls.Add(this.pnlPrnLogTools);
            this.grpPrinterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPrinterLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPrinterLog.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpPrinterLog.Location = new System.Drawing.Point(5, 270);
            this.grpPrinterLog.Name = "grpPrinterLog";
            this.grpPrinterLog.Size = new System.Drawing.Size(921, 527);
            this.grpPrinterLog.TabIndex = 1;
            this.grpPrinterLog.TabStop = false;
            this.grpPrinterLog.Text = "NHáº¬T KĂ TRAFFIC MĂY IN (RAW PACKET INSPECTOR [STX]...[ETX])";

            this.pnlPrnLogTools.Controls.Add(this.chkPrnAutoScroll);
            this.pnlPrnLogTools.Controls.Add(this.btnPrnClearLog);
            this.pnlPrnLogTools.Controls.Add(this.btnPrnSaveLog);
            this.pnlPrnLogTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPrnLogTools.Location = new System.Drawing.Point(3, 19);
            this.pnlPrnLogTools.Name = "pnlPrnLogTools";
            this.pnlPrnLogTools.Padding = new System.Windows.Forms.Padding(5);
            this.pnlPrnLogTools.Size = new System.Drawing.Size(915, 32);
            this.pnlPrnLogTools.TabIndex = 0;

            this.chkPrnAutoScroll.AutoSize = true;
            this.chkPrnAutoScroll.Checked = true;
            this.chkPrnAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrnAutoScroll.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkPrnAutoScroll.ForeColor = System.Drawing.Color.White;
            this.chkPrnAutoScroll.Location = new System.Drawing.Point(10, 7);
            this.chkPrnAutoScroll.Name = "chkPrnAutoScroll";
            this.chkPrnAutoScroll.Size = new System.Drawing.Size(73, 19);
            this.chkPrnAutoScroll.TabIndex = 0;
            this.chkPrnAutoScroll.Text = "Tá»± cuá»™n";
            this.chkPrnAutoScroll.UseVisualStyleBackColor = true;

            this.btnPrnClearLog.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnPrnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnClearLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnPrnClearLog.Location = new System.Drawing.Point(90, 4);
            this.btnPrnClearLog.Name = "btnPrnClearLog";
            this.btnPrnClearLog.Size = new System.Drawing.Size(90, 24);
            this.btnPrnClearLog.TabIndex = 1;
            this.btnPrnClearLog.Text = "XĂ³a Nháº­t KĂ½";
            this.btnPrnClearLog.UseVisualStyleBackColor = false;

            this.btnPrnSaveLog.BackColor = System.Drawing.Color.FromArgb(67, 76, 94);
            this.btnPrnSaveLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrnSaveLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPrnSaveLog.ForeColor = System.Drawing.Color.White;
            this.btnPrnSaveLog.Location = new System.Drawing.Point(190, 4);
            this.btnPrnSaveLog.Name = "btnPrnSaveLog";
            this.btnPrnSaveLog.Size = new System.Drawing.Size(90, 24);
            this.btnPrnSaveLog.TabIndex = 2;
            this.btnPrnSaveLog.Text = "LÆ°u File Log";
            this.btnPrnSaveLog.UseVisualStyleBackColor = false;

            this.rtbPrinterLog.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.rtbPrinterLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPrinterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPrinterLog.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.rtbPrinterLog.ForeColor = System.Drawing.Color.LightGreen;
            this.rtbPrinterLog.Location = new System.Drawing.Point(3, 51);
            this.rtbPrinterLog.Name = "rtbPrinterLog";
            this.rtbPrinterLog.ReadOnly = true;
            this.rtbPrinterLog.Size = new System.Drawing.Size(915, 473);
            this.rtbPrinterLog.TabIndex = 1;
            this.rtbPrinterLog.Text = "";

            // 
            // tabInterlock
            // 
            this.tabInterlock.BackColor = System.Drawing.Color.FromArgb(30, 30, 46);
            this.tabInterlock.Controls.Add(this.grpInterlockStats);
            this.tabInterlock.Controls.Add(this.grpInterlockOverview);
            this.tabInterlock.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabInterlock.Location = new System.Drawing.Point(4, 40);
            this.tabInterlock.Name = "tabInterlock";
            this.tabInterlock.Padding = new System.Windows.Forms.Padding(15);
            this.tabInterlock.Size = new System.Drawing.Size(1376, 817);
            this.tabInterlock.TabIndex = 2;
            this.tabInterlock.Text = "â¡ DĂ¢y Chuyá»n TĂ­ch Há»£p (Line Interlock)";

            // 
            // grpInterlockOverview
            // 
            this.grpInterlockOverview.Controls.Add(this.lblInterlockTitle);
            this.grpInterlockOverview.Controls.Add(this.lblInterlockDesc);
            this.grpInterlockOverview.Controls.Add(this.btnStartFullLineSim);
            this.grpInterlockOverview.Controls.Add(this.btnStopFullLineSim);
            this.grpInterlockOverview.Controls.Add(this.lblLineSimStatus);
            this.grpInterlockOverview.Controls.Add(this.prgLineSim);
            this.grpInterlockOverview.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInterlockOverview.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpInterlockOverview.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpInterlockOverview.Location = new System.Drawing.Point(15, 15);
            this.grpInterlockOverview.Name = "grpInterlockOverview";
            this.grpInterlockOverview.Padding = new System.Windows.Forms.Padding(12);
            this.grpInterlockOverview.Size = new System.Drawing.Size(1346, 180);
            this.grpInterlockOverview.TabIndex = 0;
            this.grpInterlockOverview.TabStop = false;
            this.grpInterlockOverview.Text = "MĂ” PHá»NG DĂ‚Y CHUYá»€N Tá»”NG THá»‚ (CLOSED-LOOP VERIFY & PRINT SIMULATION)";

            this.lblInterlockTitle.AutoSize = true;
            this.lblInterlockTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInterlockTitle.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblInterlockTitle.Location = new System.Drawing.Point(15, 25);
            this.lblInterlockTitle.Name = "lblInterlockTitle";
            this.lblInterlockTitle.Size = new System.Drawing.Size(787, 17);
            this.lblInterlockTitle.TabIndex = 0;
            this.lblInterlockTitle.Text = "â¡ LUá»’NG LIĂN Há»¢P: R-Link [DATA] â” MĂ¡y In [RSFP] â” BÄƒng Táº£i Cháº¡y â” Camera Keyence [OCR/Barcode] â” R-Link So Khá»›p";

            this.lblInterlockDesc.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInterlockDesc.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblInterlockDesc.Location = new System.Drawing.Point(15, 50);
            this.lblInterlockDesc.Name = "lblInterlockDesc";
            this.lblInterlockDesc.Size = new System.Drawing.Size(1100, 45);
            this.lblInterlockDesc.TabIndex = 1;
            this.lblInterlockDesc.Text = "Khi cháº¿ Ä‘á»™ liĂªn káº¿t Ä‘Æ°á»£c kĂ­ch hoáº¡t, báº¥t ká»³ báº£n tin in nĂ o Ä‘Æ°á»£c mĂ¡y in xáº£ ra qua gĂ³i RSFP sáº½ tá»± Ä‘á»™ng Ä‘Æ°á»£c chuyá»ƒn tiáº¿p sang Camera Keyence sau má»™t khoáº£ng trá»… bÄƒng táº£i (Conveyor Delay).\r\nCamera sáº½ gá»­i mĂ£ vá»«a in vá» pháº§n má»m R-Link Ä‘á»ƒ thá»±c hiá»‡n quy trĂ¬nh So khá»›p & XĂ¡c thá»±c (Verify and Compare) hoĂ n chá»‰nh mĂ  khĂ´ng cáº§n láº¯p rĂ¡p thiáº¿t bá»‹ pháº§n cá»©ng tháº­t.";

            this.btnStartFullLineSim.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.btnStartFullLineSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartFullLineSim.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartFullLineSim.ForeColor = System.Drawing.Color.White;
            this.btnStartFullLineSim.Location = new System.Drawing.Point(15, 105);
            this.btnStartFullLineSim.Name = "btnStartFullLineSim";
            this.btnStartFullLineSim.Size = new System.Drawing.Size(320, 35);
            this.btnStartFullLineSim.TabIndex = 2;
            this.btnStartFullLineSim.Text = "đŸ€ Báº®T Äáº¦U MĂ” PHá»NG TOĂ€N DĂ‚Y CHUYá»€N (START BOTH)";
            this.btnStartFullLineSim.UseVisualStyleBackColor = false;

            this.btnStopFullLineSim.BackColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.btnStopFullLineSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopFullLineSim.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopFullLineSim.ForeColor = System.Drawing.Color.White;
            this.btnStopFullLineSim.Location = new System.Drawing.Point(345, 105);
            this.btnStopFullLineSim.Name = "btnStopFullLineSim";
            this.btnStopFullLineSim.Size = new System.Drawing.Size(200, 35);
            this.btnStopFullLineSim.TabIndex = 3;
            this.btnStopFullLineSim.Text = "â¹ Dá»ªNG Táº¤T Cáº¢ SERVER";
            this.btnStopFullLineSim.UseVisualStyleBackColor = false;

            this.lblLineSimStatus.AutoSize = true;
            this.lblLineSimStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLineSimStatus.ForeColor = System.Drawing.Color.LightGreen;
            this.lblLineSimStatus.Location = new System.Drawing.Point(560, 114);
            this.lblLineSimStatus.Name = "lblLineSimStatus";
            this.lblLineSimStatus.Size = new System.Drawing.Size(188, 15);
            this.lblLineSimStatus.TabIndex = 4;
            this.lblLineSimStatus.Text = "Tráº¡ng thĂ¡i liĂªn há»£p: Sáºµn sĂ ng.";

            this.prgLineSim.Location = new System.Drawing.Point(15, 148);
            this.prgLineSim.Name = "prgLineSim";
            this.prgLineSim.Size = new System.Drawing.Size(1100, 14);
            this.prgLineSim.TabIndex = 5;

            // 
            // grpInterlockStats
            // 
            this.grpInterlockStats.Controls.Add(this.rtbInterlockLog);
            this.grpInterlockStats.Controls.Add(this.pnlStatsBar);
            this.grpInterlockStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpInterlockStats.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpInterlockStats.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.grpInterlockStats.Location = new System.Drawing.Point(15, 195);
            this.grpInterlockStats.Name = "grpInterlockStats";
            this.grpInterlockStats.Padding = new System.Windows.Forms.Padding(10);
            this.grpInterlockStats.Size = new System.Drawing.Size(1346, 607);
            this.grpInterlockStats.TabIndex = 1;
            this.grpInterlockStats.TabStop = false;
            this.grpInterlockStats.Text = "THá»NG KĂ DĂ‚Y CHUYá»€N THá»œI GIAN THá»°C & GIĂM SĂT TOĂ€N DIá»†N";

            this.pnlStatsBar.Controls.Add(this.lblLinePrintedCount);
            this.pnlStatsBar.Controls.Add(this.lblLineVerifiedCount);
            this.pnlStatsBar.Controls.Add(this.lblLineMatchRate);
            this.pnlStatsBar.Controls.Add(this.lblLineNgCount);
            this.pnlStatsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatsBar.Location = new System.Drawing.Point(10, 26);
            this.pnlStatsBar.Name = "pnlStatsBar";
            this.pnlStatsBar.Size = new System.Drawing.Size(1326, 40);
            this.pnlStatsBar.TabIndex = 0;

            this.lblLinePrintedCount.AutoSize = true;
            this.lblLinePrintedCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLinePrintedCount.ForeColor = System.Drawing.Color.FromArgb(163, 190, 140);
            this.lblLinePrintedCount.Location = new System.Drawing.Point(10, 10);
            this.lblLinePrintedCount.Name = "lblLinePrintedCount";
            this.lblLinePrintedCount.Size = new System.Drawing.Size(164, 15);
            this.lblLinePrintedCount.TabIndex = 0;
            this.lblLinePrintedCount.Text = "MĂ¡y In ÄĂ£ Xáº£: 0 sáº£n pháº©m";

            this.lblLineVerifiedCount.AutoSize = true;
            this.lblLineVerifiedCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLineVerifiedCount.ForeColor = System.Drawing.Color.FromArgb(136, 192, 208);
            this.lblLineVerifiedCount.Location = new System.Drawing.Point(230, 10);
            this.lblLineVerifiedCount.Name = "lblLineVerifiedCount";
            this.lblLineVerifiedCount.Size = new System.Drawing.Size(167, 15);
            this.lblLineVerifiedCount.TabIndex = 1;
            this.lblLineVerifiedCount.Text = "Camera ÄĂ£ Äá»c: 0 sáº£n pháº©m";

            this.lblLineMatchRate.AutoSize = true;
            this.lblLineMatchRate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLineMatchRate.ForeColor = System.Drawing.Color.FromArgb(235, 203, 139);
            this.lblLineMatchRate.Location = new System.Drawing.Point(460, 10);
            this.lblLineMatchRate.Name = "lblLineMatchRate";
            this.lblLineMatchRate.Size = new System.Drawing.Size(117, 15);
            this.lblLineMatchRate.TabIndex = 2;
            this.lblLineMatchRate.Text = "Tá»· Lá»‡ Khá»›p: 100.0%";

            this.lblLineNgCount.AutoSize = true;
            this.lblLineNgCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLineNgCount.ForeColor = System.Drawing.Color.FromArgb(191, 97, 106);
            this.lblLineNgCount.Location = new System.Drawing.Point(640, 10);
            this.lblLineNgCount.Name = "lblLineNgCount";
            this.lblLineNgCount.Size = new System.Drawing.Size(96, 15);
            this.lblLineNgCount.TabIndex = 3;
            this.lblLineNgCount.Text = "Lá»—i NG In/Äá»c: 0";

            this.rtbInterlockLog.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.rtbInterlockLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInterlockLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInterlockLog.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.rtbInterlockLog.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.rtbInterlockLog.Location = new System.Drawing.Point(10, 66);
            this.rtbInterlockLog.Name = "rtbInterlockLog";
            this.rtbInterlockLog.ReadOnly = true;
            this.rtbInterlockLog.Size = new System.Drawing.Size(1326, 531);
            this.rtbInterlockLog.TabIndex = 1;
            this.rtbInterlockLog.Text = "";

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 24, 37);
            this.ClientSize = new System.Drawing.Size(1384, 861);
            this.Controls.Add(this.tabMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TH True Milk - Vision & Printer Multi-Device Simulator v2.0 (Keyence VS-C + POD Printer)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);

            this.tabMain.ResumeLayout(false);
            this.tabCamera.ResumeLayout(false);
            this.tabPrinter.ResumeLayout(false);
            this.tabInterlock.ResumeLayout(false);
            this.pnlCamLeft.ResumeLayout(false);
            this.pnlCamRight.ResumeLayout(false);
            this.pnlCamRightTop.ResumeLayout(false);
            this.grpCameraServer.ResumeLayout(false);
            this.grpCameraServer.PerformLayout();
            this.grpTrigger.ResumeLayout(false);
            this.grpTrigger.PerformLayout();
            this.grpProductData.ResumeLayout(false);
            this.grpProductData.PerformLayout();
            this.grpErrorInjection.ResumeLayout(false);
            this.grpErrorInjection.PerformLayout();
            this.grpScenarios.ResumeLayout(false);
            this.grpScenarios.PerformLayout();
            this.grpPreview.ResumeLayout(false);
            this.grpFtp.ResumeLayout(false);
            this.grpFtp.PerformLayout();
            this.grpCameraLog.ResumeLayout(false);
            this.pnlCamLogTools.ResumeLayout(false);
            this.pnlCamLogTools.PerformLayout();
            this.pnlPrnLeft.ResumeLayout(false);
            this.pnlPrnRight.ResumeLayout(false);
            this.grpPrinterServer.ResumeLayout(false);
            this.grpPrinterServer.PerformLayout();
            this.grpPrinterControl.ResumeLayout(false);
            this.grpPrinterControl.PerformLayout();
            this.grpPrinterAlarms.ResumeLayout(false);
            this.grpPrinterAlarms.PerformLayout();
            this.grpPrintBuffer.ResumeLayout(false);
            this.pnlBufferStats.ResumeLayout(false);
            this.pnlBufferStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintBuffer)).EndInit();
            this.grpPrinterLog.ResumeLayout(false);
            this.pnlPrnLogTools.ResumeLayout(false);
            this.pnlPrnLogTools.PerformLayout();
            this.grpInterlockOverview.ResumeLayout(false);
            this.grpInterlockOverview.PerformLayout();
            this.grpInterlockStats.ResumeLayout(false);
            this.pnlStatsBar.ResumeLayout(false);
            this.pnlStatsBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamFtpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrnPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPrnSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrnSpeed)).EndInit();
            this.ResumeLayout(false);
        }
    }
}