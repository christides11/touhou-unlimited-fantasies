using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Entities;
using UnityEngine;

namespace TUF
{
    public class DetectionArea : MonoBehaviour
    {
        public List<EntityManager> entities = new List<EntityManager>();

        public EntityTeams team;

        public void OnTriggerEnter(Collider other)
        {
            EntityManager em;
            if (!other.TryGetComponent<EntityManager>(out em))
            {
                return;
            }

            if(!team.HasFlag((EntityTeams)em.CombatManager.GetTeam()))
            {
                return;
            }

            entities.Add(em);
        }

        public void OnTriggerExit(Collider other)
        {
            EntityManager em;
            if (!other.TryGetComponent<EntityManager>(out em))
            {
                return;
            }

            if (!entities.Contains(em))
            {
                return;
            }

            entities.Remove(em);
        }
    }
}