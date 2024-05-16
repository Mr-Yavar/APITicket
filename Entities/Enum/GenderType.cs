using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum GenderType : byte
    {

        [Description("تعیین نشده")]
        Unknown=0,
        [Description("زن")]
        Woman = 1,
        [Description("مرد")]
        Man = 2,

    }
}
