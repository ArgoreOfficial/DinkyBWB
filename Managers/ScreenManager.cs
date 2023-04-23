using Dinky_bwb.Screens;
using Dinky_bwb.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dinky_bwb.Managers
{
    public static class ScreenManager
    {
        private static List<Screen> _screens;
        
        private static Screen _activeScreen;
        private static Screen _nextScreen;

        private static bool _isPaused = false;

        public static void Init(List<Screen> screens)
        {
            _screens = screens;
        }

        public static void Update(GameTime gameTime)
        {
            _activeScreen?.UpdateRegardless(gameTime);

            if (!_isPaused)
            {
                _activeScreen?.Update(gameTime);
            }

            DialogueManager.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _activeScreen?.Draw(spriteBatch);
            DialogueManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Sets next screen. Change occurs at the start of next frame
        /// </summary>
        /// <param name="screenName"></param>
        /// <returns></returns>
        public static bool SetNextScreen(string screenName)
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

        /// <summary>
        /// cause scene change
        /// </summary>
        public static void SwitchToNextScreen()
        {
            if (_nextScreen != null)
            {
                _activeScreen = _nextScreen;
                _activeScreen.ResetTimer();
            }
            _nextScreen = null;
        }

        public static void Pause()
        {
            _isPaused = true;
        }

        public static void Unpause()
        {
            _isPaused = false;
        }
    }
}
