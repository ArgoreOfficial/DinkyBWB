using Dinky_bwb.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dinky_bwb.Screens
{
    public class SplashScreen : Screen
    {
        Texture2D SplashImage;
        
        public SplashScreen(string name, Texture2D splashImage) : base(name)
        {
            SplashImage = splashImage;
        }

        public override void Update(GameTime gameTime)
        {
            if(_time >= 3f)
            {
                ScreenManager.SetScreen("main_menu");
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                SplashImage,
                new Vector2(256, 256),
                new Rectangle(0, 0, SplashImage.Width, SplashImage.Height),
                Color.White * MathF.Min(MathF.Sin(_time * 1.2f) * 1.4f, 1),
                _time * 0.05f,
                new Vector2(SplashImage.Width / 2, SplashImage.Height / 2),
                1f + _time * 0.1f,
                SpriteEffects.None,
                1f);

            base.Draw(spriteBatch);
        }
    }
}
