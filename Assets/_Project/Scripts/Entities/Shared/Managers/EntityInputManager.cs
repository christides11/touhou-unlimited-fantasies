using System.Collections.Generic;
using TUF.Core;
using TUF.Inputs;
using UnityEngine;
using CAF.Input;
using GlobalInputManager = CAF.Input.GlobalInputManager;

namespace TUF.Entities
{
    public class EntityInputManager : CAF.Entities.EntityInputManager
    {
        [SerializeField] protected AIBrain aiBrain;

        public Vector3 visualOffset;

        public override void Awake()
        {
            InputRecord = new List<CAF.Input.InputRecordItem>(inputRecordMaxSize);
        }

        protected override void GetInputs()
        {
            GlobalInputManager gim = GlobalInputManager.instance;
            InputRecordItem recordItem = new InputRecordItem();
            recordItem.AddInput((int)EntityInputs.Movement, 
                new InputRecordAxis2D(gim.GetAxis2D(ControllerID, Action.Movement_X, Action.Movement_Y)));
            recordItem.AddInput((int)EntityInputs.Camera,
                new InputRecordAxis2D(gim.GetAxis2D(ControllerID, Action.Camera_X, Action.Camera_Y)));

            recordItem.AddInput((int)EntityInputs.Float,
                new InputRecordAxis(gim.GetAxis(ControllerID, Action.Float)));

            recordItem.AddInput((int)EntityInputs.Lockon,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Lock_On)));
            recordItem.AddInput((int)EntityInputs.Interact,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Interact)));
            recordItem.AddInput((int)EntityInputs.Bullet,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Bullet)));
            recordItem.AddInput((int)EntityInputs.Attack,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Attack)));
            recordItem.AddInput((int)EntityInputs.Special,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Special)));
            recordItem.AddInput((int)EntityInputs.Dash,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Dash)));
            recordItem.AddInput((int)EntityInputs.Jump,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Jump)));
            recordItem.AddInput((int)EntityInputs.Taunt,
                new InputRecordButton(gim.GetButton(ControllerID, Action.Taunt)));

            InputRecord.Add(recordItem);
        }
    }
}