using System;

namespace AlmonifExchange
{
    public class TransferDto
    {
        public string TheType { get; set; }
        public decimal TransferAmount { get; set; }
        public long TransferCurrencyID { get; set; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string TestState { get; set; }
        public long IncomingNumber { get; set; }
        public long IncomingAccountID { get; set; }
        public decimal IncomingAmount { get; set; }
        public long IncomingCurrencyID { get; set; }
        public decimal IncomingCommission { get; set; }
        public long IncomingCommissionCurrencyID { get; set; }
        public DateTime IncomingDate { get; set; }
        public string IncomingNotes { get; set; }
        public string IncomingPay { get; set; }
        public DateTime IncomingTime { get; set; }
        public int IncomingUserID { get; set; }
        public int IncomingDebitingWay { get; set; }
        public long IncomingBillNumber { get; set; }
        public long OutgoingNumber { get; set; }
        public long OutgoingAccountID { get; set; }
        public decimal OutgoingAmount { get; set; }
        public long OutgoingCurrencyID { get; set; }
        public decimal OutgoingCommission { get; set; }
        public long OutgoingCommissionCurrencyID { get; set; }
        public int OutgoingCommissionWay { get; set; }
        public DateTime OutgoingDate { get; set; }
        public string OutgoingNotes { get; set; }
        public string OutgoingPay { get; set; }
        public DateTime OutgoingTime { get; set; }
        public int OutgoingUserID { get; set; }
        public int OutgoingDebitingWay { get; set; }
        public long OutgoingBillNumber { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ChequeBank { get; set; }
        public string TransferPurpose { get; set; }
        public int UserID { get; set; }
        public string ReceiverPhone { get; set; }
        public string SenderPhone { get; set; }
        public string OriginalSourceName { get; set; }
        public string OutgoingAccountName { get; set; }
    }
}