namespace AuthenteficationBookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public string ImageURL { get; set; }
        public string Details { get; set; }
    }
}