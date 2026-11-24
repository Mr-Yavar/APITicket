using Microsoft.EntityFrameworkCore;
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


    public Guid? ReservedGroupId { get; set; }
    [ForeignKey(nameof(ReservedGroupId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Reservation? Reservation { get; set; }




    public Guid? TempReservedId { get; set; }
    [ForeignKey(nameof(TempReservedId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]

    public TempReservation? TempReservation { get; set; }

    [ForeignKey(nameof(EventId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual Event Event { get; set; } = null!;
    [ForeignKey(nameof(FloorSeatId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual FloorSeat FloorSeat { get; set; } = null!;
}
