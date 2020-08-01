using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Tools
{
    public class TestThingy : MonoBehaviour
    {
        void Start()
        {
            Debug.Log(GameManager.current);
        }
    }
}