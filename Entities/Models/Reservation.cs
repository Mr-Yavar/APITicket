using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Reservation : ModelBase
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    [ForeignKey(nameof(Transactions))]
    public Guid TransactionId { get; set; }

    public Ticket Ticket { get; set; }
    public Transactions Transaction { get; set; }

    public Discount Discount { get; set; }

}
