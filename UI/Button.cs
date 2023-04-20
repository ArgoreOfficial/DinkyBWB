using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Dinky_bwb.UI
{
    public abstract class Button
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected float _rotation;
        protected float _scale;
        protected float _time;

        protected bool _isHovering = false;

        private ButtonState _buttonState;
        private ButtonState _previousButtonState;
        private bool _startedPressingOn;

        protected Button(Texture2D texture, Vector2 position, float rotation = 0f, float scale = 1f)
        {
            _texture = texture;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public virtual void Update(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            MouseState mouse = Mouse.GetState();

            if(ContainsMouse(mouse.Position))
            {
                OnHover(gameTime);
                _isHovering = true;
            }
            else
            {
                _isHovering = false;
            }

            ClickCheck(mouse, gameTime);
        }

        bool ContainsMouse(Point mousePosition)
        {
            return _texture.Bounds.Contains(mousePosition.ToVector2() - _position + new Vector2(_texture.Width / 2, _texture.Height / 2));
        }

        void ClickCheck(MouseState mouse, GameTime gameTime)
        {
            _previousButtonState = _buttonState;
            _buttonState = mouse.LeftButton;

            if (_previousButtonState == _buttonState) return;
            
            // if mouse is over button
            if (ContainsMouse(mouse.Position))
            {
                // and pressing
                if(_buttonState == ButtonState.Pressed)
                {
                    _startedPressingOn = true;
                }
                else // else check for release
                {
                    if(_startedPressingOn)
                    {
                        OnClick(gameTime);
                    }
                }
            }
            else // set started pressing on to false
            {
                _startedPressingOn = false; 
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(
                _texture,
                _position,
                new Rectangle(0, 0, _texture.Width, _texture.Height),
                Color.White,
                _rotation,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                _scale,
                SpriteEffects.None,
                1f);
        }

        public abstract void OnClick(GameTime gameTime);
        public abstract void OnHover(GameTime gameTime);
    }
}
