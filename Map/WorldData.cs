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
using Dinky_bwb.Controllers;
using Dinky_bwb.Controllers.Interactions;
using System.Xml.Linq;

namespace Dinky_bwb.Map
{
    public class WorldData
    {
        int _tileSize = 64;

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private List<Entity> _entities = new List<Entity>();
        private List<Interaction> _interactions = new List<Interaction>();

        private OrthographicCamera _camera;

        public TiledMap TiledMap { get => _tiledMap; protected set => _tiledMap = value; }

        public WorldData(TiledMap tiledMap, Game1 game)
        {
            var viewportadapter = new BoxingViewportAdapter(Program.Game.Window, Program.Game.GraphicsDevice, 512, 512);
            _camera = new OrthographicCamera(viewportadapter);

            _tiledMap = tiledMap;
            _tiledMapRenderer = new TiledMapRenderer(Program.Game.GraphicsDevice, _tiledMap);

            LoadEntities(game);
            LoadInteractions();
        }

        void LoadEntities(Game1 game)
        {
            TiledMapObjectLayer o_entities = _tiledMap.GetLayer<TiledMapObjectLayer>("o_entities");

            // set player spawn location
            for (int i = 0; i < o_entities.Objects.Length; i++)
            {
                //if (o_entities.Objects[i].Type != "npc") return;

                // load npc
                _entities.Add(new NPC(
                        o_entities.Objects[i].Name,
                        game.GetTexture("Textures/Entities/npc_" + o_entities.Objects[i].Name),
                        o_entities.Objects[i].Position,
                        Vector2.Zero, 0f
                    ));

            }
        }
        Entity GetEntity(string name)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Name == name) return _entities[i];
            }
            return null;
        }


        void LoadInteractions()
        {
            TiledMapObjectLayer o_entities = _tiledMap.GetLayer<TiledMapObjectLayer>("o_dialogues");

            // set player spawn location
            for (int i = 0; i < o_entities.Objects.Length; i++)
            {
                //if (o_entities.Objects[i].Type != "npc") return;
                Rectangle rect = new Rectangle(
                            (int)o_entities.Objects[i].Position.X,
                            (int)o_entities.Objects[i].Position.Y,
                            (int)o_entities.Objects[i].Size.Width,
                            (int)o_entities.Objects[i].Size.Height);

                _interactions.Add(
                    new DialogueInteraction(
                        rect, 
                        GetEntity(o_entities.Objects[i].Type),
                        "Content/Dialogue/wawa.txt"));
            }
        }

        public void TryForInteraction(Vector2 position)
        {
            for (int i = 0; i < _interactions.Count; i++)
            {
                if (_interactions[i].TryInteract(position)) return;
            }
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public void SetPosition(Vector2 position)
        {
            _camera.LookAt(position.ToPoint().ToVector2());
        }

        public void Draw(SpriteBatch spriteBatch, Action drawPlayerAction, Vector2 playerPosition)
        {
            bool hasDrawnPlayer = false;

            foreach (TiledMapLayer layer in _tiledMap.Layers)
            {
                _tiledMapRenderer.Draw(layer, _camera.GetViewMatrix(), null, null, 0f);

                if (layer.Name == "o_entities")
                {
                    for (int i = 0; i < _entities.Count; i++)
                    {
                        if (!hasDrawnPlayer && _entities[i].GetPosition().Y > playerPosition.Y)
                        {
                            drawPlayerAction.Invoke();
                            hasDrawnPlayer = true;
                        }
                        
                        _entities[i].Draw(spriteBatch, playerPosition.ToPoint().ToVector2());
                    }

                    if(!hasDrawnPlayer)
                    {
                        drawPlayerAction.Invoke();
                    }

                    // reset spritebatch
                    spriteBatch.End();
                    spriteBatch.Begin();
                }
            }

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
