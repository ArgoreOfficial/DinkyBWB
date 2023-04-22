using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Managers
{
    static class DialogueManager
    {
        static Texture2D _font;
        
        static int _fontSize = 32;
        static int _fontSpacing = 16;

        static string _alphabet = "abcdefghijklmnopqrstuvwxyz!?., ";

        // current dialogue stuff
        static bool _drawDialogue = false;
        static string _dialogueText;
        static int _rowCount = 0;
        static int _lettersToDraw = 0;
        static int _totalLetterCount;

        static float _timeSinceLastLetter = 0;
        static float _letterDelay = 0.03f;
        static float _totalTime;

        public static void Load()
        {
            _font = Program.Game.GetTexture("Textures/UI/font");
        }

        public static void Update(GameTime gameTime)
        {
            if (!_drawDialogue) return;

            _timeSinceLastLetter -= gameTime.GetElapsedSeconds();
            if(_timeSinceLastLetter < 0 && _lettersToDraw < _totalLetterCount)
            {
                _timeSinceLastLetter = _letterDelay;
                _lettersToDraw++;
                SoundManager.DialogueSound();
            }

            _totalTime += gameTime.GetElapsedSeconds();
        }

        public static void Draw(SpriteBatch spriteBatch) 
        {
            if (!_drawDialogue) return;

            DrawDialogue(spriteBatch, _dialogueText);
        }

        public static int DrawText(SpriteBatch spriteBatch, string text, Vector2 position, int width, int skipRows = 0)
        {
            string[] words = text.Split(' ');
            int letterCount = 0;

            int row = 0;
            Point currentPoint = new Point(0, -skipRows * _fontSize);

            for (int i = 0; i < words.Length; i++)
            {
                int wordLength = words[i].Length * _fontSpacing;
                if (currentPoint.X + wordLength > width)
                {
                    currentPoint.X = 0;
                    currentPoint.Y += _fontSize;
                    row++;
                }

                if (row >= skipRows)
                {
                    for (int l = 0; l < words[i].Length; l++)
                    {
                        char c = words[i][l];
                        int index = _alphabet.IndexOf(c.ToString().ToLower());
                        if (index < 0) continue;

                        bool isLower = Char.IsLower(c);
                        Point descender = "gjqpy".Contains(c) ? new Point(0, 8) : new Point(0, 0);

                        // get position and source position for letter
                        Point pos = position.ToPoint() + currentPoint + descender;
                        Point source = new Point(
                            (index % 16) * _fontSize,
                            (index / 16) * _fontSize + (isLower ? 64 : 0));

                        spriteBatch.Draw(
                            _font,
                            new Rectangle(pos, new Point(_fontSize)),
                            new Rectangle(source, new Point(_fontSize)),
                            Color.White);

                        currentPoint.X += _fontSpacing;
                        letterCount++;

                        if (letterCount >= _lettersToDraw) return row - skipRows + 1;
                    }
                }
                else
                {
                    currentPoint.X += wordLength;
                }

                currentPoint.X += _fontSpacing;
            }

            return row - skipRows + 1;
        }

        public static void DrawDialogue(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.FillRectangle(new Rectangle(new Point(0, 360), new Point(512, 512)), Color.White);
            DrawText(spriteBatch, text, new Vector2(0, 360), 512, 0);
        }

        public static void StartDialogue(string text)
        {
            _dialogueText = text;
            _drawDialogue = true;
            _lettersToDraw = 0;
            _totalLetterCount = text.Replace(" ", "").Length;
        }

        public static void StopDialogue()
        {
            _drawDialogue = false;
        }
    }
}
