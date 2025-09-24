using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Domain.Enums
{
    public enum BookingStatus
    {
        Active,
        Completed,
        Cancelled,
        PickedUp,
        Overdue,
        Approved,
        Pending
    }
}
