using System.Collections;
using System.Collections.Generic;
using TAPI.Combat.Bullets;
using TAPI.Core;
using TAPI.Entities.Characters;
using UnityEngine;
using CharacterController = TAPI.Entities.Characters.CharacterController;

namespace TidesPack.Characters.Reimu
{
    public class ReimuController : CharacterController
    {

        [SerializeField] private BulletPatternManager bpm;
        [SerializeField] private BulletPattern bulletMove;

        public override void SimUpdate()
        {
            base.SimUpdate();

            if(InputManager.GetButton(TAPI.Core.EntityInputs.Bullet).firstPress
                && InputManager.GetMovement(0).magnitude < InputConstants.movementMagnitude)
            {
                if(bpm == null)
                {
                    GameObject go = new GameObject();
                    go.transform.SetParent(transform, false);
                    go.transform.rotation = visualTransform.rotation;
                    bpm = go.AddComponent<BulletPatternManager>();
                    bpm.Initialize(bulletMove);
                    GameManager.GameModeHanlder.SimObjectManager.RegisterObject(bpm);
                }
            }
            if (InputManager.GetButton(EntityInputs.Bullet).released)
            {
                if (bpm)
                {
                    bpm.disableLooping = true;
                }
            }
        }

        protected override void SetupDefaultStates()
        {
            StateManager.AddState(new ReimuStateTeleport(), (int)ReimuStates.SPECIAL_TELEPORT);
            base.SetupDefaultStates();
        }
    }
}