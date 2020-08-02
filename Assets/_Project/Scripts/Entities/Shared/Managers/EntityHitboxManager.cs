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

        /// <summary>
        /// Destroys all boxes and clears variables.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// Checks the hitboxes and detectboxes to see what they hit this frame.
        /// This should be called in late update, as physics update right after update.
        /// </summary>
        public override void TickBoxes()
        {
            base.TickBoxes();

            /*
            foreach(List<DetectionBox> detectboxGroup in detectboxes.Values)
            {
                for(int i = 0; i < detectboxGroup.Count; i++)
                {
                    detectboxGroup[i].CheckDetection();
                }
            }*/
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

        #region Detectboxes
        /// <summary>
        /// Create the detectboxes of the given group.
        /// </summary>
        /// <param name="group">The hitbox group to create the detectboxes for.</param>
        public void CreateDetectboxes(int group)
        {
            /*
            if (detectboxes.ContainsKey(group))
            {
                return;
            }

            // Variables.
            BoxGroup currentGroup = combatManager.currentAttack.action.hitboxGroups[group];
            if(currentGroup.hitGroupType != HitboxGroupType.DETECT)
            {
                return;
            }
            List<DetectionBox> detectionboxes = new List<DetectionBox>(currentGroup.hitboxes.Count);

            if (!detectboxesDetectedHurtables.ContainsKey(group))
            {
                detectboxesDetectedHurtables.Add(group, new List<IHurtable>());
            }
            if (!hurtablesDetected.ContainsKey(currentGroup.ID))
            {
                hurtablesDetected.Add(currentGroup.ID, new List<IHurtable>());
                hurtablesDetected[currentGroup.ID].Add(combatManager);
            }

            // Loop through all the hitboxes in the group.
            for (int i = 0; i < currentGroup.hitboxes.Count; i++)
            {
                // Instantiate the hitbox with the correct position and rotation.
                BoxDefinition currHitbox = currentGroup.hitboxes[i];
                Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * currHitbox.offset.z
                    + controller.GetVisualBasedDirection(Vector3.right) * currHitbox.offset.x
                    + controller.GetVisualBasedDirection(Vector3.up) * currHitbox.offset.y;
                GameObject dbox = GameObject.Instantiate(controller.GameManager.gameVars.combat.detectbox,
                    controller.transform.position + pos,
                    Quaternion.Euler(controller.transform.eulerAngles + currHitbox.rotation));

                switch (currentGroup.hitboxes[i].shape)
                {
                    case BoxShapeType.Rectangle:
                        dbox.GetComponent<DetectionBox>().Initialize(controller.gameObject, currentGroup.hitboxes[i].size,
                            controller.visual.transform.eulerAngles + currentGroup.hitboxes[i].rotation);
                        break;
                    case BoxShapeType.Circle:
                        dbox.GetComponent<DetectionBox>().Initialize(controller.gameObject, currentGroup.hitboxes[i].radius);
                        break;
                }

                if (currentGroup.attachToEntity)
                {
                    dbox.transform.SetParent(controller.transform);
                }
                int cID = currentGroup.ID;
                dbox.GetComponent<DetectionBox>().OnDetect += (hurtableDetected) => { OnDetectboxDetect(hurtableDetected, cID, group);  };
                dbox.GetComponent<DetectionBox>().Activate(hurtablesDetected[currentGroup.ID]);
                detectionboxes.Add(dbox.GetComponent<DetectionBox>());
            }
            detectboxes.Add(group, detectionboxes);*/
        }

        /// <summary>
        /// Called whenever a detectbox detects a hurtable.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was detected.</param>
        /// <param name="hitboxID">The ID of the detectbox.</param>
        private void OnDetectboxDetect(GameObject hurtable, int hitboxID, int hitboxGroup)
        {
            detectboxesDetectedHurtables[hitboxGroup].Add(hurtable.GetComponent<IHurtable>());
            hurtablesDetected[hitboxID].Add(hurtable.GetComponent<IHurtable>());
            UpdateIDDetectboxGroup(hitboxID);
        }

        /// <summary>
        /// Updates the ignore/detect list of all detectboxes with the same ID.
        /// </summary>
        /// <param name="hitboxID">The ID to update the detectboxes for.</param>
        private void UpdateIDDetectboxGroup(int hitboxID)
        {
            foreach (int key in detectboxes.Keys)
            {
                for (int i = 0; i < detectboxes[key].Count; i++)
                {
                    detectboxes[key][i].ignoreList = hurtablesDetected[hitboxID];
                }
            }
        }

        public List<IHurtable> GetHitList(int group)
        {
            if (!hurtablesHit.ContainsKey(group))
            {
                return null;
            }
            return hurtablesHit[group];
        }

        public List<IHurtable> GetDetectedList(int group)
        {
            if (!detectboxesDetectedHurtables.ContainsKey(group))
            {
                return null;
            }
            return detectboxesDetectedHurtables[group];
        }
        #endregion
    }
}