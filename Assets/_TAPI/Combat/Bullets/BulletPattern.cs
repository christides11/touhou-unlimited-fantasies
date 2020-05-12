using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    /// <summary>
    /// Describes a pattern of bullet events.
    /// </summary>
    [CreateAssetMenu(fileName = "BulletPattern", menuName = "Combat/Projectiles/Bullet Pattern")]
    public class BulletPattern : ScriptableObject
    {
        [SerializeReference] public List<BulletPatternAction> actions = new List<BulletPatternAction>();
    }
}