using Fresh.Domain.Common;

namespace Fresh.Domain.Entities
{
    public class ProductLetter : BaseEntity
    {
        public int Id { get; set; }
        public string ProductDescription { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public int UserId { get; set; }
        public float Price { get; set; }
    }
}
