using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum ValueTypeEnum : byte
    {
        [Description("متن")]
        STRING = 0,
        [Description("فایل")]
        FILE = 1,

    }
}
