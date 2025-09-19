using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Application.DTOs
{
    public class ToolStatisticsDto : ToolDto
    {
        public int TotalRentals { get; set; }
    }
}
