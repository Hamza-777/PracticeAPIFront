namespace PracticeAPIFront.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public float Price { get; set; }
        public DateTime Manufacturedate { get; set; }
        public DateTime? Expirydate { get; set; }
        public string? Manufacturedby { get; set; }
    }
}
