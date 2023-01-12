// MoreBuildingPieces
// a Valheim mod skeleton using Jötunn
// 
// File:    MoreBuildingPieces.cs
// Project: MoreBuildingPieces

using System;
using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Configs;
using HarmonyLib;
using MoreBuildingPieces.Behavior;
using MoreBuildingPieces.Commands;

namespace MoreBuildingPieces
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MoreBuildingPieces : BaseUnityPlugin
    {
        public const string PluginGUID = "com.bendover.MoreBuildingPieces";
        public const string PluginName = "MoreBuildingPieces";
        public const string PluginVersion = "0.1.0";

        private readonly Harmony harmony = new Harmony("com.bendover.MoreBuildingPieces");

        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("MoreBuildingPieces has landed");

            PluginConfigs.BindConfig(Config);
            LoadCommands.Load();
            Behavior.PieceManager.LoadData();
            
            harmony.PatchAll();
        }


        private void Update() {
            Behavior.Input.Update();
        }
    }
}

