using CAF.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Core;
using UnityEngine;
using AttackDefinition = TUF.Combat.AttackDefinition;
using BoxGroup = TUF.Combat.BoxGroup;
using MovesetAttackNode = TUF.Combat.MovesetAttackNode;

namespace TUF.Entities.Shared
{
    public class EntityAttack : EntityState
    {
        public override string GetName()
        {
            return $"Attack ({CombatManager.CurrentAttack?.name}).";
        }

        protected bool charging = true;

        public override void Initialize()
        {
            base.Initialize();
            if (!controller.LockedOn)
            {
                controller.PickSoftlockTarget();
            }
            AttackDefinition currentAttack = (TUF.Combat.AttackDefinition)CombatManager.CurrentAttack.attackDefinition;
            if(currentAttack.stateOverride > -1)
            {
                StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
            charging = true;
        }

        public override void OnUpdate()
        {
            AttackDefinition currentAttack = (TUF.Combat.AttackDefinition)CombatManager.CurrentAttack.attackDefinition;

            // Rotate towards target.
            for(int i = 0; i < currentAttack.faceLockonTargetWindows.Count; i++)
            {
                if(StateManager.CurrentStateFrame >= currentAttack.faceLockonTargetWindows[i].startFrame
                    && StateManager.CurrentStateFrame <= currentAttack.faceLockonTargetWindows[i].endFrame)
                {
                    Vector3 mov = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
                    Vector3 forwardDir = controller.visualTransform.forward;
                    // We're currently locked on to something.
                    if (controller.LockedOn)
                    {
                        forwardDir = controller.LockonForward;
                    }
                    // Movement is neutral, so Soft Target.
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

            // Create hitbox grou ps.
            for(int i = 0; i < currentAttack.boxGroups.Count; i++)
            {
                HandleBoxGroup(i, (BoxGroup)currentAttack.boxGroups[i]);
            }

            // Check if this move was canceled with another action.
            if (CheckCancelWindows(currentAttack))
            {
                CombatManager.Cleanup();
                return;
            }

            // Check if we can special or command attack cancel.
            if (TrySpecialCancel(currentAttack)
                || TryCommandAttackCancel(currentAttack))
            {
                return;
            }

            // Execute events.
            bool eventCancel = false;
            for (int i = 0; i < currentAttack.events.Count; i++)
            {
                if (HandleEvents(currentAttack.events[i]))
                {
                    eventCancel = true;
                    return;
                }
            }

            // Only increment the frame if the event didn't stop it.
            if (!eventCancel && !HandleChargeLevels((TUF.Entities.EntityManager)Manager, currentAttack))
            {
                controller.StateManager.IncrementFrame();
            }

            CheckInterrupt();
        }

        public override void OnInterrupted()
        {

            AttackDefinition currentAttack = (TUF.Combat.AttackDefinition)CombatManager.CurrentAttack.attackDefinition;
            if (currentAttack)
            {
                //PhysicsManager.GravityScale += currentAttack.gravityScaleAdded;
            }
        }

        protected virtual bool CheckCancelWindows(AttackDefinition currentAttack)
        {
            if (CheckEnemyStepWindows(currentAttack)
                || CheckDashCancelWindows(currentAttack)
                || CheckJumpCancelWindows(currentAttack)
                || CheckLandCancelWindows(currentAttack)
                || CheckFloatCancelWindows(currentAttack))
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
        protected virtual void HandleBoxGroup(int group, BoxGroup hitboxGroup)
        {
            // If this is the end of the group's lifetime, deactivate them.
            if (controller.StateManager.CurrentStateFrame == hitboxGroup.activeFramesEnd + 1)
            {
                CombatManager.hitboxManager.DeactivateHitboxGroup(group);
            }

            // If we're outside the lifetime of the hitbox, ignore it.
            if (controller.StateManager.CurrentStateFrame < hitboxGroup.activeFramesStart
                || controller.StateManager.CurrentStateFrame > hitboxGroup.activeFramesEnd)
            {
                return;
            }


            if (hitboxGroup.hitboxHitInfo.continuousHit &&
                (controller.StateManager.CurrentStateFrame-hitboxGroup.activeFramesEnd) % hitboxGroup.hitboxHitInfo.spaceBetweenHits == 0)
            {
                CombatManager.hitboxManager.ReactivateHitboxGroup(group);
            }

            // Create the correct box.
            switch (hitboxGroup.hitGroupType) {
                case BoxGroupType.HIT:
                    CombatManager.hitboxManager.CreateHitboxGroup(group);
                    break;
            }
        }

        /// <summary>
        /// Handles the lifetime of events.
        /// </summary>
        /// <param name="currentEvent">The event being processed.</param>
        /// <returns>True if the current attack state was canceled by the event.</returns>
        protected virtual bool HandleEvents(CAF.Combat.AttackEventDefinition currentEvent)
        {
            if (!currentEvent.active)
            {
                return false;
            }
            if(StateManager.CurrentStateFrame >= currentEvent.startFrame 
                && StateManager.CurrentStateFrame <= currentEvent.endFrame)
            {
                // Check the on hit requirement.
                if (currentEvent.onHit)
                {
                    List<IHurtable> ihList = ((EntityHitboxManager)CombatManager.hitboxManager).GetHitList(currentEvent.onHitHitboxGroup);
                    if (ihList == null)
                    {
                        return false;
                    }
                    if (ihList.Count <= 1)
                    {
                        return false;
                    }
                }

                // Check the input requirement.
                if (currentEvent.inputCheckTiming != AttackEventInputCheckTiming.NONE
                    && !CheckEventInputRequirement(currentEvent))
                {
                    return false;
                }

                // Check the charge level requirement.
                if((CombatManager.CurrentChargeLevel < currentEvent.chargeLevelMin
                    || CombatManager.CurrentChargeLevel > currentEvent.chargeLevelMax)
                    && (currentEvent.chargeLevelMin != -1))
                {
                    return false;
                }

                return currentEvent.attackEvent.Evaluate(StateManager.CurrentStateFrame - currentEvent.startFrame, 
                    currentEvent.endFrame - currentEvent.startFrame,
                    controller,
                    currentEvent.variables);
            }
            return false;
        }

        private bool CheckEventInputRequirement(CAF.Combat.AttackEventDefinition currentEvent)
        {
            if(StateManager.CurrentStateFrame != currentEvent.endFrame)
            {
                return false;
            }

            switch (currentEvent.inputCheckTiming)
            {
                case AttackEventInputCheckTiming.ONCE:
                    for(int i = (int)currentEvent.startFrame; i < currentEvent.endFrame; i++)
                    {
                        if (CombatManager.CheckForInputSequence(currentEvent.input))
                        {
                            return true;
                        }
                    }
                    break;
                case AttackEventInputCheckTiming.CONTINUOUS:
                    for (int i = (int)currentEvent.startFrame; i < currentEvent.endFrame; i++)
                    {
                        if (!CombatManager.CheckForInputSequence(currentEvent.input, i, true, true))
                        {
                            return false;
                        }
                    }
                    return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Handles processing the charge levels of the current attack.
        /// </summary>
        /// <param name="entityManager">The entity itself.</param>
        /// <param name="currentAttack">The current attack the entity is doing.</param>
        /// <returns>If the frame should be held.</returns>
        private bool HandleChargeLevels(EntityManager entityManager, AttackDefinition currentAttack)
        {
            EntityCombatManager cManager = (EntityCombatManager)entityManager.CombatManager;
            if (!charging)
            {
                return false;
            }

            if (!entityManager.InputManager.GetButton((int)EntityInputs.Attack).isDown)
            {
                charging = false;
                return false;
            }

            bool result = false;
            for (int i = 0; i < currentAttack.chargeWindows.Count; i++)
            {
                // Not on the correct frame.
                if (entityManager.StateManager.CurrentStateFrame != currentAttack.chargeWindows[i].frame)
                {
                    continue;
                }

                // Still have charge levels to go through.
                if (entityManager.CombatManager.CurrentChargeLevel < currentAttack.chargeWindows[i].chargeLevels.Count)
                {
                    cManager.IncrementChargeLevelCharge(currentAttack.chargeWindows[i].chargeLevels[cManager.CurrentChargeLevel].maxChargeFrames);
                    // Charge completed, move on to the next level.
                    if (cManager.CurrentChargeLevelCharge == currentAttack.chargeWindows[i].chargeLevels[cManager.CurrentChargeLevel].maxChargeFrames)
                    {
                        cManager.SetChargeLevel(cManager.CurrentChargeLevel + 1);
                        cManager.SetChargeLevelCharge(0);
                    }
                }
                else if (currentAttack.chargeWindows[i].releaseOnCompletion)
                {
                    charging = false;
                }
                result = true;
                // Only one charge level can be handled per frame, ignore everything else.
                break;
            }
            return result;
        }

        /// <summary>
        /// Check if we should land cancel on the current frame.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we land canceled.</returns>
        protected virtual bool CheckLandCancelWindows(AttackDefinition currentAttack)
        {
            for(int i = 0; i < currentAttack.landCancelWindows.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.landCancelWindows[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.landCancelWindows[i].y)
                {
                    if (controller.TryLandCancel())
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
        protected virtual bool CheckJumpCancelWindows(AttackDefinition currentAttack)
        {
            for(int i = 0; i < currentAttack.jumpCancelWindows.Count; i++)
            {
                if(StateManager.CurrentStateFrame >= currentAttack.jumpCancelWindows[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.jumpCancelWindows[i].y)
                {
                    if (controller.TryJump())
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
        protected virtual bool CheckEnemyStepWindows(AttackDefinition currentAttack)
        {
            for (int i = 0; i < currentAttack.enemyStepWindows.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.enemyStepWindows[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.enemyStepWindows[i].y)
                {
                    if (controller.TryEnemyStep())
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
        protected virtual bool CheckDashCancelWindows(AttackDefinition currentAttack)
        {
            for (int i = 0; i < currentAttack.dashCancelableFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.dashCancelableFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.dashCancelableFrames[i].y)
                {
                    if (InputManager.GetButton((int)EntityInputs.Dash, 0, true).firstPress)
                    {
                        if (controller.TryDash())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected virtual bool CheckFloatCancelWindows(AttackDefinition currentAttack)
        {
            for (int i = 0; i < currentAttack.floatCancelFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.floatCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.floatCancelFrames[i].y)
                {
                    if (controller.TryFloat())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to cancel into a command attack.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we attack canceled.</returns>
        protected virtual bool TryCommandAttackCancel(AttackDefinition currentAttack)
        {
            for (int i = 0; i < currentAttack.commandAttackCancelWindows.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.commandAttackCancelWindows[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.commandAttackCancelWindows[i].y)
                {
                    MovesetAttackNode man = (MovesetAttackNode)CombatManager.TryCommandAttack();
                    if (man != null)
                    {
                        CombatManager.SetAttack(man);
                        StateManager.ChangeState((int)EntityStates.ATTACK);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentAttack"></param>
        /// <returns></returns>
        protected virtual bool TrySpecialCancel(AttackDefinition currentAttack)
        {
            for (int i = 0; i < currentAttack.specialCancelFrames.Count; i++)
            {
                if (StateManager.CurrentStateFrame >= currentAttack.specialCancelFrames[i].x
                    && StateManager.CurrentStateFrame <= currentAttack.specialCancelFrames[i].y)
                {
                    MovesetAttackNode man = ((EntityCombatManager)CombatManager).TrySpecial();
                    if (man != null)
                    {
                        CombatManager.SetAttack(man);
                        StateManager.ChangeState((int)EntityStates.ATTACK);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}