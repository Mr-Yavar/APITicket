using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class FloorSeat : ModelBase
{

    public Guid SeatId { get; set; }

    public Guid FloorId { get; set; }


    public virtual Floor Floor { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
