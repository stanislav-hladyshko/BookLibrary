using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksLibrary.Dal.Models
{
    [Table("Author")]
    public class Author
    {
        [Key] public int Id { get; set; }

        public int YearOfBirth { get; set; }
        public string Name { get; set; }
    }
}