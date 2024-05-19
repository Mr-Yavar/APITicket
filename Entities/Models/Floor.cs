using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Floor : ModelBase
{

    public string Title { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public Guid HallId { get; set; }

    public string seatMap_url { get; set; } = "./drawing.svg";

    public virtual ICollection<FloorSeat> FloorSeats { get; } = new List<FloorSeat>();

    public virtual Hall Hall { get; set; } = null!;
}
