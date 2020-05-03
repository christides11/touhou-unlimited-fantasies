﻿using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;
using TAPI.Core;

namespace TAPI.Entities
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class EntityHitboxManager
    {
        // Hitbox Group : Hitboxes
        private Dictionary<int, List<Hitbox>> hitboxes = new Dictionary<int, List<Hitbox>>();
        // Hitbox ID : IHurtables Hit
        private Dictionary<int, List<IHurtable>> hurtablesHit = new Dictionary<int, List<IHurtable>>();

        // Detectbox Group : Detectboxes
        private Dictionary<int, List<DetectionBox>> detectboxes = new Dictionary<int, List<DetectionBox>>();
        // Detectbox Group : IHurtables Hit
        private Dictionary<int, List<IHurtable>> detectboxesDetectedHurtables = new Dictionary<int, List<IHurtable>>();
        // Detectbox ID : IHurtables Hit
        private Dictionary<int, List<IHurtable>> hurtablesDetected = new Dictionary<int, List<IHurtable>>();

        private EntityCombatManager combatManager;
        private EntityController controller;

        public EntityHitboxManager(EntityCombatManager combatManager, EntityController controller)
        {
            this.combatManager = combatManager;
            this.controller = controller;
        }

        public void LateUpdate()
        {
            foreach (List<Hitbox> hitboxGroup in hitboxes.Values)
            {
                for (int i = 0; i < hitboxGroup.Count; i++)
                {
                    hitboxGroup[i].CheckHits();
                }
            }

            foreach(List<DetectionBox> detectboxGroup in detectboxes.Values)
            {
                for(int i = 0; i < detectboxGroup.Count; i++)
                {
                    detectboxGroup[i].CheckDetection();
                }
            }
        }

        /// <summary>
        /// Destroy the hitboxes and detectboxes.
        /// </summary>
        public void Cleanup()
        {
            CleanupAllHitboxes();
            CleanupAllDetectboxes();
            hurtablesHit.Clear();
            detectboxesDetectedHurtables.Clear();
            hurtablesDetected.Clear();
        }

        /// <summary>
        /// Destroy all hitboxes and empty the dictionary.
        /// </summary>
        private void CleanupAllHitboxes()
        {
            foreach(int key in hitboxes.Keys)
            {
                CleanupHitboxes(key);
            }
            hitboxes.Clear();
        }

        /// <summary>
        /// Destory all detectboxes and empty the dictionary.
        /// </summary>
        private void CleanupAllDetectboxes()
        {
            foreach(int key in detectboxes.Keys)
            {
                CleanupDetectboxes(key);
            }
            detectboxes.Clear();
        }

        /// <summary>
        /// Cleanup the hitboxes of the given group.
        /// </summary>
        /// <param name="group">The group to clean the hitboxes for.</param>
        public virtual void CleanupHitboxes(int group)
        {
            if (!hitboxes.ContainsKey(group))
            {
                return;
            }

            for (int i = 0; i < hitboxes[group].Count; i++)
            {
                GameObject.Destroy(hitboxes[group][i].gameObject);
            }
        }

        /// <summary>
        /// Cleanup the detectboxes of the given group.
        /// </summary>
        /// <param name="group">The group to clean the hitboxes for.</param>
        public virtual void CleanupDetectboxes(int group)
        {
            if (!detectboxes.ContainsKey(group))
            {
                return;
            }

            for (int i = 0; i < detectboxes[group].Count; i++)
            {
                GameObject.Destroy(detectboxes[group][i].gameObject);
            }
        }

        public virtual void DeactivateHitboxes(int group)
        {
            if (!hitboxes.ContainsKey(group))
            {
                return;
            }

            for(int i = 0; i < hitboxes[group].Count; i++)
            {
                hitboxes[group][i].Deactivate();
            }
        }

        public virtual void DeactivateDetectboxes(int group)
        {
            if (!detectboxes.ContainsKey(group))
            {
                return;
            }

            for(int i = 0; i < detectboxes[group].Count; i++)
            {
                detectboxes[group][i].Deactivate();
            }
        }

        /// <summary>
        /// Reactivates the hitboxes within a given group.
        /// </summary>
        /// <param name="group">The group to reactivate the hitboxes for.</param>
        public void ReactivateHitboxes(int group)
        {
            if (!hitboxes.ContainsKey(group))
            {
                return;
            }

            for (int i = 0; i < hitboxes[group].Count; i++)
            {
                hitboxes[group][i].ReActivate(hurtablesHit[combatManager.currentAttack.action.hitboxGroups[group].ID]);
            }
        }

        /// <summary>
        /// Create the hitboxes of the given group.
        /// </summary>
        /// <param name="group">The hitbox group to create the hitboxes for.</param>
        public void CreateHitboxes(int group)
        {
            // Hitboxes were already created.
            if (hitboxes.ContainsKey(group))
            {
                return;
            }

            // Variables.
            HitboxGroup currentGroup = combatManager.currentAttack.action.hitboxGroups[group];
            if (currentGroup.hitGroupType != HitboxGroupType.HIT)
            {
                return;
            }
            List<Hitbox> hitboxList = new List<Hitbox>(currentGroup.hitboxes.Count);
            if (!hurtablesHit.ContainsKey(currentGroup.ID))
            {
                hurtablesHit.Add(currentGroup.ID, new List<IHurtable>());
                hurtablesHit[currentGroup.ID].Add(combatManager);
            }

            // Loop through all the hitboxes in the group.
            for (int i = 0; i < currentGroup.hitboxes.Count; i++)
            {
                // Instantiate the hitbox with the correct position and rotation.
                HitboxDefinition hitboxDefinition = currentGroup.hitboxes[i];
                Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * hitboxDefinition.offset.z
                    + controller.GetVisualBasedDirection(Vector3.right) * hitboxDefinition.offset.x
                    + controller.GetVisualBasedDirection(Vector3.up) * hitboxDefinition.offset.y;
                GameObject hbox = GameObject.Instantiate(controller.GameManager.gameVars.combat.hitbox,
                    controller.transform.position + pos,
                    Quaternion.Euler(controller.transform.eulerAngles + hitboxDefinition.rotation));

                // Initialize it with the correct shape.
                switch (currentGroup.hitboxes[i].shape)
                {
                    case ShapeType.Rectangle:
                        hbox.GetComponent<Hitbox>().InitRectangle(currentGroup.hitboxes[i].size,
                            controller.visual.transform.eulerAngles + currentGroup.hitboxes[i].rotation);
                        break;
                    case ShapeType.Circle:
                        hbox.GetComponent<Hitbox>().InitSphere(currentGroup.hitboxes[i].radius);
                        break;
                }

                // Attach the hitbox if neccessary.
                if (currentGroup.attachToEntity)
                {
                    hbox.transform.SetParent(controller.transform);
                }
                int cID = currentGroup.ID;
                // Activate the hitbox and add it to our list.
                hbox.GetComponent<Hitbox>().OnHurt += (hurtable, hInfo) => { OnHitboxHurt(hurtable, hInfo, cID); };
                hbox.GetComponent<Hitbox>().Activate(controller.gameObject, controller.visualTransform,
                    currentGroup.hitInfo, hurtablesHit[currentGroup.ID]);
                hitboxList.Add(hbox.GetComponent<Hitbox>());
            }
            // Add the hitbox group to our list.
            hitboxes.Add(group, hitboxList);
        }

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        private void OnHitboxHurt(GameObject hurtableHit, HitInfo hitInfo, int hitboxID)
        {
            hurtablesHit[hitboxID].Add(hurtableHit.GetComponent<IHurtable>());
            combatManager.hitStop = hitInfo.attackerHitstop;
            UpdateIDHitboxGroup(hitboxID);
        }

        /// <summary>
        /// Updates the ignore/hit list of all hitboxes with the given ID.
        /// </summary>
        /// <param name="hitboxID">The ID to update the hitboxes for.</param>
        private void UpdateIDHitboxGroup(int hitboxID)
        {
            foreach(int key in hitboxes.Keys)
            {
                for(int i = 0; i < hitboxes[key].Count; i++)
                {
                    hitboxes[key][i].ignoreList = hurtablesHit[hitboxID];
                }
            }
        }

        /// <summary>
        /// Create the detectboxes of the given group.
        /// </summary>
        /// <param name="group">The hitbox group to create the detectboxes for.</param>
        public void CreateDetectboxes(int group)
        {
            if (detectboxes.ContainsKey(group))
            {
                return;
            }

            // Variables.
            HitboxGroup currentGroup = combatManager.currentAttack.action.hitboxGroups[group];
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
                HitboxDefinition currHitbox = currentGroup.hitboxes[i];
                Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * currHitbox.offset.z
                    + controller.GetVisualBasedDirection(Vector3.right) * currHitbox.offset.x
                    + controller.GetVisualBasedDirection(Vector3.up) * currHitbox.offset.y;
                GameObject dbox = GameObject.Instantiate(controller.GameManager.gameVars.combat.detectbox,
                    controller.transform.position + pos,
                    Quaternion.Euler(controller.transform.eulerAngles + currHitbox.rotation));

                switch (currentGroup.hitboxes[i].shape)
                {
                    case ShapeType.Rectangle:
                        dbox.GetComponent<DetectionBox>().Initialize(controller.gameObject, currentGroup.hitboxes[i].size,
                            controller.visual.transform.eulerAngles + currentGroup.hitboxes[i].rotation);
                        break;
                    case ShapeType.Circle:
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
            detectboxes.Add(group, detectionboxes);
        }

        /// <summary>
        /// Called whenever a detectbox detects a hurtable.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was detected.</param>
        /// <param name="hitboxID">The ID of the detectbox.</param>
        private void OnDetectboxDetect(GameObject hurtable, int hitboxID, int hitboxGroup)
        {
            Debug.Log($"Detectbox. {hurtable.name}");
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
    }
}