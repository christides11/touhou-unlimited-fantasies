using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;
using TAPI.Core;

namespace TAPI.Entities
{
    public class EntityCombatManager : MonoBehaviour, IHurtable
    {
        public MovesetAttackNode CurrentAttack { get { return currentAttack; } }
        public bool WasFloating { get; set; } = false;
        public HitInfo LastHitBy { get; protected set; }
        public EntityTeams Team { get { return team; } }

        [SerializeField] public EntityController controller;
        [SerializeField] public MovesetAttackNode currentAttack;
        [SerializeField] protected MovesetDefinition moveset;
        public EntityHitboxManager hitboxManager;
        [SerializeField] public int hitStun;
        [SerializeField] public int hitStop;

        [SerializeField] protected EntityTeams team;

        public List<int> chargeTimes = new List<int>();


        private void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, controller);
        }

        public virtual void CLateUpdate()
        {
            if(hitStop > 0)
            {
                hitStop--;
            }else if(hitStun > 0)
            {
                hitStun--;
            }

            hitboxManager.TickBoxes();
        }

        public void Reset(bool resetAttack = true)
        {
            if(currentAttack == null)
            {
                return;
            }
            hitboxManager.Reset();
            if (resetAttack)
            {
                currentAttack = null;
            }
        }

        public bool CheckForAction(bool ignoreCurrentAttack = false)
        {
            if (CheckForSpecial(ignoreCurrentAttack))
            {
                return true;
            }
            if (CheckForAttack(ignoreCurrentAttack))
            {
                chargeTimes.Clear();
                for(int i = 0; i < currentAttack.action.chargeFrames.Count; i++)
                {
                    chargeTimes.Add(0);
                }
                return true;
            }
            return false;
        }

        private bool CheckForSpecial(bool ignoreCurrentAttack = false)
        {
            if(currentAttack == null)
            {
                if (controller.IsFloating)
                {

                }
                else
                {
                    switch (controller.IsGrounded) {
                        case true:
                            if(CheckAttackNodes(ref moveset.groundSpecialStartNodes))
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
            else
            {

            }
            return false;
        }

        private bool CheckForAttack(bool ignoreCurrentAttack = false)
        {
            // If we're not in any attack.
            if(currentAttack == null || ignoreCurrentAttack)
            {
                WasFloating = false;
                if (controller.IsFloating)
                {
                    if (CheckFloatingAttackNodes())
                    {
                        return true;
                    }
                }
                else
                {
                    switch (controller.IsGrounded)
                    {
                        case true:
                            if (CheckGroundAttackNodes())
                            {
                                return true;
                            }
                            break;
                        case false:
                            if (CheckAirAttackNodes())
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (controller.IsGrounded)
                {
                    if (CheckAttackNodes(ref moveset.groundAttackCommandNormals))
                    {
                        return true;
                    }
                }
                else
                {
                    if (CheckAttackNodes(ref moveset.airAttackCommandNormals))
                    {
                        return true;
                    }
                }

                for(int i = 0; i < currentAttack.cancelWindows.Count; i++)
                {
                    if (controller.StateManager.CurrentStateFrame >= currentAttack.cancelWindows[i].x &&
                        controller.StateManager.CurrentStateFrame <= currentAttack.cancelWindows[i].y)
                    {
                        if (CheckAttackNode(currentAttack.nextNode[i]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected virtual bool CheckGroundAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.groundAttackCommandNormals))
            {
                return true;
            }
            if (CheckAttackNodes(ref moveset.groundAttackStartNodes))
            {
                return true;
            }
            return false;
        }

        protected virtual bool CheckFloatingAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.floatingAttackStartNodes))
            {
                WasFloating = true;
                return true;
            }
            return false;
        }

        protected virtual bool CheckAirAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.airAttackCommandNormals))
            {
                return true;
            }
            if(CheckAttackNodes(ref moveset.airAttackStartNodes))
            {
                return true;
            }
            return false;
        }

        protected virtual bool CheckAttackNodes(ref List<MovesetAttackNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                MovesetAttackNode node = nodes[i];

                if (CheckAttackNode(node))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckAttackNode(MovesetAttackNode node)
        {
            if (node.action == null)
            {
                return false;
            }

            // Check execute button(s)
            bool pressedExecuteButtons = true;
            for (int e = 0; e < node.executeButton.Count; e++)
            {
                if (node.executeButton[e].button == EntityInputs.Movement)
                {
                    if (!CheckStickDirection(node.executeButton[e].stickDir, node.executeButton[e].dirDeviation, 0))
                    {
                        pressedExecuteButtons = false;
                        break;
                    }
                }
                else
                {
                    if (!controller.InputManager.GetButton(node.executeButton[e].button, 0, true, 3).firstPress)
                    {
                        pressedExecuteButtons = false;
                        break;
                    }
                }
            }

            if (node.executeButton.Count <= 0)
            {
                pressedExecuteButtons = false;
            }
            // We did not press the buttons required for this move.
            if (!pressedExecuteButtons)
            {
                return false;
            }

            bool pressedSequenceButtons = true;
            int lastFrame = 0;
            for (int s = 0; s < node.buttonSequence.Count; s++)
            {
                if (node.buttonSequence[s].button == EntityInputs.Movement)
                {
                    bool foundDir = false;
                    for (int f = lastFrame; f < lastFrame + 8; f++)
                    {
                        if (CheckStickDirection(node.buttonSequence[s].stickDir, node.buttonSequence[s].dirDeviation, f))
                        {
                            foundDir = true;
                            lastFrame = f;
                            break;
                        }
                    }
                    if (!foundDir)
                    {
                        pressedSequenceButtons = false;
                        break;
                    }
                }
            }

            if (!pressedSequenceButtons)
            {
                return false;
            }

            Reset();
            currentAttack = node;
            return true;
        }

        bool CheckStickDirection(Vector2 wantedDirection, float deviation, int framesBack)
        {
            Vector2 stickDir = controller.InputManager.GetMovement(framesBack);
            if (stickDir.magnitude < InputConstants.movementMagnitude)
            {
                return false;
            }

            Vector3 wantedDir = controller.GetVisualBasedDirection(new Vector3(wantedDirection.x, 0, wantedDirection.y)).normalized;
            Vector3 currentDirection = controller.GetMovementVector(stickDir.x, stickDir.y).normalized;

            if(Vector3.Dot(wantedDir, currentDirection) >= deviation)
            {
                return true;
            }
            return false;
        }

        public virtual HurtReactions Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfo hitInfo)
        {
            if(hitInfo.groundOnly && !controller.IsGrounded
                || hitInfo.airOnly && controller.IsGrounded)
            {
                return HurtReactions.Avoided;
            }
            LastHitBy = hitInfo;
            hitStop = hitInfo.hitstop;
            hitStun = hitInfo.hitstun;

            // Convert forces the attacker-based forward direction.
            switch (hitInfo.forceType)
            {
                case HitForceType.SET:
                    Vector3 baseForce = hitInfo.opponentForceDir * hitInfo.opponentForceMagnitude;
                    Vector3 forces = (forward * baseForce.z + right * baseForce.x);
                    forces.y = baseForce.y;
                    controller.PhysicsManager.forceGravity.y = baseForce.y;
                    forces.y = 0;
                    controller.PhysicsManager.forceMovement = forces;
                    break;
                case HitForceType.PULL:
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
                        controller.PhysicsManager.forceGravity.y = yForce;
                    }
                    controller.PhysicsManager.forceMovement = forceDir;
                    break;
            }

            if (controller.PhysicsManager.forceGravity.y > 0)
            {
                controller.IsGrounded = false;
            }

            // Change state to the correct one.
            if (controller.IsGrounded && hitInfo.groundBounces)
            {
                controller.StateManager.ChangeState((int)EntityStates.GROUND_BOUNCE);
                return HurtReactions.Hit;
            }

            if (hitInfo.causesTumble)
            {
                controller.StateManager.ChangeState((int)EntityStates.TUMBLE);
                return HurtReactions.Hit;
            }
            else
            {
                controller.StateManager.ChangeState((int)(controller.IsGrounded ? EntityStates.FLINCH : EntityStates.FLINCH_AIR));
                return HurtReactions.Hit;
            }
        }

        public virtual void Heal()
        {

        }
    }
}