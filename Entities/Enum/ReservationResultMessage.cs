using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
    public enum ReservationResultMessage : byte
    {
        [Description("رزرو بلیت‌ها با موفقیت انجام شد!")]
        Success = 0,
        [Description("رزرو موقت بلیت‌ها لغو یا مدت زمان مجاز به اتمام رسیده است است!")]
        ReservationCancelled = 1,
        [Description("پرداخت تایید نشد!")]
        NotVerified = 2,
        [Description("خطای داخلی سیستم")]
        InternalError = 3,

    }
}
