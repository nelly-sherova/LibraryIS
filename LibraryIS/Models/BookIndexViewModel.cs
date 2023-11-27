namespace LibraryIS.Models
{
    public class BookIndexViewModel
    {
        public int Collection { get; set; }
        public string Name { get; set; }
        public int BookCount { get; set; }
        public string Publication { get; set; }
        public string Language { get; set; }
        public ICollection<BookAuthor>? BookAuthor { get; set; }
        public ICollection<BookCategory>? BookCategory { get; set; }
    }
}
