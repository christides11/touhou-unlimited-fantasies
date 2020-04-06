using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Core
{
    public class InputRecordItem
    {
        public Dictionary<EntityInputs, InputRecordInput> inputs;

        public InputRecordItem()
        {
            inputs = new Dictionary<EntityInputs, InputRecordInput>();
        }

        public InputRecordItem(Vector2 movement, Vector2 camera, float floatDir, bool lockon, bool interact, bool bullet,
            bool attack, bool special, bool dash, bool jump, bool taunt, bool scOne, bool scTwo, bool scThr, bool scFour)
        {
            inputs = new Dictionary<EntityInputs, InputRecordInput>();
            InputRecordAxis2D rMovement = new InputRecordAxis2D(movement);
            inputs.Add(EntityInputs.Movement, rMovement);
            InputRecordAxis2D rCamera = new InputRecordAxis2D(camera);
            inputs.Add(EntityInputs.Camera, rCamera);

            InputRecordAxis aFloatDir = new InputRecordAxis(floatDir);
            inputs.Add(EntityInputs.Float, aFloatDir);

            InputRecordButton rJump = new InputRecordButton(jump);
            inputs.Add(EntityInputs.Jump, rJump);
            InputRecordButton rDash = new InputRecordButton(dash);
            inputs.Add(EntityInputs.Dash, rDash);

            InputRecordButton rAttack = new InputRecordButton(attack);
            inputs.Add(EntityInputs.Attack, rAttack);
            InputRecordButton rBullet = new InputRecordButton(bullet);
            inputs.Add(EntityInputs.Bullet, rBullet);
            InputRecordButton rSpecial = new InputRecordButton(special);
            inputs.Add(EntityInputs.Special, rSpecial);
            InputRecordButton rLockon = new InputRecordButton(lockon);
            inputs.Add(EntityInputs.Lockon, rLockon);
            InputRecordButton rTaunt = new InputRecordButton(taunt);
            inputs.Add(EntityInputs.Taunt, rTaunt);
        }
    }


    public interface InputRecordInput
    {
        bool UsedInBuffer();
        void Process(InputRecordInput lastStateDown);
    }

    [System.Serializable]
    public struct InputRecordAxis2D : InputRecordInput
    {
        public Vector2 axis;

        public InputRecordAxis2D(Vector2 axis)
        {
            this.axis = axis;
        }

        public bool UsedInBuffer()
        {
            return false;
        }

        public void Process(InputRecordInput lastStateDown)
        {
        }
    }

    [System.Serializable]
    public struct InputRecordAxis : InputRecordInput
    {
        public float axis;

        public InputRecordAxis(float axis)
        {
            this.axis = axis;
        }

        public bool UsedInBuffer()
        {
            return false;
        }

        public void Process(InputRecordInput lastStateDown)
        {

        }
    }

    [System.Serializable]
    public struct InputRecordButton : InputRecordInput
    {
        public bool usedInBuffer;
        public bool isDown;
        public bool firstPress; //If the button was pressed on this frame
        public bool released; //If the button was released this frame

        public InputRecordButton(bool button)
        {
            usedInBuffer = false;
            isDown = button;
            firstPress = false;
            released = false;
        }

        public void Process(InputRecordInput lastState)
        {
            InputRecordButton lsb = (InputRecordButton)lastState;
            if (isDown && !lsb.isDown)
            {
                firstPress = true;
            }
            else if (!isDown && lsb.isDown)
            {
                released = true;
            }
        }

        public bool UsedInBuffer()
        {
            return usedInBuffer;
        }

        public void UseInBuffer()
        {
            usedInBuffer = true;
        }
    }
}