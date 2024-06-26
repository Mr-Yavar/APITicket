using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Entities.Models;

public partial class Floor : ModelBase
{

    public string Title { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public Guid HallId { get; set; }

    public string seatMap_url { get; set; } = "./drawing.svg";
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    public virtual ICollection<FloorSeat> FloorSeats { get; } = new List<FloorSeat>();

    public virtual Hall Hall { get; set; } = null!;
}
