using UnityEngine;

namespace TUF.UI
{
    [CreateAssetMenu(fileName = "UIDefiniton", menuName = "TUF/UIDefinition")]
    public class UIDefinition : ScriptableObject
    {
        public string id;
        public string uiName;
    }
}