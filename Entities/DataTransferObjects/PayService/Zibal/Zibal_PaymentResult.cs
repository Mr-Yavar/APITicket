using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.PayService.Zibal
{
    public class Zibal_PaymentResult
    {

        public string trackId { get; set; }
        public string result { get; set; }
        public string payLink { get; set; }
        public string message { get; set; }


    }
}
