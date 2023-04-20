using Dinky_bwb.Controllers;
using Dinky_bwb.Map;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Screens
{
    public class GameScreen : Screen
    {
        Player _player;
        MapData _activeMap;

        public GameScreen(string name, Player player, MapData map) : base(name)
        {
            _player = player;
            _activeMap = map;
        }

        public void SetMap(MapData map)
        {
            _activeMap = map;
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime, _activeMap);

            Debug.WriteLine(_activeMap.GetTileFromScreen((_player.GetPosition()).ToPoint()));

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _activeMap?.Draw(spriteBatch, _player.GetPosition() - new Vector2(256, 256));

            _player?.Draw(spriteBatch, _time);
            base.Draw(spriteBatch);
        }

    }
}
