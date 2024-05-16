using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Seat : ModelBase
{

    public int? Row { get; set; }

    public int? Column { get; set; }

    public Guid? ParentId { get; set; }

    public virtual ICollection<FloorSeat> FloorSeats { get; } = new List<FloorSeat>();
}
