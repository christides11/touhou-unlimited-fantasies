using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    public interface IHurtable
    {
        EntityTeams Team { get; }
        /// <summary>
        /// Called when damage is dealt to this object.
        /// </summary>
        /// <param name="forward">The forward direction that's used to determine what direction
        /// the hit's forces send this object in.</param>
        /// <param name="right">The right direction that's used to determine what direction
        /// the hit's forces send this object in.</param>
        /// <param name="hitInfo">The information on the hit.</param>
        /// <returns></returns>
        HurtReactions Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfo hitInfo);
        void Heal();
    }
}