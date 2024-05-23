// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Containers;

namespace SampleGame
{
    public partial class CustomTextBox : BasicTextBox
    {
        public CustomTextBox(float fontSize = 20)
        {
            FontSize = fontSize;
        }
        protected override Drawable GetDrawableCharacter(char c)
        {
            return new ScalingText(c, FontSize);
        }

        private partial class ScalingText : CompositeDrawable
        {
            private readonly SpriteText text;

            public ScalingText(char c, float textSize)
            {
                AddInternal(text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = c.ToString(),
                    Font = FrameworkFont.Condensed.With(size: textSize),

                });
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Size = text.DrawSize;
            }

            public override void Show()
            {
                text.Scale = Vector2.Zero;
                text.FadeIn(200).ScaleTo(1, 200);
            }

            public override void Hide()
            {
                text.Scale = Vector2.One;
                text.ScaleTo(0, 200).FadeOut(200);
            }
        }

        protected override Caret CreateCaret() => new BorderCaret();

        private partial class BorderCaret : Caret
        {
            private const float caret_width = 2;

            public BorderCaret()
            {
                InternalChild = new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Colour4.White,
                    BorderThickness = 3f,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Transparent,
                    },
                };
            }

            public override void DisplayAt(Vector2 position, float? selectionWidth)
            {
                Position = position - Vector2.UnitX;
                Width = selectionWidth + 1 ?? caret_width;
            }
        }
    }
}
