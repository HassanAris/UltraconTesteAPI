namespace UltraconTesteAPI.Models
{
    public class LoanResponse
    {
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public List<PaymentSchedule> PaymentSchedule { get; set; } = new List<PaymentSchedule>();
    }

    public class PaymentSchedule
    {
        public int Month { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }

}
