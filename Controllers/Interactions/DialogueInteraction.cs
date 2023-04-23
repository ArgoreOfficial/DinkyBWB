using Dinky_bwb.Managers;
using Dinky_bwb.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers.Interactions
{
    public enum DialogueType
    {
        Random,
        Sequential
    }

    public class DialogueInteraction : Interaction
    {
        bool _inDialogue = false;
        List<string> _dialogue;
        int _currentDialogue;
        DialogueType _dialogueType = DialogueType.Sequential;
        public DialogueInteraction(Rectangle hitbox, Entity targetEntity, string dialoguePath) : base(hitbox, targetEntity)
        {
            _dialogue = File.ReadAllLines(dialoguePath).ToList();
        }

        protected override void Interact()
        {
            ScreenManager.Unpause();
            DialogueManager.StopDialogue();

            if(_dialogueType == DialogueType.Sequential)
            {
                if (_currentDialogue >= _dialogue.Count)
                {
                    _currentDialogue = 0;
                }
                else
                {
                    DialogueManager.StartDialogue(_dialogue[_currentDialogue]);
                    ScreenManager.Pause();
                    _currentDialogue++;
                }
            }

            _inDialogue = !_inDialogue;
            
            base.Interact();
        }
    }
}
