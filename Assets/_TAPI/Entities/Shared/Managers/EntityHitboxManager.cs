﻿using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;
using TAPI.Core;
using TAPI.Sound;

namespace TAPI.Entities
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class EntityHitboxManager
    {
        public delegate void HitboxGroupEventAction(GameObject hurtableHit, int hitboxGroupIndex, MovesetAttackNode attack);
        public event HitboxGroupEventAction OnHitboxHit;

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

        /// <summary>
        /// Destroys all boxes and clears variables.
        /// </summary>
        public void Reset()
        {
            CleanupAllBoxes<Hitbox>(ref hitboxes);
            CleanupAllBoxes<DetectionBox>(ref detectboxes);
            hurtablesHit.Clear();
            detectboxesDetectedHurtables.Clear();
            hurtablesDetected.Clear();
        }

        /// <summary>
        /// Checks the hitboxes and detectboxes to see what they hit this frame.
        /// This should be called in late update, as physics update right after update.
        /// </summary>
        public void TickBoxes()
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
        /// Destroy all the boxes and clears the dictionary.
        /// </summary>
        private void CleanupAllBoxes<T>(ref Dictionary<int, List<T>> d)
        {
            foreach(int key in d.Keys)
            {
                for (int i = 0; i < hitboxes[key].Count; i++)
                {
                    GameObject.Destroy(hitboxes[key][i].gameObject);
                }
            }
            d.Clear();
        }

        /// <summary>
        /// Deactivate the hitboxes of the given index.
        /// </summary>
        /// <param name="index">The index of the hitbox group.</param>
        public virtual void DeactivateHitboxes(int index)
        {
            if (!hitboxes.ContainsKey(index))
            {
                return;
            }

            for(int i = 0; i < hitboxes[index].Count; i++)
            {
                hitboxes[index][i].Deactivate();
            }
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

        /// <summary>
        /// Reactivates the hitboxes with the given ID.
        /// </summary>
        /// <param name="ID">The group to reactivate the hitboxes for.</param>
        public void ReactivateHitboxID(int ID)
        {
            AttackSO atk = combatManager.currentAttack.action;

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
        }

        #region Hitboxes
        /// <summary>
        /// Create the hitboxes of the given hitbox group index.
        /// </summary>
        /// <param name="index">The index to create the hitboxes for.</param>
        public void CreateHitboxes(int index)
        {
            // Hitboxes were already created.
            if (hitboxes.ContainsKey(index))
            {
                return;
            }

            // Variables.
            HitboxGroup currentGroup = combatManager.currentAttack.action.hitboxGroups[index];
            if (currentGroup.hitGroupType != HitboxGroupType.HIT)
            {
                return;
            }
            List<Hitbox> hitboxList = new List<Hitbox>(currentGroup.hitboxes.Count);
            
            // Keep track of what this ID has hit.
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
                int groupLocalVar = index;
                // Activate the hitbox and add it to our list.
                hbox.GetComponent<Hitbox>().OnHurt += (hurtable, hInfo) => { OnHitboxHurt(hurtable, hInfo, cID, index); };
                hbox.GetComponent<Hitbox>().Activate(controller.gameObject, controller.visualTransform,
                    currentGroup.hitInfo, hurtablesHit[currentGroup.ID]);
                hitboxList.Add(hbox.GetComponent<Hitbox>());
            }
            // Add the hitbox group to our list.
            hitboxes.Add(index, hitboxList);
        }

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        private void OnHitboxHurt(GameObject hurtableHit, HitInfo hitInfo, int hitboxID, int hitboxGroup)
        {
            SoundDefinition sd = combatManager.controller.GameManager.ModManager.GetSoundDefinition(combatManager.currentAttack.action.hitboxGroups[hitboxGroup].hitSound?.reference);
            SoundManager.Play(sd, 0, controller.transform);
            hurtablesHit[hitboxID].Add(hurtableHit.GetComponent<IHurtable>());
            combatManager.hitStop = hitInfo.attackerHitstop;
            UpdateIDHitboxGroup(hitboxID);
            OnHitboxHit?.Invoke(hurtableHit, hitboxGroup, combatManager.currentAttack);
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
        #endregion

        #region Detectboxes
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