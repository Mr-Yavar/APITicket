using Entities.Enum;
using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Transactions : ModelBase
{
    public PayServiceType ServiceType { get; set; }
    public string refId { get; set; }
    public PayStatusType Status { get; set; }
    public Decimal Amount { get; set; }
    public Decimal BillAmount { get; set; }
    public Guid DiscountId { get; set; }
    
    public Reservation Reservation { get; set; }

}
