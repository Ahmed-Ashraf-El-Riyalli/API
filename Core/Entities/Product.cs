
namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string ImageName { get; set; }
    }
}
