using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Reservation : ModelBase
{

    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

}
