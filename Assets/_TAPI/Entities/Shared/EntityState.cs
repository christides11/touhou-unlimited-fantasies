using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities
{
    public class EntityState : CAF.Entities.EntityState
    {
        public virtual EntityController controller { get; set; }
        protected EntityCombatManager CombatManager { get { return controller.CombatManager; } }
        protected EntityStateManager StateManager { get { return controller.StateManager; } }
        protected EntityInputManager InputManager { get { return controller.InputManager; } }
        protected EntityPhysicsManager PhysicsManager { get { return controller.PhysicsManager; } }

        public override void Initialize()
        {
            controller.StateManager.SetFrame(1);
        }
    }
}
