using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ConcertDto
    {
        public string Title { get; set; } = null!;

        public Guid PromoterId { get; set; }

        public Guid PlaceId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
