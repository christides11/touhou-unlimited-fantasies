﻿using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityAttack : EntityState
    {

        public override void OnStart()
        {
            if (!controller.LockedOn)
            {
                controller.PickSoftlockTarget();
            }
            AttackSO currentAttack = CombatManager.CurrentAttack.action;
            if(currentAttack.stateOverride > -1)
            {
                StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
            if (currentAttack)
            {
                if (currentAttack.modifiesInertia)
                {
                    PhysicsManager.forceInertia *= currentAttack.inertiaModifer;
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

            for(int i = 0; i < currentAttack.faceLockonTargetWindows.Count; i++)
            {
                if(StateManager.CurrentStateFrame >= currentAttack.faceLockonTargetWindows[i].startFrame
                    && StateManager.CurrentStateFrame <= currentAttack.faceLockonTargetWindows[i].endFrame)
                {
                    Vector3 mov = controller.InputManager.GetMovement(0);
                    Vector3 forwardDir = controller.visualTransform.forward;
                    // We're currently locked on to something.
                    if (controller.LockedOn)
                    {
                        forwardDir = controller.LockonForward;
                    }
                    // Movement is neutral, so target the soft lockon target (if it exist).
                    else if (mov.magnitude < InputConstants.movementMagnitude)
                    {
                        if (controller.LockonTarget)
                        {
                            forwardDir = (controller.LockonTarget.transform.position - controller.transform.position);
                            forwardDir.y = 0;
                            forwardDir.Normalize();
                        }
                    }
                    // Movement is pointing in a direction, so face that direction.
                    else
                    {
                        forwardDir = controller.GetMovementVector(mov.x, mov.y);
                    }
                    controller.RotateVisual(forwardDir, currentAttack.faceLockonTargetWindows[i].amount);
                }
            }

            for(int i = 0; i < currentAttack.hitboxGroups.Count; i++)
            {
                HandleHitboxGroup(i, currentAttack.hitboxGroups[i]);
            }

            for(int i = 0; i < currentAttack.bulletGroups.Count; i++)
            {
                HandleBulletGroup(i, currentAttack.bulletGroups[i]);
            }

            if (CheckDashCancelWindows(currentAttack)
                || CheckJumpCancelWindows(currentAttack)
                || CheckLandCancelWindows(currentAttack))
            {
                CombatManager.Reset();
                return;
            }

            if (CheckAttackCancelWindows(currentAttack))
            {
                return;
            }

            bool eventCancel = false;
            for (int i = 0; i < currentAttack.events.Count; i++)
            {
                if (HandleEvents(currentAttack.events[i]))
                {
                    eventCancel = true;
                    return;
                }
            }

            if (!eventCancel)
            {
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
        }

        public override void OnInterrupted()
        {
            AttackSO currentAttack = CombatManager.CurrentAttack.action;
            if (currentAttack)
            {
                PhysicsManager.CurrentGravityScale += currentAttack.gravityScaleAdded;
                if (currentAttack.carriesInertia)
                {
                    PhysicsManager.forceInertia += (PhysicsManager.forceMovement + PhysicsManager.forceGravity)
                        * currentAttack.carriedInertia;
                }
            }
            PhysicsManager.forceMovement = Vector3.zero;
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

        private void HandleHitboxGroup(int group, HitboxGroup hitboxGroup)
        {
            if (controller.StateManager.CurrentStateFrame == hitboxGroup.activeFramesEnd + 1)
            {
                CombatManager.hitboxManager.DeactivateHitboxes(group);
                CombatManager.hitboxManager.DeactivateDetectboxes(group);
            }

            if (controller.StateManager.CurrentStateFrame < hitboxGroup.activeFramesStart
                || controller.StateManager.CurrentStateFrame > hitboxGroup.activeFramesEnd)
            {
                return;
            }

            switch (hitboxGroup.hitGroupType) {
                case HitboxGroupType.HIT:
                    CombatManager.hitboxManager.CreateHitboxes(group);

                    if (hitboxGroup.hitInfo.continuousHit)
                    {
                        if ((controller.StateManager.CurrentStateFrame - hitboxGroup.activeFramesStart) % hitboxGroup.hitInfo.spaceBetweenHits
                            == 0
                            && CombatManager.hitStop == 0)
                        {
                            CombatManager.hitboxManager.ReactivateHitboxes(group);
                        }
                    }
                    break;
                case HitboxGroupType.DETECT:
                    CombatManager.hitboxManager.CreateDetectboxes(group);
                    break;
            }
        }

        protected virtual bool HandleEvents(AttackEventDefinition currentEvent)
        {
            if (!currentEvent.active)
            {
                return false;
            }
            if(StateManager.CurrentStateFrame >= currentEvent.startFrame 
                && StateManager.CurrentStateFrame <= currentEvent.endFrame)
            {
                // If the event needs a detection to happen, check if it happened.
                if (currentEvent.onDetect)
                {
                    List<IHurtable> ihList = CombatManager.hitboxManager.GetDetectedList(currentEvent.onDetectHitboxGroup);
                    if(ihList == null)
                    {
                        return false;
                    }
                    if(ihList.Count == 0)
                    {
                        return false;
                    }
                }
                return currentEvent.attackEvent.Evaluate(StateManager.CurrentStateFrame - currentEvent.startFrame, 
                    currentEvent.endFrame - currentEvent.startFrame,
                    this, 
                    controller,
                    currentEvent.variables);
            }
            return false;
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

        protected virtual bool CheckAttackCancelWindows(AttackSO currentAttack)
        {
            for (int i = 0; i < currentAttack.attackCancelFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.attackCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.attackCancelFrames[i].y)
                {
                    if (CombatManager.CheckForAction(true))
                    {
                        StateManager.ChangeState((int)EntityStates.ATTACK);
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