using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.PayingDto
{
    /// <summary>
    /// درخواست برای پرداخت صورتحساب
    /// </summary>
    /// <property name="refid">شناسه پرداخت می باشد که به کمک آن یک پرداخت تایید و یا اصطلاحا وریفای می شود</property>
    /// <property name="clientrefid">کد و یا عبارت اختصاصی است که توسط کاربر برای سرویس پرداخت در زمان ساخت پرداخت ارسال شده است وبعد از برگشت از درگاه برای پذیرنده ارسال می گردد            </property>
    /// <property name="cardnumber">
    /// شماره کارت پرداخت کننده. برای داشتن این پارامتر باید عبارت rrn
    /// در پایان ReturnUrl اضافه نمایید
    /// تا بعد از برگشت از درگاه، به آدرس برگشت اضافه گردد
    /// </property>
    public class toVerifyDto
    {
        /// <summary>
        /// شناسه پرداخت می باشد که به کمک آن یک پرداخت تایید و یا اصطلاحا وریفای می شود
        /// </summary>
        public string refId { get; set; }
        /// <summary>
        /// کد و یا عبارت اختصاصی است که توسط کاربر برای سرویس پرداخت در زمان ساخت پرداخت ارسال شده است وبعد از برگشت از درگاه برای پذیرنده ارسال می گردد
        /// اینجا همان آیدی تراکنش است
        /// </summary>
        public Guid clientrefid { get; set; }
        /// <summary>
        /// شماره کارت پرداخت کننده. برای داشتن این پارامتر باید عبارت rrn
        /// در پایان ReturnUrl اضافه نمایید
        /// تا بعد از برگشت از درگاه، به آدرس برگشت اضافه گردد
        /// </summary>
        public string CardNumber { get; set; }
        public string CardHashPan { get; set; }

        /// <summary>
        /// مقداری که باستی پرداخت میشده --- در سرویس پر میشود
        /// </summary>
        public Decimal? BillAmount { get; set; }
    }
}
