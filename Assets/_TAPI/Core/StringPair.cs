using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Core
{
    [System.Serializable]
    public class StringPair<T>
    {
        public string key;
        public T value;

        public StringPair()
        {

        }

        public StringPair(string key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
}