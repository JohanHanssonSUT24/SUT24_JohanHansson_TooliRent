using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TooliRent.Application.DTOs
{
    public class AddToolDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        public decimal DailyRentalPrice { get; set; }
    }
}
