using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TooliRent.Domain.Enums;

namespace TooliRent.Application.DTOs
{
    public class CreateToolDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RentalPrice { get; set; }
        [Required]
        public int? ToolCategoryId { get; set; }
        public ToolStatus Status { get; set; } = ToolStatus.Available;
        
    }
}
