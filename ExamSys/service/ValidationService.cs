using System;
using System.Text.RegularExpressions;

namespace ExamSys.service
{
    internal class ValidationService
    {
        public bool ValidateUserType(int type)
        {
            return type == 1 || type == 2;
        }

        public bool ValidateField(string fieldValue)
        {
            return !string.IsNullOrWhiteSpace(fieldValue);
        }

        public bool ValidateInteger(string input)
        {
            return int.TryParse(input, out _);
        }

        public bool ValidatePasswordStrength(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[a-zA-Z]") &&
                   Regex.IsMatch(password, @"[0-9]") &&
                   Regex.IsMatch(password, @"[\W_]");
        }
    }
}
