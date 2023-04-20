using Dinky_bwb.Map;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers
{
    public class Player
    {
        Texture2D _playerTexture;
        Rectangle _hitbox;
        Rectangle _rayHitbox;
        Vector2 _position;
        Vector2 _velocity;
        float _speed;
        Vector2 _direction;



        int _rayLength = 64;

        public Player(Texture2D playerTexture, Rectangle hitbox, Vector2 position, Vector2 direction, float speed)
        {
            _playerTexture = playerTexture;
            _hitbox = hitbox;
            _position = position;
            _direction = direction;
            _speed = speed;
        }

        public void Move(Vector2 direction, float speed, GameTime gameTime)
        {
            _position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        void Input()
        {
            KeyboardState key = Keyboard.GetState();

            _velocity = Vector2.Zero;

            if (key.IsKeyDown(Keys.W))
            {
                _velocity.Y -= 1;
                _direction = new Vector2(0, -1);
            }
            if (key.IsKeyDown(Keys.S))
            {
                _velocity.Y += 1;
                _direction = new Vector2(0, 1);
            }
            if (key.IsKeyDown(Keys.A))
            {
                _velocity.X -= 1;
                _direction = new Vector2(-1, 0);
            }
            if (key.IsKeyDown(Keys.D))
            {
                _velocity.X += 1;
                _direction = new Vector2(1, 0);
            }
        }

        public void Update(GameTime gameTime, WorldData map)
        {
            Input();

            // get where the player will be next frame
            Vector2 nextPosition = _position + _velocity * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 nextPositionX = new Vector2(nextPosition.X, _position.Y);
            Vector2 nextPositionY = new Vector2(_position.X, nextPosition.Y);
            
            // check for walls
            if (CheckWall(nextPositionX, map)) nextPosition.X = _position.X;
            if (CheckWall(nextPositionY, map)) nextPosition.Y = _position.Y;
            
            // update position
            _position = nextPosition;

            // update hitbox location
            _hitbox.Location = (_position - _hitbox.Size.ToVector2() / 2f).ToPoint();

            Point rayPos = _position.ToPoint();
            if (_direction.X < 0) rayPos += new Point(-_rayLength, 0);
            if (_direction.Y < 0) rayPos += new Point(0, -_rayLength);

            _rayHitbox.Location = rayPos;
            _rayHitbox.Size = new Point((int)MathF.Abs(_direction.X * _rayLength), (int)MathF.Abs(_direction.Y * _rayLength));

        }

        public void Draw(SpriteBatch spriteBatch, float time) 
        {
            
            spriteBatch.Draw(
                _playerTexture,
                new Vector2(256, 256),
                new Rectangle(0, 0, _playerTexture.Width, _playerTexture.Height),
                Color.White,
                0f,
                new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
                1,
                (_direction.X < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                1f);

            spriteBatch.DrawRectangle(_hitbox.Location.ToVector2() - _position + new Vector2(256, 256), _hitbox.Size.ToVector2(), Color.Green);
            spriteBatch.DrawRectangle(new Rectangle((_rayHitbox.Location.ToVector2() - _position + new Vector2(256, 256)).ToPoint(), _rayHitbox.Size), Color.Red);
        }

        bool CheckWall(Vector2 position, WorldData map)
        {
            // get hitbox corners
            Vector2 topRight = new Vector2(position.X + _hitbox.Width / 2, position.Y - _hitbox.Height / 2);
            Vector2 bottomRight = new Vector2(position.X + _hitbox.Width / 2, position.Y + _hitbox.Height / 2);
            Vector2 topLeft = new Vector2(position.X - _hitbox.Width / 2, position.Y - _hitbox.Height / 2);
            Vector2 bottomLeft = new Vector2(position.X - _hitbox.Width / 2, position.Y + _hitbox.Height / 2);

            int wallid = 97;

            // check for collision, 2 is brick. Temporary for now until proper mapdata is done
            if (map.GetTileFromScreen(topRight.ToPoint()).Value.GlobalIdentifier == wallid) return true;
            if (map.GetTileFromScreen(bottomRight.ToPoint()).Value.GlobalIdentifier == wallid) return true;
            if (map.GetTileFromScreen(topLeft.ToPoint()).Value.GlobalIdentifier == wallid) return true;
            if (map.GetTileFromScreen(bottomLeft.ToPoint()).Value.GlobalIdentifier == wallid) return true;

            return false;
        }
    }
}
