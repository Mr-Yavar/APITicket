using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class PromoterDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    
        public string? img_url { get; set; }
        public string CategoryName { get; set; }

    }
}
