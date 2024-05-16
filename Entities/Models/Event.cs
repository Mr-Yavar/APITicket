using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Event :  ModelBase
{


    public Guid ConcertId { get; set; }

    public Guid HallId { get; set; }

    public DateTime StartSaleDate { get; set; }

    public DateTime EndSaleDate { get; set; }

    public DateTime ExecutionDate { get; set; }

    public string? Note { get; set; }

    public virtual Concert Concert { get; set; } = null!;

    public virtual Hall Hall { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
