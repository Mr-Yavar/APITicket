using Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Transactions : ModelBase
{
    public PayServiceType ServiceType { get; set; }
    /// <summary>
    /// کد درگاه پرداخت
    /// </summary>
    public string refId { get; set; }
    public string? CardNumber { get; set; }
    public PayStatusType Status { get; set; }
    /// <summary>
    /// بدون تخفیف
    /// </summary>
    [Column("Amount",TypeName = "Decimal(16,2)")]
    public Decimal Amount { get; set; }
    /// <summary>
    /// با تخفیف
    /// </summary>
    [Column("BillAmount", TypeName = "Decimal(16,2)")]
    public Decimal BillAmount { get; set; }



    public Guid? ReservedGroupId { get; set; }
    [ForeignKey(nameof(ReservedGroupId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Reservation? Reservation { get; set; }



    public Guid? TempReservedId { get; set; }
    [ForeignKey(nameof(TempReservedId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public TempReservation? TempReservation { get; set; }

}
