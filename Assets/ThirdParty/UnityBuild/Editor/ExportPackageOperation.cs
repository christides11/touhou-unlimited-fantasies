using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SuperSystems.UnityBuild
{
    public class ExportPackageOperation : BuildAction, IPreBuildAction, IPreBuildPerPlatformAction, IPostBuildAction, IPostBuildPerPlatformAction
    {
        [FilePath(true)]
        public string packageFolderPath;
        [FilePath(true)]
        public string outputPath;
        public string packageName;

        public override void Execute()
        {

        }

        public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform, BuildArchitecture architecture, BuildDistribution distribution, System.DateTime buildTime, ref BuildOptions options, string configKey, string buildPath)
        {
            string resolvedInputPath = BuildProject.ResolvePath(packageFolderPath.Replace("$BUILDPATH", buildPath), releaseType, platform, architecture, distribution, buildTime);
            string resolvedOutputPath = BuildProject.ResolvePath(outputPath.Replace("$BUILDPATH", buildPath), releaseType, platform, architecture, distribution, buildTime);

            Export(resolvedInputPath, resolvedOutputPath);
        }

        private void Export(string iPath, string oPath)
        {
            List<string> tempList = new List<string>();
            tempList.AddRange(Directory.GetFiles(iPath, "*.*"));
            tempList.AddRange(Directory.GetDirectories(iPath));

            string exportPackage = oPath + $"/{packageName}.unitypackage";
            ExportPackageOptions exportFlags = ExportPackageOptions.Default | ExportPackageOptions.Recurse;
            AssetDatabase.ExportPackage(tempList.ToArray(), exportPackage, exportFlags);
        }

        protected override void DrawProperties(SerializedObject obj)
        {
            EditorGUILayout.PropertyField(obj.FindProperty("packageFolderPath"));
            EditorGUILayout.PropertyField(obj.FindProperty("outputPath"));
            EditorGUILayout.PropertyField(obj.FindProperty("packageName"));
        }
    }
}
