namespace UltraconTesteAPI.Models
{
    public class LoanRequest
    {
        public decimal LoanAmount { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int NumberOfMonths { get; set; }
    }

}
