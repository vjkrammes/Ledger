namespace LedgerClient.Models
{
    public struct PasswordOptions
    {
        public int MinimumLength;
        public int MinumumUniqueCharacters;
        public bool RequireSpecialCharacters;
        public bool RequireLowercase;
        public bool RequireUppercase;
        public bool RequireDigits;
    }
}
