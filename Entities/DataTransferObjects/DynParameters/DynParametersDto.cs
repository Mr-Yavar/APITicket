using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DynParameters
{
    public class DynParametersDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public Guid? ParentId { get; set; }
        public ValueTypeEnum ValueType { get; set; }

        public List<DynParamterValueDto> Values { get; set; }
    }
}
