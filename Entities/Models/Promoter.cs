using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Promoter  :  ModelBase
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid CategoryId { get; set; }


    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Concert> Concerts { get; } = new List<Concert>();
}
