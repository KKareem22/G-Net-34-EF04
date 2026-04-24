using G_Net_34_EF04.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class Transaction
    {
        public int TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
        public TransactionType TransactionType { get; set; }

        public long AccountNumber { get; set; }
        public Account Account { get; set; } = default!;
    }
}
