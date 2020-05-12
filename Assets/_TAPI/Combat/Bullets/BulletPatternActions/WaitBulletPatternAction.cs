using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    public class WaitBulletPatternAction : BulletPatternAction
    {
        public int waitFrames;

        public string variableName;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            if (!data.floatVariables.ContainsKey(variableName))
            {
                data.floatVariables.Add(variableName, 0);
            }

            if(((int)data.floatVariables[variableName]) < waitFrames)
            {
                data.floatVariables[variableName] += 1;
                return true;
            }
            data.floatVariables.Remove(variableName);
            return false;
        }
    }
}