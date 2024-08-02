using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum DiscountType : byte
    {
        [Description("کنسرت")]
        Concert = 0,
        [Description("اجرا کننده")]
        Promoter = 1
    }
}
