using G_Net_34_EF04.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class CustomerAccount
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }=default!;

        public long AccountId { get; set; }
        public Account Account { get; set; } = default!;
        public DateTime OwnershipStartDate { get; set; }
        public OwnershipType  OwnershipType { get; set; }
        public AccountStatus Status { get; set; }
    }
}
