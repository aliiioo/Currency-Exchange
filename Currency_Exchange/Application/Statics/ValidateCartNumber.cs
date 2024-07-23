using System.Text.RegularExpressions;

namespace Application.Statics
{
    public static class ValidateCartNumber
    {
        public static bool IsValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrEmpty(cardNumber) && Regex.IsMatch(cardNumber, @"^\d{16}$");
        }
    }
}
