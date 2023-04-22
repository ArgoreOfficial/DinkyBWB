using Dinky_bwb.Controllers;
using Dinky_bwb.Map;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dinky_bwb.Screens
{
    public class GameScreen : Screen
    {
        Player _player;
        WorldData _activeMap;

        float _transitionTime = 0;
        float _transitionTotalTime = 1f;
        bool _playingTransition = false;
        bool _transitionOut = false;
        Action _transitionCallback;

        public GameScreen(string name, Player player, WorldData map) : base(name)
        {
            _player = player;
            SetMap(map);
        }

        public void SetMap(WorldData map)
        {
            _activeMap = map;

            TiledMapObjectLayer o_static = map.TiledMap.GetLayer<TiledMapObjectLayer>("o_static");

            // set player spawn location
            for (int i = 0; i < o_static.Objects.Length; i++)
            {
                if (o_static.Objects[i].Name != "spawn") continue;

                _player.SetPosition(o_static.Objects[i].Position);
                break;
            }
        }

        public bool CauseTransition(float totalTime, Action callback = null)
        {
            if (_transitionTime > 0) return false;

            _transitionTotalTime = totalTime;
            _playingTransition = true;
            _transitionCallback = callback;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime, this, _activeMap);
            _activeMap.SetPosition(_player.GetPosition());

            if (_playingTransition)
            {
                if(_transitionTime < _transitionTotalTime) _transitionTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    if(!_transitionOut)
                    {
                        _transitionCallback?.Invoke();
                        _transitionOut = true;
                        _transitionTime = 0;
                    }
                    else
                    {
                        _playingTransition = false;
                    }
                }
            }
            else
            {
                _transitionOut = false;
                _transitionTime = 0;
            }

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            void action()
            {
                _player.Draw(spriteBatch, _time);
            };

            _activeMap.Draw(spriteBatch, action, _player.GetPosition());
            //_player.Draw(spriteBatch, _time);

            if(_transitionOut)
                spriteBatch.FillRectangle(new Rectangle(0, 0 - (int)(512f * (_transitionTime / _transitionTotalTime)), 512, 512), Color.Black);
            else
                spriteBatch.FillRectangle(new Rectangle(0, 512 - (int)(512f * (_transitionTime / _transitionTotalTime)), 512, 512), Color.Black);
            
            base.Draw(spriteBatch);
        }

    }
}
