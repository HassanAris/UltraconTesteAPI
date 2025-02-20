using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UltraconTesteAPI.Models;

namespace UltraconTesteAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PaymentFlowSummary> PaymentFlowSummaries { get; set; }
    }

}
