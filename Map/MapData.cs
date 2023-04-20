using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Map
{
    public class MapData
    {
        int[,] _tileData;
        int _tileSize = 64;

        Texture2D[] _textures;

        public MapData(Texture2D[] textures)
        {
            _textures = textures;

            _tileData = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 2, 2, 0, 0, 0, 0 },
                { 0, 0, 2, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (_textures == null) return;

            for (int y = 0; y < _tileData.GetLength(0); y++)
            {
                for (int x = 0; x < _tileData.GetLength(1); x++)
                {
                    spriteBatch.Draw(_textures[_tileData[y, x]], new Vector2(x * _tileSize, y * _tileSize) - offset, Color.White);
                }
            }
        }

        public int GetTile(Point position)
        {
            if (position.X < 0 || position.X >= _tileData.GetLength(1) ||
                position.Y < 0 || position.Y >= _tileData.GetLength(0)) return -1;

            return _tileData[position.Y, position.X];
        }

        public int GetTileFromScreen(Point screenPosition)
        {
            return GetTile((screenPosition.ToVector2() / _tileSize).ToPoint());
        }
    }
}
