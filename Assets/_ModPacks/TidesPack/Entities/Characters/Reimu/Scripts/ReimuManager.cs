using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Combat.Danmaku;
using TUF.Core;
using UnityEngine;
using CharacterManager = TUF.Entities.Characters.CharacterManager;

namespace TidesPack.Characters.Reimu
{
    public class ReimuManager : CharacterManager
    {

        bool shooting;
        int timer;

        [Header("Reimu")]
        public Transform YinYangTransform;
        public int bulletCooldown = 60;
        public DanmakuSequence dd;
        public DanmakuConfig baseConfig;
        public HitInfo bulletHitInfo;

        public override void SimUpdate()
        {
            base.SimUpdate();

            if (InputManager.GetButton((int)TUF.Core.EntityInputs.Bullet).firstPress
                && InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude < InputConstants.movementMagnitude)
            {
                if(CombatManager.CurrentAttack == null)
                {
                    shooting = true;
                    timer = bulletCooldown;
                }
            }

            if (shooting && InputManager.GetButton((int)TUF.Core.EntityInputs.Bullet).isDown)
            {
                if(LockonTarget != null)
                {
                    YinYangTransform.LookAt(LockonTarget.transform.position + new Vector3(0, 1f, 0));
                }
                else
                {
                    YinYangTransform.localEulerAngles = Vector3.zero;
                }
                timer--;

                if(timer <= 0)
                {
                    timer = bulletCooldown;
                    baseConfig.rotation = YinYangTransform.eulerAngles;
                    baseConfig.position = YinYangTransform.position;
                    danmakuManager.Fire(dd, baseConfig, (EntityTeams)CombatManager.GetTeam(), bulletHitInfo);
                }
            }

            if (InputManager.GetButton((int)TUF.Core.EntityInputs.Bullet).released)
            {
                shooting = false;
            }
        }

        protected override void SetupDefaultStates()
        {
            StateManager.AddState(new ReimuStateTeleport(), (int)ReimuStates.SPECIAL_TELEPORT);
            StateManager.AddState(new ReimuShoot(), (int)ReimuStates.SPECIAL_SHOOT);
            base.SetupDefaultStates();
        }
    }
}