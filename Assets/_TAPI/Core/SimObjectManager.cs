using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace TAPI.Core
{
    public class SimObjectManager
    {
        private List<SimObject> simObjects = new List<SimObject>();

        /// <summary>
        /// Registers an object to the simulation.
        /// </summary>
        /// <param name="simObject"></param>
        public void RegisterObject(SimObject simObject)
        {
            if (simObjects.Contains(simObject))
            {
                return;
            }
            simObjects.Add(simObject);
            simObject.Init(this);
        }

        /// <summary>
        /// Instantiate an object and registers it in the simulation.
        /// </summary>
        /// <param name="prefab">The object to instantiate.</param>
        /// <param name="position">The position to instantiate at.</param>
        /// <param name="rotation">The rotation to instantiate with.</param>
        /// <returns></returns>
        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject obj = GameObject.Instantiate(prefab, position, rotation);
            RegisterObject(obj.GetComponent<SimObject>());
            return obj;
        }

        public void DestroyObject(SimObject simObject)
        {
            if (simObjects.Contains(simObject))
            {
                simObjects.Remove(simObject);
            }
            GameObject.Destroy(simObject.gameObject);
        }

        /// <summary>
        /// Calls every object's SimUpdate method that is in the simulation.
        /// After that, it ticks the physics engine.
        /// </summary>
        /// <param name="deltatime">The time between this and the last frame.</param>
        public void Update(float deltatime)
        {
            for(int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimUpdate();
            }
            KinematicCharacterSystem.PreSimulationInterpolationUpdate(deltatime);
            KinematicCharacterSystem.Simulate(deltatime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
            KinematicCharacterSystem.PostSimulationInterpolationUpdate(deltatime);
            Physics.Simulate(deltatime);
        }

        /// <summary>
        /// Calls every object's SimLateUpate method that is in the simulation.
        /// </summary>
        /// <param name="deltatime"></param>
        public void LateUpdate(float deltatime)
        {
            for(int i = 0; i < simObjects.Count; i++)
            {
                simObjects[i].SimLateUpdate();
            }
        }
    }
}