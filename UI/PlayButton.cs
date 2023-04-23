using Dinky_bwb.Extra;
using Dinky_bwb.Managers;
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
    public class PlayButton : Button
    {
        float _hoverLerp = 0f;
        float _hoverLerpSpeed = 4f;

        Vector2 _basePosition;
        public PlayButton(Texture2D texture, Vector2 position, float rotation = 0, float scale = 1) : base(texture, position, rotation, scale)
        {
            _basePosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isHovering && _hoverLerp > 0) _hoverLerp -= (float)gameTime.ElapsedGameTime.TotalSeconds * _hoverLerpSpeed;

            _scale = MathF.Sin(_time * 54) * 0.05f * _hoverLerp + 1;
            _rotation = MathF.Sin(_time * 36) * 0.1f * _hoverLerp;


            _position = _basePosition + new Vector2(
                   ExtraMath.Noise(_time * 20) * _hoverLerp * 4f,
                   ExtraMath.Noise(_time * 20 + 834) * _hoverLerp * 4f
                );

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnClick(GameTime gameTime)
        {
            ScreenManager.SetNextScreen("game");
        }

        public override void OnHover(GameTime gameTime)
        {
            if (_hoverLerp < 1) _hoverLerp += (float)gameTime.ElapsedGameTime.TotalSeconds * _hoverLerpSpeed;
        }
    }
}
