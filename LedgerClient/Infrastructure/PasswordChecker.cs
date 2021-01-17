using LedgerClient.Models;

using System.Collections.Generic;
using System.Linq;

namespace LedgerClient.Infrastructure
{
    public static class PasswordChecker
    {
        public static PasswordStrength GetPasswordStrength(string password)
        {
            var score = 0;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim()))
            {
                return PasswordStrength.Blank;
            }

            if (!HasMinimumLength(password, 5))
            {
                return PasswordStrength.VeryWeak;
            }

            score++;
            if (HasMinimumLength(password, 8))
            {
                score++;
            }

            if (HasUppercaseLetter(password) && HasLowercaseLetter(password))
            {
                score++;
            }

            if (HasDigit(password))
            {
                score++;
            }

            if (HasSpecialCharacter(password))
            {
                score++;
            }

            return (PasswordStrength)score;
        }

        public static bool IsStrongPassword(string password) =>
            HasMinimumLength(password, 8) &&
            HasUppercaseLetter(password) &&
            HasLowercaseLetter(password) &&
            HasSpecialCharacter(password) &&
            HasDigit(password);

        public static bool IsValidPassword(string password, PasswordOptions options) =>
            IsValidPassword(password, options.MinimumLength, options.MinumumUniqueCharacters,
                options.RequireSpecialCharacters, options.RequireLowercase, options.RequireUppercase,
                options.RequireDigits);

        public static bool IsValidPassword(string password, int minlen, int minunique, bool specials, bool lowers, bool uppers, bool digits)
        {
            if (!HasMinimumLength(password, minlen))
            {
                return false;
            }

            if (!HasMinimumUniqueCharacters(password, minunique))
            {
                return false;
            }

            if (specials && !HasSpecialCharacter(password))
            {
                return false;
            }

            if (lowers && !HasLowercaseLetter(password))
            {
                return false;
            }

            if (uppers && !HasUppercaseLetter(password))
            {
                return false;
            }

            if (digits && !HasDigit(password))
            {
                return false;
            }

            return true;
        }

        #region Has.... methods

        public static bool HasMinimumLength(string password, int min) => password.Length >= min;
        public static bool HasMinimumUniqueCharacters(string password, int count) => password.Distinct().Count() >= count;
        public static bool HasDigit(string password) => password.Any(c => char.IsDigit(c));
        public static bool HasSpecialCharacter(string password) =>
            password.IndexOfAny("!@#$%^&*()_+-={}[];:'\"<>,./?|\\".ToCharArray()) != -1;
        public static bool HasUppercaseLetter(string password) => password.Any(c => char.IsUpper(c));
        public static bool HasLowercaseLetter(string password) => password.Any(c => char.IsLower(c));

        #endregion
    }
}
