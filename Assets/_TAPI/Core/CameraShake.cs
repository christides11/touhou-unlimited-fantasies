using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TAPI.Inputs;

namespace TAPI.Core
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook freeLook;

        private CameraShakeDefinition currentShake;

        private float currentAmplitude;
        private float currentFrequency;

        private float holdTimer;
        private float fadeTimer;

        private void Update()
        {
            if (currentShake != null)
            {
                if(holdTimer < currentShake.holdLength)
                {
                    holdTimer += Time.deltaTime;
                }
                else if(fadeTimer < currentShake.fadeLength)
                {
                    currentAmplitude *= currentShake.amplitudeDecay;
                    currentFrequency *= currentShake.frequencyDecay;
                    fadeTimer += Time.deltaTime;
                }
                else
                {
                    currentAmplitude = 0;
                    currentFrequency = 0;
                    currentShake = null;
                    holdTimer = 0;
                    fadeTimer = 0;
                }
                UpdateValues();
            }
        }

        public void Shake(CameraShakeDefinition definition)
        {
            currentShake = definition;
            currentAmplitude = currentShake.amplitude;
            currentFrequency = currentShake.frequency;
            holdTimer = 0;
            fadeTimer = 0;
            UpdateValues();
        }

        public void UpdateValues()
        {
            for (int i = 0; i < 3; i++)
            {
                var c = freeLook.GetRig(i).GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
                c.m_AmplitudeGain = currentAmplitude;
                c.m_FrequencyGain = currentFrequency;
            }
        }
    }
}
