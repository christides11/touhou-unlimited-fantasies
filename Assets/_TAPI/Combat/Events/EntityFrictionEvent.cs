using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityFrictionEvent", menuName = "Attack Events/Friction")]
    public class EntityFrictionEvent : AttackEvent
    {
        public bool yFriction;
        public bool xzFriction;

        public override void Evaluate(uint frame, uint endFrame, 
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            if (xzFriction)
            {
                controller.PhysicsManager.ApplyMovementFriction(variables.floatVars[0]);
            }
            if (yFriction)
            {
                controller.PhysicsManager.ApplyGravityFriction(variables.floatVars[0]);
            }
        }
    }
}