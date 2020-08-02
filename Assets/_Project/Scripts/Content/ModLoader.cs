using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using System.IO;
using System;
using TUF.Core;

namespace TUF.Modding
{
    /// <summary>
    /// Handles loading and unloading mods, along with keeping track of what is
    /// currently installed.
    /// </summary>
    [System.Serializable]
    public class ModLoader
    {
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
            inited = true;
            gameManager.ConsoleWindow.WriteLine($"ModLoader initialized. Path: {modInstallPath}");
            UpdateModList();
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
                    mi.identifier = $"{modInfo.ModAuthor.ToLower()}.{modInfo.NameInfo.ModName.ToLower()}";
                    modList.Add(mi);
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

        public virtual bool LoadMod(ModInfo modInfo)
        {
            if(Mod.IsModLoaded(modInfo.path)){
                return false;
            }

            ModHost mod = Mod.Load(modInfo.path);

            if (mod.IsModLoaded)
            {
                GetReferencedMods(mod);

                if (mod.Assets.Exists("ModDefinition"))
                {
                    loadedMods.Add(modInfo.identifier, mod);
                    ModDefinition modDefinition = mod.Assets.Load("ModDefinition") as ModDefinition;
                    modManager.mods.Add(modInfo.identifier, modDefinition);
                }
                gameManager.ConsoleWindow.WriteLine($"Loaded mod {modInfo.identifier}.");
                return true;
            }
            gameManager.ConsoleWindow.WriteLine($"Failed loading mod {modInfo.identifier} from {modInfo.path}.");
            return false;
        }

        /// <summary>
        /// When mods are loaded, any mod that they reference are loaded alongside them.
        /// This method makes sure that these mods are added to our loaded list.
        /// </summary>
        /// <param name="mod"></param>
        private void GetReferencedMods(ModHost mod)
        {
            foreach(IModInfo referencedMod in mod.ReferencedMods)
            {

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