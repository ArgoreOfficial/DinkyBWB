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
    public abstract class Screen
    {
        protected float _time;
        protected string _name;

        protected Screen(string name)
        {
            _name = name;
        }

        public virtual void Update(GameTime gameTime) 
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public string GetName()
        {
            return _name;
        }

        public void ResetTimer()
        {
            _time = 0;
        }
    }
}
