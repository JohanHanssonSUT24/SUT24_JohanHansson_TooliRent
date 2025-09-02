using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Domain.Entities
{
    public  class Booking
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPickedUp { get; set; }
        public bool IsReturned { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Tool> Tools { get; set; }

    }
}
