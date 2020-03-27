using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;
using Rewired;

namespace Touhou.Core
{
    public class GameManager : TAPI.Core.GameManager
    {
        protected override void Awake()
        {
            base.Awake();
            GlobalInputManager.instance = new GlobalInputManager();
        }
    }
}