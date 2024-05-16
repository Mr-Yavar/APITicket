using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Concert :ModelBase
{

    public string Title { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Guid PromoterId { get; set; }

    public Guid PlaceId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
 

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Event> Events { get; } = new List<Event>();

    public virtual Place Place { get; set; } = null!;

    public virtual Promoter Promoter { get; set; } = null!;
}
