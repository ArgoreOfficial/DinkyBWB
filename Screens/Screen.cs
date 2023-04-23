using Dinky_bwb.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;

namespace Dinky_bwb.Screens
{
    public abstract class Screen
    {
        protected float _time;
        protected string _name;

        float _transitionTime = 0;
        float _transitionTotalTime = 1f;
        bool _playingTransition = false;
        bool _transitionOut = false;
        Action _transitionCallback;

        protected Screen(string name)
        {
            _name = name;
        }

        public virtual void Update(GameTime gameTime) 
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        public virtual void UpdateRegardless(GameTime gameTime)
        {
            if (_playingTransition)
            {
                if (_transitionTime < _transitionTotalTime) _transitionTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    if (!_transitionOut)
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
        }

        public virtual void Draw(SpriteBatch spriteBatch) 
        {
            if (_transitionOut)
                spriteBatch.FillRectangle(new Rectangle(0, 0 - (int)(512f * (_transitionTime / _transitionTotalTime)), 512, 512), Color.Black);
            else
                spriteBatch.FillRectangle(new Rectangle(0, 512 - (int)(512f * (_transitionTime / _transitionTotalTime)), 512, 512), Color.Black);
        }

        public string GetName()
        {
            return _name;
        }

        public void ResetTimer()
        {
            _time = 0;
        }

        public bool CauseTransition(float totalTime, Action callback = null)
        {
            if (_transitionTime > 0) return false;
            SoundManager.DoorSound();
            _transitionTotalTime = totalTime;
            _playingTransition = true;
            _transitionCallback = callback;

            return true;
        }

    }
}
