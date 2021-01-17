using System;

namespace LedgerClient.Infrastructure
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        private readonly string _description;
        public DescriptionAttribute(string description) => _description = description;
        public string Description => _description;
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ExplorerIconAttribute : Attribute
    {
        private readonly string _icon;
        public ExplorerIconAttribute(string icon) => _icon = icon;
        public string ExplorerIcon => _icon;
    }
}
