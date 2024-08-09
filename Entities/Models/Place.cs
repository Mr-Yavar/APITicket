using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Place : ModelBase
{

    public string Title { get; set; } = null!;

    public string? Address { get; set; }

    public string? Description { get; set; }


    public string? img_Url { get; set; }
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    [Column("lng", TypeName = "Decimal(9,6)")]
    public Decimal lng { get; set; }
    [Column("lat", TypeName = "Decimal(9,6)")]
    public Decimal lat { get; set; }

    [ForeignKey(nameof(City))]
    public Guid CityId { get; set; }
    public Definitions City { get; set; }
    public virtual ICollection<Concert> Concerts { get; } = new List<Concert>();

    public virtual ICollection<Hall> Halls { get; } = new List<Hall>();
}
