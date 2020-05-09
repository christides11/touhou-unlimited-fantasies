using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TAPI.Inputs;
using TAPI.Entities;
using TAPI.Combat;
using System;

namespace TAPI.Core
{
    public class PlayerCamera : SimObject
    {
        public static PlayerCamera current;

        public Camera Cam { get { return cam; } }

        private Transform lockonTarget = null;
        [SerializeField] private Camera cam;
        [SerializeField] private CinemachineBrain brain;
        [SerializeField] private CinemachineFreeLook thirdPersonLook;
        [SerializeField] private CinemachineVirtualCamera lockOnCam;
        [SerializeField] private CinemachineTargetGroup lockOnTargetGroup;
        [SerializeField] private CameraShake cameraShake;

        [SerializeField] private float mouseDeadzone = 0.05f;
        [SerializeField] private float mouseXAxisSpeed = 1.0f;
        [SerializeField] private float mouseYAxisSpeed = 1.0f;

        [Header("Controller")]
        [SerializeField] private float stickDeadzone = 0.2f;
        [SerializeField] private float stickAxialDeadZone = 0.15f;
        [SerializeField] private float stickXAxisSpeed = 1.0f;
        [SerializeField] private float stickYAxisSpeed = 1.0f;

        private EntityController currentEntity;

        public virtual void Initialize(EntityController entity)
        {
            currentEntity = entity;
            currentEntity.CombatManager.hitboxManager.OnHitboxHit += CauseShake;
        }

        private void CauseShake(GameObject hurtableHit, int hitboxGroupIndex, MovesetAttackNode attack)
        {
            cameraShake.Shake(attack.action.hitboxGroups[hitboxGroupIndex].cameraShake);
        }

        public virtual void Update()
        {
            Vector2 stickInput = new Vector2(GlobalInputManager.instance.GetAxis(0, Inputs.Action.Camera_X),
                GlobalInputManager.instance.GetAxis(0, Inputs.Action.Camera_Y));
            switch (GlobalInputManager.instance.GetCurrentInputMethod(0))
            {
                case CurrentInputMethod.MK:
                    if (Mathf.Abs(stickInput.x) <= mouseDeadzone)
                    {
                        stickInput.x = 0;
                    }
                    if (Mathf.Abs(stickInput.y) <= mouseDeadzone)
                    {
                        stickInput.y = 0;
                    }
                    stickInput.x *= mouseXAxisSpeed;
                    stickInput.y *= mouseYAxisSpeed;
                    break;
                case CurrentInputMethod.CONTROLLER:
                    if (stickInput.magnitude < stickDeadzone)
                    {
                        stickInput = Vector2.zero;
                    }
                    else
                    {
                        float d = ((stickInput.magnitude - stickDeadzone) / (1.0f - stickDeadzone));
                        d = Mathf.Min(d, 1.0f);
                        d *= d;
                        stickInput = stickInput.normalized * d;
                    }
                    if (Mathf.Abs(stickInput.x) < stickAxialDeadZone)
                    {
                        stickInput.x = 0;
                    }
                    if(Mathf.Abs(stickInput.y) < stickAxialDeadZone)
                    {
                        stickInput.y = 0;
                    }
                    stickInput.x *= stickXAxisSpeed;
                    stickInput.y *= stickYAxisSpeed;
                    break;
            }

            thirdPersonLook.m_XAxis.m_InputAxisValue = stickInput.x;
            thirdPersonLook.m_YAxis.m_InputAxisValue = stickInput.y;
        }

        public virtual void UpdateTarget(Transform newTarget)
        {
            thirdPersonLook.LookAt = newTarget;
            thirdPersonLook.Follow = newTarget;
            lockOnTargetGroup.m_Targets[0].target = newTarget;
        }

        public virtual void LockOn(Transform target)
        {
            lockonTarget = target;
            thirdPersonLook.gameObject.SetActive(false);
            thirdPersonLook.m_RecenterToTargetHeading.m_enabled = true;
            lockOnTargetGroup.m_Targets[1].target = lockonTarget;
            lockOnCam.gameObject.SetActive(true);
        }

        public virtual void UnlockOn()
        {
            lockonTarget = null;
            thirdPersonLook.gameObject.SetActive(true);
            thirdPersonLook.m_RecenterToTargetHeading.m_enabled = false;
            lockOnTargetGroup.m_Targets[1].target = lockonTarget;
            lockOnCam.gameObject.SetActive(false);
        }
    }
}