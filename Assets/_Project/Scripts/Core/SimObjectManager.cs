using KinematicCharacterController;

namespace TUF.Core
{
    public class SimObjectManager : CAF.Simulation.SimObjectManager
    {
        protected override void SimulatePhysics(float deltatime)
        {
            KinematicCharacterSystem.PreSimulationInterpolationUpdate(deltatime);
            KinematicCharacterSystem.Simulate(deltatime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
            KinematicCharacterSystem.PostSimulationInterpolationUpdate(deltatime);
            base.SimulatePhysics(deltatime);
        }
    }
}