using System;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace SampleGame
{
    public partial class MyCircle : Circle
    {
        public Action? Click { get; set; }
        public Action? RightClick { get; set; }
        public Action? DoubleClick { get; set; }

        protected override bool OnClick(ClickEvent e)
        {
            if (e.Button == osuTK.Input.MouseButton.Left)
            {
                Click?.Invoke();
            }
            else
            {
                RightClick?.Invoke();
            }
            return base.OnClick(e);
        }
        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            DoubleClick?.Invoke();
            return base.OnDoubleClick(e);
        }
    }
}

