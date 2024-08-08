using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.PayingDto
{
    /// <summary>
    /// جواب درخواست برای پرداخت صورتحساب
    /// </summary>
    public class toVerifyResponse
    {

        public bool Success { get; set; }

        public List<SeatInfo> Seats { get; set; }
        public TempReservationDto Reservation { get; set; }
        public Guid ReservationId { get; set; }
        public string Message { get; set; }

    }
}
