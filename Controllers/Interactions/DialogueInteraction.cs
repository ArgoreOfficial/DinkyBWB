using Dinky_bwb.Managers;
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
    public class DialogueInteraction : Interaction
    {
        bool _inDialogue = false;
        List<string> _dialogue;
        int _currentDialogue;
        
        public DialogueInteraction(Rectangle hitbox, Entity targetEntity, string dialoguePath) : base(hitbox, targetEntity)
        {
            _dialogue = File.ReadAllLines(dialoguePath).ToList();
        }

        protected override void Interact()
        {
            DialogueManager.StopDialogue();

            if (_currentDialogue >= _dialogue.Count)
            {
                _currentDialogue = 0;
            }
            else
            {
                if(!_inDialogue)
                {
                    DialogueManager.StartDialogue(_dialogue[_currentDialogue]);
                    _currentDialogue++;
                }
            }

            _inDialogue = !_inDialogue;
            
            base.Interact();
        }
    }
}
