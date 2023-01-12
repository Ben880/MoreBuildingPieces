using BepInEx.Configuration;
using Jotunn.Configs;
using MonoMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace MoreBuildingPieces {

    public partial class PluginConfigs {
        public static ConfigEntry<bool> IsModEnabled { get; private set; }

        // sets
        public static ConfigEntry<bool> BlackMarbleSet { get; private set; }
        public static ConfigEntry<bool> DvergrSet { get; private set; }
        public static ConfigEntry<bool> OtherSet { get; private set; }
        public static ConfigEntry<bool> GoblinSet { get; private set; }
        public static ConfigEntry<bool> ExpSet { get; private set; }

        // config
        public static ConfigEntry<bool> FillLoot { get; private set; }
        public static ConfigEntry<bool> DropLoot { get; private set; }
        public static ConfigEntry<float> InteractionRange { get; private set; }
        public static ConfigEntry<bool> DevTools { get; private set; }

        // shouts
        public static ConfigEntry<String> ArriveMessage;
        public static ConfigEntry<String> ShoutMessage;
        public static ConfigEntry<KeyboardShortcut> SendShoutButton;
   

        public static void BindConfig(ConfigFile config) {
            Jotunn.Logger.LogInfo("MoreBuildingPieces loading configs");
            Jotunn.Logger.LogInfo("    | Main");
            // Global
            IsModEnabled = config.Bind<bool>("Global", "isModEnabled", true, "Globally enable or disable this mod (restart required).");

            //sets
            BlackMarbleSet = config.Bind<bool>("Sets", "BlackMarbleSet", true, "Load Black Marble Set (restart required).");
            DvergrSet = config.Bind<bool>("Sets", "DvergrSets", true, "Load Dvergr Sets (restart required).");
            GoblinSet = config.Bind<bool>("Sets", "GoblinSets", true, "Load Set (restart required).");
            OtherSet = config.Bind<bool>("Sets", "OtherSets", true, "Load Set (restart required).");
            ExpSet = config.Bind<bool>("Sets", "ExpSets", false, "Load Experimental Sets (restart required).");

            //config
            FillLoot = config.Bind<bool>("Config", "FillLoot", false, "should placed containers filled with loot loot.");
            DropLoot = config.Bind<bool>("Config", "DropLoot", false, "should placed buildings drop loot or the items used to build them.");
            InteractionRange = config.Bind<float>("Config", "InteractionRange", 20f, "interaction range for mod.");
            DevTools = config.Bind<bool>("Sets", "DevTools", false, "Allow dev tools.");

            // shouts
            var defaultSendShoutButton = new KeyboardShortcut(KeyCode.S, KeyCode.LeftControl);
            SendShoutButton = config.Bind("Shout", "SendShoutButton", defaultSendShoutButton, "Button to send shout.");
            ShoutMessage = config.Bind("Shout", "ShoutMessage", "Some string", "Client side string");
            ArriveMessage = config.Bind("Shout", "ArriveMessage", "I HAVE CUM!", "Client side string");


            BindConfigBinds(config);
        }

        public static bool ShouldLoadSet(string set) {
            switch (set) {
                case "BlackMarble":
                    return BlackMarbleSet.Value;
                case "Dvergr":
                    return DvergrSet.Value;
                case "Other":
                    return OtherSet.Value;
                case "Goblin":
                    return GoblinSet.Value;
                case "EXP-Caution":
                    return ExpSet.Value;
                default:
                    return false;
            }
        }
    }
}
