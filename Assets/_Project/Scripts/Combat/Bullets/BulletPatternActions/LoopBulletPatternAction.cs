using System.Collections;
using System.Collections.Generic;
using TUF.Combat.Bullets;
using UnityEngine;

namespace TUF.Combat
{
    [System.Serializable]
    public class LoopBulletPatternAction : BulletPatternAction
    {
        public int loopAmount = 0;
        public int loopToPosition = 0;

        public string variableName = "a";

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            if (!data.floatVariables.ContainsKey(variableName))
            {
                data.floatVariables.Add(variableName, 0);
            }
            if (manager.settings.disableLooping)
            {
                data.floatVariables.Remove(variableName);
            }
            else
            {
                if (loopAmount == -1)
                {
                    data.patternPosition = loopToPosition - 1;
                }
                else
                {
                    if (((int)data.floatVariables[variableName]) < loopAmount)
                    {
                        data.floatVariables[variableName] += 1;
                        data.patternPosition = loopToPosition - 1;
                    }
                    else
                    {
                        data.floatVariables.Remove(variableName);
                    }
                }
            }
            return false;
        }
    }
}