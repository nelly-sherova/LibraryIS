namespace LibraryIS.Models
{
    public class BookUserVM
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReturnDate { get; set; }    
    }
}
