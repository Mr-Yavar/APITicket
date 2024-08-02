using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entities.Models;

public partial class Discount : ModelBase
{

    public DateTime ExpirationDate { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public Guid ObjectId { get; set; }
    public DiscountType DiscountType {get;set;}
    public Decimal? MaxDiscount {  get; set; }
    public Decimal Percentage { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
