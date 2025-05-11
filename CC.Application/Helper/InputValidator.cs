using System.Text.RegularExpressions;

namespace CC.Application.Helper
{
    internal class InputValidator
    {
        public static bool IsEmail(string input)
        {
            // Simple email validation (checks for presence of '@' and '.' as basic markers)
            return input.Contains("@") && input.Contains(".");
        }

        public static bool IsPhoneNumber(string input)
        {
            // Simple phone number validation (checks for digits, with optional "+" for international format)
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$"); // E.164 format, supports numbers like "+1234567890"
            return phoneRegex.IsMatch(input);
        }
    }
}
