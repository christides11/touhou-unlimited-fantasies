using KinematicCharacterController;
using UnityEngine;

namespace TUF.Core
{
    public class SimObjectManager : CAF.Simulation.SimObjectManager
    {
        protected override void SimulatePhysics()
        {
            KinematicCharacterSystem.PreSimulationInterpolationUpdate(Time.fixedDeltaTime);
            KinematicCharacterSystem.Simulate(Time.fixedDeltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
            KinematicCharacterSystem.PostSimulationInterpolationUpdate(Time.fixedDeltaTime);
            base.SimulatePhysics();
        }
    }
}