using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.DataTransferObjects.PayingDto
{
    /// <summary>
    /// آبجکت وریفای کردن پرداخت
    /// </summary>
    public class RequestVerify
    {
        public int Amount { get; set; }
        public string RefId { get; set; }
    }
}