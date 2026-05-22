namespace kitap_y.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        // Kitap ile bağlantı (İlişki için)
        public Book? Book { get; set; }
    }
}