using Entities.Enum;
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
    public Decimal Amount { get; set; }
    /// <summary>
    /// با تخفیف
    /// </summary>
    public Decimal BillAmount { get; set; }

    public Guid? ReservedGroupId { get; set; }
    public Reservation? Reservation { get; set; }

    public Guid? TempReservedId { get; set; }
    public TempReservation? TempReservation { get; set; }

}
