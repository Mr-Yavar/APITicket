using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TmpReservation : ModelBase
{

    public Guid UserId { get; set; }
    public Guid TicketId { get; set; }
    public DateTime Expire {  get; set; }

}
