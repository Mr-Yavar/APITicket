using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entities.Models;

public partial class Hall : ModelBase
{

    public string Title { get; set; } = null!;

    public Guid PlaceId { get; set; }

    public string? Description { get; set; }

    public string? ImgUrl { get; set; }
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    
    public Decimal lng { get; set; }
    public Decimal lat { get; set; }
    public virtual ICollection<Event> Events { get; } = new List<Event>();

    public virtual ICollection<Floor> Floors { get; } = new List<Floor>();

    public virtual Place Place { get; set; } = null!;
}
