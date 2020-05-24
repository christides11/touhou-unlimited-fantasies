using System.Collections.Generic;
using TAPI.Core;
using TAPI.Inputs;
using UnityEngine;
using CAF.Input;
using GlobalInputManager = CAF.Input.GlobalInputManager;

namespace TAPI.Entities
{
    public class EntityInputManager : CAF.Entities.EntityInputManager
    {
        public EntityControlType controlType;
        [SerializeField] protected AIBrain aiBrain;
        [SerializeField] private EntityController controller;

        public bool frameInterpolate;
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
                new InputRecordAxis2D(gim.GetAxis2D(controllerID, Action.Movement_X, Action.Movement_Y)));
            recordItem.AddInput((int)EntityInputs.Camera,
                new InputRecordAxis2D(gim.GetAxis2D(controllerID, Action.Camera_X, Action.Camera_Y)));
            recordItem.AddInput((int)EntityInputs.Float,
                new InputRecordAxis(gim.GetAxis(controllerID, Action.Float)));
            recordItem.AddInput((int)EntityInputs.Lockon,
                new InputRecordButton(gim.GetButton(controllerID, Action.Lock_On)));
            recordItem.AddInput((int)EntityInputs.Interact,
                new InputRecordButton(gim.GetButton(controllerID, Action.Interact)));
            recordItem.AddInput((int)EntityInputs.Bullet,
                new InputRecordButton(gim.GetButton(controllerID, Action.Bullet)));
            recordItem.AddInput((int)EntityInputs.Attack,
                new InputRecordButton(gim.GetButton(controllerID, Action.Attack)));
            recordItem.AddInput((int)EntityInputs.Special,
                new InputRecordButton(gim.GetButton(controllerID, Action.Special)));
            recordItem.AddInput((int)EntityInputs.Dash,
                new InputRecordButton(gim.GetButton(controllerID, Action.Dash)));
            recordItem.AddInput((int)EntityInputs.Jump,
                new InputRecordButton(gim.GetButton(controllerID, Action.Jump)));
            recordItem.AddInput((int)EntityInputs.Taunt,
                new InputRecordButton(gim.GetButton(controllerID, Action.Taunt)));
        }
    }
}