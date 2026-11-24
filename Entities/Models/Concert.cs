using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Entities.Models;

public partial class Concert :ModelBase
{


    public string Title { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Guid PromoterId { get; set; }

    public Guid PlaceId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
 
    public string img_url { get; set; }
    public string? poster {  get; set; }

    [MaxLength(72)]
    public string? Note { get; set; }
    [ForeignKey(nameof(CategoryId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Event> Events { get; } = new List<Event>();
    [ForeignKey(nameof(PlaceId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual Place Place { get; set; } = null!;
    [ForeignKey(nameof(PromoterId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual Promoter Promoter { get; set; } = null!;
}
