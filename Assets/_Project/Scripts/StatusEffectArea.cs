using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAF.Combat;

namespace TUF.Combat
{
    public class StatusEffectArea : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IStatusEffectable>() != null)
            {
                //DisableFloatStatusEffect dfse = new DisableFloatStatusEffect();
                //dfse.SetTime(-1);
                //other.GetComponent<IStatusEffectable>().ApplyStatusEffect(dfse);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IStatusEffectable>() != null)
            {
                //other.GetComponent<IStatusEffectable>().ApplyStatusEffect(dfse);
            }
        }
    }
}