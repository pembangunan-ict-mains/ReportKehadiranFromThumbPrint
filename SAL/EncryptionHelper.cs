using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL
{
    public static class EncryptionHelper
    {
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var base64 = Convert.ToBase64String(bytes);

            // Make it URL-safe: replace '+' with '-', '/' with '_', and remove '=' padding
            return base64.Replace('+', '-')
                         .Replace('/', '_')
                         .TrimEnd('=');
        }

        public static string Decode(string encoded)
        {
            if (string.IsNullOrEmpty(encoded)) return string.Empty;

            // Restore padding
            string padded = encoded.Replace('-', '+')
                                   .Replace('_', '/');
            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            var bytes = Convert.FromBase64String(padded);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
