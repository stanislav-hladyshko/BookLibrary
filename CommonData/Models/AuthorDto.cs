using System.ComponentModel.DataAnnotations;
using CommonData.CustomValidationAttributes;

namespace CommonData.Models
{
    public class AuthorDto
    {
        [YearOfBirth(-1800)] public int YearOfBirth { get; set; }

        [DataType(DataType.Text)] public string Name { get; set; }
    }
}