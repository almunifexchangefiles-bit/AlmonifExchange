using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlmonifExchange
{
    public partial class Form_Main : Form
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu_File;
        private ToolStripMenuItem Menu_Transfers;
        private ToolStripMenuItem Menu_Agents;
        private ToolStripMenuItem Menu_Reports;
        private ToolStripMenuItem Menu_Settings;
        private ToolStripMenuItem Menu_Help;
        private ToolStripMenuItem Menu_Exit;
        private ToolStripMenuItem Menu_NewTransfer;
        private ToolStripMenuItem Menu_EditTransfer;
        private ToolStripMenuItem Menu_DeleteTransfer;
        private ToolStripMenuItem Menu_AgentsData;
        private ToolStripMenuItem Menu_Commissions;
        private ToolStripMenuItem Menu_Ceilings;
        private ToolStripMenuItem Menu_DailyReport;
        private ToolStripMenuItem Menu_MonthlyReport;
        private ToolStripMenuItem Menu_UserSettings;
        private ToolStripMenuItem Menu_ChangePassword;
        private ToolStripMenuItem Menu_About;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel_User;
        private ToolStripStatusLabel statusLabel_Date;
        private ToolStripStatusLabel statusLabel_Time;
        private Panel panel_Content;
        private System.Windows.Forms.Timer timer1;

        public Form_Main()
        {
            InitializeComponent();
            DatabaseHelper.LoadApplicationIcon(this);
            this.Load += Form_Main_Load;
            this.FormClosing += Form_Main_FormClosing;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new MenuStrip();
            this.statusStrip1 = new StatusStrip();
            this.panel_Content = new Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            
            // MenuStrip
            this.menuStrip1.Font = new Font("Arial", 11F, FontStyle.Bold);
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Size = new Size(1200, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            
            // File Menu
            this.Menu_File = new ToolStripMenuItem();
            this.Menu_File.Text = "ملف";
            this.Menu_File.Size = new Size(50, 26);
            
            this.Menu_Exit = new ToolStripMenuItem();
            this.Menu_Exit.Text = "خروج";
            this.Menu_Exit.Size = new Size(180, 28);
            this.Menu_Exit.Click += new EventHandler(this.Menu_Exit_Click);
            
            this.Menu_File.DropDownItems.Add(this.Menu_Exit);
            
            // Transfers Menu
            this.Menu_Transfers = new ToolStripMenuItem();
            this.Menu_Transfers.Text = "الحوالات";
            this.Menu_Transfers.Size = new Size(70, 26);
            
            this.Menu_NewTransfer = new ToolStripMenuItem();
            this.Menu_NewTransfer.Text = "إصدار حوالة جديدة";
            this.Menu_NewTransfer.Size = new Size(220, 28);
            this.Menu_NewTransfer.Click += new EventHandler(this.Menu_NewTransfer_Click);
            
            this.Menu_EditTransfer = new ToolStripMenuItem();
            this.Menu_EditTransfer.Text = "تعديل حوالة";
            this.Menu_EditTransfer.Size = new Size(220, 28);
            this.Menu_EditTransfer.Click += new EventHandler(this.Menu_EditTransfer_Click);
            
            this.Menu_DeleteTransfer = new ToolStripMenuItem();
            this.Menu_DeleteTransfer.Text = "حذف حوالة";
            this.Menu_DeleteTransfer.Size = new Size(220, 28);
            this.Menu_DeleteTransfer.Click += new EventHandler(this.Menu_DeleteTransfer_Click);
            
            this.Menu_Transfers.DropDownItems.Add(this.Menu_NewTransfer);
            this.Menu_Transfers.DropDownItems.Add(this.Menu_EditTransfer);
            this.Menu_Transfers.DropDownItems.Add(this.Menu_DeleteTransfer);
            
            // Agents Menu
            this.Menu_Agents = new ToolStripMenuItem();
            this.Menu_Agents.Text = "الوكلاء";
            this.Menu_Agents.Size = new Size(60, 26);
            
            this.Menu_AgentsData = new ToolStripMenuItem();
            this.Menu_AgentsData.Text = "بيانات الوكلاء";
            this.Menu_AgentsData.Size = new Size(200, 28);
            this.Menu_AgentsData.Click += new EventHandler(this.Menu_AgentsData_Click);
            
            this.Menu_Commissions = new ToolStripMenuItem();
            this.Menu_Commissions.Text = "العمولات";
            this.Menu_Commissions.Size = new Size(200, 28);
            this.Menu_Commissions.Click += new EventHandler(this.Menu_Commissions_Click);
            
            this.Menu_Ceilings = new ToolStripMenuItem();
            this.Menu_Ceilings.Text = "تسقيف العملاء";
            this.Menu_Ceilings.Size = new Size(200, 28);
            this.Menu_Ceilings.Click += new EventHandler(this.Menu_Ceilings_Click);
            
            this.Menu_Agents.DropDownItems.Add(this.Menu_AgentsData);
            this.Menu_Agents.DropDownItems.Add(this.Menu_Commissions);
            this.Menu_Agents.DropDownItems.Add(this.Menu_Ceilings);
            
            // Reports Menu
            this.Menu_Reports = new ToolStripMenuItem();
            this.Menu_Reports.Text = "التقارير";
            this.Menu_Reports.Size = new Size(60, 26);
            
            this.Menu_DailyReport = new ToolStripMenuItem();
            this.Menu_DailyReport.Text = "التقرير اليومي";
            this.Menu_DailyReport.Size = new Size(200, 28);
            this.Menu_DailyReport.Click += new EventHandler(this.Menu_DailyReport_Click);
            
            this.Menu_MonthlyReport = new ToolStripMenuItem();
            this.Menu_MonthlyReport.Text = "التقرير الشهري";
            this.Menu_MonthlyReport.Size = new Size(200, 28);
            this.Menu_MonthlyReport.Click += new EventHandler(this.Menu_MonthlyReport_Click);
            
            this.Menu_Reports.DropDownItems.Add(this.Menu_DailyReport);
            this.Menu_Reports.DropDownItems.Add(this.Menu_MonthlyReport);
            
            // Settings Menu
            this.Menu_Settings = new ToolStripMenuItem();
            this.Menu_Settings.Text = "الإعدادات";
            this.Menu_Settings.Size = new Size(70, 26);
            
            this.Menu_UserSettings = new ToolStripMenuItem();
            this.Menu_UserSettings.Text = "إعدادات المستخدم";
            this.Menu_UserSettings.Size = new Size(200, 28);
            this.Menu_UserSettings.Click += new EventHandler(this.Menu_UserSettings_Click);
            
            this.Menu_ChangePassword = new ToolStripMenuItem();
            this.Menu_ChangePassword.Text = "تغيير كلمة المرور";
            this.Menu_ChangePassword.Size = new Size(200, 28);
            this.Menu_ChangePassword.Click += new EventHandler(this.Menu_ChangePassword_Click);
            
            this.Menu_Settings.DropDownItems.Add(this.Menu_UserSettings);
            this.Menu_Settings.DropDownItems.Add(this.Menu_ChangePassword);
            
            // Help Menu
            this.Menu_Help = new ToolStripMenuItem();
            this.Menu_Help.Text = "مساعدة";
            this.Menu_Help.Size = new Size(70, 26);
            
            this.Menu_About = new ToolStripMenuItem();
            this.Menu_About.Text = "حول البرنامج";
            this.Menu_About.Size = new Size(180, 28);
            this.Menu_About.Click += new EventHandler(this.Menu_About_Click);
            
            this.Menu_Help.DropDownItems.Add(this.Menu_About);
            
            // Add menus to MenuStrip
            this.menuStrip1.Items.Add(this.Menu_File);
            this.menuStrip1.Items.Add(this.Menu_Transfers);
            this.menuStrip1.Items.Add(this.Menu_Agents);
            this.menuStrip1.Items.Add(this.Menu_Reports);
            this.menuStrip1.Items.Add(this.Menu_Settings);
            this.menuStrip1.Items.Add(this.Menu_Help);
            
            // StatusStrip
            this.statusStrip1.Location = new Point(0, 650);
            this.statusStrip1.Size = new Size(1200, 25);
            this.statusStrip1.TabIndex = 1;
            
            this.statusLabel_User = new ToolStripStatusLabel();
            this.statusLabel_User.Text = "المستخدم: ";
            this.statusLabel_User.AutoSize = false;
            this.statusLabel_User.Size = new Size(200, 20);
            this.statusLabel_User.TextAlign = ContentAlignment.MiddleLeft;
            
            this.statusLabel_Date = new ToolStripStatusLabel();
            this.statusLabel_Date.Text = "التاريخ: ";
            this.statusLabel_Date.AutoSize = false;
            this.statusLabel_Date.Size = new Size(150, 20);
            this.statusLabel_Date.TextAlign = ContentAlignment.MiddleLeft;
            
            this.statusLabel_Time = new ToolStripStatusLabel();
            this.statusLabel_Time.Text = "الوقت: ";
            this.statusLabel_Time.AutoSize = false;
            this.statusLabel_Time.Size = new Size(150, 20);
            this.statusLabel_Time.TextAlign = ContentAlignment.MiddleLeft;
            this.statusLabel_Time.Spring = true;
            
            this.statusStrip1.Items.Add(this.statusLabel_User);
            this.statusStrip1.Items.Add(this.statusLabel_Date);
            this.statusStrip1.Items.Add(this.statusLabel_Time);
            
            // Panel Content
            this.panel_Content.Dock = DockStyle.Fill;
            this.panel_Content.Location = new Point(0, 30);
            this.panel_Content.Size = new Size(1200, 620);
            this.panel_Content.BackColor = Color.FromArgb(241, 245, 249);
            
            // Timer
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(this.Timer1_Tick);
            
            // Form_Main
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.panel_Content);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_Main";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "شبكة المنيف للصرافة - النظام الرئيسي";
            this.WindowState = FormWindowState.Maximized;
            
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            // التحقق من تسجيل الدخول
            if (!DatabaseHelper.IsLoggedIn)
            {
                using (Form_Login login = new Form_Login())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }
            }
            
            // تحديث معلومات المستخدم في شريط الحالة
            UpdateStatusBar();
            
            // بدء المؤقت
            timer1.Start();
            
            // عرض رسالة ترحيب
            MessageBox.Show(
                "مرحباً بك في نظام شبكة المنيف للصرافة\n\n" +
                "المستخدم: " + DatabaseHelper.CurrentUserName + "\n" +
                "الوكيل: " + DatabaseHelper.CurrentAgentName,
                "مرحباً",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من إغلاق البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            statusLabel_User.Text = "المستخدم: " + DatabaseHelper.CurrentUserName;
            statusLabel_Date.Text = "التاريخ: " + DateTime.Now.ToString("dd/MM/yyyy");
            statusLabel_Time.Text = "الوقت: " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void Menu_Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من إغلاق البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Menu_NewTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Send_Money form = new Form_Send_Money();
                form.MdiParent = this;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح نافذة الحوالات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Menu_EditTransfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("نافذة تعديل الحوالات - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_DeleteTransfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("نافذة حذف الحوالات - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_AgentsData_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Agents form = new Form_Agents();
                form.MdiParent = this;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح نافذة الوكلاء: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Menu_Commissions_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Agents form = new Form_Agents();
                form.tabControl1.SelectedIndex = 1; // التبويب الثاني للعمولات
                form.MdiParent = this;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح نافذة العمولات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Menu_Ceilings_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Agents form = new Form_Agents();
                form.tabControl1.SelectedIndex = 2; // التبويب الثالث للتسقيف
                form.MdiParent = this;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح نافذة التسقيف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Menu_DailyReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("التقرير اليومي - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_MonthlyReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("التقرير الشهري - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_UserSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("إعدادات المستخدم - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_ChangePassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("تغيير كلمة المرور - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "شبكة المنيف للصرافة\n" +
                "نظام إدارة الحوالات المالية\n" +
                "الإصدار 1.0\n\n" +
                "جميع الحقوق محفوظة © 2026",
                "حول البرنامج",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}