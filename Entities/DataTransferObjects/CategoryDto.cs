namespace Entities.DataTransferObjects
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
  
    }
}