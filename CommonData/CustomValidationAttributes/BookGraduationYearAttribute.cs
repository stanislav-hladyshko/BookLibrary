using System;
using System.ComponentModel.DataAnnotations;


namespace CommonData.CustomValidationAttributes
{
    public class BookGraduationYearAttribute : ValidationAttribute
    {
        private readonly int _firstAuthorBirthYear;
        private const int FutureBookReleaseCase = 10;

        public BookGraduationYearAttribute(int firstAuthorBirthYear)
        {
            _firstAuthorBirthYear = firstAuthorBirthYear;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                int? parseResult = int.Parse(value.ToString()!);

                return parseResult <= DateTime.UtcNow.Year + FutureBookReleaseCase
                       && parseResult >= _firstAuthorBirthYear;
            }

            return false;
        }
    }
}