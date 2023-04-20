using Dinky_bwb.Controllers;
using Dinky_bwb.Managers;
using Dinky_bwb.Map;
using Dinky_bwb.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Dinky_bwb
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 512;
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Init(
                new Screen[]
                {
                    new SplashScreen("splash", Content.Load<Texture2D>("Textures/UI/splash_gamebyargore")),
                    new MainMenuScreen(
                        "main_menu",
                        GetTexture("Textures/UI/title_dinky"),
                        GetTexture("Textures/UI/title_text_dinky"),
                        GetTexture("Textures/UI/title_text_butwhere"),
                        GetTexture("Textures/UI/title_text_bubbo"),
                        GetTexture("Textures/UI/play"),
                        GetTexture("Textures/UI/exit")
                        ),
                    new GameScreen(
                        "game", 
                        new Player(
                            GetTexture("Textures/Entities/player_dinky"),
                            new Rectangle(0,0,40,40),
                            new Vector2(256, 256),
                            new Vector2(1, 0),
                            128
                            ),
                        new MapData(new Texture2D[]
                        {
                            GetTexture("Textures/Tiles/grass"),
                            GetTexture("Textures/Tiles/grass_decorated1"),
                            GetTexture("Textures/Tiles/brick")
                        }))
                }.ToList()
                );

            ScreenManager.SetScreen("game");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ScreenManager.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            ScreenManager.Draw(_spriteBatch);

            base.Draw(gameTime);

            ScreenManager.SwitchToNextScreen();
        }

        public Texture2D GetTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }
    }
}