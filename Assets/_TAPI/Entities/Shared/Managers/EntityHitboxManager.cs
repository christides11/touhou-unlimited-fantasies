﻿using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;
using TAPI.Core;
using TAPI.Sound;
using CAF.Combat;
using MovesetAttackNode = TAPI.Combat.MovesetAttackNode;

namespace TAPI.Entities
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

        public EntityHitboxManager(EntityCombatManager combatManager, EntityController controller) : base(combatManager, controller)
        {

        }

        /// <summary>
        /// Destroys all boxes and clears variables.
        /// </summary>
        public override void Reset()
        {
            /*
            CleanupAllHitboxes();
            CleanupAllDetectboxes();
            hurtablesHit.Clear();
            detectboxesDetectedHurtables.Clear();
            hurtablesDetected.Clear();*/
        }

        /// <summary>
        /// Checks the hitboxes and detectboxes to see what they hit this frame.
        /// This should be called in late update, as physics update right after update.
        /// </summary>
        public override void TickBoxes()
        {
            base.TickBoxes();

            foreach(List<DetectionBox> detectboxGroup in detectboxes.Values)
            {
                for(int i = 0; i < detectboxGroup.Count; i++)
                {
                    detectboxGroup[i].CheckDetection();
                }
            }
        }

        /// <summary>
        /// Destroy all the boxes and clears the dictionary.
        /// </summary>
        private void CleanupAllDetectboxes()
        {
            foreach (int key in detectboxes.Keys)
            {
                for (int i = 0; i < detectboxes[key].Count; i++)
                {
                    GameObject.Destroy(detectboxes[key][i].gameObject);
                }
            }
            detectboxes.Clear();
        }

        /// <summary>
        /// Deactivate the detectboxes of the given index.
        /// </summary>
        /// <param name="index">The inex of the detectbox group.</param>
        public virtual void DeactivateDetectboxes(int index)
        {
            if (!detectboxes.ContainsKey(index))
            {
                return;
            }

            for(int i = 0; i < detectboxes[index].Count; i++)
            {
                detectboxes[index][i].Deactivate();
            }
        }

        /*
        /// <summary>
        /// Reactivates the hitboxes with the given ID.
        /// </summary>
        /// <param name="ID">The group to reactivate the hitboxes for.</param>
        public void ReactivateHitboxID(int ID)
        {
            Combat.AttackDefinition atk = combatManager.currentAttack.attackDefinition;

            hurtablesHit[ID]?.Clear();

            for(int i = 0; i < atk.hitboxGroups.Count; i++)
            {
                if(atk.hitboxGroups[i].ID == ID)
                {
                    if (hitboxes.ContainsKey(i))
                    {
                        for(int w = 0; w < hitboxes[i].Count; w++)
                        {
                            hitboxes[i][w].ReActivate(hurtablesHit[ID]);
                        }
                    }
                }
            }
        }*/

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        protected override void OnHitboxHurt(GameObject hurtableHit, HitInfo hitInfo, int hitboxID, int hitboxGroup)
        {
            //SoundDefinition sd = combatManager.controller.GameManager.ModManager
            //    .GetSoundDefinition(combatManager.currentAttack.attackDefinition.boxGroups[hitboxGroup].hitSound?.reference);
            //SoundManager.Play(sd, 0, controller.transform);
            base.OnHitboxHurt(hurtableHit, hitInfo, hitboxID, hitboxGroup);
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