using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Modding
{
    /// <summary>
    /// Info that is stored for any currently loaded mod.
    /// The identifier follows the format of "author.modName".
    /// </summary>
    [System.Serializable]
    public class ModInfo
    {
        public Uri path;
        public string fileName;
        public string identifier;
        public bool commandLine;
    }
}