using System.ComponentModel.DataAnnotations.Schema;

namespace BooksLibrary.Dal.Models
{
    [Table("BookAuthor")]
    public class BookAuthor
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }
}