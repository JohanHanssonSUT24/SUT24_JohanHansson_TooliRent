using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Domain.Entities
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RentalPrice { get; set; }
        public bool IsAvalible { get; set; }
        public bool IsDeleted { get; set; }

        public int ToolCategoryId { get; set; }
        public ToolCategory ToolCategory { get; set; }
        public ICollection<Booking> Bookings
        {
            get; set;
        }
    }
}
