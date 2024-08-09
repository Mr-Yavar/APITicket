using Entities.Enum;
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
        public string? Title { get; set; }
        public string Value { get; set; }

        public ValueTypeEnum? ValueType { get; set; }


        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        public DynParameters Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        public ICollection<DynParameters> Children {  get; set; }

     
 

    }
}
