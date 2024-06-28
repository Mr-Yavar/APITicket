using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class HallDto
    {
        public Guid ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; } = null!;

        public Guid PlaceId { get; set; }

        public string? Description { get; set; }

        public string? ImgUrl { get; set; }
        public bool IsDeleted { get; set; }


        public Decimal lng { get; set; }
        public Decimal lat { get; set; }

      
    }
}
