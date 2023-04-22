using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Managers
{
    public static class SoundManager
    {
        static SoundEffect _speak;
        static SoundEffect _door;

        static Random rng = new Random();

        public static void Load()
        {
            _speak = Program.Game.Content.Load<SoundEffect>("Audio/speak");
            _door = Program.Game.Content.Load<SoundEffect>("Audio/door");

            SoundEffect.MasterVolume = 0.1f;
        }

        public static void DialogueSound()
        {
            _speak.Play(1f, (float)(rng.NextDouble() * 0.2 - 0.1), 0);
        }

        public static void DoorSound()
        {
            _door.Play();
        }
    }
}
