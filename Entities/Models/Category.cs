using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Category : ModelBase
{

    public string Title { get; set; } = null!;

    public Guid? ParentId { get; set; }
   
    public virtual ICollection<Concert> Concerts { get; } = new List<Concert>();

    public virtual ICollection<Promoter> Promoters { get; } = new List<Promoter>();
}
