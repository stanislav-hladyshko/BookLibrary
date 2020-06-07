using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksLibrary.Dal.Models
{
    [Table("Book")]
    public class Book
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int GraduationYear { get; set; }
        public int PagesAmount { get; set; }
    }
}