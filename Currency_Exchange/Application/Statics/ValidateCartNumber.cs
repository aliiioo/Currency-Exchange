using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
