using Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Entities.Models;

[PrimaryKey(nameof(Id))]
public partial class TempReservation : ModelBase
{
   
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User User { get; set; }

    public ReservationStatusType Status { get; set; }

    public DateTime? ExpirationDate { get; set; }


    public Guid? DiscountId { get; set; }

    [ForeignKey(nameof(DiscountId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Discount? Discount { get; set; }
    
    public ICollection<Ticket> Tickets { get; set; }


    public ICollection<Transactions> Transactions { get; set; }

}
