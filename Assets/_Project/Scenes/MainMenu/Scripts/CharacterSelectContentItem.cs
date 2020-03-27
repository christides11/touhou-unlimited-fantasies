using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Touhou.Menus
{
    public class CharacterSelectContentItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterName;

        public void SetName(string characterName)
        {
            this.characterName.text = characterName;
        }
    }
}