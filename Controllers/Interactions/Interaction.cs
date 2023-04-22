using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinky_bwb.Controllers.Interactions
{
    public class Interaction
    {
        protected Rectangle _hitbox;
        protected Entity _targetEntity;
        protected bool _canInteract = true;

        public Interaction(Rectangle hitbox, Entity targetEntity)
        {
            _hitbox = hitbox;
            _targetEntity = targetEntity;
        }

        public bool TryInteract(Vector2 position)
        {
            if (!_canInteract) return false;

            if (_hitbox.Contains(position))
            {
                Interact();
                //DisableInteraction();
                return true;
            }
            return false;
        }

        public bool TryInteract(Rectangle rect)
        {
            if (!_canInteract) return false;

            if (_hitbox.Intersects(rect))
            {
                Interact();
                //DisableInteraction();
                return true;
            }
            return false;
        }

        public void DisableInteraction() { _canInteract = false; }
        public void EnableInteraction() { _canInteract = true; }

        protected virtual void Interact() 
        { 
            
        }
    }
}
