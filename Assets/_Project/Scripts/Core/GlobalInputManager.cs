using Rewired;
using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Inputs;
using UnityEngine;

namespace Touhou.Core
{
    public class GlobalInputManager : TAPI.Inputs.GlobalInputManager
    {

        public override CurrentInputMethod GetCurrentInputMethod(int playerID)
        {
            Controller c = ReInput.players.GetPlayer(playerID).controllers.GetLastActiveController();

            if(c == null || c.name == "Keyboard" || c.name == "Mouse")
            {
                return CurrentInputMethod.MK;
            }
            else
            {
                return CurrentInputMethod.CONTROLLER;
            }
        }

        public override Vector2 GetAxis2D(int playerID, string horizontal, string vertical)
        {
            return ReInput.players.GetPlayer(playerID).GetAxis2D(horizontal, vertical);
        }

        public override Vector2 GetAxis2D(int playerID, int horizontal, int vertical)
        {
            return ReInput.players.GetPlayer(playerID).GetAxis2D(horizontal, vertical);
        }

        public override float GetAxis(int playerID, string axisName)
        {
            return ReInput.players.GetPlayer(playerID).GetAxis(axisName);
        }

        public override float GetAxis(int playerID, int axis)
        {
            return ReInput.players.GetPlayer(playerID).GetAxis(axis);
        }

        public override bool GetButton(int playerID, string buttonName)
        {
            return ReInput.players.GetPlayer(playerID).GetButton(buttonName);
        }

        public override bool GetButton(int playerID, int button)
        {
            return ReInput.players.GetPlayer(playerID).GetButton(button);
        }

        public override bool GetButtonDown(int playerID, string buttonName)
        {
            return ReInput.players.GetPlayer(playerID).GetButtonDown(buttonName);
        }

        public override bool GetButtonDown(int playerID, int button)
        {
            return ReInput.players.GetPlayer(playerID).GetButtonDown(button);
        }

        public override bool GetButtonUp(int playerID, string buttonName)
        {
            return ReInput.players.GetPlayer(playerID).GetButtonUp(buttonName);
        }

        public override bool GetButtonUp(int playerID, int button)
        {
            return ReInput.players.GetPlayer(playerID).GetButtonUp(button);
        }
    }
}