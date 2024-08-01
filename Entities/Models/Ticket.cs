using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Ticket : ModelBase
{

    public Guid EventId { get; set; }

    public Guid FloorSeatId { get; set; }

    public decimal Price { get; set; }

    public Guid? ReservedGroupId { get; set; }

    public Guid? UserId { get; set; }
    public DateTime? Expire { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual FloorSeat FloorSeat { get; set; } = null!;
}
