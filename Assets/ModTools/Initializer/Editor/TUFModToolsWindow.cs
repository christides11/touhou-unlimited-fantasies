using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Reflection;

namespace TUF.ModdingTools
{
    public class TUFModToolsWindow : EditorWindow
    {
        public int currentTab = 0;

        public static ListRequest listRequest;

        // Add menu named "My Window" to the Window menu
        [MenuItem("TUF/Initializer")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TUFModToolsWindow window = (TUFModToolsWindow)EditorWindow.GetWindow(typeof(TUFModToolsWindow));
            window.Show();
        }

        private void OnEnable()
        {
            listRequest = Client.List();
        }

        private void OnGUI()
        {
            switch (currentTab)
            {
                case 0:
                    InitializationTab();
                    break;
            }
        }

        private void InitializationTab()
        {
            if (listRequest.Status != StatusCode.Success)
            {
                return;
            }

            int packagesInstalled = 0;
            GUILayout.Label("1. Install Packages");
            for (int i = 0; i < TUFDependencies.packages.Length; i++)
            {
                var pName = TUFDependencies.packages[i];
                var res = listRequest.Result.FirstOrDefault(x => ($"{x.name}@{x.version}" == pName.Item1));
                if (res == null)
                {
                    if (GUILayout.Button($"{pName.Item1}"))
                    {
                        AddRequest currentRequest = Client.Add(TUFDependencies.packages[i].Item2);
                    }
                }
                else
                {
                    packagesInstalled++;
                }
            }

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(packagesInstalled < TUFDependencies.packages.Length);
            GUILayout.Label("2. Finalize");
            if (GUILayout.Button("Finalize"))
            {
                // GET DLLs
                string path = EditorUtility.OpenFilePanel("Select Game Executable", "", "exe");

                if (!File.Exists(path))
                {
                    Debug.LogError("Could not find game executable.");
                    return;
                }

                var source = Path.GetDirectoryName(path) + @"\Touhou-UF_Data\Managed";
                Debug.Log(source);
                Debug.Log(Application.dataPath + @"/assemblies");
                var destination = Application.dataPath + @"/assemblies";
                if (Directory.Exists(destination))
                {
                    Directory.Delete(destination, true);
                }
                Directory.CreateDirectory(destination);

                foreach (string assembly in TUFDependencies.dlls)
                {
                    if (File.Exists($@"{source}\{assembly}.dll"))
                    {
                        File.Copy($@"{source}\{assembly}.dll", $@"{destination}/{assembly}.dll", true);
                    }
                    else
                    {
                        Debug.LogWarning($@"Modding Tools: Couldn't find {assembly}.dll.");
                    }
                }

                // IMPORT PACKAGE
                AssetDatabase.ImportPackage(Path.GetDirectoryName(path) + @"\Modding\TUFModdingTools.unitypackage", false);

                AssetDatabase.Refresh();

                ClearConsole();

                Debug.Log("Successfully imported mod tools.");
            }
            EditorGUI.EndDisabledGroup();
        }

        private void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}