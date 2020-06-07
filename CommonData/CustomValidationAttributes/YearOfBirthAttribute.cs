using System;
using System.ComponentModel.DataAnnotations;

namespace CommonData.CustomValidationAttributes
{
    public class YearOfBirthAttribute : ValidationAttribute
    {
        private readonly int _firstAuthorBirthYear;

        public YearOfBirthAttribute(int authorYearOfBirth)
        {
            _firstAuthorBirthYear = authorYearOfBirth;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                int parseResult = int.Parse(value.ToString()!);

                return parseResult <= DateTime.UtcNow.Year
                       && parseResult >= _firstAuthorBirthYear;
            }

            return false;
        }
    }
}