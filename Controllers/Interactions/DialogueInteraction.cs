using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers.Interactions
{
    public class DialogueInteraction : Interaction
    {
        public DialogueInteraction(Rectangle hitbox, Entity targetEntity) : base(hitbox, targetEntity)
        {

        }

        protected override void Interact()
        {
            Debug.WriteLine("I'VE BEEN ROBBED");

            base.Interact();
        }
    }
}
