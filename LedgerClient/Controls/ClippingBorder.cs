using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LedgerClient.Controls
{
    public class ClippingBorder : Border
    {
        private readonly RectangleGeometry _clipRectangle = new RectangleGeometry();
        private object _oldClip;

        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                if (Child != null)
                {
                    Child.SetValue(ClipProperty, _oldClip);
                }
                if (value != null)
                {
                    _oldClip = value.ReadLocalValue(ClipProperty);
                }
                else
                {
                    _oldClip = null;
                }
                base.Child = value;
            }
        }

        protected virtual void OnApplyChildClip()
        {
            var child = Child;
            if (child != null)
            {
                _clipRectangle.RadiusX = _clipRectangle.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - (BorderThickness.Left * 0.5));
                _clipRectangle.Rect = new Rect(child.RenderSize);
                child.Clip = _clipRectangle;
            }
        }
    }
}
