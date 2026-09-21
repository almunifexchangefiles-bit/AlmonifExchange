using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace AlmonifExchange
{
    public class Form_Login : Form
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtPass, txtusername;
        private Label label1, label2, label3, label4, label5, label6, label7, label_wait, label_Chek_DataInLogin;
        private Button button_Login, button_Exit, button_Setting, button1;
        private CheckBox check_Save_dataLogin;
        private PictureBox pictureBox1, pictureBox2, pictureBox3;
        private Panel panel1, panel2;
        private GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Timer timer1;

        public Form_Login()
        {
            InitializeComponent();
            DatabaseHelper.LoadApplicationIcon(this);
            try { this.Icon = new System.Drawing.Icon(Path.Combine(Application.StartupPath, "ايقونة النظام.ico")); } catch { }
            this.Load += new EventHandler(Form_Login_Load);
            button_Login.Image = CreateCircleIcon(Color.FromArgb(34, 139, 34), "\u2713", 26);
            button_Login.ImageAlign = ContentAlignment.MiddleRight;
            button_Login.TextImageRelation = TextImageRelation.TextBeforeImage;
            button_Login.TextAlign = ContentAlignment.MiddleLeft;
            button_Login.FlatStyle = FlatStyle.Flat;
            button_Exit.Image = CreateCircleIcon(Color.FromArgb(200, 30, 30), "\u2717", 26);
            button_Exit.ImageAlign = ContentAlignment.MiddleRight;
            button_Exit.TextImageRelation = TextImageRelation.TextBeforeImage;
            button_Exit.TextAlign = ContentAlignment.MiddleLeft;
            button_Exit.FlatStyle = FlatStyle.Flat;
        }

        private Bitmap CreateCircleIcon(Color color, string symbol, int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.Transparent);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                { g.FillEllipse(shadowBrush, 2, 2, size - 2, size - 2); }
                using (SolidBrush brush = new SolidBrush(color))
                { g.FillEllipse(brush, 1, 1, size - 3, size - 3); }
                using (Pen pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1.5f))
                { g.DrawEllipse(pen, 1, 1, size - 3, size - 3); }
                using (Font font = new Font("Segoe UI Symbol", size * 0.5f, FontStyle.Bold))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(symbol, font, Brushes.White, new RectangleF(0, 0, size, size), sf);
                }
            }
            return bmp;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtPass = new TextBox();
            this.txtusername = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.panel1 = new Panel();
            this.label_wait = new Label();
            this.button_Setting = new Button();
            this.panel2 = new Panel();
            this.groupBox1 = new GroupBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.button1 = new Button();
            this.button_Login = new Button();
            this.pictureBox1 = new PictureBox();
            this.pictureBox3 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.label3 = new Label();
            this.check_Save_dataLogin = new CheckBox();
            this.label_Chek_DataInLogin = new Label();
            this.button_Exit = new Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            this.txtPass.BackColor = Color.White;
            this.txtPass.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.txtPass.Location = new Point(108, 126);
            this.txtPass.Size = new Size(158, 27);
            this.txtPass.TabIndex = 1;
            this.txtPass.TextAlign = HorizontalAlignment.Center;
            this.txtPass.UseSystemPasswordChar = true;
            this.txtPass.KeyDown += new KeyEventHandler(this.TxtPass_KeyDown);
            this.txtusername.BorderStyle = BorderStyle.FixedSingle;
            this.txtusername.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.txtusername.Location = new Point(108, 88);
            this.txtusername.Size = new Size(158, 27);
            this.txtusername.TabIndex = 0;
            this.txtusername.TextAlign = HorizontalAlignment.Center;
            this.txtusername.KeyDown += new KeyEventHandler(this.Txtusername_KeyDown);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.label1.Location = new Point(271, 91);
            this.label1.Text = ": أسـم المستخدم  ";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.label2.Location = new Point(271, 130);
            this.label2.Text = ": كـلمــة الســـر  ";
            this.panel1.BackColor = Color.White;
            this.panel1.Controls.Add(this.label_wait);
            this.panel1.Controls.Add(this.button_Setting);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button_Login);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.check_Save_dataLogin);
            this.panel1.Controls.Add(this.label_Chek_DataInLogin);
            this.panel1.Controls.Add(this.txtPass);
            this.panel1.Controls.Add(this.button_Exit);
            this.panel1.Controls.Add(this.txtusername);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Size = new Size(409, 293);
            this.label_wait.AutoSize = true;
            this.label_wait.Font = new Font("Arial", 11F, FontStyle.Bold);
            this.label_wait.Location = new Point(116, 58);
            this.label_wait.Text = "تسجيـــل الـدخـــول إلـى النظــــام ";
            this.button_Setting.Enabled = false;
            this.button_Setting.Location = new Point(6, 203);
            this.button_Setting.Size = new Size(104, 46);
            this.button_Setting.Text = "إعدادات الإتصال ";
            this.button_Setting.Visible = false;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 257);
            this.panel2.Size = new Size(409, 36);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = DockStyle.Fill;
            this.groupBox1.Font = new Font("Arial", 9.5F, FontStyle.Bold);
            this.groupBox1.RightToLeft = RightToLeft.Yes;
            this.groupBox1.Text = "قسم الدعم الفني: ";
            this.label7.Text = "770222512 ";
            this.label6.Text = "778577858 ";
            this.label5.Text = "734864457 ";
            this.label4.Text = "778577857 ";
            this.label7.AutoSize = true;
            this.label6.AutoSize = true;
            this.label5.AutoSize = true;
            this.label4.AutoSize = true;
            this.label7.Location = new Point(13, 17);
            this.label6.Location = new Point(105, 18);
            this.label5.Location = new Point(207, 18);
            this.label4.Location = new Point(303, 18);
            this.button1.Visible = false;
            this.button_Login.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.button_Login.Location = new Point(235, 203);
            this.button_Login.Size = new Size(92, 46);
            this.button_Login.Text = "موافق ";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new EventHandler(this.Button_Login_Click);
            this.pictureBox1.Location = new Point(32, 88);
            this.pictureBox1.Size = new Size(71, 62);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Visible = true;
            this.pictureBox2.Location = new Point(320, 15);
            this.pictureBox2.Size = new Size(71, 64);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox2.Visible = true;
            this.pictureBox3.Location = new Point(10, 10);
            this.pictureBox3.Size = new Size(141, 75);
            this.pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox3.Visible = true;
            try
            {
                string appPath = Application.StartupPath;
                string img1 = Path.Combine(appPath, "logo1.png");
                string img2 = Path.Combine(appPath, "logo2.png");
                string img3 = Path.Combine(appPath, "logo3.png");
                if (File.Exists(img1)) this.pictureBox1.Image = Image.FromFile(img1);
                if (File.Exists(img2)) this.pictureBox2.Image = Image.FromFile(img2);
                if (File.Exists(img3)) this.pictureBox3.Image = Image.FromFile(img3);
            }
            catch { }
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.White;
            this.label3.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            this.label3.ForeColor = SystemColors.Highlight;
            this.label3.Location = new Point(362, 225);
            this.label3.Text = "1.11  ";
            this.check_Save_dataLogin.AutoSize = true;
            this.check_Save_dataLogin.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.check_Save_dataLogin.Location = new Point(105, 161);
            this.check_Save_dataLogin.Text = "حفظ بيانات الدخول  ";
            this.button_Exit.DialogResult = DialogResult.Cancel;
            this.button_Exit.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.button_Exit.Location = new Point(98, 203);
            this.button_Exit.Size = new Size(92, 46);
            this.button_Exit.Text = "خروج  ";
            this.button_Exit.Click += new EventHandler(this.Button_Exit_Click);
            this.label_Chek_DataInLogin.Text = "  ";
            this.label_Chek_DataInLogin.Visible = false;
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.ButtonFace;
            this.CancelButton = this.button_Exit;
            this.ClientSize = new Size(409, 293);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Login";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "شبكة المنيف نضام الصراف - تسجيل الدخول";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
        }

        private void Button_Exit_Click(object sender, EventArgs e) { Application.Exit(); }

        private void Form_Login_Load(object sender, EventArgs e)
        {
            try
            {
                string cfgPath = Path.Combine(Application.StartupPath, "login.cfg");
                if (File.Exists(cfgPath))
                {
                    string[] lines = File.ReadAllLines(cfgPath);
                    if (lines.Length > 0 && lines[0] == "true")
                    {
                        check_Save_dataLogin.Checked = true;
                        if (lines.Length > 1) txtusername.Text = lines[1];
                    }
                }
            }
            catch { }
            txtusername.Focus();
        }

        private void Txtusername_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) txtPass.Focus(); }
        private void TxtPass_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) button_Login.PerformClick(); }

        private string GetPasswordString(object obj)
        {
            if (obj == null || obj == DBNull.Value) return "";
            byte[] bytes = obj as byte[];
            if (bytes != null)
            {
                try { return Encoding.GetEncoding(1256).GetString(bytes); }
                catch { return Encoding.ASCII.GetString(bytes); }
            }
            return obj.ToString();
        }

        private byte[][] GetKeyCandidates(int len)
        {
            byte[] prefix = new byte[] { 222, 221, 237, 228, 227, 225, 199, 211 };
            if (len <= 0) return new byte[][] { };
            if (len <= 8)
            {
                byte[] key = new byte[len];
                for (int i = 0; i < len; i++) key[i] = prefix[i];
                return new byte[][] { key };
            }
            if (len == 9)
            {
                byte[] k1 = new byte[] { 222, 221, 237, 228, 227, 225, 199, 211, 237 };
                byte[] k2 = new byte[] { 222, 221, 237, 228, 227, 225, 199, 211, 228 };
                return new byte[][] { k1, k2 };
            }
            if (len == 10)
            {
                byte[] k = new byte[] { 222, 221, 237, 228, 227, 225, 199, 211, 228, 199 };
                return new byte[][] { k };
            }
            byte[] fb = new byte[len];
            for (int i = 0; i < len; i++) fb[i] = (byte)(i < 8 ? prefix[i] : 222);
            return new byte[][] { fb };
        }

        private string XorTransform(string text, byte[] key)
        {
            Encoding enc = Encoding.GetEncoding(1256);
            byte[] bytes = enc.GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte k = (i < key.Length) ? key[i] : (byte)222;
                bytes[i] = (byte)(bytes[i] ^ k);
            }
            return enc.GetString(bytes);
        }

        private bool VerifyPassword(string inputPass, string storedPass)
        {
            if (inputPass == null) inputPass = "";
            if (storedPass == null) storedPass = "";
            inputPass = inputPass.Trim();
            storedPass = storedPass.Trim();
            if (inputPass.Length == 0) return false;
            if (string.Equals(inputPass, storedPass, StringComparison.Ordinal)) return true;
            byte[][] inputKeys = GetKeyCandidates(inputPass.Length);
            foreach (byte[] key in inputKeys)
            {
                if (string.Equals(XorTransform(inputPass, key), storedPass, StringComparison.Ordinal)) return true;
            }
            byte[][] storedKeys = GetKeyCandidates(storedPass.Length);
            foreach (byte[] key in storedKeys)
            {
                if (string.Equals(XorTransform(storedPass, key), inputPass, StringComparison.Ordinal)) return true;
            }
            return false;
        }

        private void Button_Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtusername.Text)) { MessageBox.Show("الرجاء إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtusername.Focus(); return; }
            if (string.IsNullOrEmpty(txtPass.Text)) { MessageBox.Show("الرجاء إدخال كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPass.Focus(); return; }

            string connectionString = DatabaseHelper.GetConnectionString();
            string query = "SELECT ID, UserName, UserPassword, UserGroupID, Notes FROM tblUsers WHERE UserName=@user AND UserState=N'ممكن'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", txtusername.Text.Trim());
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = GetPasswordString(reader["UserPassword"]);
                                if (VerifyPassword(txtPass.Text, storedPassword))
                                {
                                    int userId = Convert.ToInt32(reader["ID"]);
                                    string userName = reader["UserName"].ToString();
                                    string agentName = reader["Notes"] == DBNull.Value ? userName : reader["Notes"].ToString();
                                    if (string.IsNullOrEmpty(agentName)) agentName = userName;

                                    DatabaseHelper.SetCurrentUser(userId, userName, agentName);

                                    if (check_Save_dataLogin.Checked)
                                    {
                                        string cfg = Path.Combine(Application.StartupPath, "login.cfg");
                                        File.WriteAllLines(cfg, new string[] { "true", txtusername.Text.Trim() });
                                    }
                                    else
                                    {
                                        string cfg = Path.Combine(Application.StartupPath, "login.cfg");
                                        if (File.Exists(cfg)) File.Delete(cfg);
                                    }

                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                    return;
                                }
                                else { MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); txtPass.Clear(); txtPass.Focus(); }
                            }
                            else { MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); txtPass.Clear(); txtPass.Focus(); }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }
    }
}
