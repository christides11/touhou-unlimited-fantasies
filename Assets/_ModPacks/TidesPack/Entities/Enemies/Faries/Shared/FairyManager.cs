using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TidesPack.Enemies.Fairies
{
    public class FairyManager : EntityManager
    {
        public override EntityStats EntityStats { get { return fairyStats; } }

        public FairyStats fairyStats;

        protected override void SetupDefaultStates()
        {
            StateManager.AddState(new FairyIdle(), (int)EntityStates.IDLE);
            StateManager.AddState(new FairyFall(), (int)EntityStates.FALL);

            // Hit Reactions
            StateManager.AddState(new EntityFlinch(), (int)EntityStates.FLINCH);
            StateManager.AddState(new EntityFlinchAir(), (int)EntityStates.FLINCH_AIR);
            StateManager.AddState(new EntityTumble(), (int)EntityStates.TUMBLE);
            StateManager.AddState(new EntityGroundBounce(), (int)EntityStates.GROUND_BOUNCE);
            StateManager.AddState(new EntityWallBounce(), (int)EntityStates.WALL_BOUNCE);

            // Start State Machine
            StateManager.ChangeState((int)EntityStates.IDLE);
        }
    }
}