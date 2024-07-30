using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    public class DynParameters : ModelBase
    {
       
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }

        [ForeignKey(nameof(DynParameters))]
        public Guid? ParentId { get; set; }

        [InverseProperty("Parent")]
        public ICollection<DynParameters> Children {  get; set; } 
        
        public DynParameters Parent { get; set; }
    }
}
