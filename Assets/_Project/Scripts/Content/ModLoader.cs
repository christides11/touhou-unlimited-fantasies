using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using System.IO;
using System;
using TUF.Core;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace TUF.Modding
{
    /// <summary>
    /// Handles loading and unloading mods, along with keeping track of what is
    /// currently installed.
    /// </summary>
    [System.Serializable]
    public class ModLoader
    {
        public static string modsLoadedFileName = "LoadedMods.json";
        /// <summary>
        /// A list of all mods in the Mods folder.
        /// </summary>
        public List<ModInfo> modList = new List<ModInfo>();
        /// <summary>
        /// A list of all currently enabled mods.
        /// </summary>
        public Dictionary<string, ModHost> loadedMods = new Dictionary<string, ModHost>();
        /// <summary>
        /// The path where mods are installed.
        /// </summary>
        private string modInstallPath = "";
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
            modDirectory = new ModDirectory(modInstallPath, true, false);
            Mod.DefaultDirectory = modDirectory;
            inited = true;
            gameManager.ConsoleWindow.WriteLine($"ModLoader initialized. Path: {modInstallPath}");
            UpdateModList();
            List<string> loadedMods = SaveLoadService.Load<List<string>>("");
            if(loadedMods == null)
            {
                loadedMods = new List<string>();
            }
            List<string> unloadedMods = LoadMods(loadedMods);
            foreach(string um in unloadedMods)
            {
                loadedMods.Remove(um);
            }
            SaveLoadService.Save(modsLoadedFileName, JsonUtility.ToJson(loadedMods));
        }

        public virtual void UpdateModList()
        {
            if (!inited)
            {
                gameManager.ConsoleWindow.WriteLine("ModLoader was not initialized, can't update mod list.");
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
                    mi.modName = modInfo.NameInfo.ModName;
                    mi.identifier = $"{modInfo.ModAuthor.ToLower()}.{modInfo.NameInfo.ModName.ToLower()}";
                    modList.Add(mi);
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
                    mi.modName = modInfo.NameInfo.ModName;
                    mi.identifier = $"{modInfo.ModAuthor.ToLower()}.{modInfo.NameInfo.ModName.ToLower()}";
                    modList.Add(mi);
                }
            }
        }

        /// <summary>
        /// Check to see if any mods have loaded that we didn't catch.
        /// These will usually be dependencies since these get loaded automatically.
        /// </summary>
        protected virtual void CheckLoadedModList()
        {
            foreach(ModInfo mi in modList)
            {
                if(ModHost.IsModInUse(mi.path) && !loadedMods.ContainsKey(mi.identifier))
                {
                    gameManager.ConsoleWindow.WriteLine($"Found stray mod {mi.identifier}.");
                    LoadMod(mi);
                }
            }
        }

        #region Loading
        public virtual void LoadAllMods()
        {
            foreach(ModInfo mi in modList)
            {
                LoadMod(mi);
            }
        }

        public virtual bool LoadMod(string identifier)
        {
            if (modList.Exists(x => x.identifier == identifier))
            {
                return LoadMod(modList.Find(x => x.identifier == identifier));
            }
            return false;
        }

        public virtual List<string> LoadMods(List<string> identifiers)
        {
            List<string> notLoaded = new List<string>();
            for(int i = 0; i < identifiers.Count; i++)
            {
                if (!LoadMod(identifiers[i]))
                {
                    notLoaded.Add(identifiers[i]);
                }
            }
            return notLoaded;
        }

        public virtual bool LoadMod(ModInfo modInfo)
        {
            if(loadedMods.ContainsKey(modInfo.identifier)){
                gameManager.ConsoleWindow.WriteLine($"Mod {modInfo.identifier} is already loaded.");
                return false;
            }

            ModHost mod = Mod.Load(modInfo.path);

            try
            {
                if (mod.IsModLoaded)
                {
                    if (mod.Assets.Exists("ModDefinition"))
                    {
                        ModDefinition modDefinition = mod.Assets.Load("ModDefinition") as ModDefinition;
                        loadedMods.Add(modInfo.identifier, mod);
                        modManager.mods.Add(modInfo.identifier, modDefinition);
                        gameManager.ConsoleWindow.WriteLine($"Loaded mod {modInfo.identifier}.");
                        CheckLoadedModList();
                        return true;
                    }
                    throw new Exception($"No ModDefinition found.");
                }
                throw new Exception($"{mod.LoadResult.Error}");
            }catch(Exception e)
            {
                gameManager.ConsoleWindow.WriteLine($"Failed loading mod {modInfo.identifier}: {e.Message}");
                if (mod.IsModLoaded)
                {
                    mod.UnloadMod();
                }
                CheckLoadedModList();
                return false;
            }
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