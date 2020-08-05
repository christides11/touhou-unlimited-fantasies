using TUF.Modding;
using UnityEngine;

namespace TUF.UI
{
    [CreateAssetMenu(fileName = "UIOverrideDefiniton", menuName = "TUF/UIOverrideDefinition")]
    public class UIOverrideDefinition : ScriptableObject
    {
        public ModObjectLink uiDefinition;
        public UIOverrideBase uiPrefab;
    }
}