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

        private Dictionary<int, List<Hitbox>> attackHitboxes = new Dictionary<int, List<Hitbox>>();
        private Dictionary<int, List<IHurtable>> usedHitboxGroups = new Dictionary<int, List<IHurtable>>();

        [SerializeField] protected EntityController controller;

        [SerializeField] protected MovesetAttackNode currentAttack;
        [SerializeField] protected MovesetDefinition moveset;

        [SerializeField] public int hitStun;
        [SerializeField] public int hitStop;

        [SerializeField] protected EntityTeams team;

        public virtual void CLateUpdate()
        {
            if(hitStop > 0)
            {
                hitStop--;
            }else if(hitStun > 0)
            {
                hitStun--;
            }

            foreach (List<Hitbox> hitboxGroup in attackHitboxes.Values)
            {
                for(int i = 0; i < hitboxGroup.Count; i++)
                {
                    hitboxGroup[i].CheckHits();
                }
            }
        }

        public void Reset(bool resetAttack = true)
        {
            if(currentAttack == null)
            {
                return;
            }
            for(int i = 0; i < currentAttack.action.hitboxGroups.Count; i++)
            {
                CleanupHitboxes(i);
            }
            if (resetAttack)
            {
                currentAttack = null;
            }
        }

        public bool CheckForAction()
        {
            if (CheckForAttack())
            {
                return true;
            }
            return false;
        }

        private bool CheckForAttack()
        {
            // If we're not in any attack.
            if(currentAttack == null)
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
                    if (CheckAttackNodes(ref moveset.groundCommandNormals))
                    {
                        return true;
                    }
                }
                else
                {
                    if (CheckAttackNodes(ref moveset.airCommandNormals))
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

        public void ResetHitboxes(int index)
        {
            if (!attackHitboxes.ContainsKey(index))
            {
                return;
            }

            for(int i = 0; i < attackHitboxes[index].Count; i++)
            {
                attackHitboxes[index][i].ReActivate(new List<IHurtable>() { this });
            }

        }

        public void CreateHitboxes(int index)
        {
            if (attackHitboxes.ContainsKey(index))
            {
                return;
            }

            HitboxGroup currentGroup = currentAttack.action.hitboxGroups[index];

            List<Hitbox> hitboxes = new List<Hitbox>(currentGroup.hitboxes.Count);

            for(int i = 0; i < currentGroup.hitboxes.Count; i++)
            {
                HitboxDefinition currHitbox = currentGroup.hitboxes[i];
                Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * currHitbox.offset.z
                    + controller.GetVisualBasedDirection(Vector3.right) * currHitbox.offset.x
                    + controller.GetVisualBasedDirection(Vector3.up) * currHitbox.offset.y;
                GameObject hbox = Instantiate(controller.GameManager.gameVars.combat.hitbox, 
                    controller.transform.position + pos,
                    Quaternion.Euler(controller.transform.eulerAngles + currHitbox.rotation));

                switch (currentGroup.hitboxes[i].shape) {
                    case ShapeType.Rectangle:
                        hbox.GetComponent<Hitbox>().InitRectangle(currentGroup.hitboxes[i].size,
                            controller.visual.transform.eulerAngles + currentGroup.hitboxes[i].rotation);
                        break;
                }

                if (currentGroup.attachToEntity)
                {
                    hbox.transform.SetParent(transform);
                }
                hbox.GetComponent<Hitbox>().OnHurt += (HitInfo hinfo) => { hitStop = hinfo.attackerHitstop; };
                hbox.GetComponent<Hitbox>().Activate(gameObject, controller.visualTransform, 
                    currentGroup.hitInfo, new List<IHurtable>() { this });
                hitboxes.Add(hbox.GetComponent<Hitbox>());
            }

            attackHitboxes.Add(index, hitboxes);
        }

        public virtual void CleanupHitboxes(int index)
        {
            if (!attackHitboxes.ContainsKey(index))
            {
                return;
            }

            for(int i = 0; i < attackHitboxes[index].Count; i++)
            {
                Destroy(attackHitboxes[index][i].gameObject);
            }

            attackHitboxes.Remove(index);
        }

        protected virtual bool CheckGroundAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.groundCommandNormals))
            {
                return true;
            }
            if (CheckAttackNodes(ref moveset.groundStartNodes))
            {
                return true;
            }
            return false;
        }

        protected virtual bool CheckFloatingAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.floatingStartNodes))
            {
                WasFloating = true;
                return true;
            }
            return false;
        }

        protected virtual bool CheckAirAttackNodes()
        {
            if(CheckAttackNodes(ref moveset.airCommandNormals))
            {
                return true;
            }
            if(CheckAttackNodes(ref moveset.airStartNodes))
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

        public virtual HurtReactions Hurt(Vector3 forward, Vector3 right, HitInfo hitInfo)
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
            controller.ForcesManager.ApplyGravity = false;
            Vector3 baseForce = hitInfo.opponentForceDir * hitInfo.opponentForceMagnitude;
            Vector3 forces = (forward * baseForce.z + right * baseForce.x);
            forces.y = baseForce.y;
            if(forces.y > 0)
            {
                controller.IsGrounded = false;
            }
            controller.ForcesManager.forceGravity.y = baseForce.y;
            forces.y = 0;
            controller.ForcesManager.forceMovement = forces;


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