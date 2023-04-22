using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers
{
    public class Entity
    {
        private string _name;
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Vector2 _velocity;
        protected float _rotation;

        public string Name { get => _name; protected set => _name = value; }

        public Entity(string name, Texture2D texture, Vector2 position, Vector2 velocity, float rotation)
        {
            _name = name;
            _texture = texture;
            _position = position;
            _velocity = velocity;
            _rotation = rotation;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public virtual void Update(GameTime gameTime)
        {
            _position += _velocity * gameTime.GetElapsedSeconds();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(
                _texture,
                _position - playerPosition + new Vector2(256, 256),
                new Rectangle(0, 0, _texture.Width, _texture.Height),
                Color.White,
                0f,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                1,
                SpriteEffects.None,
                0.5f);

        }
    }
}
