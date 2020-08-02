using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.ModdingTools
{
    public static class TUFDependencies
    {
        public static readonly Tuple<string, string>[] packages =
        {
            new Tuple<string, string>("com.github.siccity.xnode@1.8.0", "https://github.com/siccity/xNode.git#1.8.0"),
            new Tuple<string, string>("com.unity.cinemachine@2.5.0", "com.unity.cinemachine@2.5.0"),
            new Tuple<string, string>("com.malee.reorderablelist@1.0.1", "https://github.com/cfoulston/Unity-Reorderable-List.git#1.0.1"),
            new Tuple<string, string>("com.christides.character-action-framework@1.0.0", "https://github.com/christides11/Character-Action-Framework.git#upm/v1.0.0")
        };
        public static readonly List<string> dlls = new List<string>()
        {
            "UMod",
            "Kilosoft_CSharp",
            "kcc_CSharp",
            "AsyncAwait_CSharp",
            "Rewired_CSharp",
            "Rewired_Core",
            "Rewired_Windows",
            "Rewired_Linux",
            "Rewired_OSX",
            "TUF_CSharp"
        };
    }
}
