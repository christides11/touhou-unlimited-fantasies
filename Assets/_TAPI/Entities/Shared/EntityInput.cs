using System.Collections.Generic;
using TAPI.Core;
using TAPI.Inputs;
using UnityEngine;

namespace TAPI.Entities
{
    public class EntityInput : MonoBehaviour
    {
        public EntityControlType controlType;
        [SerializeField] protected AIBrain aiBrain;
        [SerializeField] private EntityController controller;

        public List<InputRecordItem> InputRecord { get; protected set; } = new List<InputRecordItem>();
        protected int inputRecordMaxSize = 600; //60 = second
        protected int controllerIndex = 0;

        public bool frameInterpolate;
        public Vector3 visualOffset;

        protected virtual void Awake()
        {
            InputRecord = new List<InputRecordItem>(inputRecordMaxSize);
        }

        public virtual void Tick()
        {
            if (controlType == EntityControlType.Direct)
            {
                GetInputs();
                if (InputRecord.Count > 1)
                {
                    ProcessInputs();
                }
            }
        }

        #region Get Inputs
        protected virtual void GetInputs()
        {
            GlobalInputManager gim = GlobalInputManager.instance;
            Vector2 lStick = gim.GetAxis2D(controllerIndex, Action.Movement_X, Action.Movement_Y);
            Vector2 rStick = gim.GetAxis2D(controllerIndex, Action.Camera_X, Action.Camera_Y);
            float floatDir = gim.GetAxis(controllerIndex, Action.Float);
            bool lockon = gim.GetButton(controllerIndex, Action.Lock_On);
            bool inter = gim.GetButton(controllerIndex, Action.Interact);
            bool bullet = gim.GetButton(controllerIndex, Action.Bullet);
            bool atk = gim.GetButton(controllerIndex, Action.Attack);
            bool dash = gim.GetButton(controllerIndex, Action.Dash);
            bool jmp = gim.GetButton(controllerIndex, Action.Jump);
            bool taunt = gim.GetButton(controllerIndex, Action.Taunt);
            bool spellCardOne = gim.GetButton(controllerIndex, Action.Spell_Card_1);
            bool spellCardTwo = gim.GetButton(controllerIndex, Action.Spell_Card_2);
            bool spellCardThr = gim.GetButton(controllerIndex, Action.Spell_Card_3);
            bool spellCardFou = gim.GetButton(controllerIndex, Action.Spell_Card_4);
            InputRecord.Add(new InputRecordItem(lStick, rStick, floatDir, lockon, inter, bullet, atk,
                dash, jmp, taunt, spellCardOne, spellCardTwo, spellCardThr, spellCardFou));
        }

        protected virtual void ProcessInputs()
        {
            foreach(var r in InputRecord[InputRecord.Count - 1].inputs)
            {
                r.Value.Process(InputRecord[InputRecord.Count - 2].inputs[r.Key]);
            }
            if (InputRecord.Count > inputRecordMaxSize)
            {
                InputRecord.RemoveAt(0);
            }
        }
        #endregion

        #region Input Hooks
        public virtual Vector2 GetMovement(int frame = 0)
        {
            if(InputRecord.Count == 0)
            {
                return Vector2.zero;
            }
            if((InputRecord.Count-1) > frame)
            {
                return ((InputRecordAxis2D)InputRecord[(InputRecord.Count - 1) - frame].inputs[EntityInputs.Movement]).axis;
            }
            return Vector2.zero;
        }

        public virtual Vector2 GetCamera(int frame = 0)
        {
            if (InputRecord.Count == 0)
            {
                return Vector2.zero;
            }
            if ((InputRecord.Count - 1) >= frame)
            {
                return ((InputRecordAxis2D)InputRecord[(InputRecord.Count - 1) - frame].inputs[EntityInputs.Camera]).axis;
            }
            return Vector2.zero;
        }

        public virtual float GetFloatDir(int frame = 0)
        {
            if (InputRecord.Count == 0)
            {
                return 0;
            }
            if ((InputRecord.Count - 1) >= frame)
            {
                return ((InputRecordAxis)InputRecord[(InputRecord.Count - 1) - frame].inputs[EntityInputs.Float]).axis;
            }
            return 0;
        }

        public virtual InputRecordButton GetButton(EntityInputs input, int frame = 0, bool checkBuffer = false, int bufferFrames = 3)
        {
            if(InputRecord.Count == 0)
            {
                return new InputRecordButton();
            }

            if (checkBuffer)
            {
                if((InputRecord.Count-1) >= (frame+bufferFrames))
                {
                    for(int i = 0; i < bufferFrames; i++)
                    {
                        InputRecordButton b = ((InputRecordButton)InputRecord[(InputRecord.Count - 1) - (frame + bufferFrames)].inputs[input]);
                        //Can't go further, already used buffer past here.
                        if (b.usedInBuffer)
                        {
                            break;
                        }
                        if (b.firstPress)
                        {
                            return b;
                        }
                    }
                }
            }
            return (InputRecordButton)InputRecord[(InputRecord.Count - 1) - frame].inputs[input];
        }

        //Clear every input's buffer.
        public virtual void ClearBuffer()
        {
            foreach (var r in InputRecord[InputRecord.Count - 1].inputs)
            {
                r.Value.UsedInBuffer();
            }
        }

        //Clear a specific buffer.
        public virtual void ClearBuffer(EntityInputs input)
        {
            InputRecord[InputRecord.Count - 1].inputs[input].UsedInBuffer();
        }
        #endregion
    }
}