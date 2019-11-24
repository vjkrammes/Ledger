using System;
using System.Windows.Markup;

namespace LedgerClient.Infrastructure
{
    public sealed class BoolExtension : MarkupExtension
    {
        public bool Value { get; set; }
        public BoolExtension(bool value) => Value = value;
        public override object ProvideValue(IServiceProvider _) => Value;
    }

    public sealed class IntExtension : MarkupExtension
    {
        public int Value { get; set; }
        public IntExtension(int value) => Value = value;
        public override object ProvideValue(IServiceProvider _) => Value;
    }

    public sealed class LongExtension : MarkupExtension
    {
        public long Value { get; set; }
        public LongExtension(long value) => Value = value;
        public override object ProvideValue(IServiceProvider _) => Value;
    }

    public sealed class FloatExtension : MarkupExtension
    {
        public float Value { get; set; }
        public FloatExtension(float value) => Value = value;
        public override object ProvideValue(IServiceProvider _) => Value;
    }

    public sealed class DoubleExtension : MarkupExtension
    {
        public double Value { get; set; }
        public DoubleExtension(double value) => Value = value;
        public override object ProvideValue(IServiceProvider _) => Value;
    }

    public sealed class GenericObjectFactoryExtension : MarkupExtension
    {
        public Type Type { get; set; }
        public Type T { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var generictype = Type.MakeGenericType(T);
            return Activator.CreateInstance(generictype);
        }
    }
}
