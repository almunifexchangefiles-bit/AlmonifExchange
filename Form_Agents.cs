using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AlmonifExchange
{
    public partial class Form_Agents : Form
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl1;
        private TabPage tabAgents, tabCommissions, tabCeilings;
        
        // تبويب الوكلاء
        private DataGridView dgvAgents;
        private TextBox txtSearchAgent;
        private Button btnAddAgent, btnEditAgent, btnDeleteAgent, btnRefreshAgent;
        private TextBox txtAgentName, txtAgentEntity, txtAgentPhone, txtAgentAddress;
        
        // تبويب العمولات
        private DataGridView dgvCommissions;
        private ComboBox comboBoxCommissionAgent;
        private TextBox textBoxCommissionFrom, textBoxCommissionTo, textBoxCommissionAmount;
        private Button btnAddCommission, btnEditCommission, btnDeleteCommission;
        private Label labelCommissionAgent, labelCommissionFrom, labelCommissionTo, labelCommissionAmount;
        
        // تبويب التسقيف
        private DataGridView dgvCeilings;
        private ComboBox comboBoxCeilingAccount;
        private TextBox textBoxCeilingAmount;
        private Button btnAddCeiling, btnEditCeiling, btnDeleteCeiling;
        private Label labelCeilingAccount, labelCeilingAmount;

        public Form_Agents()
        {
            InitializeComponent();
            DatabaseHelper.LoadApplicationIcon(this);
            this.Load += Form_Agents_Load;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new TabControl();
            this.tabAgents = new TabPage();
            this.tabCommissions = new TabPage();
            this.tabCeilings = new TabPage();
            
            // إعدادات التبويبات
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Font = new Font("Arial", 11F, FontStyle.Bold);
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Size = new Size(1200, 700);
            this.tabControl1.TabIndex = 0;
            
            // تبويب الوكلاء
            this.tabAgents.Text = "دليل الوكلاء";
            this.tabAgents.Padding = new Padding(10);
            
            // تبويب العمولات
            this.tabCommissions.Text = "عمولات الحسابات";
            this.tabCommissions.Padding = new Padding(10);
            
            // تبويب التسقيف
            this.tabCeilings.Text = "تسقيف العملاء";
            this.tabCeilings.Padding = new Padding(10);
            
            this.tabControl1.Controls.Add(this.tabAgents);
            this.tabControl1.Controls.Add(this.tabCommissions);
            this.tabControl1.Controls.Add(this.tabCeilings);
            
            // إعدادات النموذج
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form_Agents";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "إدارة الوكلاء والعمولات";
            this.WindowState = FormWindowState.Maximized;
            
            this.tabControl1.ResumeLayout(false);
            this.tabAgents.ResumeLayout(false);
            this.tabCommissions.ResumeLayout(false);
            this.tabCeilings.ResumeLayout(false);
            this.ResumeLayout(false);
            
            // بناء محتوى كل تبويب
            BuildAgentsTab();
            BuildCommissionsTab();
            BuildCeilingsTab();
        }

        private void BuildAgentsTab()
        {
            // لوحة التحكم العلوية
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 60;
            topPanel.BackColor = Color.FromArgb(241, 245, 249);
            
            txtSearchAgent = new TextBox();
            txtSearchAgent.Location = new Point(20, 20);
            txtSearchAgent.Size = new Size(300, 26);
            txtSearchAgent.Font = new Font("Arial", 10F);
            txtSearchAgent.PlaceholderText = "بحث عن وكيل...";
            txtSearchAgent.TextChanged += TxtSearchAgent_TextChanged;
            
            btnAddAgent = new Button();
            btnAddAgent.Text = "إضافة";
            btnAddAgent.Location = new Point(340, 15);
            btnAddAgent.Size = new Size(100, 35);
            btnAddAgent.BackColor = Color.FromArgb(136, 19, 55);
            btnAddAgent.ForeColor = Color.White;
            btnAddAgent.FlatStyle = FlatStyle.Flat;
            btnAddAgent.Click += BtnAddAgent_Click;
            
            btnEditAgent = new Button();
            btnEditAgent.Text = "تعديل";
            btnEditAgent.Location = new Point(450, 15);
            btnEditAgent.Size = new Size(100, 35);
            btnEditAgent.BackColor = Color.FromArgb(212, 175, 55);
            btnEditAgent.ForeColor = Color.White;
            btnEditAgent.FlatStyle = FlatStyle.Flat;
            btnEditAgent.Click += BtnEditAgent_Click;
            
            btnDeleteAgent = new Button();
            btnDeleteAgent.Text = "حذف";
            btnDeleteAgent.Location = new Point(560, 15);
            btnDeleteAgent.Size = new Size(100, 35);
            btnDeleteAgent.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteAgent.ForeColor = Color.White;
            btnDeleteAgent.FlatStyle = FlatStyle.Flat;
            btnDeleteAgent.Click += BtnDeleteAgent_Click;
            
            btnRefreshAgent = new Button();
            btnRefreshAgent.Text = "تحديث";
            btnRefreshAgent.Location = new Point(670, 15);
            btnRefreshAgent.Size = new Size(100, 35);
            btnRefreshAgent.BackColor = Color.FromArgb(15, 23, 42);
            btnRefreshAgent.ForeColor = Color.White;
            btnRefreshAgent.FlatStyle = FlatStyle.Flat;
            btnRefreshAgent.Click += BtnRefreshAgent_Click;
            
            topPanel.Controls.Add(txtSearchAgent);
            topPanel.Controls.Add(btnAddAgent);
            topPanel.Controls.Add(btnEditAgent);
            topPanel.Controls.Add(btnDeleteAgent);
            topPanel.Controls.Add(btnRefreshAgent);
            
            // جدول الوكلاء
            dgvAgents = new DataGridView();
            dgvAgents.Dock = DockStyle.Fill;
            dgvAgents.AllowUserToAddRows = false;
            dgvAgents.AllowUserToDeleteRows = false;
            dgvAgents.ReadOnly = true;
            dgvAgents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAgents.MultiSelect = false;
            dgvAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAgents.BackgroundColor = Color.White;
            dgvAgents.BorderStyle = BorderStyle.None;
            dgvAgents.EnableHeadersVisualStyles = false;
            dgvAgents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvAgents.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
            dgvAgents.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Bold);
            dgvAgents.ColumnHeadersHeight = 45;
            dgvAgents.RowHeadersVisible = false;
            dgvAgents.RowsDefaultCellStyle.BackColor = Color.White;
            dgvAgents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvAgents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAgents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(136, 19, 55);
            dgvAgents.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAgents.DefaultCellStyle.Font = new Font("Arial", 10F);
            dgvAgents.RowTemplate.Height = 35;
            dgvAgents.CellClick += DgvAgents_CellClick;
            
            tabAgents.Controls.Add(dgvAgents);
            tabAgents.Controls.Add(topPanel);
        }

        private void BuildCommissionsTab()
        {
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 120;
            topPanel.BackColor = Color.FromArgb(241, 245, 249);
            
            labelCommissionAgent = new Label();
            labelCommissionAgent.Text = "اسم الوكيل:";
            labelCommissionAgent.Location = new Point(20, 20);
            labelCommissionAgent.AutoSize = true;
            labelCommissionAgent.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            comboBoxCommissionAgent = new ComboBox();
            comboBoxCommissionAgent.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxCommissionAgent.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCommissionAgent.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxCommissionAgent.Location = new Point(120, 15);
            comboBoxCommissionAgent.Size = new Size(260, 27);
            comboBoxCommissionAgent.Font = new Font("Arial", 10F);
            
            labelCommissionFrom = new Label();
            labelCommissionFrom.Text = "من مبلغ:";
            labelCommissionFrom.Location = new Point(400, 20);
            labelCommissionFrom.AutoSize = true;
            labelCommissionFrom.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            textBoxCommissionFrom = new TextBox();
            textBoxCommissionFrom.Location = new Point(490, 15);
            textBoxCommissionFrom.Size = new Size(120, 26);
            textBoxCommissionFrom.Font = new Font("Arial", 10F);
            
            labelCommissionTo = new Label();
            labelCommissionTo.Text = "إلى مبلغ:";
            labelCommissionTo.Location = new Point(630, 20);
            labelCommissionTo.AutoSize = true;
            labelCommissionTo.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            textBoxCommissionTo = new TextBox();
            textBoxCommissionTo.Location = new Point(720, 15);
            textBoxCommissionTo.Size = new Size(120, 26);
            textBoxCommissionTo.Font = new Font("Arial", 10F);
            
            labelCommissionAmount = new Label();
            labelCommissionAmount.Text = "العمولة:";
            labelCommissionAmount.Location = new Point(860, 20);
            labelCommissionAmount.AutoSize = true;
            labelCommissionAmount.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            textBoxCommissionAmount = new TextBox();
            textBoxCommissionAmount.Location = new Point(940, 15);
            textBoxCommissionAmount.Size = new Size(120, 26);
            textBoxCommissionAmount.Font = new Font("Arial", 10F);
            
            btnAddCommission = new Button();
            btnAddCommission.Text = "إضافة";
            btnAddCommission.Location = new Point(20, 60);
            btnAddCommission.Size = new Size(100, 35);
            btnAddCommission.BackColor = Color.FromArgb(136, 19, 55);
            btnAddCommission.ForeColor = Color.White;
            btnAddCommission.FlatStyle = FlatStyle.Flat;
            btnAddCommission.Click += BtnAddCommission_Click;
            
            btnEditCommission = new Button();
            btnEditCommission.Text = "تعديل";
            btnEditCommission.Location = new Point(130, 60);
            btnEditCommission.Size = new Size(100, 35);
            btnEditCommission.BackColor = Color.FromArgb(212, 175, 55);
            btnEditCommission.ForeColor = Color.White;
            btnEditCommission.FlatStyle = FlatStyle.Flat;
            btnEditCommission.Click += BtnEditCommission_Click;
            
            btnDeleteCommission = new Button();
            btnDeleteCommission.Text = "حذف";
            btnDeleteCommission.Location = new Point(240, 60);
            btnDeleteCommission.Size = new Size(100, 35);
            btnDeleteCommission.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteCommission.ForeColor = Color.White;
            btnDeleteCommission.FlatStyle = FlatStyle.Flat;
            btnDeleteCommission.Click += BtnDeleteCommission_Click;
            
            topPanel.Controls.Add(labelCommissionAgent);
            topPanel.Controls.Add(comboBoxCommissionAgent);
            topPanel.Controls.Add(labelCommissionFrom);
            topPanel.Controls.Add(textBoxCommissionFrom);
            topPanel.Controls.Add(labelCommissionTo);
            topPanel.Controls.Add(textBoxCommissionTo);
            topPanel.Controls.Add(labelCommissionAmount);
            topPanel.Controls.Add(textBoxCommissionAmount);
            topPanel.Controls.Add(btnAddCommission);
            topPanel.Controls.Add(btnEditCommission);
            topPanel.Controls.Add(btnDeleteCommission);
            
            dgvCommissions = new DataGridView();
            dgvCommissions.Dock = DockStyle.Fill;
            dgvCommissions.AllowUserToAddRows = false;
            dgvCommissions.AllowUserToDeleteRows = false;
            dgvCommissions.ReadOnly = true;
            dgvCommissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCommissions.MultiSelect = false;
            dgvCommissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCommissions.BackgroundColor = Color.White;
            dgvCommissions.BorderStyle = BorderStyle.None;
            dgvCommissions.EnableHeadersVisualStyles = false;
            dgvCommissions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvCommissions.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
            dgvCommissions.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Bold);
            dgvCommissions.ColumnHeadersHeight = 45;
            dgvCommissions.RowHeadersVisible = false;
            dgvCommissions.RowsDefaultCellStyle.BackColor = Color.White;
            dgvCommissions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvCommissions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCommissions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(136, 19, 55);
            dgvCommissions.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCommissions.DefaultCellStyle.Font = new Font("Arial", 10F);
            dgvCommissions.RowTemplate.Height = 35;
            dgvCommissions.CellClick += DgvCommissions_CellClick;
            
            tabCommissions.Controls.Add(dgvCommissions);
            tabCommissions.Controls.Add(topPanel);
        }

        private void BuildCeilingsTab()
        {
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;
            topPanel.BackColor = Color.FromArgb(241, 245, 249);
            
            labelCeilingAccount = new Label();
            labelCeilingAccount.Text = "رقم الحساب:";
            labelCeilingAccount.Location = new Point(20, 20);
            labelCeilingAccount.AutoSize = true;
            labelCeilingAccount.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            comboBoxCeilingAccount = new ComboBox();
            comboBoxCeilingAccount.DropDownStyle = ComboBoxStyle.DropDown;
            comboBoxCeilingAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCeilingAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxCeilingAccount.Location = new Point(120, 15);
            comboBoxCeilingAccount.Size = new Size(260, 27);
            comboBoxCeilingAccount.Font = new Font("Arial", 10F);
            
            labelCeilingAmount = new Label();
            labelCeilingAmount.Text = "مبلغ السقف:";
            labelCeilingAmount.Location = new Point(400, 20);
            labelCeilingAmount.AutoSize = true;
            labelCeilingAmount.Font = new Font("Arial", 10F, FontStyle.Bold);
            
            textBoxCeilingAmount = new TextBox();
            textBoxCeilingAmount.Location = new Point(500, 15);
            textBoxCeilingAmount.Size = new Size(200, 26);
            textBoxCeilingAmount.Font = new Font("Arial", 10F);
            
            btnAddCeiling = new Button();
            btnAddCeiling.Text = "إضافة";
            btnAddCeiling.Location = new Point(720, 15);
            btnAddCeiling.Size = new Size(100, 35);
            btnAddCeiling.BackColor = Color.FromArgb(136, 19, 55);
            btnAddCeiling.ForeColor = Color.White;
            btnAddCeiling.FlatStyle = FlatStyle.Flat;
            btnAddCeiling.Click += BtnAddCeiling_Click;
            
            btnEditCeiling = new Button();
            btnEditCeiling.Text = "تعديل";
            btnEditCeiling.Location = new Point(830, 15);
            btnEditCeiling.Size = new Size(100, 35);
            btnEditCeiling.BackColor = Color.FromArgb(212, 175, 55);
            btnEditCeiling.ForeColor = Color.White;
            btnEditCeiling.FlatStyle = FlatStyle.Flat;
            btnEditCeiling.Click += BtnEditCeiling_Click;
            
            btnDeleteCeiling = new Button();
            btnDeleteCeiling.Text = "حذف";
            btnDeleteCeiling.Location = new Point(940, 15);
            btnDeleteCeiling.Size = new Size(100, 35);
            btnDeleteCeiling.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteCeiling.ForeColor = Color.White;
            btnDeleteCeiling.FlatStyle = FlatStyle.Flat;
            btnDeleteCeiling.Click += BtnDeleteCeiling_Click;
            
            topPanel.Controls.Add(labelCeilingAccount);
            topPanel.Controls.Add(comboBoxCeilingAccount);
            topPanel.Controls.Add(labelCeilingAmount);
            topPanel.Controls.Add(textBoxCeilingAmount);
            topPanel.Controls.Add(btnAddCeiling);
            topPanel.Controls.Add(btnEditCeiling);
            topPanel.Controls.Add(btnDeleteCeiling);
            
            dgvCeilings = new DataGridView();
            dgvCeilings.Dock = DockStyle.Fill;
            dgvCeilings.AllowUserToAddRows = false;
            dgvCeilings.AllowUserToDeleteRows = false;
            dgvCeilings.ReadOnly = true;
            dgvCeilings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCeilings.MultiSelect = false;
            dgvCeilings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCeilings.BackgroundColor = Color.White;
            dgvCeilings.BorderStyle = BorderStyle.None;
            dgvCeilings.EnableHeadersVisualStyles = false;
            dgvCeilings.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvCeilings.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
            dgvCeilings.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Bold);
            dgvCeilings.ColumnHeadersHeight = 45;
            dgvCeilings.RowHeadersVisible = false;
            dgvCeilings.RowsDefaultCellStyle.BackColor = Color.White;
            dgvCeilings.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvCeilings.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCeilings.DefaultCellStyle.SelectionBackColor = Color.FromArgb(136, 19, 55);
            dgvCeilings.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCeilings.DefaultCellStyle.Font = new Font("Arial", 10F);
            dgvCeilings.RowTemplate.Height = 35;
            dgvCeilings.CellClick += DgvCeilings_CellClick;
            
            tabCeilings.Controls.Add(dgvCeilings);
            tabCeilings.Controls.Add(topPanel);
        }

        private void Form_Agents_Load(object sender, EventArgs e)
        {
            LoadAgentsData();
            LoadCommissionsData();
            LoadCeilingsData();
            LoadAgentNames();
        }

        private void LoadAgentsData()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT [اسم الوكيل 1] AS AgentName, [الجهة] AS Entity, [رقم الهاتف] AS Phone, [العنوان] AS Address FROM [بيانات الوكلاء]";
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvAgents.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات الوكلاء: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCommissionsData()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT [اسم الوكيل] AS AgentName, [من مبلغ] AS FromAmount, [إلى مبلغ] AS ToAmount, [مبلغ العمولة] AS CommissionAmount FROM [عمولات الحسابات]";
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCommissions.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات العمولات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCeilingsData()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT [حساب السقف] AS Account, [مبلغ السقف] AS CeilingAmount FROM [بيانات الوكلاء] WHERE [حساب السقف] IS NOT NULL AND [حساب السقف] != ''";
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCeilings.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات التسقيف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAgentNames()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT DISTINCT [اسم الوكيل 1] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] IS NOT NULL";
                
                comboBoxCommissionAgent.Items.Clear();
                comboBoxCeilingAccount.Items.Clear();
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string agentName = reader["اسم الوكيل 1"].ToString().Trim();
                                if (!string.IsNullOrEmpty(agentName))
                                {
                                    comboBoxCommissionAgent.Items.Add(agentName);
                                    comboBoxCeilingAccount.Items.Add(agentName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل أسماء الوكلاء: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearchAgent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearchAgent.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    LoadAgentsData();
                    return;
                }
                
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT [اسم الوكيل 1] AS AgentName, [الجهة] AS Entity, [رقم الهاتف] AS Phone, [العنوان] AS Address FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] LIKE @search";
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvAgents.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في البحث: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvAgents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAgents.Rows[e.RowIndex];
                // يمكن إضافة منطق لعرض البيانات في حقول التعديل هنا
            }
        }

        private void DgvCommissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCommissions.Rows[e.RowIndex];
                if (row.Cells["AgentName"].Value != null)
                    comboBoxCommissionAgent.Text = row.Cells["AgentName"].Value.ToString();
                if (row.Cells["FromAmount"].Value != null)
                    textBoxCommissionFrom.Text = row.Cells["FromAmount"].Value.ToString().Replace(",", "");
                if (row.Cells["ToAmount"].Value != null)
                    textBoxCommissionTo.Text = row.Cells["ToAmount"].Value.ToString().Replace(",", "");
                if (row.Cells["CommissionAmount"].Value != null)
                    textBoxCommissionAmount.Text = row.Cells["CommissionAmount"].Value.ToString().Replace(",", "");
            }
        }

        private void DgvCeilings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCeilings.Rows[e.RowIndex];
                if (row.Cells["Account"].Value != null)
                    comboBoxCeilingAccount.Text = row.Cells["Account"].Value.ToString();
                if (row.Cells["CeilingAmount"].Value != null)
                    textBoxCeilingAmount.Text = row.Cells["CeilingAmount"].Value.ToString().Replace(",", "");
            }
        }

        private void BtnAddAgent_Click(object sender, EventArgs e)
        {
            MessageBox.Show("نافذة إضافة وكيل جديد - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditAgent_Click(object sender, EventArgs e)
        {
            if (dgvAgents.SelectedRows.Count > 0)
            {
                MessageBox.Show("نافذة تعديل الوكيل - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("الرجاء تحديد وكيل للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteAgent_Click(object sender, EventArgs e)
        {
            if (dgvAgents.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذا الوكيل؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAgentsData();
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد وكيل للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefreshAgent_Click(object sender, EventArgs e)
        {
            LoadAgentsData();
            txtSearchAgent.Clear();
        }

        private void BtnAddCommission_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comboBoxCommissionAgent.Text) || 
                    string.IsNullOrEmpty(textBoxCommissionFrom.Text) || 
                    string.IsNullOrEmpty(textBoxCommissionTo.Text) || 
                    string.IsNullOrEmpty(textBoxCommissionAmount.Text))
                {
                    MessageBox.Show("الرجاء تعبئة جميع الحقول", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal fromAmount, toAmount, commissionAmount;
                if (!decimal.TryParse(textBoxCommissionFrom.Text.Trim(), out fromAmount) ||
                    !decimal.TryParse(textBoxCommissionTo.Text.Trim(), out toAmount) ||
                    !decimal.TryParse(textBoxCommissionAmount.Text.Trim(), out commissionAmount))
                {
                    MessageBox.Show("الرجاء إدخال أرقام صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string connString = DatabaseHelper.GetConnectionString();
                string query = "INSERT INTO [عمولات الحسابات] ([اسم الوكيل], [من مبلغ], [إلى مبلغ], [مبلغ العمولة]) VALUES (@agent, @from, @to, @comm)";
                
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@agent", comboBoxCommissionAgent.Text);
                        cmd.Parameters.AddWithValue("@from", fromAmount);
                        cmd.Parameters.AddWithValue("@to", toAmount);
                        cmd.Parameters.AddWithValue("@comm", commissionAmount);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تمت إضافة العمولة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCommissionsData();
                comboBoxCommissionAgent.Text = "";
                textBoxCommissionFrom.Text = "";
                textBoxCommissionTo.Text = "";
                textBoxCommissionAmount.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحفظ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditCommission_Click(object sender, EventArgs e)
        {
            if (dgvCommissions.SelectedRows.Count > 0)
            {
                MessageBox.Show("نافذة تعديل العمولة - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("الرجاء تحديد عمولة للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteCommission_Click(object sender, EventArgs e)
        {
            if (dgvCommissions.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذه العمولة؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        DataGridViewRow row = dgvCommissions.SelectedRows[0];
                        string agentName = row.Cells["AgentName"].Value.ToString();
                        string fromAmount = row.Cells["FromAmount"].Value.ToString();
                        
                        string connString = DatabaseHelper.GetConnectionString();
                        string query = "DELETE FROM [عمولات الحسابات] WHERE [اسم الوكيل] = @agent AND [من مبلغ] = @from";
                        
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@agent", agentName);
                                cmd.Parameters.AddWithValue("@from", fromAmount);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCommissionsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("خطأ في الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد عمولة للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAddCeiling_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comboBoxCeilingAccount.Text) || string.IsNullOrEmpty(textBoxCeilingAmount.Text))
                {
                    MessageBox.Show("الرجاء تعبئة جميع الحقول", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal ceilingAmount;
                if (!decimal.TryParse(textBoxCeilingAmount.Text.Trim(), out ceilingAmount))
                {
                    MessageBox.Show("الرجاء إدخال رقم صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ceilingAmount <= 0)
                {
                    MessageBox.Show("مبلغ السقف يجب أن يكون أكبر من صفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string connString = DatabaseHelper.GetConnectionString();
                
                // التحقق من وجود سقف مسبق
                string checkQuery = "SELECT COUNT(1) FROM [بيانات الوكلاء] WHERE [حساب السقف] = @account";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@account", comboBoxCeilingAccount.Text);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        if (count > 0)
                        {
                            MessageBox.Show("هذا الحساب لديه سقف مسبقاً. استخدم زر التعديل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                string updateQuery = "UPDATE [بيانات الوكلاء] SET [حساب السقف] = @account, [مبلغ السقف] = @amount WHERE [اسم الوكيل 1] = @agent";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@account", comboBoxCeilingAccount.Text);
                        cmd.Parameters.AddWithValue("@amount", ceilingAmount);
                        cmd.Parameters.AddWithValue("@agent", comboBoxCeilingAccount.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تمت إضافة السقف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCeilingsData();
                comboBoxCeilingAccount.Text = "";
                textBoxCeilingAmount.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحفظ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditCeiling_Click(object sender, EventArgs e)
        {
            if (dgvCeilings.SelectedRows.Count > 0)
            {
                MessageBox.Show("نافذة تعديل السقف - قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("الرجاء تحديد سقف للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteCeiling_Click(object sender, EventArgs e)
        {
            if (dgvCeilings.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذا السقف؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        DataGridViewRow row = dgvCeilings.SelectedRows[0];
                        string account = row.Cells["Account"].Value.ToString();
                        
                        string connString = DatabaseHelper.GetConnectionString();
                        string query = "UPDATE [بيانات الوكلاء] SET [حساب السقف] = NULL, [مبلغ السقف] = NULL WHERE [حساب السقف] = @account";
                        
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@account", account);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCeilingsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("خطأ في الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد سقف للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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