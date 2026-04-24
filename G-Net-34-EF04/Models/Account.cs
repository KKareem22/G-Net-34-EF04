using G_Net_34_EF04.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime OpeningDate { get; set; }

        public string BranchId { get; set; } = default!;
        public Branch Branch { get; set; } = default!;
        public AccountType AccountType { get; set; }
        public AccountStatus Status { get; set; }
        public ICollection<Transaction>? Transactions { get; set; } = new HashSet<Transaction>();
        public ICollection<CustomerAccount> Owners { get; set; } = new HashSet<CustomerAccount>();
    }
}
