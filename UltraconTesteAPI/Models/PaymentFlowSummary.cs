namespace UltraconTesteAPI.Models
{
    public class PaymentFlowSummary
    {
        public int Id { get; set; }
        public int PropostaId { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
