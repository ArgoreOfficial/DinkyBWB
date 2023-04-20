using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.UI
{
    public static class ExtraDraw
    {

        public static void DrawCentered(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float rotation)
        {
            spriteBatch.Draw(
                texture,
                position,
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                new Vector2(texture.Width / 2, texture.Height / 2),
                1f,
                SpriteEffects.None,
                1f);
        }

    }
}
