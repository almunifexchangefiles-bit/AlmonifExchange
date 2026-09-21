using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace AlmonifExchange
{
    public partial class Form_Main : Form
    {
        private IContainer components = null;
        private ImageList imageList1;
        private Panel panel1;
        public MenuStrip Men_Strip;
        public ToolStripMenuItem Menu_File;
        private ToolStripMenuItem Menu_Send_Money;
        private ToolStripMenuItem Menu_Transfer_money;
        private ToolStripMenuItem Menu_Transferring_update;
        private ToolStripMenuItem Menu_Transfer_imports;
        private ToolStripMenuItem Menu_Report;
        private ToolStripMenuItem MenuI_Account_statement;
        private ToolStripMenuItem Menu_Sync_settings;
        private ToolStripMenuItem Menu_Transfer_requests;
        private ToolStripMenuItem Menu_Remittance_inquiries;
        private ToolStripMenuItem Menu_requests_covering;
        private PictureBox pictureBox1;
        private ToolStripMenuItem Menu_Form_Transfer_money_Remove;
        private ToolStripMenuItem Menu_Curr_Ebda;
        private ToolStripMenuItem SadadServices;
        private ToolStripMenuItem Menu_Synchronizing_transfers;
        private StatusStrip Strip;
        private ToolStripStatusLabel Strip_user;
        private ToolStripStatusLabel Strip_UserName;
        private ToolStripStatusLabel Strip_space;
        private ToolStripStatusLabel Strip_time;
        private ToolStripStatusLabel Strip_date;
        private ToolStripStatusLabel Strip_AgentName;
        private ToolStripStatusLabel Strip_label_Chek_DataInLogin;
        private Panel panel_Label;
        private ToolStripMenuItem Menu_Sync_Auto;
        private ToolStripMenuItem Menu_Sync_H;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tool_Count_remittances_send_Sync_ME;
        private ToolStripMenuItem menu_FormUpdate_System;
        private LinkLabel label_shop;
        private ToolStripMenuItem Menu_Report_Remitins;
        private ToolStripMenuItem Menu_Report_Remit_Transfer;
        private ToolStripMenuItem Menu_Report_requests_covering;
        private ToolStripMenuItem accountsAuthenticationsToolStripMenuItem;
        private ToolStripMenuItem Menu_sarfAutoDelivery;
        private ToolStripMenuItem Menu_LogOff;
        private ToolStripMenuItem Menu_Exit;
        private ToolStripMenuItem Menu_Chang_pass;
        public System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem Menu_Agents;
        private BackgroundWorker backgroundWorker1;
        private BackgroundWorker backgroundWorker2;
        private ToolStripMenuItem Menu_Note_Agent;
        private ToolStripStatusLabel toolSpace;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem Menu_PC_Enable;
        private ToolStripMenuItem Menu_Cash;
        public Panel panel_Main;
        public Panel panel_Info;
        private Button button_PiNfo_Hide;
        private ToolStripMenuItem Menu_Shamel;
        private ToolStripMenuItem Menu_Aden_Bank;
        private ToolStripMenuItem Menu_Sync_Card;
        private ToolStripStatusLabel PC_NAME_;
        private TabControl mainTabControl;
        private string mot = "";
        private System.Windows.Forms.Timer timerMarquee;

        public Form_Main()
        {
            InitializeComponent();
            DatabaseHelper.LoadApplicationIcon(this);
            
            // قيم افتراضية تظهر قبل تسجيل الدخول
            Strip_UserName.Text = "في انتظار تسجيل الدخول...";
            Strip_AgentName.Text = "";
            Strip_label_Chek_DataInLogin.Text = " ";
            toolSpace.Text = "      ";
            toolStripStatusLabel1.Text = "الإصدار 1.11";
            PC_NAME_.Text = "    " + Environment.MachineName;
            this.mot = "اهلا فيك في نضام شبكة المنيف نضام الصراف";
            
            // تشغيل الوقت والتاريخ فوراً (قبل تسجيل الدخول)
            UpdateTimeAndDate();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void LoadMenuIcons()
        {
            string iconsPath = Application.StartupPath;
            Action<ToolStripMenuItem, string> loadIcon = (menuItem, fileName) =>
            {
                if (menuItem == null) return;
                string filePath = Path.Combine(iconsPath, fileName);
                if (File.Exists(filePath))
                {
                    try
                    {
                        menuItem.Image = Image.FromFile(filePath);
                        menuItem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    }
                    catch { }
                }
            };

            loadIcon(Menu_Send_Money, "إصدار حوالة.png");
            loadIcon(Menu_Transfer_money, "صرف حوالة.png");
            loadIcon(Menu_Transferring_update, "تعديل حوالة.png");
            loadIcon(Menu_Form_Transfer_money_Remove, "توقيف حوالة.png");
            loadIcon(Menu_Transfer_requests, "طلبات الحوالات.png");
            loadIcon(Menu_requests_covering, "طلبات التغطيات.png");
            loadIcon(Menu_Agents, "بيانات الوكلاء.png");
            loadIcon(Menu_Report, "التقارير.png");
            loadIcon(MenuI_Account_statement, "كشف حساب.png");
            loadIcon(SadadServices, "سداد الخدمات.png");
            loadIcon(Menu_Synchronizing_transfers, "مزامنة الحوالات.png");
            loadIcon(Menu_Cash, "محفظة كاش.png");
            loadIcon(Menu_Chang_pass, "تغيير كلمة السر.png");
            loadIcon(Menu_Note_Agent, "إرسال الطلبات.png");
            loadIcon(menu_FormUpdate_System, "تحديث النظام.png");
            loadIcon(Menu_Exit, "خروج من النظام.png");
        }

        private void UpdateTimeAndDate()
        {
            Strip_time.Text = DateTime.Now.ToString("hh:mm tt");
            Strip_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.imageList1 = new ImageList(this.components);
            this.panel1 = new Panel();
            this.Men_Strip = new MenuStrip();
            this.Menu_File = new ToolStripMenuItem();
            this.Menu_Curr_Ebda = new ToolStripMenuItem();
            this.Menu_Send_Money = new ToolStripMenuItem();
            this.Menu_Transfer_money = new ToolStripMenuItem();
            this.Menu_Transferring_update = new ToolStripMenuItem();
            this.Menu_Form_Transfer_money_Remove = new ToolStripMenuItem();
            this.Menu_Transfer_requests = new ToolStripMenuItem();
            this.Menu_requests_covering = new ToolStripMenuItem();
            this.Menu_Agents = new ToolStripMenuItem();
            this.Menu_Transfer_imports = new ToolStripMenuItem();
            this.Menu_Report = new ToolStripMenuItem();
            this.Menu_Report_Remitins = new ToolStripMenuItem();
            this.Menu_Report_Remit_Transfer = new ToolStripMenuItem();
            this.Menu_Report_requests_covering = new ToolStripMenuItem();
            this.accountsAuthenticationsToolStripMenuItem = new ToolStripMenuItem();
            this.MenuI_Account_statement = new ToolStripMenuItem();
            this.Menu_Remittance_inquiries = new ToolStripMenuItem();
            this.Menu_Sync_settings = new ToolStripMenuItem();
            this.SadadServices = new ToolStripMenuItem();
            this.Menu_Synchronizing_transfers = new ToolStripMenuItem();
            this.Menu_sarfAutoDelivery = new ToolStripMenuItem();
            this.Menu_Sync_Auto = new ToolStripMenuItem();
            this.Menu_Sync_H = new ToolStripMenuItem();
            this.Menu_Sync_Card = new ToolStripMenuItem();
            this.Menu_Cash = new ToolStripMenuItem();
            this.Menu_Shamel = new ToolStripMenuItem();
            this.Menu_Aden_Bank = new ToolStripMenuItem();
            this.Menu_Chang_pass = new ToolStripMenuItem();
            this.Menu_Note_Agent = new ToolStripMenuItem();
            this.Menu_PC_Enable = new ToolStripMenuItem();
            this.menu_FormUpdate_System = new ToolStripMenuItem();
            this.Menu_LogOff = new ToolStripMenuItem();
            this.Menu_Exit = new ToolStripMenuItem();
            this.pictureBox1 = new PictureBox();
            this.Strip = new StatusStrip();
            this.Strip_user = new ToolStripStatusLabel();
            this.Strip_space = new ToolStripStatusLabel();
            this.Strip_UserName = new ToolStripStatusLabel();
            this.Strip_AgentName = new ToolStripStatusLabel();
            this.Strip_label_Chek_DataInLogin = new ToolStripStatusLabel();
            this.Strip_time = new ToolStripStatusLabel();
            this.Strip_date = new ToolStripStatusLabel();
            this.toolSpace = new ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.PC_NAME_ = new ToolStripStatusLabel();
            this.panel_Label = new Panel();
            this.label_shop = new LinkLabel();
            this.statusStrip1 = new StatusStrip();
            this.tool_Count_remittances_send_Sync_ME = new ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker2 = new BackgroundWorker();
            this.panel_Info = new Panel();
            this.button_PiNfo_Hide = new Button();
            this.panel_Main = new Panel();
            this.panel1.SuspendLayout();
            this.Men_Strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Strip.SuspendLayout();
            this.panel_Label.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel_Info.SuspendLayout();
            this.SuspendLayout();
            this.imageList1.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new Size(16, 16);
            this.imageList1.TransparentColor = Color.Transparent;
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Men_Strip);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(6, 4, 6, 4);
            this.panel1.Size = new Size(1290, 70);
            this.panel1.TabIndex = 18;
            this.Men_Strip.AutoSize = false;
            this.Men_Strip.BackColor = Color.WhiteSmoke;
            this.Men_Strip.Font = new Font("Arial", 11.5f, FontStyle.Bold);
            this.Men_Strip.GripMargin = new Padding(1);
            this.Men_Strip.GripStyle = ToolStripGripStyle.Visible;
            this.Men_Strip.ImageScalingSize = new Size(20, 20);
            this.Men_Strip.Items.AddRange(new ToolStripItem[] {
                this.Menu_File, this.Menu_Send_Money, this.Menu_Transfer_money,
                this.Menu_Transferring_update, this.Menu_Form_Transfer_money_Remove,
                this.Menu_Transfer_requests, this.Menu_requests_covering, this.Menu_Agents,
                this.Menu_Transfer_imports, this.Menu_Report, this.MenuI_Account_statement,
                this.Menu_Remittance_inquiries, this.Menu_Sync_settings, this.SadadServices,
                this.Menu_Synchronizing_transfers, this.Menu_Cash, this.Menu_Shamel,
                this.Menu_Aden_Bank, this.Menu_Chang_pass, this.Menu_Note_Agent,
                this.Menu_PC_Enable, this.menu_FormUpdate_System, this.Menu_LogOff, this.Menu_Exit
            });
            this.Men_Strip.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.Men_Strip.Location = new Point(0, 0);
            this.Men_Strip.Name = "Men_Strip";
            this.Men_Strip.Padding = new Padding(2, 3, 2, 2);
            this.Men_Strip.RenderMode = ToolStripRenderMode.Professional;
            this.Men_Strip.Size = new Size(1288, 68);
            this.Men_Strip.TabIndex = 1;
            this.Men_Strip.Text = "menuStrip1";
            this.Menu_File.DropDownItems.AddRange(new ToolStripItem[] { this.Menu_Curr_Ebda });
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Padding = new Padding(2, 5, 2, 0);
            this.Menu_File.Size = new Size(46, 33);
            this.Menu_File.Text = "ملف";
            this.Menu_File.Visible = false;
            this.Menu_Curr_Ebda.Enabled = false;
            this.Menu_Curr_Ebda.Name = "Menu_Curr_Ebda";
            this.Menu_Curr_Ebda.Size = new Size(176, 28);
            this.Menu_Curr_Ebda.Text = "التعابير البديلة";
            this.Menu_Curr_Ebda.Visible = false;
            this.Menu_Send_Money.AutoToolTip = true;
            this.Menu_Send_Money.Name = "Menu_Send_Money";
            this.Menu_Send_Money.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Send_Money.ShortcutKeys = Keys.F11;
            this.Menu_Send_Money.Size = new Size(138, 33);
            this.Menu_Send_Money.Text = "إصدار حوالة F11";
            this.Menu_Send_Money.ToolTipText = "إصدار حوالة F11";
            this.Menu_Send_Money.Click += new EventHandler(this.Menu_Send_Money_Click);
            this.Menu_Transfer_money.Name = "Menu_Transfer_money";
            this.Menu_Transfer_money.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Transfer_money.ShortcutKeys = Keys.F10;
            this.Menu_Transfer_money.Size = new Size(139, 33);
            this.Menu_Transfer_money.Text = "صرف حوالة F10";
            this.Menu_Transfer_money.Click += new EventHandler(this.Menu_Transfer_money_Click);
            this.Menu_Transferring_update.Name = "Menu_Transferring_update";
            this.Menu_Transferring_update.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Transferring_update.ShortcutKeys = Keys.F9;
            this.Menu_Transferring_update.Size = new Size(122, 33);
            this.Menu_Transferring_update.Text = "تعديل حوالة F9";
            this.Menu_Transferring_update.Click += new EventHandler(this.Menu_Transferring_update_Click);
            this.Menu_Form_Transfer_money_Remove.Enabled = false;
            this.Menu_Form_Transfer_money_Remove.Name = "Menu_Form_Transfer_money_Remove";
            this.Menu_Form_Transfer_money_Remove.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Form_Transfer_money_Remove.Size = new Size(99, 33);
            this.Menu_Form_Transfer_money_Remove.Text = "توقيف الحوالة";
            this.Menu_Form_Transfer_money_Remove.Visible = false;
            this.Menu_Transfer_requests.Name = "Menu_Transfer_requests";
            this.Menu_Transfer_requests.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Transfer_requests.ShortcutKeys = Keys.F8;
            this.Menu_Transfer_requests.Size = new Size(146, 33);
            this.Menu_Transfer_requests.Text = "طلبات الحوالات F8";
            this.Menu_Transfer_requests.Click += new EventHandler(this.Menu_Transfer_requests_Click);
            this.Menu_requests_covering.Name = "Menu_requests_covering";
            this.Menu_requests_covering.Padding = new Padding(2, 5, 2, 0);
            this.Menu_requests_covering.ShortcutKeys = Keys.F6;
            this.Menu_requests_covering.Size = new Size(145, 33);
            this.Menu_requests_covering.Text = "طلبات التغطيات F6";
            this.Menu_requests_covering.Click += new EventHandler(this.Menu_requests_covering_Click);
            this.Menu_Agents.Name = "Menu_Agents";
            this.Menu_Agents.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Agents.Size = new Size(107, 33);
            this.Menu_Agents.Text = "بيانات الوكلاء";
            this.Menu_Agents.Click += new EventHandler(this.Menu_Agents_Click);
            this.Menu_Transfer_imports.Enabled = false;
            this.Menu_Transfer_imports.Name = "Menu_Transfer_imports";
            this.Menu_Transfer_imports.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Transfer_imports.Size = new Size(116, 33);
            this.Menu_Transfer_imports.Text = "إستيراد حوالات";
            this.Menu_Transfer_imports.Visible = false;
            this.Menu_Report.DropDownItems.AddRange(new ToolStripItem[] {
                this.Menu_Report_Remitins, this.Menu_Report_Remit_Transfer,
                this.Menu_Report_requests_covering, this.accountsAuthenticationsToolStripMenuItem
            });
            this.Menu_Report.Name = "Menu_Report";
            this.Menu_Report.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Report.Size = new Size(66, 33);
            this.Menu_Report.Text = "التقارير";
            this.Menu_Report_Remitins.Name = "Menu_Report_Remitins";
            this.Menu_Report_Remitins.Size = new Size(256, 28);
            this.Menu_Report_Remitins.Text = "تقارير الحوالات";
            this.Menu_Report_Remitins.Click += new EventHandler(this.Menu_Report_Remitins_Click);
            this.Menu_Report_Remit_Transfer.Name = "Menu_Report_Remit_Transfer";
            this.Menu_Report_Remit_Transfer.Size = new Size(256, 28);
            this.Menu_Report_Remit_Transfer.Text = "تقارير الحوالات المصروفة";
            this.Menu_Report_Remit_Transfer.Visible = false;
            this.Menu_Report_requests_covering.Name = "Menu_Report_requests_covering";
            this.Menu_Report_requests_covering.Size = new Size(256, 28);
            this.Menu_Report_requests_covering.Text = "تقارير طلبات التغطيات";
            this.Menu_Report_requests_covering.Visible = false;
            this.accountsAuthenticationsToolStripMenuItem.Name = "accountsAuthenticationsToolStripMenuItem";
            this.accountsAuthenticationsToolStripMenuItem.Size = new Size(256, 28);
            this.accountsAuthenticationsToolStripMenuItem.Text = "مصادقات الوكلاء";
            this.MenuI_Account_statement.Name = "MenuI_Account_statement";
            this.MenuI_Account_statement.Padding = new Padding(2, 5, 2, 0);
            this.MenuI_Account_statement.ShortcutKeys = Keys.F3;
            this.MenuI_Account_statement.Size = new Size(127, 33);
            this.MenuI_Account_statement.Text = "كشف حساب F3";
            this.MenuI_Account_statement.Click += new EventHandler(this.MenuI_Account_statement_Click);
            this.Menu_Remittance_inquiries.Name = "Menu_Remittance_inquiries";
            this.Menu_Remittance_inquiries.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Remittance_inquiries.Size = new Size(125, 33);
            this.Menu_Remittance_inquiries.Text = "إستعلام الحوالات";
            this.Menu_Remittance_inquiries.Visible = false;
            this.Menu_Sync_settings.Name = "Menu_Sync_settings";
            this.Menu_Sync_settings.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Sync_settings.Size = new Size(125, 33);
            this.Menu_Sync_settings.Text = "إعدادات المزامنة";
            this.Menu_Sync_settings.Visible = false;
            this.SadadServices.Name = "SadadServices";
            this.SadadServices.Padding = new Padding(2, 5, 2, 0);
            this.SadadServices.Size = new Size(106, 33);
            this.SadadServices.Text = "سداد الخدمات";
            this.SadadServices.Click += new EventHandler(this.SadadServices_Click);
            this.Menu_Synchronizing_transfers.DropDownItems.AddRange(new ToolStripItem[] {
                this.Menu_sarfAutoDelivery, this.Menu_Sync_Auto, this.Menu_Sync_H, this.Menu_Sync_Card
            });
            this.Menu_Synchronizing_transfers.Name = "Menu_Synchronizing_transfers";
            this.Menu_Synchronizing_transfers.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Synchronizing_transfers.Size = new Size(127, 33);
            this.Menu_Synchronizing_transfers.Text = "مزامنة الحوالات ";
            this.Menu_sarfAutoDelivery.Name = "Menu_sarfAutoDelivery";
            this.Menu_sarfAutoDelivery.Size = new Size(393, 28);
            this.Menu_sarfAutoDelivery.Text = "مزامنة الحوالات مصروفة إلى نظام الصراف";
            this.Menu_sarfAutoDelivery.Click += new EventHandler(this.Menu_sarfAutoDelivery_Click);
            this.Menu_Sync_Auto.Name = "Menu_Sync_Auto";
            this.Menu_Sync_Auto.Size = new Size(393, 28);
            this.Menu_Sync_Auto.Text = "تشغيل مزامنة الحوالات من نظام الصراف تلقائي";
            this.Menu_Sync_Auto.Click += new EventHandler(this.Menu_Sync_Auto_Click);
            this.Menu_Sync_H.Name = "Menu_Sync_H";
            this.Menu_Sync_H.Size = new Size(393, 28);
            this.Menu_Sync_H.Text = "مزامنة الحوالات من نظام الصراف يدوي";
            this.Menu_Sync_H.Click += new EventHandler(this.Menu_Sync_H_Click);
            this.Menu_Sync_Card.Name = "Menu_Sync_Card";
            this.Menu_Sync_Card.Size = new Size(393, 28);
            this.Menu_Sync_Card.Text = "مزامنة وثائق إثبات الهوية";
            this.Menu_Sync_Card.Click += new EventHandler(this.Menu_Sync_Card_Click);
            this.Menu_Cash.BackColor = Color.White;
            this.Menu_Cash.Name = "Menu_Cash";
            this.Menu_Cash.Size = new Size(108, 28);
            this.Menu_Cash.Text = "محفظة كــاش";
            this.Menu_Cash.Click += new EventHandler(this.Menu_Cash_Click);
            this.Menu_Shamel.Name = "Menu_Shamel";
            this.Menu_Shamel.Size = new Size(145, 28);
            this.Menu_Shamel.Text = "حركة الشامل موني";
            this.Menu_Shamel.Click += new EventHandler(this.Menu_Shamel_Click);
            this.Menu_Aden_Bank.Name = "Menu_Aden_Bank";
            this.Menu_Aden_Bank.Size = new Size(115, 28);
            this.Menu_Aden_Bank.Text = "ايداع بنك عدن";
            this.Menu_Aden_Bank.Visible = false;
            this.Menu_Chang_pass.Name = "Menu_Chang_pass";
            this.Menu_Chang_pass.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Chang_pass.Size = new Size(114, 33);
            this.Menu_Chang_pass.Text = "تغير كلمة السر";
            this.Menu_Chang_pass.Click += new EventHandler(this.Menu_Chang_pass_Click);
            this.Menu_Note_Agent.Name = "Menu_Note_Agent";
            this.Menu_Note_Agent.Size = new Size(114, 28);
            this.Menu_Note_Agent.Text = "إرسال الطلبات";
            this.Menu_Note_Agent.Click += new EventHandler(this.Menu_Note_Agent_Click);
            this.Menu_PC_Enable.Name = "Menu_PC_Enable";
            this.Menu_PC_Enable.Size = new Size(207, 28);
            this.Menu_PC_Enable.Text = "الأجهزة المصرحة للدخول";
            this.Menu_PC_Enable.Visible = false;
            this.menu_FormUpdate_System.Name = "menu_FormUpdate_System";
            this.menu_FormUpdate_System.Padding = new Padding(2, 5, 2, 0);
            this.menu_FormUpdate_System.Size = new Size(100, 33);
            this.menu_FormUpdate_System.Text = "تحديث النظام";
            this.menu_FormUpdate_System.Click += new EventHandler(this.menu_FormUpdate_System_Click);
            this.Menu_LogOff.Name = "Menu_LogOff";
            this.Menu_LogOff.Padding = new Padding(2, 5, 2, 0);
            this.Menu_LogOff.Size = new Size(105, 33);
            this.Menu_LogOff.Text = "تسجيل خروج";
            this.Menu_LogOff.Visible = false;
            this.Menu_Exit.Name = "Menu_Exit";
            this.Menu_Exit.Padding = new Padding(2, 5, 2, 0);
            this.Menu_Exit.Size = new Size(125, 33);
            this.Menu_Exit.Text = "خروج من النظام";
            this.Menu_Exit.Click += new EventHandler(this.Menu_Exit_Click);
            this.pictureBox1.Location = new Point(-40, 475);
            this.pictureBox1.Margin = new Padding(6, 4, 6, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(10, 23);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 392;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.Strip.BackColor = Color.FromArgb(234, 238, 255);
            this.Strip.Font = new Font("Arial", 11.5f, FontStyle.Bold);
            this.Strip.ImageScalingSize = new Size(20, 20);
            this.Strip.Items.AddRange(new ToolStripItem[] {
                this.Strip_user, this.Strip_space, this.Strip_UserName, this.Strip_AgentName,
                this.Strip_label_Chek_DataInLogin, this.Strip_time, this.Strip_date,
                this.toolSpace, this.toolStripStatusLabel1, this.PC_NAME_
            });
            this.Strip.Location = new Point(0, 1004);
            this.Strip.Name = "Strip";
            this.Strip.Padding = new Padding(30, 0, 1, 0);
            this.Strip.Size = new Size(1290, 29);
            this.Strip.TabIndex = 403;
            this.Strip.Text = "المستخدم";
            this.Strip_user.Name = "Strip_user";
            this.Strip_user.Size = new Size(68, 24);
            this.Strip_user.Text = "المستخدم";
            this.Strip_space.Name = "Strip_space";
            this.Strip_space.Size = new Size(17, 24);
            this.Strip_space.Text = "-";
            this.Strip_UserName.Name = "Strip_UserName";
            this.Strip_UserName.Size = new Size(17, 24);
            this.Strip_UserName.Text = "-";
            this.Strip_AgentName.Name = "Strip_AgentName";
            this.Strip_AgentName.Size = new Size(0, 24);
            this.Strip_AgentName.Text = "";
            this.Strip_label_Chek_DataInLogin.Name = "Strip_label_Chek_DataInLogin";
            this.Strip_label_Chek_DataInLogin.Size = new Size(16, 24);
            this.Strip_label_Chek_DataInLogin.Text = " ";
            this.Strip_time.Font = new Font("Arial", 11.5f, FontStyle.Bold);
            this.Strip_time.Name = "Strip_time";
            this.Strip_time.Size = new Size(47, 24);
            this.Strip_time.Text = "الوقت";
            this.Strip_date.Name = "Strip_date";
            this.Strip_date.Size = new Size(54, 24);
            this.Strip_date.Text = "التاريخ";
            this.toolSpace.Name = "toolSpace";
            this.toolSpace.Size = new Size(46, 24);
            this.toolSpace.Text = "      ";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(103, 24);
            this.toolStripStatusLabel1.Text = "الإصدار 1.11";
            this.PC_NAME_.Font = new Font("Arial", 9f, FontStyle.Bold);
            this.PC_NAME_.Name = "PC_NAME_";
            this.PC_NAME_.Size = new Size(0, 24);
            this.PC_NAME_.Text = "";
            this.panel_Label.BackColor = Color.SteelBlue;
            this.panel_Label.Controls.Add(this.label_shop);
            this.panel_Label.Dock = DockStyle.Top;
            this.panel_Label.Location = new Point(0, 70);
            this.panel_Label.Size = new Size(1290, 28);
            this.panel_Label.TabIndex = 402;
            this.label_shop.AutoSize = true;
            this.label_shop.Font = new Font("Times New Roman", 10.8f, FontStyle.Bold);
            this.label_shop.ForeColor = Color.AliceBlue;
            this.label_shop.LinkColor = Color.Cyan;
            this.label_shop.Location = new Point(this.panel_Label.Width, 2);
            this.label_shop.Name = "label_shop";
            this.label_shop.Size = new Size(229, 22);
            this.label_shop.TabIndex = 0;
            this.label_shop.TabStop = true;
            this.label_shop.Text = "اهلا فيك في نضام شبكة المنيف نضام الصراف";
            this.statusStrip1.Font = new Font("Segoe UI", 5f);
            this.statusStrip1.ImageScalingSize = new Size(10, 10);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.tool_Count_remittances_send_Sync_ME });
            this.statusStrip1.Location = new Point(0, 1033);
            this.statusStrip1.Size = new Size(1290, 22);
            this.statusStrip1.TabIndex = 405;
            this.statusStrip1.Text = "statusStrip1";
            this.tool_Count_remittances_send_Sync_ME.Name = "tool_Count_remittances_send_Sync_ME";
            this.tool_Count_remittances_send_Sync_ME.Size = new Size(9, 17);
            this.tool_Count_remittances_send_Sync_ME.Text = "-";
            this.timer1.Enabled = false;
            this.timer1.Interval = 20;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.backgroundWorker2 = new BackgroundWorker();
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.panel_Info.Controls.Add(this.button_PiNfo_Hide);
            this.panel_Info.Dock = DockStyle.Left;
            this.panel_Info.Location = new Point(0, 98);
            this.panel_Info.Size = new Size(220, 906);
            this.panel_Info.TabIndex = 418;
            this.panel_Info.Visible = false;
            this.button_PiNfo_Hide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.button_PiNfo_Hide.Font = new Font("Microsoft Sans Serif", 8.25f);
            this.button_PiNfo_Hide.Location = new Point(201, 435);
            this.button_PiNfo_Hide.Size = new Size(18, 39);
            this.button_PiNfo_Hide.TabIndex = 0;
            this.button_PiNfo_Hide.Text = ">";
            this.button_PiNfo_Hide.UseVisualStyleBackColor = true;
            this.button_PiNfo_Hide.Click += new EventHandler(this.button_PiNfo_Hide_Click);
            this.panel_Main.AutoScroll = true;
            this.panel_Main.BackColor = Color.White;
            this.panel_Main.Dock = DockStyle.Fill;
            this.panel_Main.Font = new Font("Times New Roman", 11.5f, FontStyle.Bold);
            this.panel_Main.ForeColor = Color.MidnightBlue;
            this.panel_Main.Location = new Point(220, 98);
            this.panel_Main.Size = new Size(1070, 906);
            this.panel_Main.TabIndex = 419;
            this.AutoScaleDimensions = new SizeF(18f, 35f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1290, 1055);
            this.Controls.Add(this.panel_Main);
            this.Controls.Add(this.panel_Info);
            this.Controls.Add(this.Strip);
            this.Controls.Add(this.panel_Label);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new Font("Times New Roman", 18f, FontStyle.Bold);
            this.ForeColor = Color.MidnightBlue;
            this.IsMdiContainer = true;
            this.Name = "Form_Main";
            this.RightToLeft = RightToLeft.Yes;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "شبكة المنيف نضام الصراف";
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += new FormClosingEventHandler(this.Form_Main_FormClosing);
            this.FormClosed += new FormClosedEventHandler(this.Form_Main_FormClosed);
            this.Load += new EventHandler(this.Form_Main_Load);
            this.panel1.ResumeLayout(false);
            this.Men_Strip.ResumeLayout(false);
            this.Men_Strip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Strip.ResumeLayout(false);
            this.Strip.PerformLayout();
            this.panel_Label.ResumeLayout(false);
            this.panel_Label.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel_Info.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ========== Event Handlers ==========
        private void Menu_Send_Money_Click(object sender, EventArgs e) { Form_Send_Money frm = new Form_Send_Money(Strip_UserName.Text); OpenFormInTab(frm); }
        private void Menu_Transfer_money_Click(object sender, EventArgs e) { MessageBox.Show("شاشة صرف حوالة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Transferring_update_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تعديل حوالة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Form_Transfer_money_Remove_Click(object sender, EventArgs e) { MessageBox.Show("شاشة توقيف حوالة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Transfer_requests_Click(object sender, EventArgs e) { MessageBox.Show("شاشة طلبات الحوالات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_requests_covering_Click(object sender, EventArgs e) { MessageBox.Show("شاشة طلبات التغطيات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Agents_Click(object sender, EventArgs e) { Form_Agents frm = new Form_Agents(); OpenFormInTab(frm); }
        private void Menu_Transfer_imports_Click(object sender, EventArgs e) { MessageBox.Show("شاشة استيراد حوالات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Report_Remitins_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تقارير الحوالات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Report_Remit_Transfer_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تقارير الحوالات المصروفة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Report_requests_covering_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تقارير طلبات التغطيات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void accountsAuthenticationsToolStripMenuItem_Click(object sender, EventArgs e) { MessageBox.Show("شاشة مصادقات الوكلاء قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void MenuI_Account_statement_Click(object sender, EventArgs e) { MessageBox.Show("شاشة كشف حساب قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Remittance_inquiries_Click(object sender, EventArgs e) { MessageBox.Show("شاشة استعلام الحوالات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void SadadServices_Click(object sender, EventArgs e) { MessageBox.Show("شاشة سداد الخدمات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_sarfAutoDelivery_Click(object sender, EventArgs e) { MessageBox.Show("شاشة مزامنة إلى نظام الصراف قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Sync_Auto_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تشغيل مزامنة تلقائي قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Sync_H_Click(object sender, EventArgs e) { MessageBox.Show("شاشة مزامنة يدوية قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Sync_Card_Click(object sender, EventArgs e) { MessageBox.Show("شاشة مزامنة وثائق الهوية قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Cash_Click(object sender, EventArgs e) { MessageBox.Show("شاشة محفظة كاش قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Shamel_Click(object sender, EventArgs e) { MessageBox.Show("شاشة حركة الشامل موني قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Aden_Bank_Click(object sender, EventArgs e) { MessageBox.Show("شاشة ايداع بنك عدن قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Chang_pass_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تغيير كلمة السر قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Note_Agent_Click(object sender, EventArgs e) { MessageBox.Show("شاشة إرسال الطلبات قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_PC_Enable_Click(object sender, EventArgs e) { MessageBox.Show("شاشة الأجهزة المصرحة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void menu_FormUpdate_System_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تحديث النظام قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_LogOff_Click(object sender, EventArgs e) { MessageBox.Show("شاشة تسجيل خروج قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void Menu_Exit_Click(object sender, EventArgs e) { Application.Exit(); }
        private void timer1_Tick(object sender, EventArgs e) { UpdateTimeAndDate(); }
        private void button_PiNfo_Hide_Click(object sender, EventArgs e) { panel_Info.Visible = false; }
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e) { }
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e) { }

        // ========== Form_Main_Load ==========
        private void Form_Main_Load(object sender, EventArgs e)
        {
            // 1. بناء الواجهة الأساسية (تظهر فوراً)
            this.mot = "اهلا فيك في نضام شبكة المنيف نضام الصراف";
            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.RightToLeft = RightToLeft.Yes;
            mainTabControl.RightToLeftLayout = true;
            mainTabControl.BackColor = Color.White;
            this.panel_Main.Controls.Add(mainTabControl);

            // 2. تحميل أيقونات الأزرار فوراً (قبل تسجيل الدخول)
            LoadMenuIcons();

            // 3. فتح شاشة تسجيل الدخول كـ Dialog (يوقف التنفيذ حتى الإغلاق)
            using (Form_Login loginForm = new Form_Login())
            {
                loginForm.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = loginForm.ShowDialog(this);

                if (result != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
            }

            // 4. بعد نجاح تسجيل الدخول فقط، يتم تحميل صورة الواجهة الرئيسية
            TabPage welcomePage = new TabPage("الرئيسية");
            welcomePage.BackColor = Color.White;
            PictureBox welcomePicture = new PictureBox();
            welcomePicture.Dock = DockStyle.Fill;
            welcomePicture.SizeMode = PictureBoxSizeMode.StretchImage;

            try
            {
                string imagePath = Path.Combine(Application.StartupPath, "الواجهة الرئيسية.png");
                if (File.Exists(imagePath))
                {
                    welcomePicture.Image = Image.FromFile(imagePath);
                }
            }
            catch { }

            welcomePage.Controls.Add(welcomePicture);
            mainTabControl.TabPages.Add(welcomePage);
            mainTabControl.SelectedTab = welcomePage;
            mainTabControl.DoubleClick += new EventHandler(mainTabControl_DoubleClick);

            // تحديث بيانات المستخدم في الشريط السفلي
            Strip_UserName.Text = DatabaseHelper.CurrentUserName;
            Strip_AgentName.Text = DatabaseHelper.CurrentAgentName;

            // تحديث الوقت والتاريخ
            UpdateTimeAndDate();

            // تشغيل تحريك النص
            timerMarquee = new System.Windows.Forms.Timer();
            timerMarquee.Interval = 60;
            timerMarquee.Tick += new EventHandler(timerMarquee_Tick);
            timerMarquee.Start();
        }

        private void timerMarquee_Tick(object sender, EventArgs e)
        {
            if (label_shop.Location.X + label_shop.Width > 0)
                label_shop.Location = new Point(label_shop.Location.X - 3, label_shop.Location.Y);
            else
                label_shop.Location = new Point(panel_Label.Width, label_shop.Location.Y);
        }

        public bool tab_show(string frm_txt)
        {
            try
            {
                for (int i = 0; i < mainTabControl.TabCount; i++)
                {
                    if (mainTabControl.TabPages[i].Text == frm_txt)
                    {
                        mainTabControl.SelectedTab = mainTabControl.TabPages[i];
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        private void AddNewTab(Form frm)
        {
            try
            {
                TabPage tabPage = new TabPage(frm.Text);
                tabPage.BackColor = Color.White;
                frm.TopLevel = false;
                frm.Parent = tabPage;
                frm.Visible = true;
                frm.FormClosed += (s, args) => { mainTabControl.TabPages.Remove(tabPage); tabPage.Dispose(); };
                mainTabControl.TabPages.Add(tabPage);
                frm.Dock = DockStyle.Fill;
                mainTabControl.SelectedTab = tabPage;
            }
            catch { }
        }

        private void mainTabControl_DoubleClick(object sender, EventArgs e)
        {
            if (mainTabControl.SelectedTab != null && mainTabControl.SelectedTab.Text != "الرئيسية")
                mainTabControl.TabPages.Remove(mainTabControl.SelectedTab);
        }

        private bool IsFormOpen(Form frm)
        {
            foreach (TabPage tab in mainTabControl.TabPages)
            {
                if (tab.Controls.Count > 0 && tab.Controls[0].GetType() == frm.GetType()) return true;
            }
            return false;
        }

        private void ActivateTab(Form frm)
        {
            foreach (TabPage tab in mainTabControl.TabPages)
            {
                if (tab.Controls.Count > 0 && tab.Controls[0].GetType() == frm.GetType())
                {
                    mainTabControl.SelectedTab = tab;
                    break;
                }
            }
        }

        private void OpenFormInTab(Form frm)
        {
            if (IsFormOpen(frm)) ActivateTab(frm);
            else AddNewTab(frm);
        }
    }
}





