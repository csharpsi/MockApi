using System.ComponentModel.DataAnnotations;

namespace Sonovate.BackEnd
{
    public class StartsWithAttribute : ValidationAttribute
    {
        public char StartCharacter { get; set; }

        public StartsWithAttribute(char startWith)
        {
            StartCharacter = startWith;
            ErrorMessage = $"Value must start with the '{StartCharacter}' character";
        }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                ErrorMessage = "Value cannot be null or white space";
                return false;
            }

            return value.ToString().StartsWith(StartCharacter.ToString());
        }
    }
}