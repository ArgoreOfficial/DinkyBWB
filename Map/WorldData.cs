using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;

namespace Dinky_bwb.Map
{
    public class WorldData
    {
        int _tileSize = 64;

        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        private OrthographicCamera _camera;

        Texture2D[] _textures;

        public WorldData(TiledMap tiledMap)
        {
            var viewportadapter = new BoxingViewportAdapter(Program.Game.Window, Program.Game.GraphicsDevice, 512, 512);
            _camera = new OrthographicCamera(viewportadapter);

            _tiledMap = tiledMap;
            _tiledMapRenderer = new TiledMapRenderer(Program.Game.GraphicsDevice, _tiledMap);
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void SetPosition(Vector2 position)
        {
            _camera.LookAt(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());
        }

        public TiledMapTile? GetTile(Point position)
        {
            TiledMapTileLayer layer = _tiledMap.GetLayer<TiledMapTileLayer>("walls");

            if (position.X < 0 || position.X >= layer.Width ||
                position.Y < 0 || position.Y >= layer.Height) return new TiledMapTile(0, 0, 0);

            TiledMapTile? outTile;

            layer.TryGetTile((ushort)position.X, (ushort)position.Y, out outTile);

            return outTile;
        }

        public TiledMapTile? GetTileFromScreen(Point screenPosition)
        {
            return GetTile((screenPosition.ToVector2() / _tileSize).ToPoint());
        }
    }
}
