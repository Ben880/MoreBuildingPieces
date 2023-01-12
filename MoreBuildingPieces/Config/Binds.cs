using BepInEx.Configuration;
using Jotunn.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace MoreBuildingPieces {
    public partial class PluginConfigs {



        public static ConfigEntry<KeyboardShortcut> DeleteLookingAt;
        public static ConfigEntry<KeyboardShortcut> PrintLookingAt;


        public static void BindConfigBinds(ConfigFile config) {
            Jotunn.Logger.LogInfo("    | Binds");
            DeleteLookingAt     = config.Bind("Binds", "DeleteLookingAt",   new KeyboardShortcut(KeyCode.Delete), ".");
            PrintLookingAt      = config.Bind("Binds", "PrintLookingAt",    new KeyboardShortcut(KeyCode.KeypadMultiply), ".");

        }
    }
}
