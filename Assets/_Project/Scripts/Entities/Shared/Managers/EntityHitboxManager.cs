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

        protected override HitboxBase InstantiateHitbox(BoxDefinitionBase hitboxDefinitionBase)
        {
            return base.InstantiateHitbox(hitboxDefinitionBase);
        }

        /*
        protected override CAF.Combat.HitboxBase InstantiateHitbox(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(((EntityManager)manager).GameManager.gameVariables.combat.hitbox,
                position, rotation).GetComponent<Combat.Hitbox>();
        }*/

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        protected override void OnHitboxHurt(GameObject hurtableHit, HitInfoBase hitInfo, int hitboxID, int hitboxGroup)
        {
            SoundDefinition sd = ((EntityManager)combatManager.manager).GameManager.ModManager
                .GetSoundDefinition(
                ((Combat.BoxGroup)combatManager.CurrentAttack.attackDefinition.boxGroups[hitboxGroup]).hitSound?.reference);
            SoundManager.Play(sd, 0, manager.transform);
            base.OnHitboxHurt(hurtableHit, hitInfo, hitboxID, hitboxGroup);
        }

        public override void CreateHitboxGroup(int index)
        {
            base.CreateHitboxGroup(index);
        }
    }
}