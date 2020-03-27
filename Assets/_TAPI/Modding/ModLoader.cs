using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using System.IO;
using System;
using TAPI.Core;

namespace TAPI.Modding
{
    [System.Serializable]
    public class ModLoader
    {
        public List<ModInfo> modList = new List<ModInfo>();
        public Dictionary<string, ModHost> loadedMods = new Dictionary<string, ModHost>();
        public string modInstallPath = "";
        private ModDirectory modDirectory = null;

        protected bool inited;
        private GameManager gameManager;
        private ModManager modManager;

        public virtual void Init(ModManager modManager, GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.modManager = modManager;
            modInstallPath = Path.Combine(Application.persistentDataPath, "Mods");
            Directory.CreateDirectory(modInstallPath);
            modDirectory = new ModDirectory(modInstallPath);
            inited = true;
            Debug.Log($"ModLoader initialized. Path: {modInstallPath}");
            UpdateModList();
        }

        public virtual void UpdateModList()
        {
            if (!inited)
            {
                Debug.Log("ModLoader was not initialized.");
                return;
            }

            modList.Clear();

            // Create a list of the mods we have in the mod directory.
            if (modDirectory.HasMods)
            {
                foreach (string modName in modDirectory.GetModNames())
                {
                    ModInfo mi = new ModInfo();
                    mi.path = modDirectory.GetModPath(modName);
                    mi.fileName = modName;
                    IModInfo modInfo = modDirectory.GetMod(mi.fileName);
                    mi.identifier = $"{modInfo.ModAuthor.ToLower()}.{modInfo.NameInfo.ModName.ToLower()}";
                    modList.Add(mi);
                    Debug.Log($"Found {mi.identifier} from {mi.path}");
                }
            }

            // Add mods from the command line.
            if (Mod.CommandLine.HasMods)
            {
                foreach (Uri modPath in Mod.CommandLine.AllMods)
                {
                    ModInfo mi = new ModInfo();
                    mi.commandLine = true;
                    mi.path = modPath;
                    mi.fileName = System.IO.Path.GetFileName(modPath.LocalPath);
                    IModInfo modInfo = ModDirectory.GetMod(new FileInfo(mi.path.LocalPath));
                    mi.identifier = $"{modInfo.ModAuthor.ToLower()}.{modInfo.NameInfo.ModName.ToLower()}";
                    modList.Add(mi);
                    Debug.Log($"(command line) Found {mi.identifier} from {mi.path}");
                }
            }
        }

        #region Loading
        public virtual bool LoadMod(string identifier)
        {
            if (modList.Exists(x => x.identifier == identifier))
            {
                return LoadMod(modList.Find(x => x.identifier == identifier));
            }
            return false;
        }

        public virtual bool LoadMod(ModInfo modInfo)
        {
            ModHost mod = Mod.Load(modInfo.path);
            if (mod.IsModLoaded)
            {
                if (mod.Assets.Exists("ModDefinition"))
                {
                    loadedMods.Add(modInfo.identifier, mod);
                    ModDefinition modDefinition = mod.Assets.Load("ModDefinition") as ModDefinition;
                    modManager.mods.Add(modInfo.identifier, modDefinition);
                    return true;
                }
                Debug.Log($"Mod {modInfo.identifier} does not have a definition.");
                mod.UnloadMod();
                return false;
            }
            Debug.Log($"Failed loading mod {modInfo.identifier} from {modInfo.path}.");
            return false;
        }
        #endregion

        #region Unloading
        public virtual bool UnloadMod(string modIdentifier)
        {
            if (loadedMods.ContainsKey(modIdentifier))
            {
                // Cleanup mod
                modManager.mods.Remove(modIdentifier);

                // Unload mod
                (loadedMods[modIdentifier] as ModHost).UnloadMod();
                loadedMods.Remove(modIdentifier);
                return true;
            }
            return false;
        }

        public virtual void UnloadAllMods()
        {
            foreach (string k in loadedMods.Keys)
            {
                (loadedMods[k] as ModHost).UnloadMod();
            }
            loadedMods.Clear();
        }
        #endregion

        public bool IsLoaded(string modIdentifier)
        {
            return loadedMods.ContainsKey(modIdentifier);
        }

        public IModInfo GetModInfo(ModInfo modInfo)
        {
            return ModDirectory.GetMod(new FileInfo(modInfo.path.LocalPath));
        }
    }
}