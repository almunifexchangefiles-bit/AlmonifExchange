using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AlmonifExchange
{
    public partial class Form_Agents : Form
    {
        private IContainer components = null;
        private Label labelHeader;
        private Panel panelMain, panelLeft, panelRight;
        private Button buttonAddAgent, buttonEditAgent, buttonDeleteAgent;
        private Button buttonAddCommission, buttonEditCommission, buttonDeleteCommission;
        private Button buttonAddCeiling, buttonEditCeiling, buttonDeleteCeiling;
        private Button buttonExit;
        private TextBox textBoxCommissionFrom, textBoxCommissionTo, textBoxCommissionAmount;
        private TextBox textBoxCeilingAmount;
        private ComboBox comboBoxAgentName, comboBoxEntity, comboBoxCommissionAgent;
        private ComboBox comboBoxCeilingAccount;
        private DataGridView dataGridViewAgents, dataGridViewCommissions, dataGridViewCeilings;
        private RadioButton radioNewAgent, radioNewEntity;
        private Label labelAgentName, labelEntity, labelCommissionAgent, labelCommissionFrom, labelCommissionTo, labelCommissionAmount;
        private Label labelSectionAgents, labelSectionCommissions, labelSectionCeilings;
        private Label labelCeilingAccount, labelCeilingAmount;

        public Form_Agents()
        {
            InitializeComponent();
            DatabaseHelper.LoadApplicationIcon(this);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.labelHeader = new Label();
            this.labelHeader.BackColor = Color.FromArgb(150, 0, 0);
            this.labelHeader.Dock = DockStyle.Top;
            this.labelHeader.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.labelHeader.ForeColor = Color.White;
            this.labelHeader.Location = new Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new Size(1218, 35);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "بيانات الوكلاء";
            this.labelHeader.TextAlign = ContentAlignment.MiddleCenter;

            this.panelMain = new Panel();
            this.panelMain.Dock = DockStyle.Fill;
            this.panelMain.Location = new Point(0, 35);
            this.panelMain.Name = "panelMain";
            this.panelMain.TabIndex = 1;

            // ========== القسم الأيمن (موسع ليشمل 3 أقسام) ==========
            this.panelRight = new Panel();
            this.panelRight.BorderStyle = BorderStyle.FixedSingle;
            this.panelRight.Location = new Point(650, 10);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new Size(550, 580);
            this.panelRight.TabIndex = 0;
            this.panelRight.AutoScroll = true;

            // ===== القسم 1: حسابات الوكلاء =====
            this.labelSectionAgents = new Label();
            this.labelSectionAgents.BackColor = Color.FromArgb(150, 0, 0);
            this.labelSectionAgents.Font = new Font("Arial", 11f, FontStyle.Bold);
            this.labelSectionAgents.ForeColor = Color.White;
            this.labelSectionAgents.Location = new Point(0, 0);
            this.labelSectionAgents.Name = "labelSectionAgents";
            this.labelSectionAgents.Size = new Size(548, 30);
            this.labelSectionAgents.TabIndex = 0;
            this.labelSectionAgents.Text = "حسابات الوكلاء (عمولات الوكلاء)";
            this.labelSectionAgents.TextAlign = ContentAlignment.MiddleCenter;

            this.labelAgentName = new Label();
            this.labelAgentName.AutoSize = true;
            this.labelAgentName.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelAgentName.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelAgentName.Location = new Point(420, 45);
            this.labelAgentName.Name = "labelAgentName";
            this.labelAgentName.Size = new Size(90, 19);
            this.labelAgentName.Text = "اسم الوكيل:";

            this.comboBoxAgentName = new ComboBox();
            this.comboBoxAgentName.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBoxAgentName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.comboBoxAgentName.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBoxAgentName.Location = new Point(150, 42);
            this.comboBoxAgentName.Name = "comboBoxAgentName";
            this.comboBoxAgentName.Size = new Size(260, 27);
            this.comboBoxAgentName.TabIndex = 0;
            this.comboBoxAgentName.Font = new Font("Arial", 9.5f, FontStyle.Bold);

            this.radioNewAgent = new RadioButton();
            this.radioNewAgent.AutoSize = true;
            this.radioNewAgent.Checked = true;
            this.radioNewAgent.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.radioNewAgent.Location = new Point(300, 80);
            this.radioNewAgent.Name = "radioNewAgent";
            this.radioNewAgent.Size = new Size(110, 24);
            this.radioNewAgent.TabIndex = 2;
            this.radioNewAgent.TabStop = true;
            this.radioNewAgent.Text = "وكيل جديد";
            this.radioNewAgent.UseVisualStyleBackColor = true;
            this.radioNewAgent.CheckedChanged += new EventHandler(this.radioNewAgent_CheckedChanged);

            this.radioNewEntity = new RadioButton();
            this.radioNewEntity.AutoSize = true;
            this.radioNewEntity.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.radioNewEntity.Location = new Point(150, 80);
            this.radioNewEntity.Name = "radioNewEntity";
            this.radioNewEntity.Size = new Size(115, 24);
            this.radioNewEntity.TabIndex = 3;
            this.radioNewEntity.Text = "جهة جديدة";
            this.radioNewEntity.UseVisualStyleBackColor = true;
            this.radioNewEntity.CheckedChanged += new EventHandler(this.radioNewEntity_CheckedChanged);

            this.labelEntity = new Label();
            this.labelEntity.AutoSize = true;
            this.labelEntity.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelEntity.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelEntity.Location = new Point(440, 115);
            this.labelEntity.Name = "labelEntity";
            this.labelEntity.Size = new Size(55, 19);
            this.labelEntity.Text = "الجهة:";
            this.labelEntity.Visible = false;

            this.comboBoxEntity = new ComboBox();
            this.comboBoxEntity.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBoxEntity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.comboBoxEntity.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBoxEntity.Location = new Point(150, 112);
            this.comboBoxEntity.Name = "comboBoxEntity";
            this.comboBoxEntity.Size = new Size(260, 27);
            this.comboBoxEntity.TabIndex = 4;
            this.comboBoxEntity.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.comboBoxEntity.Visible = false;

            this.buttonAddAgent = new Button();
            this.buttonAddAgent.BackColor = Color.ForestGreen;
            this.buttonAddAgent.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonAddAgent.ForeColor = Color.White;
            this.buttonAddAgent.Location = new Point(420, 150);
            this.buttonAddAgent.Name = "buttonAddAgent";
            this.buttonAddAgent.Size = new Size(90, 35);
            this.buttonAddAgent.TabIndex = 5;
            this.buttonAddAgent.Text = "إضافة";
            this.buttonAddAgent.UseVisualStyleBackColor = false;
            this.buttonAddAgent.Click += new EventHandler(this.buttonAddAgent_Click);

            this.buttonEditAgent = new Button();
            this.buttonEditAgent.BackColor = Color.Orange;
            this.buttonEditAgent.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonEditAgent.ForeColor = Color.White;
            this.buttonEditAgent.Location = new Point(310, 150);
            this.buttonEditAgent.Name = "buttonEditAgent";
            this.buttonEditAgent.Size = new Size(90, 35);
            this.buttonEditAgent.TabIndex = 6;
            this.buttonEditAgent.Text = "تعديل";
            this.buttonEditAgent.UseVisualStyleBackColor = false;
            this.buttonEditAgent.Click += new EventHandler(this.buttonEditAgent_Click);

            this.buttonDeleteAgent = new Button();
            this.buttonDeleteAgent.BackColor = Color.DarkRed;
            this.buttonDeleteAgent.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonDeleteAgent.ForeColor = Color.White;
            this.buttonDeleteAgent.Location = new Point(200, 150);
            this.buttonDeleteAgent.Name = "buttonDeleteAgent";
            this.buttonDeleteAgent.Size = new Size(90, 35);
            this.buttonDeleteAgent.TabIndex = 7;
            this.buttonDeleteAgent.Text = "حذف";
            this.buttonDeleteAgent.UseVisualStyleBackColor = false;
            this.buttonDeleteAgent.Click += new EventHandler(this.buttonDeleteAgent_Click);

            this.dataGridViewAgents = new DataGridView();
            this.dataGridViewAgents.AllowUserToAddRows = false;
            this.dataGridViewAgents.AllowUserToDeleteRows = false;
            this.dataGridViewAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAgents.BackgroundColor = SystemColors.Control;
            this.dataGridViewAgents.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridViewAgents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAgents.Location = new Point(10, 195);
            this.dataGridViewAgents.Name = "dataGridViewAgents";
            this.dataGridViewAgents.ReadOnly = true;
            this.dataGridViewAgents.RightToLeft = RightToLeft.Yes;
            this.dataGridViewAgents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAgents.Size = new Size(528, 85);
            this.dataGridViewAgents.TabIndex = 8;
            this.dataGridViewAgents.CellClick += new DataGridViewCellEventHandler(this.dataGridViewAgents_CellClick);

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.FromArgb(150, 0, 0);
            columnHeaderStyle.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            columnHeaderStyle.ForeColor = Color.White;
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewAgents.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            this.dataGridViewAgents.Columns.Add("AgentName", "اسم الوكيل");
            this.dataGridViewAgents.Columns.Add("Entity", "الجهة");

            this.panelRight.Controls.Add(this.labelSectionAgents);
            this.panelRight.Controls.Add(this.labelAgentName);
            this.panelRight.Controls.Add(this.comboBoxAgentName);
            this.panelRight.Controls.Add(this.radioNewAgent);
            this.panelRight.Controls.Add(this.radioNewEntity);
            this.panelRight.Controls.Add(this.labelEntity);
            this.panelRight.Controls.Add(this.comboBoxEntity);
            this.panelRight.Controls.Add(this.buttonAddAgent);
            this.panelRight.Controls.Add(this.buttonEditAgent);
            this.panelRight.Controls.Add(this.buttonDeleteAgent);
            this.panelRight.Controls.Add(this.dataGridViewAgents);

            // ===== القسم 2: تسقيف العملاء (جديد) =====
            this.labelSectionCeilings = new Label();
            this.labelSectionCeilings.BackColor = Color.FromArgb(0, 100, 150);
            this.labelSectionCeilings.Font = new Font("Arial", 11f, FontStyle.Bold);
            this.labelSectionCeilings.ForeColor = Color.White;
            this.labelSectionCeilings.Location = new Point(0, 295);
            this.labelSectionCeilings.Name = "labelSectionCeilings";
            this.labelSectionCeilings.Size = new Size(548, 30);
            this.labelSectionCeilings.TabIndex = 50;
            this.labelSectionCeilings.Text = "تسقيف العملاء";
            this.labelSectionCeilings.TextAlign = ContentAlignment.MiddleCenter;

            this.labelCeilingAccount = new Label();
            this.labelCeilingAccount.AutoSize = true;
            this.labelCeilingAccount.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCeilingAccount.ForeColor = Color.FromArgb(0, 100, 150);
            this.labelCeilingAccount.Location = new Point(420, 340);
            this.labelCeilingAccount.Name = "labelCeilingAccount";
            this.labelCeilingAccount.Size = new Size(100, 19);
            this.labelCeilingAccount.Text = "اسم الحساب:";

            this.comboBoxCeilingAccount = new ComboBox();
            this.comboBoxCeilingAccount.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBoxCeilingAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.comboBoxCeilingAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBoxCeilingAccount.Location = new Point(150, 337);
            this.comboBoxCeilingAccount.Name = "comboBoxCeilingAccount";
            this.comboBoxCeilingAccount.Size = new Size(260, 27);
            this.comboBoxCeilingAccount.TabIndex = 51;
            this.comboBoxCeilingAccount.Font = new Font("Arial", 9.5f, FontStyle.Bold);

            this.labelCeilingAmount = new Label();
            this.labelCeilingAmount.AutoSize = true;
            this.labelCeilingAmount.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCeilingAmount.ForeColor = Color.FromArgb(0, 100, 150);
            this.labelCeilingAmount.Location = new Point(420, 375);
            this.labelCeilingAmount.Name = "labelCeilingAmount";
            this.labelCeilingAmount.Size = new Size(110, 19);
            this.labelCeilingAmount.Text = "مبلغ السقف:";

            this.textBoxCeilingAmount = new TextBox();
            this.textBoxCeilingAmount.BorderStyle = BorderStyle.FixedSingle;
            this.textBoxCeilingAmount.Location = new Point(150, 372);
            this.textBoxCeilingAmount.Name = "textBoxCeilingAmount";
            this.textBoxCeilingAmount.Size = new Size(260, 26);
            this.textBoxCeilingAmount.TabIndex = 52;
            this.textBoxCeilingAmount.TextAlign = HorizontalAlignment.Center;
            this.textBoxCeilingAmount.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.textBoxCeilingAmount.KeyPress += new KeyPressEventHandler(this.textBoxCeilingAmount_KeyPress);

            this.buttonAddCeiling = new Button();
            this.buttonAddCeiling.BackColor = Color.ForestGreen;
            this.buttonAddCeiling.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonAddCeiling.ForeColor = Color.White;
            this.buttonAddCeiling.Location = new Point(420, 410);
            this.buttonAddCeiling.Name = "buttonAddCeiling";
            this.buttonAddCeiling.Size = new Size(90, 35);
            this.buttonAddCeiling.TabIndex = 53;
            this.buttonAddCeiling.Text = "إضافة";
            this.buttonAddCeiling.UseVisualStyleBackColor = false;
            this.buttonAddCeiling.Click += new EventHandler(this.buttonAddCeiling_Click);

            this.buttonEditCeiling = new Button();
            this.buttonEditCeiling.BackColor = Color.Orange;
            this.buttonEditCeiling.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonEditCeiling.ForeColor = Color.White;
            this.buttonEditCeiling.Location = new Point(310, 410);
            this.buttonEditCeiling.Name = "buttonEditCeiling";
            this.buttonEditCeiling.Size = new Size(90, 35);
            this.buttonEditCeiling.TabIndex = 54;
            this.buttonEditCeiling.Text = "تعديل";
            this.buttonEditCeiling.UseVisualStyleBackColor = false;
            this.buttonEditCeiling.Click += new EventHandler(this.buttonEditCeiling_Click);

            this.buttonDeleteCeiling = new Button();
            this.buttonDeleteCeiling.BackColor = Color.DarkRed;
            this.buttonDeleteCeiling.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonDeleteCeiling.ForeColor = Color.White;
            this.buttonDeleteCeiling.Location = new Point(200, 410);
            this.buttonDeleteCeiling.Name = "buttonDeleteCeiling";
            this.buttonDeleteCeiling.Size = new Size(90, 35);
            this.buttonDeleteCeiling.TabIndex = 55;
            this.buttonDeleteCeiling.Text = "حذف";
            this.buttonDeleteCeiling.UseVisualStyleBackColor = false;
            this.buttonDeleteCeiling.Click += new EventHandler(this.buttonDeleteCeiling_Click);

            this.dataGridViewCeilings = new DataGridView();
            this.dataGridViewCeilings.AllowUserToAddRows = false;
            this.dataGridViewCeilings.AllowUserToDeleteRows = false;
            this.dataGridViewCeilings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCeilings.BackgroundColor = SystemColors.Control;
            this.dataGridViewCeilings.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridViewCeilings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCeilings.Location = new Point(10, 455);
            this.dataGridViewCeilings.Name = "dataGridViewCeilings";
            this.dataGridViewCeilings.ReadOnly = true;
            this.dataGridViewCeilings.RightToLeft = RightToLeft.Yes;
            this.dataGridViewCeilings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCeilings.Size = new Size(528, 110);
            this.dataGridViewCeilings.TabIndex = 56;
            this.dataGridViewCeilings.CellClick += new DataGridViewCellEventHandler(this.dataGridViewCeilings_CellClick);

            DataGridViewCellStyle ceilingHeaderStyle = new DataGridViewCellStyle();
            ceilingHeaderStyle.BackColor = Color.FromArgb(0, 100, 150);
            ceilingHeaderStyle.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            ceilingHeaderStyle.ForeColor = Color.White;
            ceilingHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewCeilings.ColumnHeadersDefaultCellStyle = ceilingHeaderStyle;
            this.dataGridViewCeilings.Columns.Add("AccountName", "اسم الحساب");
            this.dataGridViewCeilings.Columns.Add("CeilingAmount", "مبلغ السقف");

            this.panelRight.Controls.Add(this.labelSectionCeilings);
            this.panelRight.Controls.Add(this.labelCeilingAccount);
            this.panelRight.Controls.Add(this.comboBoxCeilingAccount);
            this.panelRight.Controls.Add(this.labelCeilingAmount);
            this.panelRight.Controls.Add(this.textBoxCeilingAmount);
            this.panelRight.Controls.Add(this.buttonAddCeiling);
            this.panelRight.Controls.Add(this.buttonEditCeiling);
            this.panelRight.Controls.Add(this.buttonDeleteCeiling);
            this.panelRight.Controls.Add(this.dataGridViewCeilings);

            // ========== القسم الأيسر (عمولات الحسابات) ==========
            this.panelLeft = new Panel();
            this.panelLeft.BorderStyle = BorderStyle.FixedSingle;
            this.panelLeft.Location = new Point(10, 10);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new Size(630, 290);
            this.panelLeft.TabIndex = 1;

            this.labelSectionCommissions = new Label();
            this.labelSectionCommissions.BackColor = Color.FromArgb(150, 0, 0);
            this.labelSectionCommissions.Font = new Font("Arial", 11f, FontStyle.Bold);
            this.labelSectionCommissions.ForeColor = Color.White;
            this.labelSectionCommissions.Location = new Point(0, 0);
            this.labelSectionCommissions.Name = "labelSectionCommissions";
            this.labelSectionCommissions.Size = new Size(628, 30);
            this.labelSectionCommissions.TabIndex = 0;
            this.labelSectionCommissions.Text = "عمولات الحسابات (عمولات العملاء)";
            this.labelSectionCommissions.TextAlign = ContentAlignment.MiddleCenter;

            this.labelCommissionAgent = new Label();
            this.labelCommissionAgent.AutoSize = true;
            this.labelCommissionAgent.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCommissionAgent.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelCommissionAgent.Location = new Point(500, 45);
            this.labelCommissionAgent.Name = "labelCommissionAgent";
            this.labelCommissionAgent.Size = new Size(95, 19);
            this.labelCommissionAgent.Text = "اسم الوكيل:";

            this.comboBoxCommissionAgent = new ComboBox();
            this.comboBoxCommissionAgent.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBoxCommissionAgent.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.comboBoxCommissionAgent.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBoxCommissionAgent.Location = new Point(370, 42);
            this.comboBoxCommissionAgent.Name = "comboBoxCommissionAgent";
            this.comboBoxCommissionAgent.Size = new Size(120, 27);
            this.comboBoxCommissionAgent.TabIndex = 0;
            this.comboBoxCommissionAgent.Font = new Font("Arial", 9.5f, FontStyle.Bold);

            this.labelCommissionFrom = new Label();
            this.labelCommissionFrom.AutoSize = true;
            this.labelCommissionFrom.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCommissionFrom.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelCommissionFrom.Location = new Point(270, 45);
            this.labelCommissionFrom.Name = "labelCommissionFrom";
            this.labelCommissionFrom.Size = new Size(95, 19);
            this.labelCommissionFrom.Text = "المبلغ من:";

            this.textBoxCommissionFrom = new TextBox();
            this.textBoxCommissionFrom.BorderStyle = BorderStyle.FixedSingle;
            this.textBoxCommissionFrom.Location = new Point(140, 42);
            this.textBoxCommissionFrom.Name = "textBoxCommissionFrom";
            this.textBoxCommissionFrom.Size = new Size(120, 26);
            this.textBoxCommissionFrom.TabIndex = 1;
            this.textBoxCommissionFrom.TextAlign = HorizontalAlignment.Center;

            this.labelCommissionTo = new Label();
            this.labelCommissionTo.AutoSize = true;
            this.labelCommissionTo.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCommissionTo.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelCommissionTo.Location = new Point(500, 80);
            this.labelCommissionTo.Name = "labelCommissionTo";
            this.labelCommissionTo.Size = new Size(85, 19);
            this.labelCommissionTo.Text = "إلى مبلغ:";

            this.textBoxCommissionTo = new TextBox();
            this.textBoxCommissionTo.BorderStyle = BorderStyle.FixedSingle;
            this.textBoxCommissionTo.Location = new Point(370, 77);
            this.textBoxCommissionTo.Name = "textBoxCommissionTo";
            this.textBoxCommissionTo.Size = new Size(120, 26);
            this.textBoxCommissionTo.TabIndex = 2;
            this.textBoxCommissionTo.TextAlign = HorizontalAlignment.Center;

            this.labelCommissionAmount = new Label();
            this.labelCommissionAmount.AutoSize = true;
            this.labelCommissionAmount.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.labelCommissionAmount.ForeColor = Color.FromArgb(150, 0, 0);
            this.labelCommissionAmount.Location = new Point(270, 80);
            this.labelCommissionAmount.Name = "labelCommissionAmount";
            this.labelCommissionAmount.Size = new Size(125, 19);
            this.labelCommissionAmount.Text = "مبلغ العمولة:";

            this.textBoxCommissionAmount = new TextBox();
            this.textBoxCommissionAmount.BorderStyle = BorderStyle.FixedSingle;
            this.textBoxCommissionAmount.Location = new Point(140, 77);
            this.textBoxCommissionAmount.Name = "textBoxCommissionAmount";
            this.textBoxCommissionAmount.Size = new Size(120, 26);
            this.textBoxCommissionAmount.TabIndex = 3;
            this.textBoxCommissionAmount.TextAlign = HorizontalAlignment.Center;

            this.buttonAddCommission = new Button();
            this.buttonAddCommission.BackColor = Color.ForestGreen;
            this.buttonAddCommission.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonAddCommission.ForeColor = Color.White;
            this.buttonAddCommission.Location = new Point(490, 120);
            this.buttonAddCommission.Name = "buttonAddCommission";
            this.buttonAddCommission.Size = new Size(90, 35);
            this.buttonAddCommission.TabIndex = 4;
            this.buttonAddCommission.Text = "إضافة";
            this.buttonAddCommission.UseVisualStyleBackColor = false;
            this.buttonAddCommission.Click += new EventHandler(this.buttonAddCommission_Click);

            this.buttonEditCommission = new Button();
            this.buttonEditCommission.BackColor = Color.Orange;
            this.buttonEditCommission.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonEditCommission.ForeColor = Color.White;
            this.buttonEditCommission.Location = new Point(370, 120);
            this.buttonEditCommission.Name = "buttonEditCommission";
            this.buttonEditCommission.Size = new Size(90, 35);
            this.buttonEditCommission.TabIndex = 5;
            this.buttonEditCommission.Text = "تعديل";
            this.buttonEditCommission.UseVisualStyleBackColor = false;
            this.buttonEditCommission.Click += new EventHandler(this.buttonEditCommission_Click);

            this.buttonDeleteCommission = new Button();
            this.buttonDeleteCommission.BackColor = Color.DarkRed;
            this.buttonDeleteCommission.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.buttonDeleteCommission.ForeColor = Color.White;
            this.buttonDeleteCommission.Location = new Point(250, 120);
            this.buttonDeleteCommission.Name = "buttonDeleteCommission";
            this.buttonDeleteCommission.Size = new Size(90, 35);
            this.buttonDeleteCommission.TabIndex = 6;
            this.buttonDeleteCommission.Text = "حذف";
            this.buttonDeleteCommission.UseVisualStyleBackColor = false;
            this.buttonDeleteCommission.Click += new EventHandler(this.buttonDeleteCommission_Click);

            this.dataGridViewCommissions = new DataGridView();
            this.dataGridViewCommissions.AllowUserToAddRows = false;
            this.dataGridViewCommissions.AllowUserToDeleteRows = false;
            this.dataGridViewCommissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCommissions.BackgroundColor = SystemColors.Control;
            this.dataGridViewCommissions.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridViewCommissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommissions.Location = new Point(10, 170);
            this.dataGridViewCommissions.Name = "dataGridViewCommissions";
            this.dataGridViewCommissions.ReadOnly = true;
            this.dataGridViewCommissions.RightToLeft = RightToLeft.Yes;
            this.dataGridViewCommissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCommissions.Size = new Size(608, 110);
            this.dataGridViewCommissions.TabIndex = 7;
            this.dataGridViewCommissions.CellClick += new DataGridViewCellEventHandler(this.dataGridViewCommissions_CellClick);

            DataGridViewCellStyle commHeaderStyle = new DataGridViewCellStyle();
            commHeaderStyle.BackColor = Color.FromArgb(150, 0, 0);
            commHeaderStyle.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            commHeaderStyle.ForeColor = Color.White;
            commHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewCommissions.ColumnHeadersDefaultCellStyle = commHeaderStyle;
            this.dataGridViewCommissions.Columns.Add("AgentName", "اسم الوكيل");
            this.dataGridViewCommissions.Columns.Add("FromAmount", "المبلغ من");
            this.dataGridViewCommissions.Columns.Add("ToAmount", "إلى مبلغ");
            this.dataGridViewCommissions.Columns.Add("CommissionAmount", "مبلغ العمولة");

            this.panelLeft.Controls.Add(this.labelSectionCommissions);
            this.panelLeft.Controls.Add(this.labelCommissionAgent);
            this.panelLeft.Controls.Add(this.comboBoxCommissionAgent);
            this.panelLeft.Controls.Add(this.labelCommissionFrom);
            this.panelLeft.Controls.Add(this.textBoxCommissionFrom);
            this.panelLeft.Controls.Add(this.labelCommissionTo);
            this.panelLeft.Controls.Add(this.textBoxCommissionTo);
            this.panelLeft.Controls.Add(this.labelCommissionAmount);
            this.panelLeft.Controls.Add(this.textBoxCommissionAmount);
            this.panelLeft.Controls.Add(this.buttonAddCommission);
            this.panelLeft.Controls.Add(this.buttonEditCommission);
            this.panelLeft.Controls.Add(this.buttonDeleteCommission);
            this.panelLeft.Controls.Add(this.dataGridViewCommissions);

            // زر الخروج
            this.buttonExit = new Button();
            this.buttonExit.BackColor = Color.DarkRed;
            this.buttonExit.Font = new Font("Arial", 10f, FontStyle.Bold);
            this.buttonExit.ForeColor = Color.White;
            this.buttonExit.Location = new Point(550, 610);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new Size(100, 40);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "خروج";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new EventHandler(this.buttonExit_Click);

            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.buttonExit);

            this.AutoScaleDimensions = new SizeF(7F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1218, 700);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.labelHeader);
            this.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "Form_Agents";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "بيانات الوكلاء";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.Form_Agents_Load);

            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommissions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCeilings)).BeginInit();
            this.ResumeLayout(false);
        }

        private void Form_Agents_Load(object sender, EventArgs e)
        {
            LoadAccountNamesFromDatabase();
            LoadAgentsData();
            LoadCommissionsData();
            LoadCeilingsData();
        }

        private bool IsAccountExistsInDatabase(string accountName)
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT COUNT(1) FROM tblAccounts WHERE AccountName = @accountName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@accountName", accountName);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch { return false; }
        }

        private void LoadAccountNamesFromDatabase()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT AccountName FROM tblAccounts WHERE AccountName IS NOT NULL AND AccountName != ''";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string accountName = reader["AccountName"].ToString();
                                if (!string.IsNullOrEmpty(accountName))
                                {
                                    if (!this.comboBoxAgentName.Items.Contains(accountName))
                                        this.comboBoxAgentName.Items.Add(accountName);
                                    if (!this.comboBoxCommissionAgent.Items.Contains(accountName))
                                        this.comboBoxCommissionAgent.Items.Add(accountName);
                                    if (!this.comboBoxCeilingAccount.Items.Contains(accountName))
                                        this.comboBoxCeilingAccount.Items.Add(accountName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في جلب بيانات الحسابات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== قسم حسابات الوكلاء ==========
        private void LoadAgentsData()
        {
            this.dataGridViewAgents.Rows.Clear();
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT [اسم الوكيل 1], [الجهة] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] IS NOT NULL AND [الجهة] <> N'تسقيف'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string agent = reader["اسم الوكيل 1"] != DBNull.Value ? reader["اسم الوكيل 1"].ToString() : "";
                                string entity = reader["الجهة"] != DBNull.Value ? reader["الجهة"].ToString() : "";
                                this.dataGridViewAgents.Rows.Add(agent, entity);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void radioNewAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNewAgent.Checked)
            {
                comboBoxEntity.Visible = false;
                labelEntity.Visible = false;
                comboBoxEntity.Text = "";
            }
        }

        private void radioNewEntity_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNewEntity.Checked)
            {
                comboBoxEntity.Visible = true;
                labelEntity.Visible = true;
                comboBoxEntity.Focus();
            }
        }

        private void buttonAddAgent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxAgentName.Text))
            {
                MessageBox.Show("الرجاء إدخال أو اختيار اسم الوكيل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxAgentName.Focus();
                return;
            }
            if (!IsAccountExistsInDatabase(comboBoxAgentName.Text))
            {
                MessageBox.Show("عذراً، اسم الوكيل غير موجود في جدول الحسابات (tblAccounts).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxAgentName.Focus();
                return;
            }
            if (radioNewEntity.Checked && string.IsNullOrEmpty(comboBoxEntity.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم الجهة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxEntity.Focus();
                return;
            }
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO [بيانات الوكلاء] ([اسم الوكيل 1], [الجهة]) VALUES (@agent, @entity)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@agent", comboBoxAgentName.Text);
                        cmd.Parameters.AddWithValue("@entity", radioNewEntity.Checked ? (object)comboBoxEntity.Text : DBNull.Value);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("تمت إضافة الوكيل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAgentsData();
                comboBoxAgentName.Text = "";
                comboBoxEntity.Text = "";
                radioNewAgent.Checked = true;
                comboBoxAgentName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحفظ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== زر التعديل لحسابات الوكلاء (مُفعّل) ==========
        private void buttonEditAgent_Click(object sender, EventArgs e)
        {
            if (dataGridViewAgents.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار وكيل للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string oldAgentName = dataGridViewAgents.CurrentRow.Cells["AgentName"].Value.ToString();
            if (string.IsNullOrEmpty(comboBoxAgentName.Text))
            {
                MessageBox.Show("الرجاء إدخال الاسم الجديد", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxAgentName.Focus();
                return;
            }
            if (!IsAccountExistsInDatabase(comboBoxAgentName.Text))
            {
                MessageBox.Show("عذراً، الاسم الجديد غير موجود في جدول الحسابات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxAgentName.Focus();
                return;
            }
            DialogResult result = MessageBox.Show("هل أنت متأكد من تعديل الوكيل؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "UPDATE [بيانات الوكلاء] SET [اسم الوكيل 1] = @newAgent, [الجهة] = @entity WHERE [اسم الوكيل 1] = @oldAgent AND ([الجهة] IS NULL OR [الجهة] <> N'تسقيف')";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@newAgent", comboBoxAgentName.Text);
                            cmd.Parameters.AddWithValue("@entity", radioNewEntity.Checked ? (object)comboBoxEntity.Text : (radioNewAgent.Checked ? (object)DBNull.Value : comboBoxEntity.Text));
                            cmd.Parameters.AddWithValue("@oldAgent", oldAgentName);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("تم التعديل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAgentsData();
                    comboBoxAgentName.Text = "";
                    comboBoxEntity.Text = "";
                    radioNewAgent.Checked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ في التعديل: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonDeleteAgent_Click(object sender, EventArgs e)
        {
            if (dataGridViewAgents.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار وكيل للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string agentName = dataGridViewAgents.CurrentRow.Cells["AgentName"].Value.ToString();
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] = @agent AND ([الجهة] IS NULL OR [الجهة] <> N'تسقيف')";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@agent", agentName);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAgentsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ في الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewAgents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewAgents.Rows[e.RowIndex];
                comboBoxAgentName.Text = (row.Cells["AgentName"].Value != null ? row.Cells["AgentName"].Value.ToString() : "");
                string entity = (row.Cells["Entity"].Value != null ? row.Cells["Entity"].Value.ToString() : "");
                if (!string.IsNullOrEmpty(entity))
                {
                    radioNewEntity.Checked = true;
                    comboBoxEntity.Text = entity;
                }
                else
                {
                    radioNewAgent.Checked = true;
                }
            }
        }

        // ========== قسم عمولات الحسابات ==========
        private void LoadCommissionsData()
        {
            this.dataGridViewCommissions.Rows.Clear();
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT [اسم الوكيل 2], [المبلغ من], [الى مبلغ], [مبلغ العمولة] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 2] IS NOT NULL";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string agent = reader["اسم الوكيل 2"] != DBNull.Value ? reader["اسم الوكيل 2"].ToString() : "";
                                string fromAmt = reader["المبلغ من"] != DBNull.Value ? reader["المبلغ من"].ToString() : "";
                                string toAmt = reader["الى مبلغ"] != DBNull.Value ? reader["الى مبلغ"].ToString() : "";
                                string commAmt = reader["مبلغ العمولة"] != DBNull.Value ? reader["مبلغ العمولة"].ToString() : "";
                                this.dataGridViewCommissions.Rows.Add(agent, fromAmt, toAmt, commAmt);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonAddCommission_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxCommissionAgent.Text))
            {
                MessageBox.Show("الرجاء اختيار اسم الوكيل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCommissionAgent.Focus();
                return;
            }
            if (!IsAccountExistsInDatabase(comboBoxCommissionAgent.Text))
            {
                MessageBox.Show("عذراً، اسم الوكيل غير موجود في جدول الحسابات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCommissionAgent.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBoxCommissionFrom.Text) || string.IsNullOrEmpty(textBoxCommissionTo.Text) || string.IsNullOrEmpty(textBoxCommissionAmount.Text))
            {
                MessageBox.Show("الرجاء تعبئة جميع الحقول", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal fromAmount, toAmount, commissionAmount;
            if (!decimal.TryParse(textBoxCommissionFrom.Text, out fromAmount) || !decimal.TryParse(textBoxCommissionTo.Text, out toAmount) || !decimal.TryParse(textBoxCommissionAmount.Text, out commissionAmount))
            {
                MessageBox.Show("الرجاء إدخال أرقام صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fromAmount >= toAmount)
            {
                MessageBox.Show("المبلغ من يجب أن يكون أقل من المبلغ إلى", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO [بيانات الوكلاء] ([اسم الوكيل 2], [المبلغ من], [الى مبلغ], [مبلغ العمولة]) VALUES (@agent, @from, @to, @comm)";
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
                textBoxCommissionFrom.Text = "";
                textBoxCommissionTo.Text = "";
                textBoxCommissionAmount.Text = "";
                textBoxCommissionFrom.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحفظ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== زر التعديل لعمولات الحسابات (مُفعّل) ==========
        private void buttonEditCommission_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommissions.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار عمولة للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(comboBoxCommissionAgent.Text) || string.IsNullOrEmpty(textBoxCommissionFrom.Text) || string.IsNullOrEmpty(textBoxCommissionTo.Text) || string.IsNullOrEmpty(textBoxCommissionAmount.Text))
            {
                MessageBox.Show("الرجاء تعبئة جميع الحقول", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal fromAmount, toAmount, commissionAmount;
            if (!decimal.TryParse(textBoxCommissionFrom.Text, out fromAmount) || !decimal.TryParse(textBoxCommissionTo.Text, out toAmount) || !decimal.TryParse(textBoxCommissionAmount.Text, out commissionAmount))
            {
                MessageBox.Show("الرجاء إدخال أرقام صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fromAmount >= toAmount)
            {
                MessageBox.Show("المبلغ من يجب أن يكون أقل من المبلغ إلى", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldAgent = dataGridViewCommissions.CurrentRow.Cells["AgentName"].Value.ToString();
            string oldFrom = dataGridViewCommissions.CurrentRow.Cells["FromAmount"].Value.ToString();

            DialogResult result = MessageBox.Show("هل أنت متأكد من التعديل؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "UPDATE [بيانات الوكلاء] SET [اسم الوكيل 2] = @newAgent, [المبلغ من] = @newFrom, [الى مبلغ] = @to, [مبلغ العمولة] = @comm WHERE [اسم الوكيل 2] = @oldAgent AND [المبلغ من] = @oldFrom";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@newAgent", comboBoxCommissionAgent.Text);
                            cmd.Parameters.AddWithValue("@newFrom", fromAmount);
                            cmd.Parameters.AddWithValue("@to", toAmount);
                            cmd.Parameters.AddWithValue("@comm", commissionAmount);
                            cmd.Parameters.AddWithValue("@oldAgent", oldAgent);
                            cmd.Parameters.AddWithValue("@oldFrom", oldFrom);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("تم التعديل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCommissionsData();
                    comboBoxCommissionAgent.Text = "";
                    textBoxCommissionFrom.Text = "";
                    textBoxCommissionTo.Text = "";
                    textBoxCommissionAmount.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ في التعديل: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonDeleteCommission_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommissions.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار عمولة للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string agentName = dataGridViewCommissions.CurrentRow.Cells["AgentName"].Value.ToString();
                    string fromAmt = dataGridViewCommissions.CurrentRow.Cells["FromAmount"].Value.ToString();
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM [بيانات الوكلاء] WHERE [اسم الوكيل 2] = @agent AND [المبلغ من] = @from";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@agent", agentName);
                            cmd.Parameters.AddWithValue("@from", fromAmt);
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

        private void dataGridViewCommissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewCommissions.Rows[e.RowIndex];
                comboBoxCommissionAgent.Text = (row.Cells["AgentName"].Value != null ? row.Cells["AgentName"].Value.ToString() : "");
                textBoxCommissionFrom.Text = (row.Cells["FromAmount"].Value != null ? row.Cells["FromAmount"].Value.ToString().Replace(",", "") : "");
                textBoxCommissionTo.Text = (row.Cells["ToAmount"].Value != null ? row.Cells["ToAmount"].Value.ToString().Replace(",", "") : "");
                textBoxCommissionAmount.Text = (row.Cells["CommissionAmount"].Value != null ? row.Cells["CommissionAmount"].Value.ToString().Replace(",", "") : "");
            }
        }

        // ========== قسم تسقيف العملاء (جديد) ==========
        private void LoadCeilingsData()
        {
            this.dataGridViewCeilings.Rows.Clear();
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT [حساب السقف], [مبلغ السقف] FROM [بيانات الوكلاء] WHERE [حساب السقف] IS NOT NULL AND [حساب السقف] != ''";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string account = reader["حساب السقف"] != DBNull.Value ? reader["حساب السقف"].ToString() : "";
                                string amount = reader["مبلغ السقف"] != DBNull.Value ? Convert.ToDecimal(reader["مبلغ السقف"]).ToString("0.##") : "";
                                this.dataGridViewCeilings.Rows.Add(account, amount);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void textBoxCeilingAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 8)
                e.Handled = true;
        }

        private void buttonAddCeiling_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxCeilingAccount.Text))
            {
                MessageBox.Show("الرجاء اختيار اسم الحساب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCeilingAccount.Focus();
                return;
            }
            if (!IsAccountExistsInDatabase(comboBoxCeilingAccount.Text))
            {
                MessageBox.Show("عذراً، اسم الحساب غير موجود في جدول الحسابات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCeilingAccount.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBoxCeilingAmount.Text))
            {
                MessageBox.Show("الرجاء إدخال مبلغ السقف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCeilingAmount.Focus();
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
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // التحقق من وجود سقف مسبق لنفس الحساب
                    string checkQuery = "SELECT COUNT(1) FROM [بيانات الوكلاء] WHERE [حساب السقف] = @account";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@account", comboBoxCeilingAccount.Text);
                        conn.Open();
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("هذا الحساب لديه سقف مسبق مسبقاً. استخدم زر التعديل لتغييره.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO [بيانات الوكلاء] ([حساب السقف], [مبلغ السقف]) VALUES (@account, @amount)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@account", comboBoxCeilingAccount.Text);
                        cmd.Parameters.AddWithValue("@amount", ceilingAmount);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("تمت إضافة السقف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCeilingsData();
                comboBoxCeilingAccount.Text = "";
                textBoxCeilingAmount.Text = "";
                comboBoxCeilingAccount.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الحفظ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEditCeiling_Click(object sender, EventArgs e)
        {
            if (dataGridViewCeilings.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار سقف للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

            string oldAccount = dataGridViewCeilings.CurrentRow.Cells["AccountName"].Value.ToString();

            DialogResult result = MessageBox.Show("هل أنت متأكد من التعديل؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "UPDATE [بيانات الوكلاء] SET [حساب السقف] = @newAccount, [مبلغ السقف] = @amount WHERE [حساب السقف] = @oldAccount";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@newAccount", comboBoxCeilingAccount.Text);
                            cmd.Parameters.AddWithValue("@amount", ceilingAmount);
                            cmd.Parameters.AddWithValue("@oldAccount", oldAccount);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("تم التعديل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCeilingsData();
                    comboBoxCeilingAccount.Text = "";
                    textBoxCeilingAmount.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ في التعديل: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonDeleteCeiling_Click(object sender, EventArgs e)
        {
            if (dataGridViewCeilings.CurrentRow == null)
            {
                MessageBox.Show("الرجاء اختيار سقف للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string accountName = dataGridViewCeilings.CurrentRow.Cells["AccountName"].Value.ToString();
                    string connString = DatabaseHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM [بيانات الوكلاء] WHERE [حساب السقف] = @account";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@account", accountName);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCeilingsData();
                    comboBoxCeilingAccount.Text = "";
                    textBoxCeilingAmount.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ في الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewCeilings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewCeilings.Rows[e.RowIndex];
                comboBoxCeilingAccount.Text = (row.Cells["AccountName"].Value != null ? row.Cells["AccountName"].Value.ToString() : "");
                textBoxCeilingAmount.Text = (row.Cells["CeilingAmount"].Value != null ? row.Cells["CeilingAmount"].Value.ToString().Replace(",", "") : "");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
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