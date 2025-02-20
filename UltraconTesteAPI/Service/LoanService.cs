using UltraconTesteAPI.Data;
using UltraconTesteAPI.Models;

namespace UltraconTesteAPI.Service
{
    public class LoanService
    {
        private readonly ApplicationDbContext _dbContext;

        public LoanService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public LoanResponse SimulateLoan(LoanRequest request)
        {
            // Calcula a taxa de juros mensal
            decimal monthlyInterestRate = request.AnnualInterestRate / 12m;

            // Calcula o valor da parcela fixa (PMT)
            decimal monthlyPayment = CalculateMonthlyPayment(request.LoanAmount, monthlyInterestRate, request.NumberOfMonths);

            // Calcula totais do empréstimo
            decimal totalPayment = Math.Round(monthlyPayment * request.NumberOfMonths, 2);
            decimal totalInterest = Math.Round(totalPayment - request.LoanAmount, 2);

            // Monta a resposta final
            var response = new LoanResponse
            {
                MonthlyPayment = monthlyPayment,
                TotalInterest = totalInterest,
                TotalPayment = totalPayment,
                PaymentSchedule = GeneratePaymentSchedule(request.LoanAmount, monthlyInterestRate, monthlyPayment, request.NumberOfMonths)
            };
            // Salva no banco de dados
            SaveLoanData(request, response);


            return response;
        }

        /// Calcula o valor da parcela mensal usando a fórmula da Tabela Price.
        private decimal CalculateMonthlyPayment(decimal loanAmount, decimal monthlyInterestRate, int numberOfMonths)
        {
            decimal factor = (decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfMonths);
            decimal monthlyPayment = loanAmount * (monthlyInterestRate * factor) / (factor - 1);
            return Math.Round(monthlyPayment, 2);
        }

        /// Gera o cronograma de pagamentos detalhado.
        private List<PaymentSchedule> GeneratePaymentSchedule(decimal loanAmount, decimal monthlyInterestRate, decimal monthlyPayment, int numberOfMonths)
        {
            var paymentSchedule = new List<PaymentSchedule>();
            decimal balance = loanAmount;

            for (int i = 1; i <= numberOfMonths; i++)
            {
                decimal interest = Math.Round(balance * monthlyInterestRate, 2);
                decimal principal = Math.Round(monthlyPayment - interest, 2);

                // Última parcela: corrigindo arredondamento para garantir saldo zero
                if (i == numberOfMonths)
                {
                    principal = balance;
                    balance = 0;
                }
                else
                {
                    balance -= principal;
                }

                balance = Math.Round(balance, 2);

                paymentSchedule.Add(new PaymentSchedule
                {
                    Month = i,
                    Principal = principal,
                    Interest = interest,
                    Balance = balance
                });
            }

            return paymentSchedule;
        }

        private void SaveLoanData(LoanRequest request, LoanResponse response)
        {
            var proposta = new Proposta
            {
                LoanAmount = request.LoanAmount,
                AnnualInterestRate = request.AnnualInterestRate,
                NumberOfMonths = request.NumberOfMonths
            };

            _dbContext.Propostas.Add(proposta);
            _dbContext.SaveChanges();

            var summary = new PaymentFlowSummary
            {
                PropostaId = proposta.Id,
                MonthlyPayment = response.MonthlyPayment,
                TotalInterest = response.TotalInterest,
                TotalPayment = response.TotalPayment
            };

            _dbContext.PaymentFlowSummaries.Add(summary);
            _dbContext.SaveChanges();
        }

    }
}
