using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Entities.Models;

public partial class Reservation : ModelBase
{
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    public ReservationStatusType Status { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [ForeignKey(nameof(Discount))]
    public Guid? DiscountId { get; set; }

    public Discount? Discount { get; set; }
    [InverseProperty("Reservation")]
    public ICollection<Ticket> Tickets { get; set; }

    [InverseProperty("TempReservation")]
    public ICollection<Ticket> TempTickets { get; set; }

    
   

}
