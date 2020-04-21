using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityGravityEvent", menuName = "Attack Events/Gravity")]
    public class EntityApplyGravityEvent : AttackEvent
    {
        public bool useEntityMaxFallSpeed;
        public bool useEntityGravity;
        public bool useEntityGravityScale;

        public override void Evaluate(uint frame, uint endFrame, 
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            float percent = (float)frame / (float)endFrame;

            float gravity = controller.definition.stats.gravity;
            if (!useEntityGravity)
            {
                gravity = variables.curveVars[0].Evaluate(percent)
                    * variables.floatVars[0];
            }
            float gravityScale = controller.PhysicsManager.CurrentGravityScale;
            if (!useEntityGravityScale)
            {
                gravityScale = variables.curveVars[1].Evaluate(percent)
                    * variables.floatVars[1];
            }
            float maxFallSpeed = controller.definition.stats.maxFallSpeed;
            if (!useEntityGravityScale)
            {
                maxFallSpeed = variables.curveVars[2].Evaluate(percent)
                    * variables.floatVars[2];
            }

            controller.PhysicsManager.HandleGravity(maxFallSpeed, gravity, gravityScale);
        }
    }
}