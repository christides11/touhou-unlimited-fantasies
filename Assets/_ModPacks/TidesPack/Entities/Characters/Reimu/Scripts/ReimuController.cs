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

            if(InputManager.GetButton((int)TAPI.Core.EntityInputs.Bullet).firstPress
                && InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude < InputConstants.movementMagnitude)
            {
                if(bpm == null)
                {
                    GameObject go = new GameObject();
                    go.transform.position = visualTransform.position;
                    go.transform.rotation = visualTransform.rotation;
                    bpm = go.AddComponent<BulletPatternManager>();
                    BulletPatternManagerSettings settings = new BulletPatternManagerSettings();
                    settings.active = true;
                    bpm.Initialize(settings, bulletMove, go.transform.position + new Vector3(0, 2, 3));
                    GameManager.GameModeHanlder.SimObjectManager.RegisterObject(bpm);
                }
                else
                {
                    bpm.patterns[0].active = true;
                    bpm.bulletSpawnPosition = visualTransform.position + new Vector3(0, 2, 3);
                    bpm.bulletSpawnRotation = visualTransform.eulerAngles;
                }
            }
            if (InputManager.GetButton((int)EntityInputs.Bullet).released)
            {
                if (bpm)
                {
                    bpm.patterns[0].active = false;
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