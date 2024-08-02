using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum PayStatusType : byte
    {
        [Description("در حال انجام")]
        InProgress = 0,
        [Description("تایید شده")]
        Verified = 0,
        [Description("باطل شده")]
        Refuted = 0,
    }
}
