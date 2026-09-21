using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AlmonifExchange
{
    public class DataSet1 : DataSet
    {
        public DataSet1() { }
    }

    public partial class Form_Send_Money : Form
    {
        private IContainer components = null;
        private string iconsBasePath = "";
        private string currentAgent = "";
        private bool Control_Pressed = false;
        private bool tab = false;
        private string REQUEST_CODE = "";
        private bool _isAccountDebit = false;
        private string _debitAccountName = "";
        private bool _debitWarningAcknowledged = false;
        private decimal _accountBalance = 0;

        private Label label4, label5, label6, label7, label11, label12, label13, label14, label15, label17, label18, label19, label23, label24, label25, label29, label31, label32, label34, label37, label38, label39, label40, label41, labelAgent;
        private Label label16, label2, label8, label9;
        private Button button2, button_GetAgent, button_Show_Data_Card_Send, button_Show_Data_Card_BEN, button_Arch, button_Exit, button_New, button_Print, button_Save, button_Sync, button_Update, button_show, button_CopyToSender, button_CopyToBeneficiary;
        private CheckBox checkBox1, checkBox_Serch_mobile, checkBox_SendTO_Ebda, check_All_User, check_SelectAll_Sync, CHe_FEE_FROM_REMIT_AMOUNT;
        private ComboBox COUNTRY, ORIGINATION_CURRENCY, PURPOSE_ID, AgentId, DestinationEntity;
        private DataGridView dGV;
        private DataGridViewTextBoxColumn Dgv_ExpressNum, Dgv_OutgoingDate, Dgv_TransferAmount, Dgv_CurrencyName, Dgv_SenderName, Dgv_SenderPhone, Dgv_ReceiverName, Dgv_ReceiverPhone, Dgv_OutgoingNumber, Dgv_IncomingPay, Dgv_IncomingAccount, Dgv_UserName, Dgv_OutgoingAccount, Dgv_Target, Dgv_Notes, Dgv_TransferPurpose, Dgv_IncomingCommission, Dgv_OutgoingCommission;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1, MenuItemArch;
        private Label CenterCommission, CommissionCurrency, DeliverStatus, ExchangerAccountAmount, ExchangerAccountCurrencyName, FEE_CUR_CODE, SoucrceName, Status, label_ORIGINATION_AMOUNT, label_RemitCount, label_Total_AMOUNT;
        private MaskedTextBox REMIT_DATE;
        private DateTimePicker REMIT_DATE_Find;
        private MenuStrip M_Proccess;
        private Panel panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8, panel9, panel10, panel11, panel12, panel13, panel14, panel15;
        private PictureBox BTN_Serch_MOB_Ben1, BTN_Serch_MOB_Sen1, pictureBoxCompanyLogo;
        private TextBox AgentCommission, BEN_MOBILE, BEN_NAME, WAKIL_FEE, PURPOSE_DESCRIPTION, ORIGINATION_AMOUNT, Note_PURPOSE, REMIT_CODE, REMIT_SEQ, SENDER_MOBILE, SENDER_NAME, textBox_Search, Total_AMOUNT;
        private ComboBox GlobalCode;
        private ComboBox CustomerAccount;
        private Label labelCustomerAccount;
        private ToolStripMenuItem M_Cancel, M_Edit, M_Exit, M_Menu_Tab, M_New, M_Print, M_Save, M_Serch;
        private BackgroundWorker backgroundWorker1;
        private BindingSource bindingSource1;
        private DataSet1 dataSet1;

        public Form_Send_Money()
        {
            InitializeComponent();
            currentAgent = "";
            iconsBasePath = Application.StartupPath;
        }

        public Form_Send_Money(string agentName)
        {
            InitializeComponent();
            currentAgent = agentName;
            iconsBasePath = Application.StartupPath;
        }

        private string GetExpressNumber(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return "";
            try
            {
                Match match = Regex.Match(notes, @"(?:الاكسبرس|الإكسبرس)[\s:/]([0-9]+)", RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1) return match.Groups[1].Value;
            }
            catch { }
            return "";
        }

        private void LoadTransfersToday()
        {
            try
            {
                dGV.Rows.Clear();
                string cs = DatabaseHelper.GetConnectionString();
                string query = "SELECT T.ID, T.OutgoingNotes, T.IncomingNotes, CONVERT(VARCHAR(10), T.OutgoingDate, 120) as OutgoingDateStr, T.TransferAmount, C.CurrencyName, T.SenderName, T.SenderPhone, T.ReceiverName, T.ReceiverPhone, T.OutgoingNumber, T.IncomingPay, AccIn.AccountName as IncomingAccountName, U.UserName, AccOut.AccountName as OutgoingAccountName, T.Target, T.TransferPurpose, T.IncomingCommission, T.OutgoingCommission FROM tblTransfers T LEFT JOIN tblCurrencies C ON T.TransferCurrencyID = C.ID LEFT JOIN tblAccounts AccIn ON T.IncomingAccountID = AccIn.ID LEFT JOIN tblAccounts AccOut ON T.OutgoingAccountID = AccOut.ID LEFT JOIN tblUsers U ON T.UserID = U.ID WHERE CONVERT(VARCHAR(10), T.OutgoingDate, 120) = CONVERT(VARCHAR(10), GETDATE(), 120) AND T.OutgoingPay = N'صادرة' ORDER BY T.ID DESC";
                
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string outNotes = reader["OutgoingNotes"] != DBNull.Value ? reader["OutgoingNotes"].ToString() : "";
                                string inNotes = reader["IncomingNotes"] != DBNull.Value ? reader["IncomingNotes"].ToString() : "";
                                string notes = string.IsNullOrEmpty(outNotes) ? inNotes : outNotes;
                                string expressNum = GetExpressNumber(notes);
                                string dateStr = reader["OutgoingDateStr"] != DBNull.Value ? Convert.ToDateTime(reader["OutgoingDateStr"]).ToString("dd/MM/yyyy") : "";
                                string amount = reader["TransferAmount"] != DBNull.Value ? FormatAmount(Convert.ToDecimal(reader["TransferAmount"])) : "0";
                                string currency = reader["CurrencyName"] != DBNull.Value ? reader["CurrencyName"].ToString() : "";
                                string senderName = reader["SenderName"] != DBNull.Value ? reader["SenderName"].ToString() : "";
                                string senderPhone = reader["SenderPhone"] != DBNull.Value ? reader["SenderPhone"].ToString() : "";
                                string receiverName = reader["ReceiverName"] != DBNull.Value ? reader["ReceiverName"].ToString() : "";
                                string receiverPhone = reader["ReceiverPhone"] != DBNull.Value ? reader["ReceiverPhone"].ToString() : "";
                                string outNum = reader["OutgoingNumber"] != DBNull.Value ? reader["OutgoingNumber"].ToString() : "";
                                string inPay = reader["IncomingPay"] != DBNull.Value ? reader["IncomingPay"].ToString() : "";
                                string inAcc = reader["IncomingAccountName"] != DBNull.Value ? reader["IncomingAccountName"].ToString() : "";
                                string userName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "";
                                string outAcc = reader["OutgoingAccountName"] != DBNull.Value ? reader["OutgoingAccountName"].ToString() : "";
                                string target = reader["Target"] != DBNull.Value ? reader["Target"].ToString() : "";
                                string purpose = reader["TransferPurpose"] != DBNull.Value ? reader["TransferPurpose"].ToString() : "";
                                string inComm = reader["IncomingCommission"] != DBNull.Value ? FormatAmount(Convert.ToDecimal(reader["IncomingCommission"])) : "0";
                                string outComm = reader["OutgoingCommission"] != DBNull.Value ? FormatAmount(Convert.ToDecimal(reader["OutgoingCommission"])) : "0";
                                
                                dGV.Rows.Add(expressNum, dateStr, amount, currency, senderName, senderPhone, receiverName, receiverPhone, outNum, inPay, inAcc, userName, outAcc, target, notes, purpose, inComm, outComm);
                            }
                        }
                    }
                }
                label_RemitCount.Text = dGV.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الحوالات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_New_Click(object sender, EventArgs e)
        {
            this.REMIT_CODE.Text = "";
            this.REMIT_DATE.Text = "";
            this.REMIT_SEQ.Text = "";
            this.button_Save.Enabled = true;
            this.button_Print.Enabled = false;
            this.Clear_text();
            this.REQUEST_CODE = "";
            this.Locked_text(false, true);
            this.ORIGINATION_AMOUNT.Focus();
            _debitWarningAcknowledged = false;
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            phone = phone.Trim();
            return phone.Length == 9 && Regex.IsMatch(phone, @"^\d{9}$");
        }

        private bool CheckAgentExists(string agentName)
        {
            if (string.IsNullOrEmpty(agentName)) return false;
            try
            {
                long accountId = TransferHelper.GetAccountIdByName(agentName, DatabaseHelper.GetConnectionString());
                return accountId > 0;
            }
            catch { return false; }
        }

        private bool CheckDestinationExists(string destination)
        {
            if (string.IsNullOrEmpty(destination)) return false;
            try
            {
                string cs = DatabaseHelper.GetConnectionString();
                string query = "SELECT COUNT(*) FROM tblRegions WHERE RegionName=@name";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", destination);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch { return false; }
        }

        private bool CheckCustomerAccountExists(string accountName)
        {
            if (string.IsNullOrEmpty(accountName)) return false;
            try
            {
                long accountId = TransferHelper.GetAccountIdByName(accountName, DatabaseHelper.GetConnectionString());
                return accountId > 0;
            }
            catch { return false; }
        }

        private bool CheckTransferExists(decimal amount, long currencyId, string receiverName, string senderName, long accountId)
        {
            try
            {
                string cs = DatabaseHelper.GetConnectionString();
                string query = "SELECT COUNT(*) FROM tblTransfers WHERE OutgoingAccountID=@acc AND ReceiverName=@rec AND SenderName=@sen AND TransferAmount=@amt AND TransferCurrencyID=@cur AND CONVERT(VARCHAR(10), OutgoingDate, 120) = CONVERT(VARCHAR(10), GETDATE(), 120)";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountId);
                        cmd.Parameters.AddWithValue("@rec", receiverName);
                        cmd.Parameters.AddWithValue("@sen", senderName);
                        cmd.Parameters.AddWithValue("@amt", amount);
                        cmd.Parameters.AddWithValue("@cur", currencyId);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch { return false; }
        }

        private bool ValidateTransferData()
        {
            if (string.IsNullOrEmpty(this.CustomerAccount.Text.Trim()))
            {
                MessageBox.Show("الرجاء اختيار حساب القبض", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.CustomerAccount.Focus();
                return false;
            }
            if (!CheckCustomerAccountExists(this.CustomerAccount.Text.Trim()))
            {
                MessageBox.Show("حساب القبض غير موجود في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.CustomerAccount.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.AgentId.Text.Trim()))
            {
                MessageBox.Show("الرجاء اختيار الوكيل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.AgentId.Focus();
                return false;
            }
            if (!CheckAgentExists(this.AgentId.Text.Trim()))
            {
                MessageBox.Show("الوكيل غير موجود في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.AgentId.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.DestinationEntity.Text.Trim()))
            {
                MessageBox.Show("الرجاء اختيار الجهة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DestinationEntity.Focus();
                return false;
            }
            if (!CheckDestinationExists(this.DestinationEntity.Text.Trim()))
            {
                MessageBox.Show("الجهة غير موجودة في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DestinationEntity.Focus();
                return false;
            }
            if (!IsValidPhone(this.BEN_MOBILE.Text.Trim()))
            {
                MessageBox.Show("رقم هاتف المستلم يجب أن يكون 9 أرقام", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.BEN_MOBILE.Focus();
                return false;
            }
            if (!IsValidPhone(this.SENDER_MOBILE.Text.Trim()))
            {
                MessageBox.Show("رقم هاتف المرسل يجب أن يكون 9 أرقام", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.SENDER_MOBILE.Focus();
                return false;
            }
            return true;
        }

        private void UpdateTestStateToApproved(long transferId)
        {
            try
            {
                string cs = DatabaseHelper.GetConnectionString();
                string query = "UPDATE tblTransfers SET TestState=N'معتمدة' WHERE ID=@id AND TestState=N'قيد المراجعة'";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", transferId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private string GetExpressNumberFromDatabase(long transferId)
        {
            try
            {
                string cs = DatabaseHelper.GetConnectionString();
                string query = "SELECT IncomingNotes, OutgoingNotes FROM tblTransfers WHERE ID=@id";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", transferId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string inNotes = reader["IncomingNotes"] != DBNull.Value ? reader["IncomingNotes"].ToString() : "";
                                string outNotes = reader["OutgoingNotes"] != DBNull.Value ? reader["OutgoingNotes"].ToString() : "";
                                string notes = string.IsNullOrEmpty(outNotes) ? inNotes : outNotes;
                                string expressNum = GetExpressNumber(notes);
                                if (!string.IsNullOrEmpty(expressNum) && expressNum.Trim() != "")
                                {
                                    return expressNum;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return "";
        }

        private void WaitForExpressNumberAndShowMessage(long transferId, decimal transferAmount, string currencyName, string receiverName, string destination)
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            int elapsedSeconds = 0;
            int maxWaitSeconds = 15;
            timer.Tick += delegate(object s, EventArgs ev)
            {
                elapsedSeconds++;
                string expressNumber = GetExpressNumberFromDatabase(transferId);
                if (!string.IsNullOrEmpty(expressNumber) || elapsedSeconds >= maxWaitSeconds)
                {
                    timer.Stop();
                    timer.Dispose();
                    ShowFinalMessage(expressNumber, transferAmount, currencyName, receiverName, destination);
                }
            };
            timer.Start();
        }

        private void ShowFinalMessage(string expressNumber, decimal transferAmount, string currencyName, string receiverName, string destination)
        {
            string displayExpressNumber = string.IsNullOrEmpty(expressNumber) ? "" : expressNumber;
            string message = string.Format(
                "رقم الحوالة:{0}\nالمستلم:{1}\nمبلغ الحوالة:{2} {3}\nالجهة:{4}",
                displayExpressNumber,
                receiverName,
                FormatAmount(transferAmount),
                currencyName,
                destination);
            CLS_MessagBox.Messag_Data("تفاصيل الحوالة", message);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (!ValidateTransferData()) return;
            
            if (_isAccountDebit && !_debitWarningAcknowledged)
            {
                string debitMsg = string.Format(
                    "إن رصيد الحساب {0} لا يسمح بهذه العملية.\nالرصيد المطلوب: {1} ريال يمني\nهل ترغب بترحيل العملية على أي حال؟",
                    _debitAccountName,
                    Math.Abs(_accountBalance));
                DialogResult warnResult = MessageBox.Show(
                    debitMsg,
                    "تحذير رصيد مديون",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (warnResult == DialogResult.No)
                {
                    this.CustomerAccount.Focus();
                    return;
                }
                else
                {
                    _debitWarningAcknowledged = true;
                }
            }

            decimal transferAmount = 0;
            if (!decimal.TryParse(this.ORIGINATION_AMOUNT.Text.Trim(), out transferAmount) || transferAmount <= 0)
            {
                MessageBox.Show("الرجاء إدخال مبلغ صحيح", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal incomingCommission = 0;
            decimal outgoingCommission = 0;
            decimal.TryParse(this.AgentCommission.Text.Trim(), out incomingCommission);
            decimal.TryParse(this.WAKIL_FEE.Text.Trim(), out outgoingCommission);

            string currencyName = string.IsNullOrEmpty(this.ORIGINATION_CURRENCY.Text) ? "ريال يمني" : this.ORIGINATION_CURRENCY.Text;
            string cs = DatabaseHelper.GetConnectionString();
            long currencyId = TransferHelper.GetCurrencyIdByName(currencyName, cs);
            long commissionCurrencyId = TransferHelper.GetCurrencyIdByName("ريال يمني", cs);
            long incomingAccountId = TransferHelper.GetAccountIdByName(this.CustomerAccount.Text.Trim(), cs);
            long outgoingAccountId = TransferHelper.GetAccountIdByName(this.AgentId.Text.Trim(), cs);

            if (incomingAccountId == 0)
            {
                MessageBox.Show("حساب القبض غير موجود في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (outgoingAccountId == 0)
            {
                MessageBox.Show("الوكيل غير موجود في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CheckTransferExists(transferAmount, currencyId, this.BEN_NAME.Text.Trim(), this.SENDER_NAME.Text.Trim(), outgoingAccountId))
            {
                DialogResult existResult = MessageBox.Show("بيانات الحوالة موجودة مسبقاً هل تريد الإستمرار", "تأكيد",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (existResult == DialogResult.No) return;
            }

            decimal totalAmount = transferAmount + incomingCommission;
            string confirmMsg = string.Format("تأكيد عملية إرسال حوالة للأخ / {0} بمبلغ وقدره/ {1} {2}",
                this.SENDER_NAME.Text.Trim(),
                FormatAmount(totalAmount),
                currencyName);
            bool isConfirmed = CLS_MessagBox.Messag_Data_return_Check("إرسال حوالة", "save", confirmMsg);
            if (!isConfirmed) return;

            string incomingPay = string.IsNullOrEmpty(this.GlobalCode.Text) ? "حساب" : this.GlobalCode.Text.Trim();
            int currentYear = DateTime.Now.Year;
            long incomingBillNumber = TransferHelper.GetNextBillNumber(cs, incomingPay, currentYear);
            long outgoingNumber = TransferHelper.GetNextOutgoingNumber(cs, outgoingAccountId, currentYear);
            long outgoingBillNumber = TransferHelper.GetNextOutgoingBillNumber(cs, currentYear);
            long incomingNumber = 1;

            try
            {
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    cn.Open();
                    string iq = "SELECT ISNULL(MAX(CAST(IncomingNumber AS BIGINT)), 0) + 1 FROM tblTransfers WHERE ISNUMERIC(IncomingNumber) = 1 AND IncomingPay = @pay AND IncomingAccountID = @acc";
                    using (SqlCommand cm = new SqlCommand(iq, cn))
                    {
                        cm.Parameters.AddWithValue("@pay", incomingPay);
                        cm.Parameters.AddWithValue("@acc", incomingAccountId);
                        object res = cm.ExecuteScalar();
                        if (res != null && res != DBNull.Value) incomingNumber = Convert.ToInt64(res);
                    }
                }
            }
            catch { }

            DateTime transferDate = DateTime.Now.Date;
            string dateText = this.REMIT_DATE.Text.Trim();
            if (!string.IsNullOrEmpty(dateText) && dateText.Length >= 10)
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(dateText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    transferDate = parsedDate;
                }
            }

            string receiverName = this.BEN_NAME.Text.Trim();
            string destination = this.DestinationEntity.Text.Trim();
            string agentName = this.AgentId.Text.Trim();
            string userNotes = this.Note_PURPOSE.Text.Trim();
            string finalNotes = string.IsNullOrEmpty(userNotes) ?
                "-/شبكة المنيف اكسبرس" :
                userNotes + " -/شبكة المنيف اكسبرس";

            TransferDto dto = new TransferDto
            {
                TheType = "داخلي",
                TransferAmount = transferAmount,
                TransferCurrencyID = currencyId,
                ReceiverName = receiverName,
                SenderName = this.SENDER_NAME.Text.Trim(),
                Source = "المركز الرئيسي",
                Target = destination,
                TestState = "قيد المراجعة",
                IncomingNumber = incomingNumber,
                IncomingAccountID = incomingAccountId,
                IncomingAmount = transferAmount,
                IncomingCurrencyID = currencyId,
                IncomingCommission = incomingCommission,
                IncomingCommissionCurrencyID = commissionCurrencyId,
                IncomingDate = transferDate,
                IncomingNotes = finalNotes,
                IncomingPay = incomingPay,
                IncomingTime = DateTime.Now,
                IncomingUserID = 1,
                IncomingDebitingWay = 0,
                IncomingBillNumber = incomingBillNumber,
                OutgoingNumber = outgoingNumber,
                OutgoingAccountID = outgoingAccountId,
                OutgoingAmount = transferAmount,
                OutgoingCurrencyID = currencyId,
                OutgoingCommission = outgoingCommission,
                OutgoingCommissionCurrencyID = commissionCurrencyId,
                OutgoingCommissionWay = 0,
                OutgoingDate = transferDate,
                OutgoingNotes = finalNotes,
                OutgoingPay = "صادرة",
                OutgoingTime = DateTime.Now,
                OutgoingUserID = 1,
                OutgoingDebitingWay = 0,
                OutgoingBillNumber = outgoingBillNumber,
                ChequeNumber = null,
                ChequeDate = null,
                ChequeBank = null,
                TransferPurpose = this.PURPOSE_ID.Text.Trim(),
                UserID = 1,
                ReceiverPhone = this.BEN_MOBILE.Text.Trim(),
                SenderPhone = this.SENDER_MOBILE.Text.Trim(),
                OriginalSourceName = "المركز الرئيسي",
                OutgoingAccountName = agentName
            };

            try
            {
                this.button_Save.Enabled = false;
                long newTransferId = TransferHelper.SaveTransfer(dto, cs);
                UpdateTestStateToApproved(newTransferId);
                this.button_New_Click(sender, e);
                LoadTransfersToday();
                WaitForExpressNumberAndShowMessage(newTransferId, transferAmount, currencyName, receiverName, destination);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ الحوالة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.button_Save.Enabled = true;
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            LoadTransfersToday();
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                Form pf = this.ParentForm;
                if (pf != null && pf.GetType().Name == "Form_Main")
                {
                    var tc = pf.Controls.Find("mainTabControl", true)[0] as TabControl;
                    if (tc != null)
                    {
                        foreach (TabPage pg in tc.TabPages)
                        {
                            if (pg.Controls.Contains(this)) { tc.TabPages.Remove(pg); break; }
                        }
                    }
                }
                this.Close();
            }
            catch { this.Close(); }
        }

        private void button_Print_Click(object sender, EventArgs e) { MessageBox.Show("ميزة الطباعة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_show_Click(object sender, EventArgs e) { MessageBox.Show("ميزة البحث المتقدم قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_Sync_Click(object sender, EventArgs e) { MessageBox.Show("ميزة المزامنة قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_Arch_Click(object sender, EventArgs e) { MessageBox.Show("ميزة الأرشيف قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_GetAgent_Click(object sender, EventArgs e) { MessageBox.Show("ميزة البحث عن وكيل قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void BTN_Serch_MOB_Sen1_Click(object sender, EventArgs e) { MessageBox.Show("ميزة البحث عن المرسل قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void BTN_Serch_MOB_Ben1_Click(object sender, EventArgs e) { MessageBox.Show("ميزة البحث عن المستلم قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_Show_Data_Card_Send_Click(object sender, EventArgs e) { MessageBox.Show("ميزة بيانات بطاقة المرسل قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void button_Show_Data_Card_BEN_Click(object sender, EventArgs e) { MessageBox.Show("ميزة بيانات بطاقة المستلم قيد التطوير حالياً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void toolStripMenuItem1_Click(object sender, EventArgs e) { }
        private void label40_Click(object sender, EventArgs e) { }
        private void textBox_Search_TextChanged(object sender, EventArgs e) { }

        private void button_CopyToSender_Click(object sender, EventArgs e)
        {
            this.SENDER_NAME.Text = this.BEN_NAME.Text;
            this.SENDER_MOBILE.Text = this.BEN_MOBILE.Text;
        }

        private void button_CopyToBeneficiary_Click(object sender, EventArgs e)
        {
            this.BEN_NAME.Text = this.SENDER_NAME.Text;
            this.BEN_MOBILE.Text = this.SENDER_MOBILE.Text;
        }

        private void dGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dGV.Rows[e.RowIndex];
            try
            {
                this.button_Save.Enabled = false;
                this.button_Print.Enabled = true;
                this.REMIT_CODE.Text = (row.Cells[0].Value == null) ? "" : row.Cells[0].Value.ToString();
                this.REMIT_DATE.Text = (row.Cells[1].Value == null) ? "" : row.Cells[1].Value.ToString();
                this.ORIGINATION_AMOUNT.Text = (row.Cells[2].Value == null) ? "" : row.Cells[2].Value.ToString();
                this.ORIGINATION_CURRENCY.Text = (row.Cells[3].Value == null) ? "" : row.Cells[3].Value.ToString();
                this.SENDER_NAME.Text = (row.Cells[4].Value == null) ? "" : row.Cells[4].Value.ToString();
                this.SENDER_MOBILE.Text = (row.Cells[5].Value == null) ? "" : row.Cells[5].Value.ToString();
                this.BEN_NAME.Text = (row.Cells[6].Value == null) ? "" : row.Cells[6].Value.ToString();
                this.BEN_MOBILE.Text = (row.Cells[7].Value == null) ? "" : row.Cells[7].Value.ToString();
                this.REMIT_SEQ.Text = (row.Cells[8].Value == null) ? "" : row.Cells[8].Value.ToString();
                this.GlobalCode.Text = (row.Cells[9].Value == null) ? "" : row.Cells[9].Value.ToString();
                this.CustomerAccount.Text = (row.Cells[10].Value == null) ? "" : row.Cells[10].Value.ToString();
                this.AgentId.Text = (row.Cells[12].Value == null) ? "" : row.Cells[12].Value.ToString();
                this.DestinationEntity.Text = (row.Cells[13].Value == null) ? "" : row.Cells[13].Value.ToString();
                this.Note_PURPOSE.Text = (row.Cells[14].Value == null) ? "" : row.Cells[14].Value.ToString();
                this.PURPOSE_ID.Text = (row.Cells[15].Value == null) ? "" : row.Cells[15].Value.ToString();
                this.AgentCommission.Text = (row.Cells[16].Value == null) ? "" : row.Cells[16].Value.ToString();
                this.WAKIL_FEE.Text = (row.Cells[17].Value == null) ? "" : row.Cells[17].Value.ToString();
                this.Locked_text(true, false);
            }
            catch (Exception ex) { MessageBox.Show("خطأ: " + ex.Message); }
        }

        private void dGV_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dGV.SelectedRows.Count > 0)
                {
                    this.button_Save.Enabled = true;
                    this.button_Print.Enabled = false;
                    this.Locked_text(false, true);
                    this.REMIT_CODE.Text = "";
                    this.REMIT_DATE.Text = "";
                    this.REMIT_SEQ.Text = "";
                    this.REQUEST_CODE = "";
                    this.ORIGINATION_AMOUNT.Focus();
                }
                else { this.button_New_Click(sender, e); }
            }
            catch (Exception ex) { MessageBox.Show("خطأ: " + ex.Message); }
        }

        private void check_SelectAll_Sync_CheckedChanged(object sender, EventArgs e)
        {
            try { foreach (DataGridViewRow row in this.dGV.Rows) { if (!row.IsNewRow) row.Selected = this.check_SelectAll_Sync.Checked; } }
            catch { }
        }

        private void SENDER_NAME_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                TextBox txt = sender as TextBox;
                if (txt != null && txt.Text.Length > 0 && txt.Text[txt.Text.Length - 1] == ' ') { e.Handled = true; return; }
            }
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ') e.Handled = true;
        }

        private void SENDER_MOBILE_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true; }

        private void Form_Send_Money_Load(object sender, EventArgs e)
        {
            REMIT_DATE_Find.Value = DateTime.Now;
            LoadAgentsIntoComboBox();
            LoadCurrenciesIntoComboBox();
            LoadPurposesIntoComboBox();
            LoadCountriesIntoComboBox();
            LoadCustomerAccountsIntoComboBox();
            LoadBeneficiaryNamesAutoComplete();
            LoadSenderNamesAutoComplete();
            LoadTransfersToday();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) { }

        private void COUNTRY_SelectedIndexChanged(object sender, EventArgs e) { }

        private void AgentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.DestinationEntity.Items.Clear();
                this.DestinationEntity.Text = "";
                if (string.IsNullOrEmpty(this.AgentId.Text)) return;
                string cs = DatabaseHelper.GetConnectionString();
                string q = "SELECT [الجهة] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] = @a AND [الجهة] IS NOT NULL AND [الجهة] != ''";
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    using (SqlCommand cm = new SqlCommand(q, cn))
                    {
                        cm.Parameters.AddWithValue("@a", this.AgentId.Text.Trim());
                        cn.Open();
                        using (SqlDataReader rd = cm.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                string ent = rd["الجهة"].ToString().Trim();
                                if (!string.IsNullOrEmpty(ent) && !this.DestinationEntity.Items.Contains(ent))
                                    this.DestinationEntity.Items.Add(ent);
                            }
                        }
                    }
                }
                if (this.DestinationEntity.Items.Count > 0) this.DestinationEntity.SelectedIndex = 0;
                this.ORIGINATION_AMOUNT_TextChanged(sender, e);
            }
            catch { }
        }

        private void ORIGINATION_CURRENCY_SelectedIndexChanged(object sender, EventArgs e) { this.ORIGINATION_AMOUNT_TextChanged(sender, e); }

        private void ORIGINATION_AMOUNT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.WAKIL_FEE.Text = "";
                if (string.IsNullOrEmpty(this.ORIGINATION_AMOUNT.Text) || string.IsNullOrEmpty(this.AgentId.Text) || string.IsNullOrEmpty(this.ORIGINATION_CURRENCY.Text))
                { CalculateTotalAmount(); return; }
                decimal amount;
                if (!decimal.TryParse(this.ORIGINATION_AMOUNT.Text.Trim(), out amount)) { CalculateTotalAmount(); return; }
                string cn2 = this.ORIGINATION_CURRENCY.Text.Trim();
                decimal rate = 1.0m;
                if (!cn2.Contains("يمني"))
                {
                    string cs = DatabaseHelper.GetConnectionString();
                    using (SqlConnection cn = new SqlConnection(cs))
                    {
                        using (SqlCommand cm = new SqlCommand("SELECT ExchangePrice FROM tblCurrencies WHERE CurrencyName = @c", cn))
                        {
                            cm.Parameters.AddWithValue("@c", cn2);
                            cn.Open();
                            object rv = cm.ExecuteScalar();
                            if (rv != null && rv != DBNull.Value) rate = Convert.ToDecimal(rv);
                        }
                    }
                }
                decimal equiv = amount * rate;
                decimal fee = 0;
                string cs2 = DatabaseHelper.GetConnectionString();
                string fq = "SELECT [مبلغ العمولة] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 2] = @a AND @e >= ISNULL([المبلغ من], 0) AND @e <= ISNULL([الى مبلغ], 999999999999)";
                using (SqlConnection cn = new SqlConnection(cs2))
                {
                    using (SqlCommand cm = new SqlCommand(fq, cn))
                    {
                        cm.Parameters.AddWithValue("@a", this.AgentId.Text.Trim());
                        cm.Parameters.AddWithValue("@e", equiv);
                        cn.Open();
                        object rv = cm.ExecuteScalar();
                        if (rv != null && rv != DBNull.Value) fee = Convert.ToDecimal(rv);
                    }
                }
                this.WAKIL_FEE.Text = FormatAmount(fee);
                this.FEE_CUR_CODE.Text = "ريال يمني";
                CalculateTotalAmount();
            }
            catch { }
        }

        private void CalculateTotalAmount()
        {
            try
            {
                decimal amount = 0;
                decimal.TryParse(this.ORIGINATION_AMOUNT.Text.Trim(), out amount);
                decimal commission = 0;
                decimal.TryParse(this.AgentCommission.Text.Trim(), out commission);
                string currency = string.IsNullOrEmpty(this.ORIGINATION_CURRENCY.Text) ? "ريال" : this.ORIGINATION_CURRENCY.Text;
                decimal total = 0;
                if (currency.Contains("يمني"))
                {
                    total = amount + commission;
                }
                else
                {
                    decimal exchangeRate = 1.0m;
                    try
                    {
                        string cs = DatabaseHelper.GetConnectionString();
                        string rq = "SELECT ExchangePrice FROM tblCurrencies WHERE CurrencyName = @c";
                        using (SqlConnection cn = new SqlConnection(cs))
                        {
                            using (SqlCommand cm = new SqlCommand(rq, cn))
                            {
                                cm.Parameters.AddWithValue("@c", currency);
                                cn.Open();
                                object rv = cm.ExecuteScalar();
                                if (rv != null && rv != DBNull.Value)
                                {
                                    exchangeRate = Convert.ToDecimal(rv);
                                }
                            }
                        }
                    }
                    catch { }
                    total = (amount * exchangeRate) + commission;
                }
                this.Total_AMOUNT.Text = FormatAmount(total);
                this.label_Total_AMOUNT.Text = NumberToArabic(total, "ريال يمني");
            }
            catch { }
        }

        private void TRANSACTION_AMOUNT_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 8) e.Handled = true; }
        private void AgentCommission_TextChanged(object sender, EventArgs e) { CalculateTotalAmount(); }
        private void AgentCommission_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != 8) e.Handled = true; }
        private void CHe_FEE_FROM_REMIT_AMOUNT_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox_Serch_mobile_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox_SendTO_Ebda_CheckedChanged_1(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void check_All_User_CheckedChanged(object sender, EventArgs e) { }
        private void REMIT_DATE_Find_ValueChanged(object sender, EventArgs e) { }
        private void dGV_ColumnAdded(object sender, DataGridViewColumnEventArgs e) { }
        private void dGV_Sorted(object sender, EventArgs e) { }
        private void REMIT_CODE_Click(object sender, EventArgs e) { }
        private void REMIT_CODE_KeyDown(object sender, KeyEventArgs e) { }
        private void REMIT_CODE_KeyPress(object sender, KeyPressEventArgs e) { }
        private void REMIT_CODE_Leave(object sender, EventArgs e) { }
        private void Org_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) { }
        private void BEN_NAME_TextChanged(object sender, EventArgs e) { }
        private void COUNTRY_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.AgentId.Focus(); }
        private void AgentId_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.DestinationEntity.Focus(); }
        private void DestinationEntity_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.ORIGINATION_AMOUNT.Focus(); }

        private void ORIGINATION_AMOUNT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
            if (e.KeyCode == Keys.Return) { this.ORIGINATION_CURRENCY.DroppedDown = true; this.ORIGINATION_CURRENCY.Focus(); }
        }

        private void ORIGINATION_CURRENCY_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.AgentCommission.Focus(); }

        private void AgentCommission_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) this.GlobalCode.Focus();
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
        }

        private void GlobalCode_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.CustomerAccount.Focus(); }
        private void CustomerAccount_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) { e.Handled = true; this.BEN_MOBILE.Focus(); } }

        private void BEN_MOBILE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) this.BEN_NAME.Focus();
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
        }

        private void BEN_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) this.SENDER_MOBILE.Focus();
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
        }

        private void SENDER_MOBILE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) this.SENDER_NAME.Focus();
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
        }

        private void SENDER_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) this.PURPOSE_ID.Focus();
            if (e.KeyCode == Keys.ControlKey) this.Control_Pressed = true; else this.Control_Pressed = false;
        }

        private void PURPOSE_ID_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.Note_PURPOSE.Focus(); }
        private void Note_PURPOSE_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.button_Save.PerformClick(); }
        private void PURPOSE_DESCRIPTION_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Return) this.button_Save.Focus(); }

        private void COUNTRY_Enter(object sender, EventArgs e) { this.COUNTRY.BackColor = Color.YellowGreen; }
        private void COUNTRY_Leave(object sender, EventArgs e) { this.COUNTRY.BackColor = Color.White; }
        private void AgentId_Enter(object sender, EventArgs e) { this.AgentId.BackColor = Color.YellowGreen; }
        private void AgentId_Leave(object sender, EventArgs e) { this.AgentId.BackColor = Color.White; }
        private void DestinationEntity_Enter(object sender, EventArgs e) { this.DestinationEntity.BackColor = Color.YellowGreen; }
        private void DestinationEntity_Leave(object sender, EventArgs e) { this.DestinationEntity.BackColor = Color.White; }
        private void ORIGINATION_AMOUNT_Enter(object sender, EventArgs e) { this.ORIGINATION_AMOUNT.BackColor = Color.YellowGreen; }
        private void ORIGINATION_AMOUNT_Leave(object sender, EventArgs e) { this.ORIGINATION_AMOUNT.BackColor = Color.White; }
        private void ORIGINATION_CURRENCY_Enter(object sender, EventArgs e) { this.ORIGINATION_CURRENCY.BackColor = Color.YellowGreen; }
        private void ORIGINATION_CURRENCY_Leave(object sender, EventArgs e) { this.ORIGINATION_CURRENCY.BackColor = Color.White; }
        private void AgentCommission_Enter(object sender, EventArgs e) { this.AgentCommission.BackColor = Color.YellowGreen; }
        private void AgentCommission_Leave(object sender, EventArgs e) { this.AgentCommission.BackColor = Color.White; }
        private void GlobalCode_Enter(object sender, EventArgs e) { this.GlobalCode.BackColor = Color.YellowGreen; }
        private void GlobalCode_Leave(object sender, EventArgs e) { this.GlobalCode.BackColor = Color.White; }
        private void CustomerAccount_Enter(object sender, EventArgs e) { this.CustomerAccount.BackColor = Color.YellowGreen; }
        private void CustomerAccount_Leave(object sender, EventArgs e) { this.CustomerAccount.BackColor = Color.White; CheckAccountBalanceAndPhone(); }
        private void BEN_MOBILE_Enter(object sender, EventArgs e) { this.BEN_MOBILE.BackColor = Color.YellowGreen; }
        private void BEN_MOBILE_Leave(object sender, EventArgs e) { this.BEN_MOBILE.BackColor = Color.White; SearchBeneficiaryNameByPhone(); }
        private void BEN_NAME_Enter(object sender, EventArgs e) { this.BEN_NAME.BackColor = Color.YellowGreen; }
        private void BEN_NAME_Leave(object sender, EventArgs e) { this.BEN_NAME.BackColor = Color.White; }
        private void SENDER_MOBILE_Enter(object sender, EventArgs e) { this.SENDER_MOBILE.BackColor = Color.YellowGreen; }
        private void SENDER_MOBILE_Leave(object sender, EventArgs e) { this.SENDER_MOBILE.BackColor = Color.White; SearchSenderNameByPhone(); }
        private void SENDER_NAME_Enter(object sender, EventArgs e) { this.SENDER_NAME.BackColor = Color.YellowGreen; }
        private void SENDER_NAME_Leave(object sender, EventArgs e) { this.SENDER_NAME.BackColor = Color.White; }
        private void PURPOSE_ID_Enter(object sender, EventArgs e) { this.PURPOSE_ID.BackColor = Color.YellowGreen; }
        private void PURPOSE_ID_Leave(object sender, EventArgs e) { this.PURPOSE_ID.BackColor = Color.White; }
        private void Note_PURPOSE_Enter(object sender, EventArgs e) { this.Note_PURPOSE.BackColor = Color.YellowGreen; }
        private void Note_PURPOSE_Leave(object sender, EventArgs e) { this.Note_PURPOSE.BackColor = Color.White; }
        private void PURPOSE_DESCRIPTION_Enter(object sender, EventArgs e) { this.PURPOSE_DESCRIPTION.BackColor = Color.YellowGreen; }
        private void PURPOSE_DESCRIPTION_Leave(object sender, EventArgs e) { this.PURPOSE_DESCRIPTION.BackColor = Color.White; }

        private void CustomerAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            _debitWarningAcknowledged = false;
            CheckAccountBalanceAndPhone();
            if (this.CustomerAccount.SelectedIndex >= 0) this.SENDER_NAME.Focus();
        }

        private void CheckAccountBalanceAndPhone()
        {
            _isAccountDebit = false;
            _debitAccountName = "";
            _accountBalance = 0;
            string accText = this.CustomerAccount.Text.Trim();
            if (string.IsNullOrEmpty(accText)) return;
            string cs = DatabaseHelper.GetConnectionString();
            decimal totalYER = 0;
            string finalName = "";
            string finalPhone = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    cn.Open();
                    string aq = "SELECT TOP 1 a.AccountName, ISNULL(s.PhoneNumbers, N'') AS PhoneNumbers FROM tblAccounts a WITH(NOLOCK) LEFT JOIN tblSMSForAccounts s WITH(NOLOCK) ON a.ID = s.AccountID WHERE a.AccountNumber = @t OR a.AccountName LIKE @tl";
                    using (SqlCommand cm = new SqlCommand(aq, cn))
                    {
                        cm.Parameters.AddWithValue("@t", accText);
                        cm.Parameters.AddWithValue("@tl", "%" + accText + "%");
                        using (SqlDataReader rd = cm.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                finalName = rd["AccountName"].ToString();
                                finalPhone = rd["PhoneNumbers"].ToString();
                                if (finalPhone == "0" || finalPhone == "غير مسجل") finalPhone = "";
                            }
                        }
                    }
                    string bq = "SELECT c.CurrencyName, ISNULL(c.BuyPrice, 1) AS BuyPrice, ISNULL(c.SellPrice, 1) AS SellPrice, SUM(ISNULL(ed.Amount, 0)) AS Bal FROM tblAccounts p WITH(NOLOCK) INNER JOIN tblAccounts sub WITH(NOLOCK) ON sub.FatherID = p.ID OR sub.FatherNumber = p.AccountNumber OR sub.ID = p.ID INNER JOIN tblEntriesDetails ed WITH(NOLOCK) ON ed.AccountID = sub.ID INNER JOIN tblCurrencies c WITH(NOLOCK) ON ed.CurrencyID = c.ID OR sub.GroupID = c.ID WHERE p.AccountNumber = @t OR p.AccountName LIKE @tl GROUP BY c.CurrencyName, c.BuyPrice, c.SellPrice HAVING SUM(ISNULL(ed.Amount, 0)) <> 0";
                    DataTable dt = new DataTable();
                    using (SqlCommand cm = new SqlCommand(bq, cn))
                    {
                        cm.Parameters.AddWithValue("@t", accText);
                        cm.Parameters.AddWithValue("@tl", "%" + accText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cm)) { da.Fill(dt); }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        string sq = "SELECT ISNULL(c.CurrencyName, N'يمني') AS CurrencyName, ISNULL(c.BuyPrice, 1) AS BuyPrice, ISNULL(c.SellPrice, 1) AS SellPrice, SUM(ISNULL(s.Amount, 0)) AS Bal FROM tblAccounts p WITH(NOLOCK) INNER JOIN tblAccounts sub WITH(NOLOCK) ON sub.FatherID = p.ID OR sub.FatherNumber = p.AccountNumber OR sub.ID = p.ID INNER JOIN tblAccountsSlating s WITH(NOLOCK) ON s.AccountID = sub.ID LEFT JOIN tblCurrencies c WITH(NOLOCK) ON sub.GroupID = c.ID OR sub.System = c.ID WHERE p.AccountNumber = @t OR p.AccountName LIKE @tl GROUP BY c.CurrencyName, c.BuyPrice, c.SellPrice HAVING SUM(ISNULL(s.Amount, 0)) <> 0";
                        using (SqlCommand cm = new SqlCommand(sq, cn))
                        {
                            cm.Parameters.AddWithValue("@t", accText);
                            cm.Parameters.AddWithValue("@tl", "%" + accText + "%");
                            using (SqlDataAdapter da = new SqlDataAdapter(cm)) { da.Fill(dt); }
                        }
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        string cur = row["CurrencyName"].ToString();
                        decimal bal = Convert.ToDecimal(row["Bal"]);
                        decimal bp = Convert.ToDecimal(row["BuyPrice"]);
                        decimal sp = Convert.ToDecimal(row["SellPrice"]);
                        decimal ar = 1;
                        if (cur.Contains("يمني")) ar = 1;
                        else if (bal > 0) ar = bp;
                        else ar = sp;
                        totalYER += (bal * ar);
                    }
                    try
                    {
                        string ceilingQuery = "SELECT ISNULL([مبلغ السقف], 0) FROM [بيانات الوكلاء] WHERE [حساب السقف] = @acc AND [حساب السقف] IS NOT NULL AND [حساب السقف] != ''";
                        using (SqlCommand cmdCeiling = new SqlCommand(ceilingQuery, cn))
                        {
                            cmdCeiling.Parameters.AddWithValue("@acc", accText);
                            object ceilingResult = cmdCeiling.ExecuteScalar();
                            if (ceilingResult != null && ceilingResult != DBNull.Value)
                            {
                                decimal ceiling = Convert.ToDecimal(ceilingResult);
                                totalYER += ceiling;
                            }
                        }
                    }
                    catch { }
                    _accountBalance = totalYER;
                    _debitAccountName = string.IsNullOrEmpty(finalName) ? accText : finalName;
                    if (totalYER < 0) _isAccountDebit = true;
                    if (!string.IsNullOrEmpty(finalName)) this.SENDER_NAME.Text = finalName;
                    if (!string.IsNullOrEmpty(finalPhone)) this.SENDER_MOBILE.Text = finalPhone;
                }
            }
            catch { }
        }

        private void SearchBeneficiaryNameByPhone()
        {
            try
            {
                string phone = this.BEN_MOBILE.Text.Trim();
                if (string.IsNullOrEmpty(phone) || phone.Length < 7) return;
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT TOP 1 ReceiverName FROM tblTransfers WHERE ReceiverPhone = @phone AND ReceiverName IS NOT NULL AND ReceiverName != '' ORDER BY ID DESC";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string name = result.ToString().Trim();
                            if (!string.IsNullOrEmpty(name))
                            {
                                this.BEN_NAME.Text = name;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void LoadBeneficiaryNamesAutoComplete()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT DISTINCT ReceiverName FROM tblTransfers WHERE ReceiverName IS NOT NULL AND ReceiverName != '' ORDER BY ReceiverName";
                AutoCompleteStringCollection nameCollection = new AutoCompleteStringCollection();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["ReceiverName"].ToString().Trim();
                                if (!string.IsNullOrEmpty(name))
                                {
                                    nameCollection.Add(name);
                                }
                            }
                        }
                    }
                }
                this.BEN_NAME.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.BEN_NAME.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.BEN_NAME.AutoCompleteCustomSource = nameCollection;
            }
            catch { }
        }

        private void SearchSenderNameByPhone()
        {
            try
            {
                string phone = this.SENDER_MOBILE.Text.Trim();
                if (string.IsNullOrEmpty(phone) || phone.Length < 7) return;
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT TOP 1 SenderName FROM tblTransfers WHERE SenderPhone = @phone AND SenderName IS NOT NULL AND SenderName != '' ORDER BY ID DESC";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string name = result.ToString().Trim();
                            if (!string.IsNullOrEmpty(name))
                            {
                                this.SENDER_NAME.Text = name;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void LoadSenderNamesAutoComplete()
        {
            try
            {
                string connString = DatabaseHelper.GetConnectionString();
                string query = "SELECT DISTINCT SenderName FROM tblTransfers WHERE SenderName IS NOT NULL AND SenderName != '' ORDER BY SenderName";
                AutoCompleteStringCollection nameCollection = new AutoCompleteStringCollection();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["SenderName"].ToString().Trim();
                                if (!string.IsNullOrEmpty(name))
                                {
                                    nameCollection.Add(name);
                                }
                            }
                        }
                    }
                }
                this.SENDER_NAME.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.SENDER_NAME.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.SENDER_NAME.AutoCompleteCustomSource = nameCollection;
            }
            catch { }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab) { this.tab = true; return base.ProcessCmdKey(ref msg, keyData); }
            else if (keyData == (Keys.Shift | Keys.Tab)) { this.tab = false; return base.ProcessCmdKey(ref msg, keyData); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void Clear_text()
        {
            try
            {
                this.REMIT_CODE.Text = ""; this.REMIT_DATE.Text = ""; this.REMIT_SEQ.Text = "";
                this.ORIGINATION_AMOUNT.Text = ""; this.label_ORIGINATION_AMOUNT.Text = "";
                this.WAKIL_FEE.Text = ""; this.FEE_CUR_CODE.Text = "ريال يمني";
                this.AgentCommission.Text = ""; this.CenterCommission.Text = ""; this.CommissionCurrency.Text = "";
                this.Total_AMOUNT.Text = ""; this.label_Total_AMOUNT.Text = "";
                this.PURPOSE_DESCRIPTION.Text = ""; this.Note_PURPOSE.Text = "";
                this.BEN_NAME.Text = ""; this.BEN_MOBILE.Text = "";
                this.SENDER_NAME.Text = ""; this.SENDER_MOBILE.Text = ""; this.Status.Text = "";
            }
            catch { }
        }

        public void Locked_text(bool boo, bool boo2)
        {
            try
            {
                this.REMIT_CODE.ReadOnly = true;
                this.COUNTRY.Enabled = boo2; this.AgentId.Enabled = boo2;
                this.ORIGINATION_AMOUNT.ReadOnly = boo; this.ORIGINATION_CURRENCY.Enabled = boo2;
                this.CHe_FEE_FROM_REMIT_AMOUNT.Enabled = boo2; this.AgentCommission.ReadOnly = boo;
                this.PURPOSE_ID.Enabled = boo2; this.PURPOSE_DESCRIPTION.ReadOnly = boo; this.Note_PURPOSE.ReadOnly = boo;
                this.BEN_NAME.ReadOnly = boo; this.BEN_MOBILE.ReadOnly = boo;
                this.SENDER_NAME.ReadOnly = boo; this.SENDER_MOBILE.ReadOnly = boo;
                this.CustomerAccount.Enabled = !boo; this.GlobalCode.Enabled = !boo;
            }
            catch { }
        }

        private void LoadAgentsIntoComboBox()
        {
            try
            {
                this.AgentId.Items.Clear();
                string cs = DatabaseHelper.GetConnectionString();
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    using (SqlCommand cm = new SqlCommand("SELECT DISTINCT [اسم الوكيل 1] FROM [بيانات الوكلاء] WHERE [اسم الوكيل 1] IS NOT NULL AND [اسم الوكيل 1] != ''", cn))
                    {
                        cn.Open();
                        using (SqlDataReader rd = cm.ExecuteReader()) { while (rd.Read()) { string n = rd["اسم الوكيل 1"].ToString().Trim(); if (!this.AgentId.Items.Contains(n)) this.AgentId.Items.Add(n); } }
                    }
                }
                if (this.AgentId.Items.Count > 0) this.AgentId.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadCurrenciesIntoComboBox()
        {
            try
            {
                this.ORIGINATION_CURRENCY.Items.Clear();
                string cs = DatabaseHelper.GetConnectionString();
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    using (SqlCommand cm = new SqlCommand("SELECT CurrencyName FROM tblCurrencies WHERE CurrencyName IS NOT NULL AND CurrencyName != '' ORDER BY ID", cn))
                    {
                        cn.Open();
                        using (SqlDataReader rd = cm.ExecuteReader()) { while (rd.Read()) { string n = rd["CurrencyName"].ToString().Trim(); if (!string.IsNullOrEmpty(n) && !this.ORIGINATION_CURRENCY.Items.Contains(n)) this.ORIGINATION_CURRENCY.Items.Add(n); } }
                    }
                }
                if (this.ORIGINATION_CURRENCY.Items.Count > 0) this.ORIGINATION_CURRENCY.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadPurposesIntoComboBox()
        {
            try
            {
                this.PURPOSE_ID.Items.Clear();
                this.PURPOSE_ID.Items.Add("شخصي"); this.PURPOSE_ID.Items.Add("تجاري");
                this.PURPOSE_ID.Items.Add("تعليمي"); this.PURPOSE_ID.Items.Add("طبي"); this.PURPOSE_ID.Items.Add("أخرى");
                this.PURPOSE_ID.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadCountriesIntoComboBox()
        {
            try
            {
                this.COUNTRY.Items.Clear();
                this.COUNTRY.Items.Add("اليمن"); this.COUNTRY.Items.Add("السعودية"); this.COUNTRY.Items.Add("الإمارات");
                this.COUNTRY.Items.Add("مصر"); this.COUNTRY.Items.Add("الأردن"); this.COUNTRY.Items.Add("السودان");
                this.COUNTRY.Items.Add("الصومال"); this.COUNTRY.Items.Add("جيبوتي");
                this.COUNTRY.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadCustomerAccountsIntoComboBox()
        {
            try
            {
                this.CustomerAccount.Items.Clear();
                string cs = DatabaseHelper.GetConnectionString();
                using (SqlConnection cn = new SqlConnection(cs))
                {
                    using (SqlCommand cm = new SqlCommand("SELECT AccountName FROM tblAccounts WHERE AccountName IS NOT NULL AND AccountName != '' ORDER BY AccountName", cn))
                    {
                        cn.Open();
                        using (SqlDataReader rd = cm.ExecuteReader()) { while (rd.Read()) { string n = rd["AccountName"].ToString().Trim(); if (!string.IsNullOrEmpty(n) && !this.CustomerAccount.Items.Contains(n)) this.CustomerAccount.Items.Add(n); } }
                    }
                }
            }
            catch { }
        }

        private string FormatAmount(decimal amount)
        {
            if (amount == Math.Floor(amount)) return amount.ToString("0");
            return amount.ToString("0.##");
        }

        private string NumberToArabic(decimal number, string currencyName)
        {
            if (string.IsNullOrEmpty(currencyName)) currencyName = "ريال";
            if (number == 0) return "صفر " + currencyName + " فقط لا غير";
            long n = (long)Math.Truncate(number);
            if (n == 0) return "صفر " + currencyName + " فقط لا غير";
            string[] ones = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            string[] tens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            string[] hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
            string result = "";
            long temp = n;
            long units = temp % 1000; temp /= 1000;
            long thousands = temp % 1000; temp /= 1000;
            long millions = temp % 1000; temp /= 1000;
            long billions = temp % 1000;
            if (billions > 0) result += FormatGroup(billions, "مليار", "ملياران", "مليارات", ones, tens, hundreds) + " ";
            if (millions > 0) result += FormatGroup(millions, "مليون", "مليونان", "ملايين", ones, tens, hundreds) + " ";
            if (thousands > 0) result += FormatGroup(thousands, "ألف", "ألفان", "آلاف", ones, tens, hundreds) + " ";
            if (units > 0) result += FormatGroup(units, "", "", "", ones, tens, hundreds);
            return result.Trim() + " " + currencyName + " فقط لا غير";
        }

        private string FormatGroup(long num, string s1, string s2, string p, string[] o, string[] t, string[] h)
        {
            if (num == 0) return "";
            if (num == 1) return s1;
            if (num == 2) return s2;
            if (num >= 3 && num <= 10) return o[num] + " " + p;
            long rem = num % 100;
            long hnd = num / 100;
            string res = "";
            if (hnd > 0) res += h[hnd];
            if (rem > 0)
            {
                if (res.Length > 0) res += " و ";
                if (rem <= 19) res += o[rem];
                else
                {
                    long unit = rem % 10;
                    long ten = rem / 10;
                    if (unit > 0) res += o[unit] + " و " + t[ten];
                    else res += t[ten];
                }
            }
            return res.Trim();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.label4 = new Label(); this.button2 = new Button(); this.label7 = new Label();
            this.COUNTRY = new ComboBox();
            this.COUNTRY.Leave += new EventHandler(this.COUNTRY_Leave);
            this.COUNTRY.Enter += new EventHandler(this.COUNTRY_Enter);
            this.COUNTRY.KeyDown += new KeyEventHandler(this.COUNTRY_KeyDown);
            this.AgentId = new ComboBox();
            this.AgentId.Leave += new EventHandler(this.AgentId_Leave);
            this.AgentId.Enter += new EventHandler(this.AgentId_Enter);
            this.AgentId.KeyDown += new KeyEventHandler(this.AgentId_KeyDown);
            this.AgentId.SelectedIndexChanged += new EventHandler(this.AgentId_SelectedIndexChanged);
            this.label6 = new Label();
            this.ORIGINATION_CURRENCY = new ComboBox();
            this.ORIGINATION_CURRENCY.Leave += new EventHandler(this.ORIGINATION_CURRENCY_Leave);
            this.ORIGINATION_CURRENCY.Enter += new EventHandler(this.ORIGINATION_CURRENCY_Enter);
            this.ORIGINATION_CURRENCY.KeyDown += new KeyEventHandler(this.ORIGINATION_CURRENCY_KeyDown);
            this.ORIGINATION_CURRENCY.SelectedIndexChanged += new EventHandler(this.ORIGINATION_CURRENCY_SelectedIndexChanged);
            this.ORIGINATION_AMOUNT = new TextBox();
            this.ORIGINATION_AMOUNT.Leave += new EventHandler(this.ORIGINATION_AMOUNT_Leave);
            this.ORIGINATION_AMOUNT.Enter += new EventHandler(this.ORIGINATION_AMOUNT_Enter);
            this.ORIGINATION_AMOUNT.KeyDown += new KeyEventHandler(this.ORIGINATION_AMOUNT_KeyDown);
            this.label5 = new Label();
            this.labelAgent = new Label();
            this.labelAgent.AutoSize = true; this.labelAgent.Location = new Point(10, 7); this.labelAgent.Name = "labelAgent"; this.labelAgent.Size = new Size(60, 19); this.labelAgent.TabIndex = 11; this.labelAgent.Text = "الوكيل: "; this.labelAgent.TextAlign = ContentAlignment.MiddleRight;
            this.CHe_FEE_FROM_REMIT_AMOUNT = new CheckBox();
            this.label12 = new Label();
            this.BEN_MOBILE = new TextBox();
            this.BEN_MOBILE.Leave += new EventHandler(this.BEN_MOBILE_Leave); this.BEN_MOBILE.Enter += new EventHandler(this.BEN_MOBILE_Enter); this.BEN_MOBILE.KeyDown += new KeyEventHandler(this.BEN_MOBILE_KeyDown);
            this.label15 = new Label();
            this.BEN_NAME = new TextBox();
            this.BEN_NAME.Leave += new EventHandler(this.BEN_NAME_Leave); this.BEN_NAME.Enter += new EventHandler(this.BEN_NAME_Enter); this.BEN_NAME.KeyDown += new KeyEventHandler(this.BEN_NAME_KeyDown);
            this.label11 = new Label();
            this.PURPOSE_DESCRIPTION = new TextBox();
            this.PURPOSE_DESCRIPTION.Leave += new EventHandler(this.PURPOSE_DESCRIPTION_Leave); this.PURPOSE_DESCRIPTION.Enter += new EventHandler(this.PURPOSE_DESCRIPTION_Enter);
            this.label13 = new Label(); this.label14 = new Label(); this.label17 = new Label();
            this.WAKIL_FEE = new TextBox(); this.label18 = new Label(); this.label23 = new Label(); this.label24 = new Label();
            this.AgentCommission = new TextBox();
            this.AgentCommission.Leave += new EventHandler(this.AgentCommission_Leave); this.AgentCommission.Enter += new EventHandler(this.AgentCommission_Enter); this.AgentCommission.KeyDown += new KeyEventHandler(this.AgentCommission_KeyDown); this.AgentCommission.TextChanged += new EventHandler(this.AgentCommission_TextChanged);
            this.label25 = new Label(); this.Total_AMOUNT = new TextBox();
            this.PURPOSE_ID = new ComboBox();
            this.PURPOSE_ID.Leave += new EventHandler(this.PURPOSE_ID_Leave); this.PURPOSE_ID.Enter += new EventHandler(this.PURPOSE_ID_Enter); this.PURPOSE_ID.KeyDown += new KeyEventHandler(this.PURPOSE_ID_KeyDown);
            this.Note_PURPOSE = new TextBox();
            this.Note_PURPOSE.Leave += new EventHandler(this.Note_PURPOSE_Leave); this.Note_PURPOSE.Enter += new EventHandler(this.Note_PURPOSE_Enter); this.Note_PURPOSE.KeyDown += new KeyEventHandler(this.Note_PURPOSE_KeyDown);
            this.label31 = new Label();
            this.SENDER_NAME = new TextBox();
            this.SENDER_NAME.Enter += new EventHandler(this.SENDER_NAME_Enter); this.SENDER_NAME.Leave += new EventHandler(this.SENDER_NAME_Leave); this.SENDER_NAME.KeyPress += new KeyPressEventHandler(this.SENDER_NAME_KeyPress); this.SENDER_NAME.KeyDown += new KeyEventHandler(this.SENDER_NAME_KeyDown);
            this.label29 = new Label();
            this.SENDER_MOBILE = new TextBox();
            this.SENDER_MOBILE.Enter += new EventHandler(this.SENDER_MOBILE_Enter); this.SENDER_MOBILE.Leave += new EventHandler(this.SENDER_MOBILE_Leave); this.SENDER_MOBILE.KeyPress += new KeyPressEventHandler(this.SENDER_MOBILE_KeyPress); this.SENDER_MOBILE.KeyDown += new KeyEventHandler(this.SENDER_MOBILE_KeyDown);
            this.label32 = new Label();
            this.GlobalCode = new ComboBox();
            this.GlobalCode.Enter += new EventHandler(this.GlobalCode_Enter); this.GlobalCode.Leave += new EventHandler(this.GlobalCode_Leave); this.GlobalCode.KeyDown += new KeyEventHandler(this.GlobalCode_KeyDown);
            this.CustomerAccount = new ComboBox();
            this.CustomerAccount.DropDownStyle = ComboBoxStyle.DropDown; this.CustomerAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend; this.CustomerAccount.AutoCompleteSource = AutoCompleteSource.ListItems; this.CustomerAccount.BackColor = Color.White; this.CustomerAccount.Location = new Point(10, 50); this.CustomerAccount.Name = "CustomerAccount"; this.CustomerAccount.Size = new Size(180, 26); this.CustomerAccount.TabIndex = 7; this.CustomerAccount.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            this.CustomerAccount.Enter += new EventHandler(this.CustomerAccount_Enter); this.CustomerAccount.Leave += new EventHandler(this.CustomerAccount_Leave); this.CustomerAccount.KeyDown += new KeyEventHandler(this.CustomerAccount_KeyDown); this.CustomerAccount.SelectedIndexChanged += new EventHandler(this.CustomerAccount_SelectedIndexChanged);
            this.labelCustomerAccount = new Label(); this.labelCustomerAccount.AutoSize = true; this.labelCustomerAccount.Location = new Point(195, 53); this.labelCustomerAccount.Name = "labelCustomerAccount"; this.labelCustomerAccount.Size = new Size(85, 19); this.labelCustomerAccount.TabIndex = 0; this.labelCustomerAccount.Text = "حساب القبض: "; this.labelCustomerAccount.TextAlign = ContentAlignment.MiddleRight; this.labelCustomerAccount.BackColor = Color.Transparent;
            this.GlobalCode.DropDownStyle = ComboBoxStyle.DropDownList; this.GlobalCode.FormattingEnabled = true; this.GlobalCode.Items.AddRange(new object[] { "حساب", "نقد" });
            this.label34 = new Label();
            this.M_Proccess = new MenuStrip(); this.M_New = new ToolStripMenuItem(); this.M_Save = new ToolStripMenuItem(); this.M_Edit = new ToolStripMenuItem(); this.M_Menu_Tab = new ToolStripMenuItem(); this.M_Cancel = new ToolStripMenuItem(); this.M_Serch = new ToolStripMenuItem(); this.M_Print = new ToolStripMenuItem(); this.M_Exit = new ToolStripMenuItem();
            this.dGV = new DataGridView(); this.dGV.ReadOnly = true;
            this.Dgv_ExpressNum = new DataGridViewTextBoxColumn();
            this.Dgv_OutgoingDate = new DataGridViewTextBoxColumn();
            this.Dgv_TransferAmount = new DataGridViewTextBoxColumn();
            this.Dgv_CurrencyName = new DataGridViewTextBoxColumn();
            this.Dgv_SenderName = new DataGridViewTextBoxColumn();
            this.Dgv_SenderPhone = new DataGridViewTextBoxColumn();
            this.Dgv_ReceiverName = new DataGridViewTextBoxColumn();
            this.Dgv_ReceiverPhone = new DataGridViewTextBoxColumn();
            this.Dgv_OutgoingNumber = new DataGridViewTextBoxColumn();
            this.Dgv_IncomingPay = new DataGridViewTextBoxColumn();
            this.Dgv_IncomingAccount = new DataGridViewTextBoxColumn();
            this.Dgv_UserName = new DataGridViewTextBoxColumn();
            this.Dgv_OutgoingAccount = new DataGridViewTextBoxColumn();
            this.Dgv_Target = new DataGridViewTextBoxColumn();
            this.Dgv_Notes = new DataGridViewTextBoxColumn();
            this.Dgv_TransferPurpose = new DataGridViewTextBoxColumn();
            this.Dgv_IncomingCommission = new DataGridViewTextBoxColumn();
            this.Dgv_OutgoingCommission = new DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components); this.toolStripMenuItem1 = new ToolStripMenuItem(); this.MenuItemArch = new ToolStripMenuItem();
            this.panel1 = new Panel(); this.CenterCommission = new Label(); this.CommissionCurrency = new Label(); this.FEE_CUR_CODE = new Label(); this.label_Total_AMOUNT = new Label();
            this.panel2 = new Panel(); this.button_Show_Data_Card_Send = new Button(); this.BTN_Serch_MOB_Sen1 = new PictureBox(); this.button_CopyToBeneficiary = new Button();
            this.panel3 = new Panel(); this.button_Show_Data_Card_BEN = new Button(); this.BTN_Serch_MOB_Ben1 = new PictureBox(); this.button_CopyToSender = new Button(); this.label19 = new Label();
            this.panel4 = new Panel(); this.panel11 = new Panel(); this.label_ORIGINATION_AMOUNT = new Label(); this.ExchangerAccountCurrencyName = new Label(); this.ExchangerAccountAmount = new Label(); this.label39 = new Label();
            this.panel10 = new Panel(); this.button_GetAgent = new Button(); this.label41 = new Label();
            this.DestinationEntity = new ComboBox(); this.DestinationEntity.KeyDown += new KeyEventHandler(this.DestinationEntity_KeyDown); this.DestinationEntity.Enter += new EventHandler(this.DestinationEntity_Enter); this.DestinationEntity.Leave += new EventHandler(this.DestinationEntity_Leave);
            this.panel9 = new Panel(); this.panel5 = new Panel(); this.panel8 = new Panel(); this.panel7 = new Panel(); this.Status = new Label(); this.DeliverStatus = new Label(); this.REMIT_DATE = new MaskedTextBox(); this.REMIT_CODE = new TextBox(); this.label16 = new Label(); this.label2 = new Label(); this.label8 = new Label(); this.REMIT_SEQ = new TextBox();
            this.panel6 = new Panel(); this.checkBox_Serch_mobile = new CheckBox(); this.checkBox_SendTO_Ebda = new CheckBox(); this.checkBox1 = new CheckBox(); this.SoucrceName = new Label(); this.label9 = new Label();
            this.backgroundWorker1 = new BackgroundWorker(); this.button_Sync = new Button(); this.check_SelectAll_Sync = new CheckBox(); this.REMIT_DATE_Find = new DateTimePicker(); this.label40 = new Label(); this.label_RemitCount = new Label(); this.label38 = new Label(); this.label37 = new Label(); this.textBox_Search = new TextBox();
            this.button_Update = new Button(); this.button_Save = new Button(); this.button_Exit = new Button(); this.button_New = new Button(); this.button_Print = new Button(); this.button_show = new Button(); this.check_All_User = new CheckBox();
            this.panel12 = new Panel(); this.panel13 = new Panel(); this.button_Arch = new Button(); this.pictureBoxCompanyLogo = new PictureBox(); this.panel14 = new Panel(); this.panel15 = new Panel();
            this.bindingSource1 = new BindingSource(this.components); this.dataSet1 = new DataSet1();
            this.panel1.SuspendLayout(); this.panel2.SuspendLayout(); this.panel3.SuspendLayout(); this.panel4.SuspendLayout(); this.panel5.SuspendLayout(); this.panel6.SuspendLayout(); this.panel7.SuspendLayout(); this.panel8.SuspendLayout(); this.panel9.SuspendLayout(); this.panel10.SuspendLayout(); this.panel11.SuspendLayout(); this.panel12.SuspendLayout(); this.panel13.SuspendLayout(); this.panel14.SuspendLayout(); this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).BeginInit(); ((System.ComponentModel.ISupportInitialize)(this.BTN_Serch_MOB_Sen1)).BeginInit(); ((System.ComponentModel.ISupportInitialize)(this.BTN_Serch_MOB_Ben1)).BeginInit(); ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).BeginInit(); ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit(); ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.SuspendLayout();
            this.label4.BackColor = Color.DarkRed; this.label4.Dock = DockStyle.Top; this.label4.Font = new Font("Arial", 10.5f, FontStyle.Bold); this.label4.ForeColor = Color.White; this.label4.Location = new Point(0, 0); this.label4.Name = "label4"; this.label4.Size = new Size(518, 22); this.label4.TabIndex = 9; this.label4.Text = "بيانات الجهة "; this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.button2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold); this.button2.Location = new Point(4, 2); this.button2.Name = "button2"; this.button2.Size = new Size(57, 29); this.button2.TabIndex = 8; this.button2.Text = "قائمة الوكلاء "; this.button2.UseVisualStyleBackColor = true; this.button2.Visible = false;
            this.label7.AutoSize = true; this.label7.Location = new Point(428, 4); this.label7.Name = "label7"; this.label7.Size = new Size(90, 19); this.label7.TabIndex = 7; this.label7.Text = " مبلغ الحوالة: ";
            this.COUNTRY.BackColor = Color.White; this.COUNTRY.DropDownStyle = ComboBoxStyle.DropDownList; this.COUNTRY.FormattingEnabled = true; this.COUNTRY.Location = new Point(319, 2); this.COUNTRY.Name = "COUNTRY"; this.COUNTRY.Size = new Size(107, 27); this.COUNTRY.TabIndex = 0; this.COUNTRY.SelectedIndexChanged += new EventHandler(this.COUNTRY_SelectedIndexChanged);
            this.AgentId.DropDownStyle = ComboBoxStyle.DropDownList; this.AgentId.FormattingEnabled = true; this.AgentId.Location = new Point(64, 2); this.AgentId.Name = "AgentId"; this.AgentId.Size = new Size(253, 27); this.AgentId.TabIndex = 1;
            this.label6.AutoSize = true; this.label6.Location = new Point(139, 6); this.label6.Name = "label6"; this.label6.Size = new Size(57, 19); this.label6.TabIndex = 4; this.label6.Text = " العملة: ";
            this.ORIGINATION_CURRENCY.DropDownStyle = ComboBoxStyle.DropDownList; this.ORIGINATION_CURRENCY.FormattingEnabled = true; this.ORIGINATION_CURRENCY.Location = new Point(36, 0); this.ORIGINATION_CURRENCY.Name = "ORIGINATION_CURRENCY"; this.ORIGINATION_CURRENCY.Size = new Size(101, 27); this.ORIGINATION_CURRENCY.TabIndex = 3; this.ORIGINATION_CURRENCY.SelectedIndexChanged += new EventHandler(this.ORIGINATION_CURRENCY_SelectedIndexChanged);
            this.ORIGINATION_AMOUNT.BorderStyle = BorderStyle.FixedSingle; this.ORIGINATION_AMOUNT.Location = new Point(306, 1); this.ORIGINATION_AMOUNT.Name = "ORIGINATION_AMOUNT"; this.ORIGINATION_AMOUNT.RightToLeft = RightToLeft.No; this.ORIGINATION_AMOUNT.Size = new Size(121, 26); this.ORIGINATION_AMOUNT.TabIndex = 2; this.ORIGINATION_AMOUNT.TextAlign = HorizontalAlignment.Center; this.ORIGINATION_AMOUNT.TextChanged += new EventHandler(this.ORIGINATION_AMOUNT_TextChanged); this.ORIGINATION_AMOUNT.KeyPress += new KeyPressEventHandler(this.TRANSACTION_AMOUNT_KeyPress); this.ORIGINATION_AMOUNT.KeyDown += new KeyEventHandler(this.ORIGINATION_AMOUNT_KeyDown);
            this.label5.AutoSize = true; this.label5.Location = new Point(428, 7); this.label5.Name = "label5"; this.label5.Size = new Size(72, 19); this.label5.TabIndex = 1; this.label5.Text = "* إلى دولة: ";
            this.CHe_FEE_FROM_REMIT_AMOUNT.AutoSize = true; this.CHe_FEE_FROM_REMIT_AMOUNT.Location = new Point(-43, 6); this.CHe_FEE_FROM_REMIT_AMOUNT.Name = "CHe_FEE_FROM_REMIT_AMOUNT"; this.CHe_FEE_FROM_REMIT_AMOUNT.RightToLeft = RightToLeft.No; this.CHe_FEE_FROM_REMIT_AMOUNT.Size = new Size(95, 23); this.CHe_FEE_FROM_REMIT_AMOUNT.TabIndex = 10; this.CHe_FEE_FROM_REMIT_AMOUNT.Text = ":مع العمولة "; this.CHe_FEE_FROM_REMIT_AMOUNT.UseVisualStyleBackColor = true; this.CHe_FEE_FROM_REMIT_AMOUNT.Visible = false; this.CHe_FEE_FROM_REMIT_AMOUNT.CheckedChanged += new EventHandler(this.CHe_FEE_FROM_REMIT_AMOUNT_CheckedChanged);
            this.label12.BackColor = Color.DarkRed; this.label12.Dock = DockStyle.Top; this.label12.Font = new Font("Arial", 10.5f, FontStyle.Bold); this.label12.ForeColor = Color.White; this.label12.Location = new Point(0, 0); this.label12.Name = "label12"; this.label12.Size = new Size(522, 21); this.label12.TabIndex = 9; this.label12.Text = "بيانات المستلم "; this.label12.TextAlign = ContentAlignment.MiddleCenter;
            this.BEN_MOBILE.BorderStyle = BorderStyle.FixedSingle; this.BEN_MOBILE.Location = new Point(337, 24); this.BEN_MOBILE.Name = "BEN_MOBILE"; this.BEN_MOBILE.RightToLeft = RightToLeft.No; this.BEN_MOBILE.Size = new Size(95, 26); this.BEN_MOBILE.TabIndex = 8; this.BEN_MOBILE.TextAlign = HorizontalAlignment.Center;
            this.label15.AutoSize = true; this.label15.Location = new Point(430, 26); this.label15.Name = "label15"; this.label15.Size = new Size(91, 19); this.label15.TabIndex = 1; this.label15.Text = "موبايل المستلم: ";
            this.BEN_NAME.BorderStyle = BorderStyle.FixedSingle; this.BEN_NAME.Location = new Point(2, 24); this.BEN_NAME.Name = "BEN_NAME"; this.BEN_NAME.Size = new Size(220, 26); this.BEN_NAME.TabIndex = 7;
            this.label11.AutoSize = true; this.label11.Location = new Point(222, 27); this.label11.Name = "label11"; this.label11.Size = new Size(84, 19); this.label11.TabIndex = 14; this.label11.Text = " اسم المستلم: ";
            this.PURPOSE_DESCRIPTION.BorderStyle = BorderStyle.FixedSingle; this.PURPOSE_DESCRIPTION.Location = new Point(9, 92); this.PURPOSE_DESCRIPTION.Name = "PURPOSE_DESCRIPTION"; this.PURPOSE_DESCRIPTION.Size = new Size(272, 26); this.PURPOSE_DESCRIPTION.TabIndex = 99;
            this.label13.AutoSize = true; this.label13.Location = new Point(404, 96); this.label13.Name = "label13"; this.label13.Size = new Size(90, 19); this.label13.TabIndex = 14; this.label13.Text = "غرض التحويل: ";
            this.label14.AutoSize = true; this.label14.Location = new Point(405, 130); this.label14.Name = "label14"; this.label14.Size = new Size(105, 19); this.label14.TabIndex = 12; this.label14.Text = "ملاحظات الإصدار: ";
            this.label17.BackColor = Color.DarkRed; this.label17.Dock = DockStyle.Top; this.label17.Font = new Font("Arial", 10.5f, FontStyle.Bold); this.label17.ForeColor = Color.White; this.label17.Location = new Point(0, 0); this.label17.Name = "label17"; this.label17.Size = new Size(512, 22); this.label17.TabIndex = 9; this.label17.Text = "بيانات الحوالة "; this.label17.TextAlign = ContentAlignment.MiddleCenter;
            this.WAKIL_FEE.BackColor = SystemColors.InactiveCaption; this.WAKIL_FEE.BorderStyle = BorderStyle.FixedSingle; this.WAKIL_FEE.Enabled = false; this.WAKIL_FEE.Location = new Point(317, 29); this.WAKIL_FEE.Name = "WAKIL_FEE"; this.WAKIL_FEE.ReadOnly = true; this.WAKIL_FEE.RightToLeft = RightToLeft.No; this.WAKIL_FEE.Size = new Size(87, 26); this.WAKIL_FEE.TabIndex = 2; this.WAKIL_FEE.TextAlign = HorizontalAlignment.Center;
            this.label18.AutoSize = true; this.label18.Location = new Point(404, 32); this.label18.Name = "label18"; this.label18.Size = new Size(81, 19); this.label18.TabIndex = 1; this.label18.Text = "عمولة الوكيل: ";
            this.label23.AutoSize = true; this.label23.Location = new Point(262, 33); this.label23.Name = "label23"; this.label23.Size = new Size(47, 19); this.label23.TabIndex = 18; this.label23.Text = "العملة: ";
            this.label24.AutoSize = true; this.label24.Location = new Point(90, 33); this.label24.Name = "label24"; this.label24.Size = new Size(85, 19); this.label24.TabIndex = 20; this.label24.Text = "خدمات التحويل: ";
            this.AgentCommission.BorderStyle = BorderStyle.FixedSingle; this.AgentCommission.Location = new Point(9, 29); this.AgentCommission.Name = "AgentCommission"; this.AgentCommission.RightToLeft = RightToLeft.No; this.AgentCommission.Size = new Size(83, 26); this.AgentCommission.TabIndex = 5; this.AgentCommission.TextAlign = HorizontalAlignment.Center;
            this.label25.AutoSize = true; this.label25.Location = new Point(406, 62); this.label25.Name = "label25"; this.label25.Size = new Size(86, 19); this.label25.TabIndex = 22; this.label25.Text = "إجمالي المبلغ: ";
            this.Total_AMOUNT.BackColor = SystemColors.InactiveCaption; this.Total_AMOUNT.BorderStyle = BorderStyle.FixedSingle; this.Total_AMOUNT.Enabled = false; this.Total_AMOUNT.Location = new Point(317, 60); this.Total_AMOUNT.Name = "Total_AMOUNT"; this.Total_AMOUNT.ReadOnly = true; this.Total_AMOUNT.RightToLeft = RightToLeft.No; this.Total_AMOUNT.Size = new Size(87, 26); this.Total_AMOUNT.TabIndex = 21; this.Total_AMOUNT.TextAlign = HorizontalAlignment.Center;
            this.PURPOSE_ID.DropDownStyle = ComboBoxStyle.DropDownList; this.PURPOSE_ID.FormattingEnabled = true; this.PURPOSE_ID.Location = new Point(287, 92); this.PURPOSE_ID.Name = "PURPOSE_ID"; this.PURPOSE_ID.Size = new Size(117, 27); this.PURPOSE_ID.TabIndex = 12;
            this.Note_PURPOSE.BorderStyle = BorderStyle.FixedSingle; this.Note_PURPOSE.Location = new Point(9, 126); this.Note_PURPOSE.Name = "Note_PURPOSE"; this.Note_PURPOSE.Size = new Size(395, 26); this.Note_PURPOSE.TabIndex = 13;
            this.label31.BackColor = Color.DarkRed; this.label31.Dock = DockStyle.Top; this.label31.Font = new Font("Arial", 10.5f, FontStyle.Bold); this.label31.ForeColor = Color.White; this.label31.Location = new Point(0, 0); this.label31.Name = "label31"; this.label31.Size = new Size(522, 21); this.label31.TabIndex = 9; this.label31.Text = "بيانات المرسل "; this.label31.TextAlign = ContentAlignment.MiddleCenter;
            this.SENDER_NAME.BorderStyle = BorderStyle.FixedSingle; this.SENDER_NAME.Location = new Point(1, 26); this.SENDER_NAME.Name = "SENDER_NAME"; this.SENDER_NAME.Size = new Size(215, 26); this.SENDER_NAME.TabIndex = 10;
            this.label29.AutoSize = true; this.label29.Location = new Point(215, 30); this.label29.Name = "label29"; this.label29.Size = new Size(88, 19); this.label29.TabIndex = 27; this.label29.Text = " اسم المرسل: ";
            this.SENDER_MOBILE.BorderStyle = BorderStyle.FixedSingle; this.SENDER_MOBILE.Location = new Point(323, 26); this.SENDER_MOBILE.Name = "SENDER_MOBILE"; this.SENDER_MOBILE.RightToLeft = RightToLeft.No; this.SENDER_MOBILE.Size = new Size(103, 26); this.SENDER_MOBILE.TabIndex = 9; this.SENDER_MOBILE.TextAlign = HorizontalAlignment.Center;
            this.label32.AutoSize = true; this.label32.Location = new Point(423, 28); this.label32.Name = "label32"; this.label32.Size = new Size(91, 19); this.label32.TabIndex = 23; this.label32.Text = "موبايل المرسل: ";
            this.GlobalCode.Location = new Point(306, 50); this.GlobalCode.Name = "GlobalCode"; this.GlobalCode.TabIndex = 6; this.GlobalCode.SelectedIndex = 0;
            this.label34.AutoSize = true; this.label34.Location = new Point(427, 53); this.label34.Name = "label34"; this.label34.Size = new Size(75, 19); this.label34.TabIndex = 35; this.label34.Text = "طريقة القبض: ";
            this.M_Proccess.AutoSize = false; this.M_Proccess.Items.AddRange(new ToolStripItem[] { this.M_New, this.M_Save, this.M_Edit, this.M_Menu_Tab, this.M_Cancel, this.M_Serch, this.M_Print, this.M_Exit }); this.M_Proccess.Location = new Point(0, 0); this.M_Proccess.Name = "M_Proccess"; this.M_Proccess.Size = new Size(1222, 34); this.M_Proccess.TabIndex = 385; this.M_Proccess.Text = "menuStrip1"; this.M_Proccess.Visible = false;
            this.M_New.Name = "M_New"; this.M_New.ShortcutKeys = Keys.Control | Keys.N; this.M_New.Size = new Size(51, 28); this.M_New.Text = "جديد "; this.M_New.Click += new EventHandler(this.button_New_Click);
            this.M_Save.Name = "M_Save"; this.M_Save.ShortcutKeys = Keys.F10; this.M_Save.Size = new Size(51, 28); this.M_Save.Text = "حفظ "; this.M_Save.Click += new EventHandler(this.button_Save_Click);
            this.M_Edit.Name = "M_Edit"; this.M_Edit.ShortcutKeys = Keys.Control | Keys.E; this.M_Edit.Size = new Size(58, 28); this.M_Edit.Text = "تعديل "; this.M_Edit.Click += new EventHandler(this.button_Update_Click);
            this.M_Menu_Tab.Checked = true; this.M_Menu_Tab.CheckState = CheckState.Checked; this.M_Menu_Tab.Name = "M_Menu_Tab"; this.M_Menu_Tab.ShortcutKeys = Keys.Delete; this.M_Menu_Tab.Size = new Size(53, 28); this.M_Menu_Tab.Text = "حذف ";
            this.M_Cancel.Name = "M_Cancel"; this.M_Cancel.ShortcutKeys = Keys.Control | Keys.Z; this.M_Cancel.Size = new Size(49, 28); this.M_Cancel.Text = "إلغاء ";
            this.M_Serch.Name = "M_Serch"; this.M_Serch.ShortcutKeys = Keys.F9; this.M_Serch.Size = new Size(49, 28); this.M_Serch.Text = "بحث ";
            this.M_Print.Name = "M_Print"; this.M_Print.ShortcutKeys = Keys.Control | Keys.P; this.M_Print.Size = new Size(60, 28); this.M_Print.Text = "طباعة "; this.M_Print.Click += new EventHandler(this.button_Print_Click);
            this.M_Exit.Name = "M_Exit"; this.M_Exit.ShortcutKeys = Keys.Control | Keys.Q; this.M_Exit.Size = new Size(53, 28); this.M_Exit.Text = "خروج "; this.M_Exit.Click += new EventHandler(this.button_Exit_Click);
            this.dGV.AllowUserToOrderColumns = true; this.dGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; this.dGV.BackgroundColor = SystemColors.Control;
            DataGridViewCellStyle chs = new DataGridViewCellStyle(); chs.Alignment = DataGridViewContentAlignment.MiddleCenter; chs.BackColor = SystemColors.Control; chs.Font = new Font("Arial", 9.5f, FontStyle.Bold); chs.WrapMode = DataGridViewTriState.True; this.dGV.ColumnHeadersDefaultCellStyle = chs;
            this.dGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV.Columns.AddRange(new DataGridViewColumn[] { this.Dgv_ExpressNum, this.Dgv_OutgoingDate, this.Dgv_TransferAmount, this.Dgv_CurrencyName, this.Dgv_SenderName, this.Dgv_SenderPhone, this.Dgv_ReceiverName, this.Dgv_ReceiverPhone, this.Dgv_OutgoingNumber, this.Dgv_IncomingPay, this.Dgv_IncomingAccount, this.Dgv_UserName, this.Dgv_OutgoingAccount, this.Dgv_Target, this.Dgv_Notes, this.Dgv_TransferPurpose, this.Dgv_IncomingCommission, this.Dgv_OutgoingCommission });
            this.dGV.ContextMenuStrip = this.contextMenuStrip1;
            DataGridViewCellStyle dcs = new DataGridViewCellStyle(); dcs.Alignment = DataGridViewContentAlignment.MiddleCenter; dcs.Font = new Font("Arial", 9.5f, FontStyle.Bold); dcs.WrapMode = DataGridViewTriState.False; this.dGV.DefaultCellStyle = dcs;
            this.dGV.Dock = DockStyle.Fill; this.dGV.Location = new Point(0, 0); this.dGV.Name = "dGV"; this.dGV.RightToLeft = RightToLeft.No;
            DataGridViewCellStyle rhs = new DataGridViewCellStyle(); rhs.Alignment = DataGridViewContentAlignment.MiddleCenter; rhs.BackColor = SystemColors.Control; rhs.Font = new Font("Arial", 9.5f, FontStyle.Bold); rhs.WrapMode = DataGridViewTriState.True; this.dGV.RowHeadersDefaultCellStyle = rhs;
            this.dGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            DataGridViewCellStyle rws = new DataGridViewCellStyle(); rws.Alignment = DataGridViewContentAlignment.MiddleCenter; rws.Font = new Font("Arial", 10.2f, FontStyle.Bold); this.dGV.RowsDefaultCellStyle = rws;
            this.dGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect; this.dGV.Size = new Size(1218, 257); this.dGV.TabIndex = 389;
            this.dGV.DoubleClick += new EventHandler(this.dGV_DoubleClick); this.dGV.CellClick += new DataGridViewCellEventHandler(this.dGV_CellClick);
            this.Dgv_ExpressNum.HeaderText = "رقم الحوالة "; this.Dgv_ExpressNum.Name = "Dgv_ExpressNum"; this.Dgv_ExpressNum.ReadOnly = true; this.Dgv_ExpressNum.Width = 98;
            this.Dgv_OutgoingDate.HeaderText = "تاريخ الارسال "; this.Dgv_OutgoingDate.Name = "Dgv_OutgoingDate"; this.Dgv_OutgoingDate.ReadOnly = true; this.Dgv_OutgoingDate.Width = 110;
            this.Dgv_TransferAmount.HeaderText = "مبلغ الحوالة "; this.Dgv_TransferAmount.Name = "Dgv_TransferAmount"; this.Dgv_TransferAmount.ReadOnly = true; this.Dgv_TransferAmount.Width = 103;
            this.Dgv_CurrencyName.HeaderText = "العملة "; this.Dgv_CurrencyName.Name = "Dgv_CurrencyName"; this.Dgv_CurrencyName.ReadOnly = true; this.Dgv_CurrencyName.Width = 70;
            this.Dgv_SenderName.HeaderText = "اسم المرسل "; this.Dgv_SenderName.Name = "Dgv_SenderName"; this.Dgv_SenderName.ReadOnly = true; this.Dgv_SenderName.Width = 101;
            this.Dgv_SenderPhone.HeaderText = "موبايل المرسل "; this.Dgv_SenderPhone.Name = "Dgv_SenderPhone"; this.Dgv_SenderPhone.ReadOnly = true; this.Dgv_SenderPhone.Width = 114;
            this.Dgv_ReceiverName.HeaderText = "اسم المستفيد "; this.Dgv_ReceiverName.Name = "Dgv_ReceiverName"; this.Dgv_ReceiverName.ReadOnly = true; this.Dgv_ReceiverName.Width = 105;
            this.Dgv_ReceiverPhone.HeaderText = "موبايل المستفيد "; this.Dgv_ReceiverPhone.Name = "Dgv_ReceiverPhone"; this.Dgv_ReceiverPhone.ReadOnly = true; this.Dgv_ReceiverPhone.Width = 118;
            this.Dgv_OutgoingNumber.HeaderText = "رقم الصادر "; this.Dgv_OutgoingNumber.Name = "Dgv_OutgoingNumber"; this.Dgv_OutgoingNumber.ReadOnly = true; this.Dgv_OutgoingNumber.Width = 96;
            this.Dgv_IncomingPay.HeaderText = "طريقة القبض "; this.Dgv_IncomingPay.Name = "Dgv_IncomingPay"; this.Dgv_IncomingPay.ReadOnly = true; this.Dgv_IncomingPay.Width = 110;
            this.Dgv_IncomingAccount.HeaderText = "حساب القبض "; this.Dgv_IncomingAccount.Name = "Dgv_IncomingAccount"; this.Dgv_IncomingAccount.ReadOnly = true; this.Dgv_IncomingAccount.Width = 120;
            this.Dgv_UserName.HeaderText = "اسم المستخدم "; this.Dgv_UserName.Name = "Dgv_UserName"; this.Dgv_UserName.ReadOnly = true; this.Dgv_UserName.Width = 120;
            this.Dgv_OutgoingAccount.HeaderText = "الوكيل "; this.Dgv_OutgoingAccount.Name = "Dgv_OutgoingAccount"; this.Dgv_OutgoingAccount.ReadOnly = true; this.Dgv_OutgoingAccount.Width = 120;
            this.Dgv_Target.HeaderText = "الجهة "; this.Dgv_Target.Name = "Dgv_Target"; this.Dgv_Target.ReadOnly = true; this.Dgv_Target.Width = 100;
            this.Dgv_Notes.HeaderText = "ملاحظات الاصدار "; this.Dgv_Notes.Name = "Dgv_Notes"; this.Dgv_Notes.ReadOnly = true; this.Dgv_Notes.Width = 150;
            this.Dgv_TransferPurpose.HeaderText = "غرض التحويل "; this.Dgv_TransferPurpose.Name = "Dgv_TransferPurpose"; this.Dgv_TransferPurpose.ReadOnly = true; this.Dgv_TransferPurpose.Width = 110;
            this.Dgv_IncomingCommission.HeaderText = "خدمات التحويل "; this.Dgv_IncomingCommission.Name = "Dgv_IncomingCommission"; this.Dgv_IncomingCommission.ReadOnly = true; this.Dgv_IncomingCommission.Width = 110;
            this.Dgv_OutgoingCommission.HeaderText = "عمولة الوكيل "; this.Dgv_OutgoingCommission.Name = "Dgv_OutgoingCommission"; this.Dgv_OutgoingCommission.ReadOnly = true; this.Dgv_OutgoingCommission.Width = 110;
            this.contextMenuStrip1.ImageScalingSize = new Size(20, 20); this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItem1, this.MenuItemArch }); this.contextMenuStrip1.Name = "contextMenuStrip1"; this.contextMenuStrip1.Size = new Size(161, 52); this.contextMenuStrip1.Text = "قائمة المزامنة ";
            this.toolStripMenuItem1.Name = "toolStripMenuItem1"; this.toolStripMenuItem1.Size = new Size(160, 24); this.toolStripMenuItem1.Text = "مزامنة "; this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
            this.MenuItemArch.Name = "MenuItemArch"; this.MenuItemArch.Size = new Size(160, 24); this.MenuItemArch.Text = "أرشيف مستند ";
            this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.panel1.BorderStyle = BorderStyle.FixedSingle; this.panel1.Controls.Add(this.CenterCommission); this.panel1.Controls.Add(this.CommissionCurrency); this.panel1.Controls.Add(this.FEE_CUR_CODE); this.panel1.Controls.Add(this.label_Total_AMOUNT); this.panel1.Controls.Add(this.Note_PURPOSE); this.panel1.Controls.Add(this.label17); this.panel1.Controls.Add(this.PURPOSE_ID); this.panel1.Controls.Add(this.label18); this.panel1.Controls.Add(this.WAKIL_FEE); this.panel1.Controls.Add(this.label25); this.panel1.Controls.Add(this.label14); this.panel1.Controls.Add(this.Total_AMOUNT); this.panel1.Controls.Add(this.label13); this.panel1.Controls.Add(this.PURPOSE_DESCRIPTION); this.panel1.Controls.Add(this.AgentCommission); this.panel1.Controls.Add(this.label23); this.panel1.Controls.Add(this.label24); this.panel1.Location = new Point(105, 0); this.panel1.Name = "panel1"; this.panel1.Size = new Size(514, 161); this.panel1.TabIndex = 4;
            this.CenterCommission.Location = new Point(15, 30); this.CenterCommission.Name = "CenterCommission"; this.CenterCommission.Size = new Size(11, 24); this.CenterCommission.TabIndex = 392; this.CenterCommission.Text = "CenterCommission"; this.CenterCommission.Visible = false;
            this.CommissionCurrency.Location = new Point(4, 31); this.CommissionCurrency.Name = "CommissionCurrency"; this.CommissionCurrency.Size = new Size(20, 24); this.CommissionCurrency.TabIndex = 391; this.CommissionCurrency.Text = "CommissionCurrency"; this.CommissionCurrency.Visible = false;
            this.FEE_CUR_CODE.BackColor = SystemColors.ActiveCaption; this.FEE_CUR_CODE.BorderStyle = BorderStyle.FixedSingle; this.FEE_CUR_CODE.Location = new Point(180, 29); this.FEE_CUR_CODE.Name = "FEE_CUR_CODE"; this.FEE_CUR_CODE.Size = new Size(83, 26); this.FEE_CUR_CODE.TabIndex = 27; this.FEE_CUR_CODE.Text = "ريال يمني"; this.FEE_CUR_CODE.TextAlign = ContentAlignment.MiddleCenter;
            this.label_Total_AMOUNT.BorderStyle = BorderStyle.Fixed3D; this.label_Total_AMOUNT.Location = new Point(9, 62); this.label_Total_AMOUNT.Name = "label_Total_AMOUNT"; this.label_Total_AMOUNT.Size = new Size(294, 21); this.label_Total_AMOUNT.TabIndex = 26;
            this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.panel2.BorderStyle = BorderStyle.FixedSingle; this.panel2.Controls.Add(this.button_Show_Data_Card_Send); this.panel2.Controls.Add(this.BTN_Serch_MOB_Sen1); this.panel2.Controls.Add(this.button_CopyToBeneficiary); this.panel2.Controls.Add(this.label31); this.panel2.Controls.Add(this.SENDER_MOBILE); this.panel2.Controls.Add(this.SENDER_NAME); this.panel2.Controls.Add(this.label32); this.panel2.Controls.Add(this.label29); this.panel2.Location = new Point(104, 163); this.panel2.Name = "panel2"; this.panel2.Size = new Size(515, 91); this.panel2.TabIndex = 391;
            this.button_Show_Data_Card_Send.BackColor = Color.WhiteSmoke; this.button_Show_Data_Card_Send.Font = new Font("Arial", 9.5f, FontStyle.Bold); this.button_Show_Data_Card_Send.ImageAlign = ContentAlignment.MiddleLeft; this.button_Show_Data_Card_Send.TextAlign = ContentAlignment.MiddleRight; this.button_Show_Data_Card_Send.Location = new Point(3, 56); this.button_Show_Data_Card_Send.Name = "button_Show_Data_Card_Send"; this.button_Show_Data_Card_Send.Size = new Size(145, 31); this.button_Show_Data_Card_Send.TabIndex = 407; this.button_Show_Data_Card_Send.Text = "بيانات بطاقة المرسل "; this.button_Show_Data_Card_Send.UseVisualStyleBackColor = false; this.button_Show_Data_Card_Send.Click += new EventHandler(this.button_Show_Data_Card_Send_Click);
            this.BTN_Serch_MOB_Sen1.Location = new Point(293, 23); this.BTN_Serch_MOB_Sen1.Name = "BTN_Serch_MOB_Sen1"; this.BTN_Serch_MOB_Sen1.Size = new Size(30, 31); this.BTN_Serch_MOB_Sen1.TabIndex = 406; this.BTN_Serch_MOB_Sen1.TabStop = false; this.BTN_Serch_MOB_Sen1.Click += new EventHandler(this.BTN_Serch_MOB_Sen1_Click);
            this.button_CopyToBeneficiary.BackColor = Color.WhiteSmoke; this.button_CopyToBeneficiary.Font = new Font("Arial", 9.5f, FontStyle.Bold); this.button_CopyToBeneficiary.Location = new Point(170, 56); this.button_CopyToBeneficiary.Name = "button_CopyToBeneficiary"; this.button_CopyToBeneficiary.Size = new Size(256, 31); this.button_CopyToBeneficiary.TabIndex = 410; this.button_CopyToBeneficiary.Text = "تحويل البيانات الى المستلم "; this.button_CopyToBeneficiary.UseVisualStyleBackColor = false; this.button_CopyToBeneficiary.Click += new EventHandler(this.button_CopyToBeneficiary_Click);
            this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.panel3.BorderStyle = BorderStyle.FixedSingle; this.panel3.Controls.Add(this.button_Show_Data_Card_BEN); this.panel3.Controls.Add(this.button_CopyToSender); this.panel3.Controls.Add(this.BTN_Serch_MOB_Ben1); this.panel3.Controls.Add(this.label19); this.panel3.Controls.Add(this.label12); this.panel3.Controls.Add(this.BEN_MOBILE); this.panel3.Controls.Add(this.BEN_NAME); this.panel3.Controls.Add(this.label11); this.panel3.Controls.Add(this.label15); this.panel3.Location = new Point(623, 163); this.panel3.Name = "panel3"; this.panel3.Size = new Size(522, 90); this.panel3.TabIndex = 391;
            this.button_Show_Data_Card_BEN.BackColor = Color.WhiteSmoke; this.button_Show_Data_Card_BEN.Font = new Font("Arial", 9.5f, FontStyle.Bold); this.button_Show_Data_Card_BEN.ImageAlign = ContentAlignment.MiddleLeft; this.button_Show_Data_Card_BEN.TextAlign = ContentAlignment.MiddleRight; this.button_Show_Data_Card_BEN.Location = new Point(6, 54); this.button_Show_Data_Card_BEN.Name = "button_Show_Data_Card_BEN"; this.button_Show_Data_Card_BEN.Size = new Size(145, 31); this.button_Show_Data_Card_BEN.TabIndex = 406; this.button_Show_Data_Card_BEN.Text = "بيانات بطاقة المستلم "; this.button_Show_Data_Card_BEN.UseVisualStyleBackColor = false; this.button_Show_Data_Card_BEN.Click += new EventHandler(this.button_Show_Data_Card_BEN_Click);
            this.button_CopyToSender.BackColor = Color.WhiteSmoke; this.button_CopyToSender.Font = new Font("Arial", 9.5f, FontStyle.Bold); this.button_CopyToSender.Location = new Point(160, 54); this.button_CopyToSender.Name = "button_CopyToSender"; this.button_CopyToSender.Size = new Size(256, 31); this.button_CopyToSender.TabIndex = 408; this.button_CopyToSender.Text = "تحويل البيانات الى المرسل "; this.button_CopyToSender.UseVisualStyleBackColor = false; this.button_CopyToSender.Click += new EventHandler(this.button_CopyToSender_Click);
            this.BTN_Serch_MOB_Ben1.Location = new Point(307, 21); this.BTN_Serch_MOB_Ben1.Name = "BTN_Serch_MOB_Ben1"; this.BTN_Serch_MOB_Ben1.Size = new Size(30, 31); this.BTN_Serch_MOB_Ben1.TabIndex = 405; this.BTN_Serch_MOB_Ben1.TabStop = false; this.BTN_Serch_MOB_Ben1.Click += new EventHandler(this.BTN_Serch_MOB_Ben1_Click);
            this.label19.AutoSize = true; this.label19.Location = new Point(435, 56); this.label19.Name = "label19"; this.label19.Size = new Size(85, 19); this.label19.TabIndex = 27; this.label19.Text = "عنوان المستلم ";
            this.panel4.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.panel4.BorderStyle = BorderStyle.FixedSingle; this.panel4.Controls.Add(this.panel11); this.panel4.Controls.Add(this.panel10); this.panel4.Controls.Add(this.panel9); this.panel4.Controls.Add(this.label4); this.panel4.Location = new Point(624, 0); this.panel4.Name = "panel4"; this.panel4.Size = new Size(522, 161); this.panel4.TabIndex = 391;
            this.panel11.Controls.Add(this.label_ORIGINATION_AMOUNT); this.panel11.Controls.Add(this.label6); this.panel11.Controls.Add(this.ORIGINATION_CURRENCY); this.panel11.Controls.Add(this.ExchangerAccountCurrencyName); this.panel11.Controls.Add(this.CHe_FEE_FROM_REMIT_AMOUNT); this.panel11.Controls.Add(this.ExchangerAccountAmount); this.panel11.Controls.Add(this.ORIGINATION_AMOUNT); this.panel11.Controls.Add(this.label39); this.panel11.Controls.Add(this.GlobalCode); this.panel11.Controls.Add(this.labelCustomerAccount); this.panel11.Controls.Add(this.CustomerAccount); this.panel11.Controls.Add(this.label34); this.panel11.Controls.Add(this.label7); this.panel11.Dock = DockStyle.Top; this.panel11.Location = new Point(0, 83); this.panel11.Name = "panel11"; this.panel11.Size = new Size(522, 77); this.panel11.TabIndex = 393;
            this.label_ORIGINATION_AMOUNT.BorderStyle = BorderStyle.Fixed3D; this.label_ORIGINATION_AMOUNT.Font = new Font("Times New Roman", 9f, FontStyle.Bold); this.label_ORIGINATION_AMOUNT.Location = new Point(6, 28); this.label_ORIGINATION_AMOUNT.Name = "label_ORIGINATION_AMOUNT"; this.label_ORIGINATION_AMOUNT.Size = new Size(479, 21); this.label_ORIGINATION_AMOUNT.TabIndex = 391;
            this.ExchangerAccountCurrencyName.Location = new Point(28, 54); this.ExchangerAccountCurrencyName.Name = "ExchangerAccountCurrencyName"; this.ExchangerAccountCurrencyName.Size = new Size(71, 21); this.ExchangerAccountCurrencyName.TabIndex = 390; this.ExchangerAccountCurrencyName.Text = "ExchangerAccountCurrencyName"; this.ExchangerAccountCurrencyName.Visible = false;
            this.ExchangerAccountAmount.Location = new Point(111, 54); this.ExchangerAccountAmount.Name = "ExchangerAccountAmount"; this.ExchangerAccountAmount.Size = new Size(71, 24); this.ExchangerAccountAmount.TabIndex = 389; this.ExchangerAccountAmount.Text = "ExchangerAccountAmount"; this.ExchangerAccountAmount.Visible = false;
            this.label39.AutoSize = true; this.label39.ForeColor = Color.Navy; this.label39.Location = new Point(203, 6); this.label39.Name = "label39"; this.label39.Size = new Size(74, 19); this.label39.TabIndex = 388; this.label39.Text = "حالة الحوالة "; this.label39.Visible = false;
            this.panel10.Controls.Add(this.button_GetAgent); this.panel10.Controls.Add(this.label41); this.panel10.Controls.Add(this.DestinationEntity); this.panel10.Dock = DockStyle.Top; this.panel10.Location = new Point(0, 53); this.panel10.Name = "panel10"; this.panel10.Size = new Size(522, 30); this.panel10.TabIndex = 392; this.panel10.Visible = true;
            this.button_GetAgent.ImageAlign = ContentAlignment.MiddleLeft; this.button_GetAgent.TextAlign = ContentAlignment.MiddleRight; this.button_GetAgent.Location = new Point(7, 2); this.button_GetAgent.Name = "button_GetAgent"; this.button_GetAgent.Size = new Size(53, 27); this.button_GetAgent.TabIndex = 413; this.button_GetAgent.Text = "بحث "; this.button_GetAgent.UseVisualStyleBackColor = true; this.button_GetAgent.Click += new EventHandler(this.button_GetAgent_Click);
            this.label41.AutoSize = true; this.label41.Location = new Point(428, 5); this.label41.Name = "label41"; this.label41.Size = new Size(47, 19); this.label41.TabIndex = 392; this.label41.Text = "الجهة: ";
            this.DestinationEntity.DropDownStyle = ComboBoxStyle.DropDownList; this.DestinationEntity.FormattingEnabled = true; this.DestinationEntity.ImeMode = ImeMode.On; this.DestinationEntity.Location = new Point(64, 2); this.DestinationEntity.Name = "DestinationEntity"; this.DestinationEntity.Size = new Size(363, 27); this.DestinationEntity.TabIndex = 391;
            this.panel9.Controls.Add(this.labelAgent); this.panel9.Controls.Add(this.AgentId); this.panel9.Controls.Add(this.COUNTRY); this.panel9.Controls.Add(this.button2); this.panel9.Controls.Add(this.label5); this.panel9.Dock = DockStyle.Top; this.panel9.Location = new Point(0, 22); this.panel9.Name = "panel9"; this.panel9.Size = new Size(522, 31); this.panel9.TabIndex = 391;
            this.panel5.BorderStyle = BorderStyle.FixedSingle; this.panel5.Controls.Add(this.panel8); this.panel5.Controls.Add(this.panel6); this.panel5.Dock = DockStyle.Top; this.panel5.Location = new Point(0, 0); this.panel5.Name = "panel5"; this.panel5.Size = new Size(1218, 62); this.panel5.TabIndex = 393;
            this.panel8.BackColor = SystemColors.Window; this.panel8.Controls.Add(this.panel7); this.panel8.Dock = DockStyle.Fill; this.panel8.Location = new Point(0, 30); this.panel8.Name = "panel8"; this.panel8.Size = new Size(1216, 30); this.panel8.TabIndex = 311;
            this.panel7.BackColor = SystemColors.Window; this.panel7.Controls.Add(this.Status); this.panel7.Controls.Add(this.DeliverStatus); this.panel7.Controls.Add(this.REMIT_DATE); this.panel7.Controls.Add(this.REMIT_CODE); this.panel7.Controls.Add(this.label16); this.panel7.Controls.Add(this.label2); this.panel7.Controls.Add(this.label8); this.panel7.Controls.Add(this.REMIT_SEQ); this.panel7.Dock = DockStyle.Right; this.panel7.ForeColor = SystemColors.GradientActiveCaption; this.panel7.Location = new Point(20, 0); this.panel7.Name = "panel7"; this.panel7.Size = new Size(1196, 30); this.panel7.TabIndex = 310;
            this.Status.AutoSize = true; this.Status.ForeColor = Color.Navy; this.Status.Location = new Point(158, 6); this.Status.Name = "Status"; this.Status.Size = new Size(74, 19); this.Status.TabIndex = 388; this.Status.Text = "حالة الحوالة "; this.Status.Visible = false;
            this.DeliverStatus.AutoSize = true; this.DeliverStatus.ForeColor = Color.Navy; this.DeliverStatus.Location = new Point(244, 6); this.DeliverStatus.Name = "DeliverStatus"; this.DeliverStatus.Size = new Size(74, 19); this.DeliverStatus.TabIndex = 387; this.DeliverStatus.Text = "حالة الحوالة ";
            this.REMIT_DATE.BackColor = Color.White; this.REMIT_DATE.BorderStyle = BorderStyle.None; this.REMIT_DATE.Location = new Point(527, 6); this.REMIT_DATE.Mask = "00/00/0000"; this.REMIT_DATE.Name = "REMIT_DATE"; this.REMIT_DATE.ReadOnly = true; this.REMIT_DATE.RightToLeft = RightToLeft.No; this.REMIT_DATE.Size = new Size(103, 19); this.REMIT_DATE.TabIndex = 386; this.REMIT_DATE.TabStop = false;
            this.REMIT_CODE.BackColor = Color.FromArgb(255, 252, 236); this.REMIT_CODE.Font = new Font("Arial", 12f, FontStyle.Bold); this.REMIT_CODE.ForeColor = Color.MidnightBlue; this.REMIT_CODE.Location = new Point(769, 0); this.REMIT_CODE.Name = "REMIT_CODE"; this.REMIT_CODE.ReadOnly = true; this.REMIT_CODE.RightToLeft = RightToLeft.No; this.REMIT_CODE.Size = new Size(234, 30); this.REMIT_CODE.TabIndex = 400; this.REMIT_CODE.TabStop = false; this.REMIT_CODE.TextAlign = HorizontalAlignment.Center;
            this.label16.AutoSize = true; this.label16.Font = new Font("Arial", 12f, FontStyle.Bold); this.label16.ForeColor = Color.MidnightBlue; this.label16.Location = new Point(1004, 5); this.label16.Name = "label16"; this.label16.Size = new Size(90, 24); this.label16.TabIndex = 1; this.label16.Text = "رقم الحوالة: ";
            this.label2.AutoSize = true; this.label2.ForeColor = Color.MidnightBlue; this.label2.Location = new Point(460, 6); this.label2.Name = "label2"; this.label2.Size = new Size(50, 19); this.label2.TabIndex = 308; this.label2.Text = "تسلسل: ";
            this.label8.AutoSize = true; this.label8.ForeColor = Color.MidnightBlue; this.label8.Location = new Point(630, 7); this.label8.Name = "label8"; this.label8.Size = new Size(85, 19); this.label8.TabIndex = 307; this.label8.Text = "تاريخ الحوالة: ";
            this.REMIT_SEQ.BackColor = Color.White; this.REMIT_SEQ.BorderStyle = BorderStyle.None; this.REMIT_SEQ.ForeColor = Color.MidnightBlue; this.REMIT_SEQ.Location = new Point(331, 4); this.REMIT_SEQ.Name = "REMIT_SEQ"; this.REMIT_SEQ.ReadOnly = true; this.REMIT_SEQ.RightToLeft = RightToLeft.No; this.REMIT_SEQ.Size = new Size(124, 19); this.REMIT_SEQ.TabIndex = 401; this.REMIT_SEQ.TabStop = false; this.REMIT_SEQ.TextAlign = HorizontalAlignment.Center;
            this.panel6.Controls.Add(this.checkBox_Serch_mobile); this.panel6.Controls.Add(this.checkBox_SendTO_Ebda); this.panel6.Controls.Add(this.checkBox1); this.panel6.Controls.Add(this.SoucrceName); this.panel6.Controls.Add(this.label9); this.panel6.Dock = DockStyle.Top; this.panel6.Location = new Point(0, 0); this.panel6.Name = "panel6"; this.panel6.Size = new Size(1216, 30); this.panel6.TabIndex = 309;
            this.checkBox_Serch_mobile.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.checkBox_Serch_mobile.AutoSize = true; this.checkBox_Serch_mobile.BackColor = Color.DarkRed; this.checkBox_Serch_mobile.Checked = true; this.checkBox_Serch_mobile.CheckState = CheckState.Checked; this.checkBox_Serch_mobile.Font = new Font("Arial", 10.8f, FontStyle.Bold); this.checkBox_Serch_mobile.ForeColor = Color.White; this.checkBox_Serch_mobile.Location = new Point(622, 3); this.checkBox_Serch_mobile.Name = "checkBox_Serch_mobile"; this.checkBox_Serch_mobile.Size = new Size(177, 26); this.checkBox_Serch_mobile.TabIndex = 399; this.checkBox_Serch_mobile.Text = "البحث من خلال الموبايل "; this.checkBox_Serch_mobile.UseVisualStyleBackColor = false;
            this.checkBox_SendTO_Ebda.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.checkBox_SendTO_Ebda.AutoSize = true; this.checkBox_SendTO_Ebda.BackColor = Color.DarkRed; this.checkBox_SendTO_Ebda.Checked = true; this.checkBox_SendTO_Ebda.CheckState = CheckState.Checked; this.checkBox_SendTO_Ebda.Font = new Font("Arial", 10.8f, FontStyle.Bold); this.checkBox_SendTO_Ebda.ForeColor = Color.White; this.checkBox_SendTO_Ebda.Location = new Point(804, 4); this.checkBox_SendTO_Ebda.Name = "checkBox_SendTO_Ebda"; this.checkBox_SendTO_Ebda.Size = new Size(184, 26); this.checkBox_SendTO_Ebda.TabIndex = 398; this.checkBox_SendTO_Ebda.Text = "مزامنة الحوالات الصادرة "; this.checkBox_SendTO_Ebda.UseVisualStyleBackColor = false;
            this.checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.checkBox1.AutoSize = true; this.checkBox1.BackColor = Color.DarkRed; this.checkBox1.Font = new Font("Arial", 10.8f, FontStyle.Bold); this.checkBox1.ForeColor = Color.White; this.checkBox1.Location = new Point(994, 4); this.checkBox1.Name = "checkBox1"; this.checkBox1.Size = new Size(218, 26); this.checkBox1.TabIndex = 397; this.checkBox1.Text = "إظهار الإشعار آلياً بعد الإرسال "; this.checkBox1.UseVisualStyleBackColor = false;
            this.SoucrceName.AutoSize = true; this.SoucrceName.Location = new Point(685, 7); this.SoucrceName.Name = "SoucrceName"; this.SoucrceName.Size = new Size(82, 19); this.SoucrceName.TabIndex = 388; this.SoucrceName.Text = "مصدر الحوالة "; this.SoucrceName.Visible = false;
            this.label9.BackColor = Color.DarkRed; this.label9.Dock = DockStyle.Fill; this.label9.Font = new Font("Arial", 12f, FontStyle.Bold); this.label9.ForeColor = Color.White; this.label9.Location = new Point(0, 0); this.label9.Name = "label9"; this.label9.Size = new Size(1216, 30); this.label9.TabIndex = 396; this.label9.Text = "شاشة إصدار الحوالة "; this.label9.TextAlign = ContentAlignment.MiddleCenter;
            this.backgroundWorker1.WorkerReportsProgress = true; this.backgroundWorker1.WorkerSupportsCancellation = true; this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.button_Sync.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Sync.Location = new Point(562, 3); this.button_Sync.Name = "button_Sync"; this.button_Sync.Size = new Size(81, 30); this.button_Sync.TabIndex = 404; this.button_Sync.TabStop = false; this.button_Sync.Text = "مزامنة "; this.button_Sync.UseVisualStyleBackColor = true; this.button_Sync.Click += new EventHandler(this.button_Sync_Click);
            this.check_SelectAll_Sync.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.check_SelectAll_Sync.AutoSize = true; this.check_SelectAll_Sync.Location = new Point(652, 7); this.check_SelectAll_Sync.Name = "check_SelectAll_Sync"; this.check_SelectAll_Sync.Size = new Size(82, 23); this.check_SelectAll_Sync.TabIndex = 405; this.check_SelectAll_Sync.Text = "تحديد الكل "; this.check_SelectAll_Sync.UseVisualStyleBackColor = true; this.check_SelectAll_Sync.CheckedChanged += new EventHandler(this.check_SelectAll_Sync_CheckedChanged);
            this.REMIT_DATE_Find.AllowDrop = true; this.REMIT_DATE_Find.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.REMIT_DATE_Find.CustomFormat = "dd/MM/yyyy"; this.REMIT_DATE_Find.Format = DateTimePickerFormat.Custom; this.REMIT_DATE_Find.Location = new Point(874, 5); this.REMIT_DATE_Find.MinDate = new DateTime(2021, 1, 1, 0, 0, 0, 0); this.REMIT_DATE_Find.Name = "REMIT_DATE_Find"; this.REMIT_DATE_Find.Size = new Size(131, 26); this.REMIT_DATE_Find.TabIndex = 405; this.REMIT_DATE_Find.Value = new DateTime(2021, 2, 28, 0, 0, 0, 0);
            this.label40.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.label40.AutoSize = true; this.label40.Location = new Point(519, 10); this.label40.Name = "label40"; this.label40.Size = new Size(31, 19); this.label40.TabIndex = 412; this.label40.Text = "بحث "; this.label40.Click += new EventHandler(this.label40_Click);
            this.label_RemitCount.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.label_RemitCount.Location = new Point(77, 8); this.label_RemitCount.Name = "label_RemitCount"; this.label_RemitCount.Size = new Size(62, 26); this.label_RemitCount.TabIndex = 410; this.label_RemitCount.Text = "0";
            this.label38.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.label38.AutoSize = true; this.label38.Location = new Point(142, 9); this.label38.Name = "label38"; this.label38.Size = new Size(83, 19); this.label38.TabIndex = 409; this.label38.Text = "عدد الحوالات: ";
            this.label37.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.label37.AutoSize = true; this.label37.Location = new Point(1008, 8); this.label37.Name = "label37"; this.label37.Size = new Size(114, 19); this.label37.TabIndex = 408; this.label37.Text = "عرض حسب التاريخ ";
            this.textBox_Search.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.textBox_Search.Location = new Point(248, 6); this.textBox_Search.Name = "textBox_Search"; this.textBox_Search.RightToLeft = RightToLeft.No; this.textBox_Search.Size = new Size(265, 26); this.textBox_Search.TabIndex = 407; this.textBox_Search.TextAlign = HorizontalAlignment.Center; this.textBox_Search.TextChanged += new EventHandler(this.textBox_Search_TextChanged);
            this.button_Update.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Update.ImageAlign = ContentAlignment.MiddleLeft; this.button_Update.Location = new Point(1129, 2); this.button_Update.Margin = new Padding(4); this.button_Update.Name = "button_Update"; this.button_Update.Size = new Size(73, 31); this.button_Update.TabIndex = 387; this.button_Update.Text = "تحديث "; this.button_Update.TextAlign = ContentAlignment.MiddleRight; this.button_Update.UseVisualStyleBackColor = true; this.button_Update.Click += new EventHandler(this.button_Update_Click);
            this.button_Save.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Save.ImageAlign = ContentAlignment.TopCenter; this.button_Save.Location = new Point(1150, 52); this.button_Save.Margin = new Padding(4); this.button_Save.Name = "button_Save"; this.button_Save.Size = new Size(64, 51); this.button_Save.TabIndex = 14; this.button_Save.Text = "إرسال "; this.button_Save.TextAlign = ContentAlignment.BottomCenter; this.button_Save.UseVisualStyleBackColor = true; this.button_Save.Click += new EventHandler(this.button_Save_Click);
            this.button_Exit.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Exit.ForeColor = Color.Red; this.button_Exit.ImageAlign = ContentAlignment.TopCenter; this.button_Exit.Location = new Point(1150, 208); this.button_Exit.Margin = new Padding(4); this.button_Exit.Name = "button_Exit"; this.button_Exit.Size = new Size(64, 47); this.button_Exit.TabIndex = 377; this.button_Exit.Text = "خروج "; this.button_Exit.TextAlign = ContentAlignment.BottomCenter; this.button_Exit.UseVisualStyleBackColor = true; this.button_Exit.Click += new EventHandler(this.button_Exit_Click);
            this.button_New.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_New.ImageAlign = ContentAlignment.TopCenter; this.button_New.Location = new Point(1150, 2); this.button_New.Margin = new Padding(4); this.button_New.Name = "button_New"; this.button_New.Size = new Size(64, 48); this.button_New.TabIndex = 15; this.button_New.Text = "جديد "; this.button_New.TextAlign = ContentAlignment.BottomCenter; this.button_New.UseVisualStyleBackColor = true; this.button_New.Click += new EventHandler(this.button_New_Click);
            this.button_Print.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Print.Enabled = false; this.button_Print.ImageAlign = ContentAlignment.MiddleLeft; this.button_Print.Location = new Point(1150, 105); this.button_Print.Margin = new Padding(4); this.button_Print.Name = "button_Print"; this.button_Print.Size = new Size(64, 51); this.button_Print.TabIndex = 400; this.button_Print.Text = "طباعة "; this.button_Print.TextAlign = ContentAlignment.MiddleRight; this.button_Print.TextImageRelation = TextImageRelation.ImageBeforeText; this.button_Print.UseVisualStyleBackColor = true; this.button_Print.Click += new EventHandler(this.button_Print_Click);
            this.button_show.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_show.ImageAlign = ContentAlignment.MiddleLeft; this.button_show.Location = new Point(2, 207); this.button_show.Margin = new Padding(4); this.button_show.Name = "button_show"; this.button_show.Size = new Size(98, 38); this.button_show.TabIndex = 296; this.button_show.Text = "بحث متقدم "; this.button_show.TextAlign = ContentAlignment.MiddleRight; this.button_show.UseVisualStyleBackColor = true; this.button_show.Click += new EventHandler(this.button_show_Click);
            this.check_All_User.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.check_All_User.AutoSize = true; this.check_All_User.Location = new Point(744, 7); this.check_All_User.Name = "check_All_User"; this.check_All_User.Size = new Size(124, 23); this.check_All_User.TabIndex = 413; this.check_All_User.Text = "جميع المستخدمين "; this.check_All_User.UseVisualStyleBackColor = true;
            this.panel12.Controls.Add(this.dGV); this.panel12.Dock = DockStyle.Fill; this.panel12.Location = new Point(0, 295); this.panel12.Name = "panel12"; this.panel12.Size = new Size(1218, 257); this.panel12.TabIndex = 414;
            this.panel13.Controls.Add(this.button_Arch); this.panel13.Controls.Add(this.button_Save); this.panel13.Controls.Add(this.button_Exit); this.panel13.Controls.Add(this.button_Print); this.panel13.Controls.Add(this.panel1); this.panel13.Controls.Add(this.panel2); this.panel13.Controls.Add(this.panel4); this.panel13.Controls.Add(this.panel3); this.panel13.Controls.Add(this.button_New); this.panel13.Controls.Add(this.button_show); this.panel13.Controls.Add(this.pictureBoxCompanyLogo); this.panel13.Dock = DockStyle.Top; this.panel13.Location = new Point(0, 0); this.panel13.Name = "panel13"; this.panel13.Size = new Size(1218, 259); this.panel13.TabIndex = 415;
            this.button_Arch.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.button_Arch.BackColor = Color.White; this.button_Arch.ImageAlign = ContentAlignment.MiddleLeft; this.button_Arch.Location = new Point(1150, 158); this.button_Arch.Margin = new Padding(4); this.button_Arch.Name = "button_Arch"; this.button_Arch.Size = new Size(64, 49); this.button_Arch.TabIndex = 429; this.button_Arch.Text = "أرشيف "; this.button_Arch.TextAlign = ContentAlignment.MiddleRight; this.button_Arch.UseVisualStyleBackColor = true; this.button_Arch.Click += new EventHandler(this.button_Arch_Click);
            this.pictureBoxCompanyLogo.BackColor = Color.White; this.pictureBoxCompanyLogo.BorderStyle = BorderStyle.FixedSingle; this.pictureBoxCompanyLogo.Location = new Point(624, 0); this.pictureBoxCompanyLogo.Name = "pictureBoxCompanyLogo"; this.pictureBoxCompanyLogo.Size = new Size(522, 161); this.pictureBoxCompanyLogo.SizeMode = PictureBoxSizeMode.StretchImage; this.pictureBoxCompanyLogo.TabIndex = 500; this.pictureBoxCompanyLogo.TabStop = false;
            try { string lp = Path.Combine(Application.StartupPath, "شعار الشركة.png"); if (File.Exists(lp)) this.pictureBoxCompanyLogo.Image = Image.FromFile(lp); } catch { }
            this.panel14.Controls.Add(this.panel12); this.panel14.Controls.Add(this.panel15); this.panel14.Controls.Add(this.panel13); this.panel14.Dock = DockStyle.Fill; this.panel14.Location = new Point(0, 62); this.panel14.Name = "panel14"; this.panel14.Size = new Size(1218, 552); this.panel14.TabIndex = 416;
            this.panel15.Controls.Add(this.button_Update); this.panel15.Controls.Add(this.REMIT_DATE_Find); this.panel15.Controls.Add(this.button_Sync); this.panel15.Controls.Add(this.check_SelectAll_Sync); this.panel15.Controls.Add(this.label37); this.panel15.Controls.Add(this.label_RemitCount); this.panel15.Controls.Add(this.textBox_Search); this.panel15.Controls.Add(this.check_All_User); this.panel15.Controls.Add(this.label40); this.panel15.Controls.Add(this.label38); this.panel15.Dock = DockStyle.Top; this.panel15.Location = new Point(0, 259); this.panel15.Name = "panel15"; this.panel15.Size = new Size(1218, 36); this.panel15.TabIndex = 416;
            this.bindingSource1.DataMember = null; this.bindingSource1.DataSource = null;
            this.dataSet1.DataSetName = "DataSet1"; this.dataSet1.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
            this.AutoScaleDimensions = new SizeF(120f, 120f); this.AutoScaleMode = AutoScaleMode.Dpi; this.BackColor = SystemColors.Control; this.ClientSize = new Size(1218, 614); this.Controls.Add(this.panel14); this.Controls.Add(this.panel5); this.Controls.Add(this.M_Proccess); this.Font = new Font("Arial", 9.5f, FontStyle.Bold); this.FormBorderStyle = FormBorderStyle.None; this.Name = "Form_Send_Money"; this.RightToLeft = RightToLeft.Yes; this.RightToLeftLayout = true; this.Text = "إصدار حوالة "; this.WindowState = FormWindowState.Maximized; this.Load += new EventHandler(this.Form_Send_Money_Load);
            this.panel1.ResumeLayout(false); this.panel1.PerformLayout(); this.panel2.ResumeLayout(false); this.panel2.PerformLayout(); this.panel3.ResumeLayout(false); this.panel3.PerformLayout(); this.panel4.ResumeLayout(false); this.panel11.ResumeLayout(false); this.panel11.PerformLayout(); this.panel10.ResumeLayout(false); this.panel10.PerformLayout(); this.panel9.ResumeLayout(false); this.panel9.PerformLayout(); this.panel5.ResumeLayout(false); this.panel8.ResumeLayout(false); this.panel7.ResumeLayout(false); this.panel7.PerformLayout(); this.panel6.ResumeLayout(false); this.panel6.PerformLayout(); this.panel12.ResumeLayout(false); this.panel13.ResumeLayout(false); this.panel14.ResumeLayout(false); this.panel15.ResumeLayout(false); this.panel15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.BTN_Serch_MOB_Sen1)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.BTN_Serch_MOB_Ben1)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false); this.PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
    }
}