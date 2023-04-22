using Dinky_bwb.Map;
using Dinky_bwb.Screens;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinky_bwb.Managers;

namespace Dinky_bwb.Controllers
{
    public enum WalkWobbleAmount
    {
        Normal = 2,
        High = 10,
        Extreme = 20,
        WHY = 50
    }

    public class Player
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        Texture2D _playerTexture;
        Rectangle _hitbox;
        Vector2 _position;
        Vector2 _velocity;
        float _speed;
        bool _facingLeft = false;

        float _movingTimer = 0f;
        float _movingTimerTarget = 0f;
        float _rotationAmount;

        bool _canMove = true;

        WalkWobbleAmount _wobbleAmount = WalkWobbleAmount.Normal;

        public Player(Texture2D playerTexture, Rectangle hitbox, Vector2 position, float speed)
        {
            _playerTexture = playerTexture;
            _hitbox = hitbox;
            _position = position;
            _speed = speed;
        }

        public void Move(Vector2 direction, float speed, GameTime gameTime)
        {
            _position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public static KeyboardState KeyboardGetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }


        void Input()
        {

            KeyboardState key = KeyboardGetState();

            _velocity = Vector2.Zero;

            if (!_canMove) return;
            
            if (key.IsKeyDown(Keys.W))
            {
                _velocity.Y -= 1;
            }
            if (key.IsKeyDown(Keys.S))
            {
                _velocity.Y += 1;
            }
            if (key.IsKeyDown(Keys.A))
            {
                _facingLeft = true;
                _velocity.X -= 1;
            }
            if (key.IsKeyDown(Keys.D))
            {
                _facingLeft = false;
                _velocity.X += 1;
            }

            if (_velocity.Length() > 0) _velocity.Normalize();
        }

        public void Update(GameTime gameTime, GameScreen gameScreen, WorldData map)
        {
            Input();
            UpdatePosition(gameTime, map);
            CheckWarps(map, gameScreen);

            if (_movingTimer < _movingTimerTarget) _movingTimer += gameTime.GetElapsedSeconds() * 12;
            else if (_velocity.Length() > 0) _movingTimerTarget += 3.14f;
            else
            {
                _movingTimerTarget = 0; 
                _movingTimer = 0;
            }

            _rotationAmount = MathF.Sin(_movingTimer) * (_facingLeft ? -1 : 1) * ((int)_wobbleAmount / 10f);

            KeyboardState key = Keyboard.GetState();
            if (IsKeyPressed(Keys.E, true))
            {
                map.TryForInteraction(_position);
            }

            
        }

        void UpdatePosition(GameTime gameTime, WorldData map)
        {
            // get where the player will be next frame
            Vector2 nextPosition = _position + _velocity * _speed * gameTime.GetElapsedSeconds();
            Vector2 nextPositionX = new Vector2(nextPosition.X, _position.Y);
            Vector2 nextPositionY = new Vector2(_position.X, nextPosition.Y);

            // check for walls
            if (CheckWall(nextPositionX, map)) nextPosition.X = _position.X;
            if (CheckWall(nextPositionY, map)) nextPosition.Y = _position.Y;

            // update position
            _position = nextPosition;

            // update hitbox location
            _hitbox.Location = (_position - _hitbox.Size.ToVector2() / 2f).ToPoint();
        }

        void CheckWarps(WorldData map, GameScreen gameScreen)
        {
            TiledMapObjectLayer objectLayer = map.TiledMap.GetLayer<TiledMapObjectLayer>("o_warp_trigger");

            for (int i = 0; i < objectLayer.Objects.Length; i++)
            {
                if (!new Rectangle(
                    (int)objectLayer.Objects[i].Position.X,
                    (int)objectLayer.Objects[i].Position.Y,
                    (int)objectLayer.Objects[i].Size.Width,
                    (int)objectLayer.Objects[i].Size.Height).Contains(_position)) continue ;

                void warp()
                {
                    WarpTo(map, objectLayer.Objects[i].Name);
                    _canMove = true;
                }

                if(gameScreen.CauseTransition(0.2f, warp)) _canMove = false;
                break;
            }
        }

        void WarpTo(WorldData map, string name)
        {
            TiledMapObjectLayer objectLayer = map.TiledMap.GetLayer<TiledMapObjectLayer>("o_warp_target");

            for (int i = 0; i < objectLayer.Objects.Length; i++)
            {
                if (objectLayer.Objects[i].Name != name) continue;

                _position = objectLayer.Objects[i].Position;
                
                break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float time) 
        {
            
            spriteBatch.Draw(
                _playerTexture,
                new Vector2(256, 256),
                new Rectangle(0, 0, _playerTexture.Width, _playerTexture.Height),
                Color.White,
                _rotationAmount,
                new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
                1,
                (_facingLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0.5f);

            //spriteBatch.DrawRectangle(_hitbox.Location.ToVector2() - _position + new Vector2(256, 256), _hitbox.Size.ToVector2(), Color.Green);
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

        public void SetWorldPos(Vector2 position)
        {
            _position = position * 64;
        }

        public static bool IsKeyPressed(Keys key, bool oneShot)
        {
            if (!oneShot) return currentKeyState.IsKeyDown(key);
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
    }
}
