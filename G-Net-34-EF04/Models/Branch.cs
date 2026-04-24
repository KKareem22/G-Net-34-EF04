using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class Branch
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Address? Address { get; set; }

        public int ManagerId { get; set; }
        public Manager Manager { get; set; } = default!;

        public ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
