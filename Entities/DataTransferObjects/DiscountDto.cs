using Entities.Enum;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class DiscountDto
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ExpirationDate { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public Guid ObjectId { get; set; }
        public DiscountType DiscountType { get; set; }
        public Decimal? MaxDiscount { get; set; }
        public Decimal? Percentage { get; set; }

    }
}