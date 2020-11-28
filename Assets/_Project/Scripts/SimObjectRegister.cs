using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF
{
    public class SimObjectRegister : MonoBehaviour
    {
        [SerializeField] private SimObject simObject;

        void Start()
        {
            GameManager.current.GameMode.SimObjectManager.RegisterObject(simObject);
        }
    }
}