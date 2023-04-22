using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers
{
    public class NPC : Entity
    {
        public NPC(string name, Texture2D texture, Vector2 position, Vector2 velocity, float rotation) : base(name, texture, position, velocity, rotation)
        {

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);
        }
    }
}
