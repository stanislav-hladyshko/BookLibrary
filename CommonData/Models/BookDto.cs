using System.ComponentModel.DataAnnotations;
using CommonData.CustomValidationAttributes;

namespace CommonData.Models
{
    public class BookDto
    {
        [MinValue(0)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [BookGraduationYear(-1800)]
        public int GraduationYear { get; set; }

        [MinValue(0)]
        public int PagesAmount { get; set; }
    }
}