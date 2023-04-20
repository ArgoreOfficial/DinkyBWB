using Dinky_bwb.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Managers
{
    public static class ScreenManager
    {
        private static List<Screen> _screens;
        
        private static Screen _activeScreen;
        private static Screen _nextScreen;

        public static void Init(List<Screen> screens)
        {
            _screens = screens;
        }

        public static void Update(GameTime gameTime)
        {
            _activeScreen?.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _activeScreen?.Draw(spriteBatch);

            spriteBatch.End();
        }

        public static bool SetScreen(string screenName)
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                if (_screens[i].GetName() == screenName)
                {
                    _nextScreen = _screens[i];
                    return true;
                }
            }

            return false;
        }

        public static void SwitchToNextScreen()
        {
            if (_nextScreen != null)
            {
                _activeScreen = _nextScreen;
                _activeScreen.ResetTimer();
            }
            _nextScreen = null;
        }
    }
}
