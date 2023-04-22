using Dinky_bwb.Extra;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Screens
{
    public class MainMenuScreen : Screen
    {
        Texture2D _titleImage;
        Texture2D _titleDinky;
        Texture2D _titleButwhere;
        Texture2D _titleBubbo;

        PlayButton _playButton;
        ExitButton _quitButton;

        public MainMenuScreen(
            string name, 
            Texture2D titleImage, 
            Texture2D titleDinky, 
            Texture2D titleButwhere, 
            Texture2D titleBubbo, 
            Texture2D playButton, 
            Texture2D exitButton) : base(name)
        {
            _titleImage = titleImage;
            _titleDinky = titleDinky;
            _titleButwhere = titleButwhere;
            _titleBubbo = titleBubbo;

            _playButton = new PlayButton(playButton, new Vector2(256, 256));
            _quitButton = new ExitButton(exitButton, new Vector2(256, 350));
        }

        public override void Update(GameTime gameTime)
        {
            _playButton.Update(gameTime);
            _quitButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawTitleText(spriteBatch, new Vector2(170, 120));

            _playButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        void DrawTitleText(SpriteBatch spriteBatch, Vector2 position)
        {
            ExtraDraw.DrawCentered(
                spriteBatch,
                _titleImage,
                new Vector2(position.X + 230 + MathF.Sin(_time * 7) * 3, position.Y + 150 + MathF.Sin(_time * 4) * 10),
                MathF.Sin(_time * 2 + 0.5f) * 0.1f);

            ExtraDraw.DrawCentered(
                spriteBatch,
                _titleDinky,
                new Vector2(position.X - 26 + MathF.Sin(_time * 4) * 3, position.Y - 47 + MathF.Sin(_time * 3) * 5),
                MathF.Sin(_time * 2 + 1.2f) * 0.2f);

            ExtraDraw.DrawCentered(
                spriteBatch,
                _titleButwhere,
                new Vector2(position.X + 8 + MathF.Sin(_time * 5) * 2, position.Y - 8 + MathF.Sin(_time * 4) * 2),
                MathF.Sin(_time * 2 + 2.5f) * 0.1f);

            ExtraDraw.DrawCentered(
                spriteBatch,
                _titleBubbo,
                new Vector2(position.X - 22 + MathF.Sin(_time * 5) * 6, position.Y + 38 + MathF.Sin(_time * 7) * 4),
                MathF.Sin(_time * 2) * 0.15f);

        }
    }
}
