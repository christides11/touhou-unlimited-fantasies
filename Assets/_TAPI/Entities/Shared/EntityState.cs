using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities
{
    public class EntityState : BaseState
    {
        public virtual EntityController controller { get; set; }
        protected EntityCombatManager CombatManager { get { return controller.CombatManager; } }
        protected EntityStateManager StateManager { get { return controller.StateManager; } }
        protected EntityInput InputManager { get { return controller.InputManager; } }
        protected EntityPhysicsManager PhysicsManager { get { return controller.PhysicsManager; } }

        public override void OnStart()
        {
            controller.StateManager.SetFrame(1);
        }
    }
}
