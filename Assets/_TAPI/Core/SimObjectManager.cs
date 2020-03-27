using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace TAPI.Core
{
    public class SimObjectManager
    {
        private List<SimObject> simObjects = new List<SimObject>();

        public void RegisterObject(SimObject simObject)
        {
            if (simObjects.Contains(simObject))
            {
                return;
            }
            simObjects.Add(simObject);
        }

        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject obj = GameObject.Instantiate(prefab, position, rotation);
            simObjects.Add(obj.GetComponent<SimObject>());
            return obj;
        }

        public void Update(float dt)
        {
            for(int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimUpdate();
            }
            KinematicCharacterSystem.PreSimulationInterpolationUpdate(dt);
            KinematicCharacterSystem.Simulate(dt, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
            KinematicCharacterSystem.PostSimulationInterpolationUpdate(dt);
            Physics.Simulate(dt);
        }

        public void LateUpdate(float dt)
        {
            for(int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimLateUpdate();
            }
        }
    }
}