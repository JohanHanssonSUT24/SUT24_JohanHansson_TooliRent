using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Application.DTOs
{
    public class StatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalTools { get; set; }
        public int TotalBookings { get; set; }
        public int AvailableTools { get; set; }
        public int RentedTools { get; set; }
    }
}
