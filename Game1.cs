using Dinky_bwb.Controllers;
using Dinky_bwb.Managers;
using Dinky_bwb.Map;
using Dinky_bwb.Screens;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;



/*

    Dinky: but where Bubbo? ( DBWB )


    This is a WIP project, a lot of things are incomplete and undocumented.
    The characters, Bubbo(yellow, not in game yet), Dinky(purple), Wawa(beige), and Mark(white) are made and owned by Jake Barton
    All other art and code were made by me.

    The Content > Demos folder has a couple snipets of in-development gifs and videos
    
    
    If MonoGame.Extended or the Content Pipeline is throwing errors, make sure the Content Pipeline has reference to the \Packages\ libraries:

    /reference:..\Packages\MonoGame.Extended.Content.Pipeline.dll
    /reference:..\Packages\MonoGame.Extended.dll
    /reference:..\Packages\MonoGame.Extended.Tiled.dll

*/



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
            DialogueManager.Load(); // load dialogue manager stuff
            SoundManager.Load(); // load soundmanager stuff

            ScreenManager.Init(
                    new Screen[]
                    {
                        new SplashScreen("splash", Content.Load<Texture2D>("Textures/UI/splash_gamebyargore")), // splash screen
                        new MainMenuScreen( // main menu
                            "main_menu",
                            GetTexture("Textures/UI/title_dinky"),
                            GetTexture("Textures/UI/title_text_dinky"),
                            GetTexture("Textures/UI/title_text_butwhere"),
                            GetTexture("Textures/UI/title_text_bubbo"),
                            GetTexture("Textures/UI/play"),
                            GetTexture("Textures/UI/exit")
                            ),
                        new GameScreen( // in-game 
                            "game", 
                            new Player(
                                GetTexture("Textures/Entities/player_dinky"),
                                new Rectangle(0,0,49,40),
                                new Vector2(5*64, 5*64),
                                192
                                ),
                            new WorldData(Content.Load<TiledMap>("Maps/map"), this))
                    }.ToList()
                );

            ScreenManager.SetNextScreen("splash"); // start on splash screen
        }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.SwitchToNextScreen();
            
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
        }

        public Texture2D GetTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }

    }
}