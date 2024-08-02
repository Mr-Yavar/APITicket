using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Transactions : ModelBase
{
    public PayServiceType ServiceType { get; set; }
    public string refId { get; set; }
    public string CardNumber { get; set; }
    public PayStatusType Status { get; set; }
    public Decimal Amount { get; set; }
    public Decimal BillAmount { get; set; }

    public Reservation Reservation { get; set; }

}
