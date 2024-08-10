using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum DiscountType : sbyte
    {

        [Description("نامعتبر")]
        INVALIAD = -2,
        [Description("بدون شرط")]
        NOCONDITION = -1,
        [Description("کنسرت")]
        Concert = 0,
        [Description("اجرا کننده")]
        Promoter = 1,
        [Description("شهر")]
        City = 2,
        [Description("مکان برگزاری")]
        Place = 3
    }
}
