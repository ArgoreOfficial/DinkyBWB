using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.UI
{
    public class ExitButton : Button
    {
        float _hoverLerp = 0f;
        float _hoverLerpSpeed = 4f;
        public ExitButton(Texture2D texture, Vector2 position, float rotation = 0, float scale = 1) : base(texture, position, rotation, scale)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (!_isHovering && _hoverLerp > 0) _hoverLerp -= (float)gameTime.ElapsedGameTime.TotalSeconds * _hoverLerpSpeed;

            _scale = MathF.Sin(_time * 4) * 0.05f * _hoverLerp + 1;
            _rotation = MathF.Sin(_time * 6) * 0.1f * _hoverLerp;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnClick(GameTime gameTime)
        {
            Program.Game.Exit();
        }

        public override void OnHover(GameTime gameTime)
        {
            if (_hoverLerp < 1) _hoverLerp += (float)gameTime.ElapsedGameTime.TotalSeconds * _hoverLerpSpeed;
        }
    }
}
