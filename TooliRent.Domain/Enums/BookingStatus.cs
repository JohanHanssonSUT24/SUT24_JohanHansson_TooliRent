using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooliRent.Domain.Enums
{
    public enum BookingStatus //Create Enums to represent fixed sets of related constants.
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
