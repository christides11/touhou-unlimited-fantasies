using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TUF.Core;
using TUF.Entities;
using TUF.Combat;

namespace TUF.Stages
{
    public class TriggerZone : MonoBehaviour
    {
        public TriggerType triggerType;
        public LayerMask layerMask;
        public EntityTeams team;
        public UnityEvent onTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!CheckCollider(other, out EntityManager em))
            {
                return;
            }
            onTriggered.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!CheckCollider(other, out EntityManager em))
            {
                return;
            }
            onTriggered.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!CheckCollider(other, out EntityManager em))
            {
                return;
            }
            onTriggered.Invoke();
        }

        protected virtual bool CheckCollider(Collider other, out EntityManager em)
        {
            em = null;
            if (triggerType != TriggerType.OnEnter)
            {
                return false;
            }
            if (!layerMask.Contains(other.gameObject.layer))
            {
                return false;
            }
            if (!other.TryGetComponent(out em))
            {
                return false;
            }
            if (!team.HasFlag((EntityTeams)em.CombatManager.GetTeam()))
            {
                return false;
            }
            return true;
        }
    }
}