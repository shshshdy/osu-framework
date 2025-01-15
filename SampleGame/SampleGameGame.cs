// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Framework.Graphics;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using System.IO;
using osu.Framework.Graphics.UserInterface;
using System;
using osu.Framework.Bindables;
using osu.Framework.IO.Stores;
using osu.Framework.Configuration;
using osu.Framework.Configuration.Tracking;
using SixLabors.ImageSharp;
using Size = System.Drawing.Size;

namespace SampleGame
{
    public partial class SampleGameGame : Game
    {
        private Box box = null!;

        private MyCircle circle = null!;
        private Texture texture = null!;
        private Button btn = null!;
        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            //修改分辩率
            var size = config.GetBindable<Size>(FrameworkSetting.WindowedSize);
            size.Value = new Size(1024, 768);
            var strore = new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(SampleGameGame).Assembly), @"Resources");
            Resources.AddStore(strore);
            AddFont(Resources, "Fonts/simsun/simsun", Fonts);

            using (var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "logo.png")))
            {
                texture = Texture.FromStream(Host.Renderer, stream)!;
            }
            Add(box = new Box
            {
                Name = "box",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(350, 350),
                Colour = Color4.Tomato
            });
            // box.Hide();
            Add(new Sprite
            {
                Name = "Sprite",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Texture = texture,
                Colour = Colour4.Blue,
                Blending = BlendingParameters.Additive
            });
            Add(circle = new MyCircle
            {
                Name = "circle",
                Size = new Vector2(150, 150),
                Colour = Colour4.Red,
                BorderColour = Color4.Blue,
                Position = new Vector2(120, 120),
                Child = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = texture,
                },
            });
            circle.Click = () =>
            {
                i++;
                circle.Colour = i % 2 == 0 ? Colour4.Yellow : Colour4.Red;
            };
            Add(new CustomTextBox(30)
            {
                Text = @"cC中文sfdASDF",
                Size = new Vector2(300, 30),
                Position = new Vector2(220, 20),
            });
            if (RuntimeInfo.IsMobile)
            {
                circle.Scale = new Vector2(2, 2);
            }
        }
        private int i = 0;
        protected override void Update()
        {
            base.Update();
            box.Rotation += (float)Time.Elapsed / 10;
        }
    }
}
