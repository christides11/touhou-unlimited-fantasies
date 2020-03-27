using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Inputs
{
    public enum CurrentInputMethod
    {
        MK = 0, CONTROLLER = 1
    }

    public class GlobalInputManager
    {
        public static GlobalInputManager instance;

        public virtual CurrentInputMethod GetCurrentInputMethod(int playerID)
        {
            return CurrentInputMethod.MK;
        }

        public virtual Vector2 GetAxis2D(int playerID, string horizontal, string vertical)
        {
            return Vector2.zero;
        }

        public virtual Vector2 GetAxis2D(int playerID, int horizontal, int vertical)
        {
            return Vector2.zero;
        }

        public virtual float GetAxis(int playerID, string axisName)
        {
            return 0;
        }

        public virtual float GetAxis(int playerID, int axis)
        {
            return 0;
        }

        public virtual bool GetButton(int playerID, string buttonName)
        {
            return false;
        }

        public virtual bool GetButton(int playerID, int button)
        {
            return false;
        }

        public virtual bool GetButtonDown(int playerID, string buttonName)
        {
            return false;
        }

        public virtual bool GetButtonDown(int playerID, int button)
        {
            return false;
        }

        public virtual bool GetButtonUp(int playerID, string buttonName)
        {
            return false;
        }

        public virtual bool GetButtonUp(int playerID, int button)
        {
            return false;
        }
    }
}