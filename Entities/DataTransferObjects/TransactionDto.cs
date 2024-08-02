using Entities.Enum;

namespace Entities.DataTransferObjects
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public PayServiceType ServiceType { get; set; }
        public string refId { get; set; }
        public PayStatusType Status { get; set; }
        public Decimal amount { get; set; }

    }
}