using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityAttack : EntityState
    {

        public override void OnStart()
        {
            AttackSO currentAttack = CombatManager.CurrentAttack.action;
            if(currentAttack.stateOverride > -1)
            {
                StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
            controller.ForcesManager.ApplyGravity = false;
            if (currentAttack)
            {
                if (currentAttack.modifiesInertia)
                {
                    ForcesManager.forceInertia *= currentAttack.inertiaModifer;
                }
            }
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            AttackSO currentAttack = CombatManager.CurrentAttack.action;

            for(int i = 0; i < currentAttack.hitboxGroups.Count; i++)
            {
                HandleHitboxGroup(i, currentAttack.hitboxGroups[i]);
            }

            for(int i = 0; i < currentAttack.bulletGroups.Count; i++)
            {
                HandleBulletGroup(i, currentAttack.bulletGroups[i]);
            }

            if (CheckDashCancelWindows(currentAttack))
            {
                CombatManager.Reset();
                return;
            }
            if (CheckJumpCancelWindows(currentAttack))
            {
                CombatManager.Reset();
                return;
            }
            if (CheckLandCancelWindows(currentAttack))
            {
                CombatManager.Reset();
                return;
            }

            for (int i = 0; i < currentAttack.events.Count; i++)
            {
                HandleEvents(currentAttack.events[i]);
            }

            // Handle charging attacks.
            bool charging = false;
            if (InputManager.GetButton(CombatManager.CurrentAttack.executeButton[0].button).isDown)
            {
                for (int i = 0; i < currentAttack.chargeFrames.Count; i++)
                {
                    if (StateManager.CurrentStateFrame == currentAttack.chargeFrames[i] &&
                        (CombatManager.chargeTimes[i] < currentAttack.chargeLength || currentAttack.chargeLength == -1))
                    {
                        CombatManager.chargeTimes[i] += 1;
                        charging = true;
                    }
                }
            }
            if (charging)
            {
                return;
            }
            controller.StateManager.IncrementFrame();
        }

        public override void OnInterrupted()
        {
            AttackSO currentAttack = CombatManager.CurrentAttack.action;
            if (currentAttack)
            {
                if (currentAttack.carriesInertia)
                {
                    ForcesManager.forceInertia += (ForcesManager.forceMovement + ForcesManager.forceGravity)
                        * currentAttack.carriedInertia;
                }
            }
            ForcesManager.forceMovement = Vector3.zero;
            controller.ForcesManager.ApplyGravity = true;
        }

        private void HandleBulletGroup(int index, BulletGroup bulletGroup)
        {
            for (int b = 0; b < bulletGroup.spawns.Count; b++)
            {
                if (controller.StateManager.CurrentStateFrame == bulletGroup.spawns[b].frame)
                {
                    Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * bulletGroup.spawns[b].offset.z
                        + controller.GetVisualBasedDirection(Vector3.right) * bulletGroup.spawns[b].offset.x
                        + controller.GetVisualBasedDirection(Vector3.up) * bulletGroup.spawns[b].offset.y;
                    GameObject bullet = GameObject.Instantiate(bulletGroup.bullet.gameObject,
                        pos,
                        Quaternion.Euler(controller.transform.eulerAngles + bulletGroup.spawns[b].rotation));
                }
            }
        }

        private void HandleHitboxGroup(int index, HitboxGroup hitboxGroup)
        {
            if(controller.StateManager.CurrentStateFrame == hitboxGroup.activeFramesEnd + 1)
            {
                CombatManager.CleanupHitboxes(index);
            }

            if(controller.StateManager.CurrentStateFrame < hitboxGroup.activeFramesStart
                || controller.StateManager.CurrentStateFrame > hitboxGroup.activeFramesEnd)
            {
                return;
            }

            CombatManager.CreateHitboxes(index);

            if (hitboxGroup.hitInfo.continuousHit)
            {
                if ((controller.StateManager.CurrentStateFrame-hitboxGroup.activeFramesStart)%hitboxGroup.hitInfo.spaceBetweenHits
                    == 0
                    && CombatManager.hitStop == 0)
                {
                    CombatManager.ResetHitboxes(index);
                }
            }
        }

        protected virtual void HandleEvents(AttackEventDefinition currentEvent)
        {
            if (!currentEvent.active)
            {
                return;
            }
            if(StateManager.CurrentStateFrame >= currentEvent.startFrame 
                && StateManager.CurrentStateFrame <= currentEvent.endFrame)
            {
                currentEvent.attackEvent.Evaluate(StateManager.CurrentStateFrame-currentEvent.startFrame, currentEvent.endFrame-currentEvent.startFrame,
                    this, controller, 
                    currentEvent.variables);
            }
        }

        protected virtual bool CheckLandCancelWindows(AttackSO currentAttack)
        {
            for(int i = 0; i < currentAttack.landCancelFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.landCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.landCancelFrames[i].y)
                {
                    if (controller.LandCancel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool CheckJumpCancelWindows(AttackSO currentAttack)
        {
            for(int i = 0; i < currentAttack.jumpCancelFrames.Count; i++)
            {
                if(StateManager.CurrentStateFrame >= currentAttack.jumpCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.jumpCancelFrames[i].y)
                {
                    if (controller.JumpCancel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool CheckDashCancelWindows(AttackSO currentAttack)
        {
            for (int i = 0; i < currentAttack.landCancelFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.landCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.landCancelFrames[i].y)
                {
                    if (controller.DashCancel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string GetName()
        {
            return $"Attack ({CombatManager.CurrentAttack?.name}).";
        }
    }
}