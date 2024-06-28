using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class PlaceDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;

        public string? Address { get; set; }
        public string? img_Url { get; set; }
        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}
