using System;
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
        private Dictionary<int, List<DetectionBox>> detectboxes = new Dictionary<int, List<DetectionBox>>();
        // Hitbox ID : IHurtables Hit
        private Dictionary<int, List<IHurtable>> hurtablesHit = new Dictionary<int, List<IHurtable>>();

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
        }

        /// <summary>
        /// Destroy the hitboxes and detectboxes.
        /// </summary>
        public void Cleanup()
        {
            CleanupAllHitboxes();
            CleanupAllDetectboxes();
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

            hitboxes.Remove(group);
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
            detectboxes.Remove(group);
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
                hitboxes[group][i].ReActivate(new List<IHurtable>() { combatManager });
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
                // Activate the hitbox and add it to our list.
                hbox.GetComponent<Hitbox>().OnHurt += (HitInfo hinfo) => { combatManager.hitStop = hinfo.attackerHitstop; };
                hbox.GetComponent<Hitbox>().Activate(controller.gameObject, controller.visualTransform,
                    currentGroup.hitInfo, new List<IHurtable>() { combatManager });
                hitboxList.Add(hbox.GetComponent<Hitbox>());
            }
            // Add the hitbox group to our list.
            hitboxes.Add(group, hitboxList);
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

            // Loop through all the hitboxes in the group.
            for (int i = 0; i < currentGroup.hitboxes.Count; i++)
            {
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
                        dbox.GetComponent<DetectionBox>().InitRectangle(currentGroup.hitboxes[i].size,
                            controller.visual.transform.eulerAngles + currentGroup.hitboxes[i].rotation);
                        break;
                    case ShapeType.Circle:
                        dbox.GetComponent<DetectionBox>().InitSphere(currentGroup.hitboxes[i].radius);
                        break;
                }

                if (currentGroup.attachToEntity)
                {
                    dbox.transform.SetParent(controller.transform);
                }
                dbox.GetComponent<DetectionBox>().Activate(controller.gameObject, controller.visualTransform,
                    currentGroup.hitInfo, new List<IHurtable>() { combatManager });
                detectionboxes.Add(dbox.GetComponent<DetectionBox>());
            }
            detectboxes.Add(group, detectionboxes);
        }
    }
}