using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.PayingDto
{
    public class toPayDto
    {
        /// <summary>
        /// temp Reservation Id
        /// </summary>
        public Guid ReservationId { get; set; }
        public Guid? DiscountId { get; set; }

    }
}
