using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DynParameters
{
    public class DynParamterValueDto
    {

        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public Guid? ParentId { get; set; }
        public ValueTypeEnum ValueType { get; set; }

    }
}
