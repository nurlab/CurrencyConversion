using System.Text.RegularExpressions;

namespace CC.Application.Helper
{
    /// <summary>
    /// Provides methods for validating common input types such as email addresses and phone numbers.
    /// </summary>
    internal class InputValidator
    {
        /// <summary>
        /// Validates if the given input is a valid email address.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <returns><c>true</c> if the input is a valid email address; otherwise, <c>false</c>.</returns>
        public static bool IsEmail(string input)
        {
            return input.Contains("@") && input.Contains(".");
        }

        /// <summary>
        /// Validates if the given input is a valid phone number in E.164 format.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <returns><c>true</c> if the input is a valid phone number in E.164 format; otherwise, <c>false</c>.</returns>
        public static bool IsPhoneNumber(string input)
        {
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$"); // E.164 format, supports numbers like "+1234567890"
            return phoneRegex.IsMatch(input);
        }
    }
}
