using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Place : ModelBase
{

    public string Title { get; set; } = null!;

    public string? Address { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Concert> Concerts { get; } = new List<Concert>();

    public virtual ICollection<Hall> Halls { get; } = new List<Hall>();
}
