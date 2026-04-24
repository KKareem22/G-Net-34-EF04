using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class Manager
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string? Email { get; set; }
        public Branch Branch { get; set; } = default!;
    }
}
