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
            return false;
        }

        /*
        protected override bool CheckStickDirection(Vector2 wantedDirection, float deviation, int framesBack)
        {
            Vector2 stickDir = Controller.InputManager.GetAxis2D((int)EntityInputs.Movement, framesBack);
            if (stickDir.magnitude < InputConstants.movementMagnitude)
            {
                return false;
            }

            Vector3 wantedDir = Controller.GetVisualBasedDirection(new Vector3(wantedDirection.x, 0, wantedDirection.y)).normalized;
            Vector3 currentDirection = Controller.GetMovementVector(stickDir.x, stickDir.y).normalized;

            if(Vector3.Dot(wantedDir, currentDirection) >= deviation)
            {
                return true;
            }
            return false;
        }*/

        public override HitReaction Hurt(HurtInfoBase hurtInfoBase)
        {
            return base.Hurt(hurtInfoBase);
        }

        /*
        public override HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfoBase hitInfoB)
        {
            Debug.Log("Hurt.");
            HitInfo hitInfo = (HitInfo)hitInfoB;
            HitReaction hitReaction = new HitReaction();
            hitReaction.reactionType = HitReactionType.Hit;
            if(hitInfoB.groundOnly && !Controller.IsGrounded
                || hitInfoB.airOnly && Controller.IsGrounded)
            {
                hitReaction.reactionType = HitReactionType.Avoided;
                return hitReaction;
            }
            LastHitBy = hitInfoB;
            SetHitStop(hitInfoB.hitstop);
            SetHitStun(hitInfoB.hitstun);

            // Convert forces the attacker-based forward direction.
            switch (hitInfoB.forceType)
            {
                case HitboxForceType.SET:
                    Vector3 baseForce = hitInfo.opponentForceDir * hitInfo.opponentForceMagnitude;
                    Vector3 forces = (forward * baseForce.z + right * baseForce.x);
                    forces.y = baseForce.y;
                    Controller.PhysicsManager.forceGravity.y = baseForce.y;
                    forces.y = 0;
                    Controller.PhysicsManager.forceMovement = forces;
                    break;
                case HitboxForceType.PULL:
                    Vector3 dir = transform.position - center;
                    if (!hitInfo.forceIncludeYForce)
                    {
                        dir.y = 0;
                    }
                    Vector3 forceDir = Vector3.ClampMagnitude((dir) * hitInfo.opponentForceMagnitude, hitInfo.opponentMaxMagnitude);
                    float yForce = forceDir.y;
                    forceDir.y = 0;
                    if (hitInfo.forceIncludeYForce)
                    {
                        Controller.PhysicsManager.forceGravity.y = yForce;
                    }
                    Controller.PhysicsManager.forceMovement = forceDir;
                    break;
            }

            if (Controller.PhysicsManager.forceGravity.y > 0)
            {
                Controller.IsGrounded = false;
            }

            //manager.HealthManager.Hurt(hitInfo.damageOnHit);

            // Change state to the correct one.
            if (Controller.IsGrounded && hitInfo.groundBounces)
            {
                Controller.StateManager.ChangeState((int)EntityStates.GROUND_BOUNCE);
            } else if (hitInfo.causesTumble)
            {
                Controller.StateManager.ChangeState((int)EntityStates.TUMBLE);
            }
            else
            {
                Controller.StateManager.ChangeState((int)(Controller.IsGrounded ? EntityStates.FLINCH : EntityStates.FLINCH_AIR));
            }
            return hitReaction;
        }*/

        public override void Heal(HealInfoBase healInfo)
        {
            base.Heal(healInfo);
        }

        public void SetTeam(EntityTeams team)
        {
            this.team = team;
        }
    }
}