using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Entities.Models;

public partial class Promoter  :  ModelBase
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid CategoryId { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    public virtual Category Category { get; set; } = null!;
    public string img_url { get; set; }
    public virtual ICollection<Concert> Concerts { get; } = new List<Concert>();
}
