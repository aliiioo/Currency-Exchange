using System.Security.Cryptography;
using System.Text;

namespace Application.Statics
{
    public static class CartNumbers
    {
        public static string GenerateUnique16DigitNumbers()
        {
            var ticks = DateTime.UtcNow.Ticks;
            var ticksString = ticks.ToString();
            var randomNumber = GenerateRandomNumber(6);
            var uniqueNumber = ticksString + randomNumber;
            if (uniqueNumber.Length > 16)
            {
                uniqueNumber = uniqueNumber.Substring(0, 16);
            }

            return uniqueNumber;
        }



        private static string GenerateRandomNumber(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                rng.GetBytes(randomNumber);
                StringBuilder result = new StringBuilder(length);
                foreach (byte b in randomNumber)
                {
                    result.Append(b % 10); 
                }
                return result.ToString();
            }
        }

        public static string FormatNumberWithDashes(string number)
        {
            StringBuilder formattedNumber = new StringBuilder();
            for (int i = 0; i < number.Length; i += 4)
            {
                if (i > 0)
                {
                    formattedNumber.Append("-");
                }
                formattedNumber.Append(number.Substring(i, 4));
            }
            return formattedNumber.ToString();
        }
    }
}
