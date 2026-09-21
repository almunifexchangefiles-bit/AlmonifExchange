using System;
using System.Data.SqlClient;

namespace AlmonifExchange
{
    public static class TransferHelper
    {
        public static long GetAccountIdByName(string accountName, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID FROM tblAccounts WHERE AccountName = @name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", accountName);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt64(result);
                    }
                }
            }
            catch { }
            return 0;
        }

        public static long GetCurrencyIdByName(string currencyName, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID FROM tblCurrencies WHERE CurrencyName = @name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", currencyName);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt64(result);
                    }
                }
            }
            catch { }
            return 0;
        }

        public static long GetNextBillNumber(string connectionString, string payType, int year)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ISNULL(MAX(BillNumber), 0) + 1 FROM tblTransfers WHERE IncomingPay = @pay AND YEAR(IncomingDate) = @year";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pay", payType);
                        cmd.Parameters.AddWithValue("@year", year);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt64(result);
                    }
                }
            }
            catch { }
            return 1;
        }

        public static long GetNextOutgoingNumber(string connectionString, long accountId, int year)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ISNULL(MAX(OutgoingNumber), 0) + 1 FROM tblTransfers WHERE OutgoingAccountID = @acc AND YEAR(OutgoingDate) = @year";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountId);
                        cmd.Parameters.AddWithValue("@year", year);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt64(result);
                    }
                }
            }
            catch { }
            return 1;
        }

        public static long GetNextOutgoingBillNumber(string connectionString, int year)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ISNULL(MAX(OutgoingBillNumber), 0) + 1 FROM tblTransfers WHERE YEAR(OutgoingDate) = @year";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@year", year);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToInt64(result);
                    }
                }
            }
            catch { }
            return 1;
        }

        public static long SaveTransfer(TransferDto dto, string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO tblTransfers 
                            (TheType, TransferAmount, TransferCurrencyID, ReceiverName, SenderName, 
                             Source, Target, TestState, IncomingNumber, IncomingAccountID, 
                             IncomingAmount, IncomingCurrencyID, IncomingCommission, IncomingCommissionCurrencyID, 
                             IncomingDate, IncomingNotes, IncomingPay, IncomingTime, IncomingUserID, 
                             IncomingDebitingWay, IncomingBillNumber, OutgoingNumber, OutgoingAccountID, 
                             OutgoingAmount, OutgoingCurrencyID, OutgoingCommission, OutgoingCommissionCurrencyID, 
                             OutgoingCommissionWay, OutgoingDate, OutgoingNotes, OutgoingPay, OutgoingTime, 
                             OutgoingUserID, OutgoingDebitingWay, OutgoingBillNumber, ChequeNumber, ChequeDate, 
                             ChequeBank, TransferPurpose, UserID, ReceiverPhone, SenderPhone, 
                             OriginalSourceName, OutgoingAccountName)
                            VALUES 
                            (@TheType, @TransferAmount, @TransferCurrencyID, @ReceiverName, @SenderName,
                             @Source, @Target, @TestState, @IncomingNumber, @IncomingAccountID,
                             @IncomingAmount, @IncomingCurrencyID, @IncomingCommission, @IncomingCommissionCurrencyID,
                             @IncomingDate, @IncomingNotes, @IncomingPay, @IncomingTime, @IncomingUserID,
                             @IncomingDebitingWay, @IncomingBillNumber, @OutgoingNumber, @OutgoingAccountID,
                             @OutgoingAmount, @OutgoingCurrencyID, @OutgoingCommission, @OutgoingCommissionCurrencyID,
                             @OutgoingCommissionWay, @OutgoingDate, @OutgoingNotes, @OutgoingPay, @OutgoingTime,
                             @OutgoingUserID, @OutgoingDebitingWay, @OutgoingBillNumber, @ChequeNumber, @ChequeDate,
                             @ChequeBank, @TransferPurpose, @UserID, @ReceiverPhone, @SenderPhone,
                             @OriginalSourceName, @OutgoingAccountName);
                            SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TheType", dto.TheType ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@TransferAmount", dto.TransferAmount);
                            cmd.Parameters.AddWithValue("@TransferCurrencyID", dto.TransferCurrencyID);
                            cmd.Parameters.AddWithValue("@ReceiverName", dto.ReceiverName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@SenderName", dto.SenderName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Source", dto.Source ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Target", dto.Target ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@TestState", dto.TestState ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IncomingNumber", dto.IncomingNumber);
                            cmd.Parameters.AddWithValue("@IncomingAccountID", dto.IncomingAccountID);
                            cmd.Parameters.AddWithValue("@IncomingAmount", dto.IncomingAmount);
                            cmd.Parameters.AddWithValue("@IncomingCurrencyID", dto.IncomingCurrencyID);
                            cmd.Parameters.AddWithValue("@IncomingCommission", dto.IncomingCommission);
                            cmd.Parameters.AddWithValue("@IncomingCommissionCurrencyID", dto.IncomingCommissionCurrencyID);
                            cmd.Parameters.AddWithValue("@IncomingDate", dto.IncomingDate);
                            cmd.Parameters.AddWithValue("@IncomingNotes", dto.IncomingNotes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IncomingPay", dto.IncomingPay ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@IncomingTime", dto.IncomingTime);
                            cmd.Parameters.AddWithValue("@IncomingUserID", dto.IncomingUserID);
                            cmd.Parameters.AddWithValue("@IncomingDebitingWay", dto.IncomingDebitingWay);
                            cmd.Parameters.AddWithValue("@IncomingBillNumber", dto.IncomingBillNumber);
                            cmd.Parameters.AddWithValue("@OutgoingNumber", dto.OutgoingNumber);
                            cmd.Parameters.AddWithValue("@OutgoingAccountID", dto.OutgoingAccountID);
                            cmd.Parameters.AddWithValue("@OutgoingAmount", dto.OutgoingAmount);
                            cmd.Parameters.AddWithValue("@OutgoingCurrencyID", dto.OutgoingCurrencyID);
                            cmd.Parameters.AddWithValue("@OutgoingCommission", dto.OutgoingCommission);
                            cmd.Parameters.AddWithValue("@OutgoingCommissionCurrencyID", dto.OutgoingCommissionCurrencyID);
                            cmd.Parameters.AddWithValue("@OutgoingCommissionWay", dto.OutgoingCommissionWay);
                            cmd.Parameters.AddWithValue("@OutgoingDate", dto.OutgoingDate);
                            cmd.Parameters.AddWithValue("@OutgoingNotes", dto.OutgoingNotes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@OutgoingPay", dto.OutgoingPay ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@OutgoingTime", dto.OutgoingTime);
                            cmd.Parameters.AddWithValue("@OutgoingUserID", dto.OutgoingUserID);
                            cmd.Parameters.AddWithValue("@OutgoingDebitingWay", dto.OutgoingDebitingWay);
                            cmd.Parameters.AddWithValue("@OutgoingBillNumber", dto.OutgoingBillNumber);
                            cmd.Parameters.AddWithValue("@ChequeNumber", (object)dto.ChequeNumber ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ChequeDate", (object)dto.ChequeDate ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ChequeBank", (object)dto.ChequeBank ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@TransferPurpose", dto.TransferPurpose ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@UserID", dto.UserID);
                            cmd.Parameters.AddWithValue("@ReceiverPhone", dto.ReceiverPhone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@SenderPhone", dto.SenderPhone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@OriginalSourceName", dto.OriginalSourceName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@OutgoingAccountName", dto.OutgoingAccountName ?? (object)DBNull.Value);

                            object result = cmd.ExecuteScalar();
                            transaction.Commit();
                            return Convert.ToInt64(result);
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}