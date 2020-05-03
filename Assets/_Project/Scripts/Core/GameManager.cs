using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;
using Rewired;
using Rewired.UI.ControlMapper;

namespace Touhou.Core
{
    public class GameManager : TAPI.Core.GameManager
    {
        public ControlMapper cMapper;

        protected override void Awake()
        {
            base.Awake();
            GlobalInputManager.instance = new GlobalInputManager();
        }
    }
}