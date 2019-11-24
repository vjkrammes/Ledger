namespace LedgerLib.Infrastructure
{
    public enum DueDateType
    {
        [Description("Unspecified")]
        Unspecified = 0,
        [Description("Monthly")]
        Monthly = 1,
        [Description("Quarterly")]
        Quarterly = 2,
        [Description("Annually")]
        Annually = 3,
        [Description("Service Related")]
        ServiceRelated = 4
    }
}
