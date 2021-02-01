using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat
{
    public class HitArea : SimObject
    {
        public Hitbox hitbox;

        public EntityTeams team;
        public HitInfo hitInfo;

        protected override void Start()
        {
            base.Start();
            hitbox.Initialize(gameObject, transform, (int)team, hitInfo, new List<CAF.Combat.IHurtable>());
            hitbox.Activate();
        }

        public override void SimUpdate()
        {
            base.SimUpdate();
            hitbox.Tick();
        }
    }
}