using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Ticket : ModelBase
{

    public Guid EventId { get; set; }

    public Guid FloorSeatId { get; set; }

    [Column("Price", TypeName = "Decimal(16,2)")]
    public Decimal Price { get; set; }

    [ForeignKey(nameof(Reservation))]
    public Guid? ReservedGroupId { get; set; }
    public Reservation? Reservation { get; set; }

    [ForeignKey(nameof(TempReservation))]
    public Guid? TempReservedId { get; set; }
    public TempReservation? TempReservation { get; set; }


    public virtual Event Event { get; set; } = null!;

    public virtual FloorSeat FloorSeat { get; set; } = null!;
}
