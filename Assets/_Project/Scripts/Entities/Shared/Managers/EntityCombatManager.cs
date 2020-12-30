using System.Collections.Generic;
using TUF.Combat;
using UnityEngine;
using TUF.Core;
using CAF.Combat;
using MovesetAttackNode = TUF.Combat.MovesetAttackNode;
using MovesetDefinition = TUF.Combat.MovesetDefinition;
using HitInfo = TUF.Combat.HitInfo;
using CAF.Input;

namespace TUF.Entities
{
    public class EntityCombatManager : CAF.Entities.EntityCombatManager
    {
        public EntityManager Controller { get { return (EntityManager)manager; } }
        public bool WasFloating { get; set; } = false;

        [SerializeField] protected MovesetDefinition moveset;
        [SerializeField] protected EntityTeams team;

        protected override void Awake()
        {
            CurrentMoveset = moveset;
            base.Awake();
            hitboxManager = new TUF.Entities.EntityHitboxManager(this, (EntityManager)manager);
        }

        protected override CAF.Combat.MovesetAttackNode CheckStartingNodes()
        {
            if (Controller.IsFloating)
            {

            }
            return base.CheckStartingNodes();
        }

        protected override bool CheckStickDirection(InputDefinition sequenceInput, int framesBack)
        {
            Vector2 stickDir = Controller.InputManager.GetAxis2D((int)EntityInputs.Movement, framesBack);
            if (stickDir.magnitude < InputConstants.movementMagnitude)
            {
                return false;
            }

            Vector3 wantedDir = Controller.GetVisualBasedDirection(new Vector3(sequenceInput.stickDirection.x, 0, sequenceInput.stickDirection.y)).normalized;
            Vector3 currentDirection = Controller.GetMovementVector(stickDir.x, stickDir.y).normalized;

            if (Vector3.Dot(wantedDir, currentDirection) >= sequenceInput.directionDeviation)
            {
                return true;
            }
            return false;
        }

        public virtual MovesetAttackNode TrySpecial()
        {
            switch (manager.IsGrounded)
            {
                case true:
                    MovesetAttackNode groundCommandNormal = (MovesetAttackNode)CheckAttackNodes(ref moveset.groundSpecialStartNodes);
                    if (groundCommandNormal != null)
                    {
                        return groundCommandNormal;
                    }
                    break;
                case false:
                    MovesetAttackNode airCommandNormal = (MovesetAttackNode)CheckAttackNodes(ref moveset.airSpecialStartNodes);
                    if (airCommandNormal != null)
                    {
                        return airCommandNormal;
                    }
                    break;
            }
            return null;
        }

        public override HitReaction Hurt(HurtInfoBase hurtInfoBase)
        {
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)Controller.PhysicsManager;
            HurtInfo3D hurtInfo = (HurtInfo3D)hurtInfoBase;

            HitReaction hitReaction = new HitReaction();
            hitReaction.reactionType = HitReactionType.Hit;

            // Check if should hit grounded/aerial opponent.
            if (hurtInfo.hitInfo.groundOnly && !Controller.IsGrounded
                || hurtInfo.hitInfo.airOnly && Controller.IsGrounded)
            {
                hitReaction.reactionType = HitReactionType.Avoided;
                return hitReaction;
            }

            HitInfo hitInfo = (HitInfo)hurtInfo.hitInfo;

            // Got hit.
            LastHitBy = hurtInfo.hitInfo;
            SetHitStop(hurtInfo.hitInfo.hitstop);
            SetHitStun(hurtInfo.hitInfo.hitstun);
            ((EntityManager)manager).healthManager.Hurt(hitInfo.damageOnHit);

            // Convert forces the attacker-based forward direction.
            switch (hitInfo.forceType)
            {
                case HitboxForceType.SET:
                    Vector3 baseForce = hitInfo.opponentForceDir * hitInfo.opponentForceMagnitude;
                    Vector3 forces = (hurtInfo.forward * baseForce.z + hurtInfo.right * baseForce.x);
                    forces.y = baseForce.y;
                    physicsManager.forceGravity.y = baseForce.y;
                    forces.y = 0;
                    physicsManager.forceMovement = forces;
                    break;
                case HitboxForceType.PULL:
                    Vector3 dir = transform.position - hurtInfo.center;
                    if (!hitInfo.forceIncludeYForce)
                    {
                        dir.y = 0;
                    }
                    Vector3 forceDir = Vector3.ClampMagnitude((dir) * hitInfo.opponentForceMagnitude, hitInfo.opponentMaxMagnitude);
                    float yForce = forceDir.y;
                    forceDir.y = 0;
                    if (hitInfo.forceIncludeYForce)
                    {
                        physicsManager.forceGravity.y = yForce;
                    }
                    physicsManager.forceMovement = forceDir;
                    break;
            }

            if (physicsManager.forceGravity.y > 0)
            {
                Controller.IsGrounded = false;
            }

            // Change state to the correct one.
            if (Controller.IsGrounded && hitInfo.groundBounces)
            {
                Controller.StateManager.ChangeState((int)EntityStates.GROUND_BOUNCE);
            }
            else if (hitInfo.causesTumble)
            {
                Controller.StateManager.ChangeState((int)EntityStates.TUMBLE);
            }
            else
            {
                Controller.StateManager.ChangeState((int)(Controller.IsGrounded ? EntityStates.FLINCH : EntityStates.FLINCH_AIR));
            }

            return hitReaction;
        }

        public override void Heal(HealInfoBase healInfo)
        {
            base.Heal(healInfo);
        }

        public void SetTeam(EntityTeams team)
        {
            this.team = team;
        }

        public override int GetTeam()
        {
            return (int)team;
        }
    }
}