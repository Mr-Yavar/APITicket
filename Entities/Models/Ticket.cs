using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Ticket : ModelBase
{

    public Guid EventId { get; set; }

    public Guid FloorSeatId { get; set; }

    public decimal Price { get; set; }

    [ForeignKey(nameof(Reservation))]
    public Guid? ReservedGroupId { get; set; }

    public Guid? UserId { get; set; }
    public DateTime? ExpirationDate { get; set; }

    public Reservation Reservation { get; set; }
    public virtual Event Event { get; set; } = null!;

    public virtual FloorSeat FloorSeat { get; set; } = null!;
}
