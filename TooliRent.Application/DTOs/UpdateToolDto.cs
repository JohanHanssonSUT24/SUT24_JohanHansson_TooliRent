using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Application.DTOs
{
    public class UpdateToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ToolCategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public decimal RentalPrice { get; set; }

    }
}
