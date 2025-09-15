using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Enums;

namespace TooliRent.Application.DTOs
{
    public class ToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RentalPrice { get; set; }
        public ToolStatus Status { get; set; }
        public string ToolCategoryName { get; set; }


    }
}
