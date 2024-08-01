using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Reservation : ModelBase
{

    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    public Guid? DiscountId { get; set; }
    public Decimal Paid { get; set; }

    public Guid TransactionId { get; set; }


}
