using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Entities.Models;

public partial class Discount : ModelBase
{

    public DateTime ExpirationDate { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public Guid? ObjectId { get; set; }
    public DiscountType DiscountType {get;set;}
    [Column("MaxDiscount", TypeName = "Decimal(16,2)")]
    public Decimal? MaxDiscount {  get; set; }
    [Column("Percentage", TypeName = "Decimal(4,2)")]
    public Decimal Percentage { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
