using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities
{
    public class EntityState : CAF.Entities.EntityState
    {
        public virtual EntityController controller { get; set; }
        protected EntityCombatManager CombatManager { get { return (EntityCombatManager)controller.CombatManager; } }
        protected EntityStateManager StateManager { get { return (EntityStateManager)controller.StateManager; } }
        protected EntityInputManager InputManager { get { return (EntityInputManager)controller.InputManager; } }
        protected EntityPhysicsManager PhysicsManager { get { return (EntityPhysicsManager)controller.PhysicsManager; } }

        public override void Initialize()
        {
            controller.StateManager.SetFrame(1);
        }
    }
}
