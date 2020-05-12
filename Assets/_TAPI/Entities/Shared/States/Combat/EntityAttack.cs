using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using TAPI.Combat.Bullets;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityAttack : EntityState
    {
        public override string GetName()
        {
            return $"Attack ({CombatManager.CurrentAttack?.name}).";
        }

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
                HandleBoxGroup(i, currentAttack.hitboxGroups[i]);
            }

            
            for(int i = 0; i < currentAttack.bulletGroups.Count; i++)
            {
                HandleBulletGroup(i, currentAttack.bulletGroups[i]);
            }

            if (CheckCancelWindows(currentAttack))
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
                if (currentAttack.chargeFrames.Count > 0)
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
                }
                controller.StateManager.IncrementFrame();
            }
        }

        private void HandleBulletGroup(int index, BulletPatternGroup bulletPatternGroup)
        {
            if (controller.StateManager.CurrentStateFrame == bulletPatternGroup.spawnFrame)
            {
                GameObject patternManager = new GameObject();
                patternManager.transform.position = controller.transform.position;
                patternManager.transform.rotation = controller.visual.transform.rotation;
                BulletPatternManager bpm = patternManager.AddComponent<BulletPatternManager>();
                bpm.Initialize(bulletPatternGroup.bulletPattern, bulletPatternGroup.attachToEntity ? controller.transform : patternManager.transform);
                controller.GameManager.GameModeHanlder.SimObjectManager.RegisterObject(bpm);
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

        protected virtual bool CheckCancelWindows(AttackSO currentAttack)
        {
            if (CheckEnemyStepWindows(currentAttack)
                || CheckDashCancelWindows(currentAttack)
                || CheckJumpCancelWindows(currentAttack)
                || CheckLandCancelWindows(currentAttack))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles the lifetime process of hitboxes and detectboxes.
        /// </summary>
        /// <param name="group">The group number being processed.</param>
        /// <param name="hitboxGroup">The group being processed.</param>
        protected virtual void HandleBoxGroup(int group, HitboxGroup hitboxGroup)
        {
            // If this is the end of the group's lifetime, deactivate them.
            if (controller.StateManager.CurrentStateFrame == hitboxGroup.activeFramesEnd + 1)
            {
                CombatManager.hitboxManager.DeactivateHitboxes(group);
                CombatManager.hitboxManager.DeactivateDetectboxes(group);
            }

            // If we're within the lifetime of the boxes, do nothing.
            if (controller.StateManager.CurrentStateFrame < hitboxGroup.activeFramesStart
                || controller.StateManager.CurrentStateFrame > hitboxGroup.activeFramesEnd)
            {
                return;
            }

            // Create the correct box.
            switch (hitboxGroup.hitGroupType) {
                case HitboxGroupType.HIT:
                    CombatManager.hitboxManager.CreateHitboxes(group);
                    break;
                case HitboxGroupType.DETECT:
                    CombatManager.hitboxManager.CreateDetectboxes(group);
                    break;
            }
        }

        /// <summary>
        /// Handles the lifetime of events.
        /// </summary>
        /// <param name="currentEvent">The event being processed.</param>
        /// <returns>True if the current attack state was canceled by the event.</returns>
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

        /// <summary>
        /// Check if we should land cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we land canceled.</returns>
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

        /// <summary>
        /// Check if we should jump cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we jump canceled</returns>
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

        /// <summary>
        /// Check if we should jump cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we jump canceled</returns>
        protected virtual bool CheckEnemyStepWindows(AttackSO currentAttack)
        {
            for (int i = 0; i < currentAttack.enemyStepFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.enemyStepFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.enemyStepFrames[i].y)
                {
                    if (controller.EnemyStepCancel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if we should dash cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we dash canceled.</returns>
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

        /// <summary>
        /// Check if we should attack cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we attack canceled.</returns>
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
    }
}