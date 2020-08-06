using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using UMod.BuildEngine;
using UnityEditor;
using UMod.Exporter;
using UMod.ModTools;
using UMod.ModTools.Export;
using UMod.Shared;
using System;
using Trivial.ImGUI;

namespace TUF.Modding
{
    [UModToolsWindow]
    public class BuildModWindow : EditorWindow
    {
        // Private
        private UModExporterResources resources = null;
        private ModToolsSettings toolSettings = null;
        private ExportSettings settings = null;
        private string currentBuildStatus = "unknown";
        private bool startBuild = false;
        private bool buildAndRun = false;
        private bool buildAndRunEnabled = false;

        // Methods
        /// <summary>
        /// Called on window open.
        /// </summary>
        protected void OnEnable()
        {

            // Load the last result
            currentBuildStatus = EditorPrefs.GetString("umod.exporter.lastbuildresult", "unknown");

            // Load the settings
            resources = UModExporterResources.LoadPackageData<UModExporterResources>();
            toolSettings = ModToolsSettings.Active.Load();

            // Load the settings
            settings = ExportSettings.Active.LoadOrCreate(FileUtil.GetProjectRelativePath(UModUtil.UModInstallDirectory.ToString()));

            // Check if build and run is enabled
            //buildAndRunEnabled = settings.ValidateExecutable();

            // Add update listener
            EditorApplication.update += () =>
            {
                // Check for build flag
                if (startBuild == true)
                {
                    startBuild = false;
                    LaunchBuild(false, OnFocus);
                }
            };
        }

        /// <summary>
        /// Called when window gains focus.
        /// </summary>
        public void OnFocus()
        {
            // Load the last result
            currentBuildStatus = EditorPrefs.GetString("umod.exporter.lastbuildresult", "unknown");

            // Check if build and run is enabled
            //buildAndRunEnabled = settings.ValidateExecutable();

            // Render the window
            Repaint();
        }

        public void OnGUI()
        {
            if(GUILayout.Button("Build Mod"))
            {
                buildAndRun = false;
                startBuild = true;

                // Update status
                currentBuildStatus = "In Progress!";

                // Repaint the window to trigger the GUI again
                Repaint();
            }
        }

        protected void ShowBuildTarget()
        {
            // Active target layout
            ImGUILayout.BeginLayout(ImGUILayoutType.Horizontal);
            {
                // Active target
                ImGUI.SetNextWidth(300);
                ImGUILayout.Label("Active Mod:");

                // Popup menu
                if (settings.ExportProfiles.Length == 0)
                {
                    ImGUI.PushEnabledVisualState(false);

                    // Show popup field
                    if (toolSettings.Options.AllowMultipleModsPerProject == true)
                    {
                        // Show an error popup
                        ImGUI.PopupItem("<Configuration Required!>");
                        ImGUILayout.Popup(0);
                    }
                    else
                    {
                        // Show configuration label
                        ImGUILayout.Label("<Configuration Required!>");
                    }

                    ImGUI.PopVisualState();
                }
                else
                {
                    // Show popup field
                    if (toolSettings.Options.AllowMultipleModsPerProject == true)
                    {
                        foreach (ExportProfileSettings item in settings.ExportProfiles)
                        {
                            string itemName = item.ModName;

                            // Check for error
                            if (string.IsNullOrEmpty(itemName) == true)
                                itemName = "<Configuration Required!>";

                            // Add the popup item
                            ImGUI.PopupItem(itemName);
                        }

                        // Display the popup
                        int index = ImGUILayout.Popup(settings.ActiveExportProfileIndex);

                        // Update active profile
                        if (index != settings.ActiveExportProfileIndex)
                        {
                            // Update selected export profile
                            settings.SetActiveExportProfile(index);

                            // Update mod references
                            ReferenceAssemblyLoader.LoadReferencedAssemblies();
                        }
                    }
                    else
                    {
                        // Show mod name label
                        ImGUILayout.Label(settings.ActiveExportProfile.ModName);
                    }
                }
            }
            ImGUILayout.EndLayout(); // End active target
        }

        /// <summary>
        /// Start a mod build.
        /// </summary>
        /// <param name="buildAndRun">Should the mod be launched in the target game when successfully built</param>
        /// <param name="onCompleted">Callback to invoke when the build is done regardless of success</param>
        public static void LaunchBuild(bool buildAndRun, Action onCompleted = null)
        {
            // Show export settings window when settings are invalid
            Action invalidSettingsCallback = () =>
            {
                // Missing required fields
                SettingsWindow.ShowWindow(true);
            };

            // Try to load settings
            ExportSettings settings = ExportSettings.Active.Load();
            ModBuildResult result = null;

            // Start build
            result = ModToolsUtil.StartBuild(settings, invalidSettingsCallback);

            if (result != null)
            {
                // Store success value
                EditorPrefs.SetString("umod.exporter.lastbuildresult", result.Successful ?
                    "successful" :
                    "Failed");
            }

            onCompleted?.Invoke();
        }
    }
}