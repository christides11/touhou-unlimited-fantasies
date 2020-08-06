using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using UnityEngine;
using TUF.Core;
using TUF.Sound;
using CAF.Combat;
using MovesetAttackNode = TUF.Combat.MovesetAttackNode;

namespace TUF.Entities
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class EntityHitboxManager : CAF.Entities.EntityHitboxManager
    {

        // Detectbox Group : Detectboxes
        private Dictionary<int, List<DetectionBox>> detectboxes = new Dictionary<int, List<DetectionBox>>();
        // Detectbox Group : IHurtables Hit
        private Dictionary<int, List<IHurtable>> detectboxesDetectedHurtables = new Dictionary<int, List<IHurtable>>();
        // Detectbox ID : IHurtables Hit
        private Dictionary<int, List<IHurtable>> hurtablesDetected = new Dictionary<int, List<IHurtable>>();

        public EntityHitboxManager(EntityCombatManager combatManager, EntityManager controller) : base(combatManager, controller)
        {

        }

        protected override CAF.Combat.Hitbox InstantiateHitbox(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(((EntityManager)controller).GameManager.gameVariables.combat.hitbox,
                position, rotation).GetComponent<Combat.Hitbox>();
        }

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        protected override void OnHitboxHurt(GameObject hurtableHit, HitInfo hitInfo, int hitboxID, int hitboxGroup)
        {
            SoundDefinition sd = ((EntityManager)combatManager.controller).GameManager.ModManager
                .GetSoundDefinition(
                ((Combat.BoxGroup)combatManager.CurrentAttack.attackDefinition.boxGroups[hitboxGroup]).hitSound?.reference);
            SoundManager.Play(sd, 0, controller.transform);
            base.OnHitboxHurt(hurtableHit, hitInfo, hitboxID, hitboxGroup);
        }

        public override void CreateHitboxGroup(int index)
        {
            base.CreateHitboxGroup(index);
        }

        public List<IHurtable> GetHitList(int group)
        {
            if (!hurtablesHit.ContainsKey(group))
            {
                return null;
            }
            return hurtablesHit[group];
        }
    }
}