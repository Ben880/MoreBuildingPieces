using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using MoreBuildingPieces.Tools;
using static Mono.Security.X509.X520;


namespace MoreBuildingPieces.Behavior {
    internal class PieceManager {
       
        private static Tools.CSVLoader<PieceData> csvData;
        private static Dictionary<string, PieceData> pieces;

        public static void LoadPieces() {
            Jotunn.Logger.LogInfo("Loading Pieces");
            FindAndRegisterPrefabs();
        }

        public static void LoadData() {
            Jotunn.Logger.LogInfo("Loading Piece Data");
            csvData = new Tools.CSVLoader<PieceData>("PieceData.csv");
            pieces = csvData.LoadToObjectDict();
        }

        public static bool ContainsPiece(string pieceName) {
            return pieces.ContainsKey(pieceName);
        }

        public static PieceData GetPiece(string pieceName) {
            return pieces[pieceName];
        }

        private static void SetupPieceComponent(Piece pieceComponent) {

            if (pieceComponent == null) return;

            pieceComponent.m_enabled = true;
            pieceComponent.m_canBeRemoved = true;
            pieceComponent.m_clipEverything = true;
            pieceComponent.m_groundPiece = false;
            pieceComponent.m_groundOnly = false;
            pieceComponent.m_noInWater = false;
            pieceComponent.m_notOnWood = false;
            pieceComponent.m_notOnTiltingSurface = false;
            pieceComponent.m_notOnFloor = false;
            pieceComponent.m_allowedInDungeons = true;
            pieceComponent.m_onlyInTeleportArea = false;
            pieceComponent.m_inCeilingOnly = false;
            pieceComponent.m_cultivatedGroundOnly = false;
            pieceComponent.m_onlyInBiome = Heightmap.Biome.None;
            pieceComponent.m_allowRotatedOverlap = true;
        }

        private static Sprite CreatePrefabIcon(GameObject prefab) {
            Sprite result = RenderManager.Instance.Render(prefab, RenderManager.IsometricRotation);

            return result;
        }

        private static void LoadPiece(GameObject gameObject) {
            string name = gameObject.name;

            if (gameObject.GetComponent<Piece>() == null) {
                Piece pieceComponent = gameObject.AddComponent<Piece>();
                SetupPieceComponent(pieceComponent);
            }
            
            if (GetPiece(name).Closeable) {
                gameObject.GetComponent<Door>().m_canNotBeClosed = false;
            }

            var pieceConfig = new PieceConfig {
                Name = name,
                Description = name,
                PieceTable = "_HammerPieceTable",
                Category = GetPiece(name).Category,
                AllowedInDungeons = true,
                Icon = CreatePrefabIcon(gameObject),
                CraftingStation = GetPiece(name).CraftingStation
            };

            Jotunn.Logger.LogInfo($"Adding Requirements for {name}");
            foreach (RequirementConfig req in GetPiece(name).Requirements) {
                Jotunn.Logger.LogInfo($"    | {req.Item}x{req.Amount}");
                pieceConfig.AddRequirement(req);
            }

            var piece = new CustomPiece(gameObject, true, pieceConfig);
            Jotunn.Managers.PieceManager.Instance.AddPiece(piece);
        }

        private static bool ShouldLoadPrefab(GameObject prefab) {
            if (pieces.ContainsKey(prefab.name) && !GetPiece(prefab.name).Enabled) {
                Jotunn.Logger.LogInfo($"Piece is disabled {prefab}");
            }
            return pieces.ContainsKey(prefab.name) && (!GetPiece(prefab.name).Exp || PluginConfigs.ExpSets.Value) && GetPiece(prefab.name).Enabled;
        }

        public static void FindAndRegisterPrefabs() {

            foreach (var gameObject in ZNetScene.instance.m_prefabs) {
                if (pieces.ContainsKey(gameObject.name)) {
                    LoadPiece(gameObject);
                }
            }
            foreach (var instance in ZNetScene.instance.m_instances.Values) {
                if (pieces.ContainsKey(instance.gameObject.name)) {
                    LoadPiece(instance.gameObject);
                }
            }
            /*            ZNetScene.instance.m_prefabs
                          .Where(gameObject => ShouldLoadPrefab(gameObject)) //gameObject.transform.parent == null &&
                          .OrderBy(gameObject => gameObject.name)
                          .ToList()
                          .ForEach(LoadPiece);*/
        }
    }

    public class PieceData : ICSV {

        public string Name = "";
        public string Category= "EXP-Caution";
        public List<RequirementConfig> Requirements = new List<RequirementConfig>();
        public int Comfort = 0;
        public string CraftingStation = "piece_workbench";
        public bool Empty = false;
        public bool Closeable = false;
        public bool Exp = false;
        public bool Enabled = true;
        public bool Fill = true;

        public bool ShouldLoadPiece() {
            if (!Enabled) {
                Jotunn.Logger.LogInfo($"Piece is disabled {Name}");
                return false;
            }
            return !Exp || PluginConfigs.ExpSets.Value;
        }

        public void Add(string key, string value) {
            switch (key) {
                case "Name":
                    Jotunn.Logger.LogInfo($"Loading  {value}");
                    Name = value;
                    break;
                case "Category":
                    Jotunn.Logger.LogInfo($"  Category for {Category}");
                    Category = value;
                    break;
                case "Requirements":
                    Jotunn.Logger.LogInfo($"  Requirements for {Name}");
                    foreach (string req in value.Split(';')) {
                        string[] vals = req.Split(' ');
                        if (vals.Length < 2) {
                            Requirements.Add(new RequirementConfig("Wood", 2, 0, true));
                            Jotunn.Logger.LogInfo($"Failed to load a Requirement for {Name}");
                            Enabled = false;
                        } else {
                            int count = 0;
                            int.TryParse(vals[1], out count);
                            Jotunn.Logger.LogInfo($"    |Loaded {vals[0]}x{count}");
                            Requirements.Add(new RequirementConfig(vals[0], count, 0, true));
                        }
                    }
                    if (Requirements.Count == 0) {
                        Requirements.Add(new RequirementConfig("Wood", 2, 0, true));
                        Jotunn.Logger.LogInfo($"Failed to load any Requirement for ${Name}");
                        Enabled = false;
                    }
                    break;
                case "Comfort":
                    int.TryParse(value, out Comfort);
                    break;
                case "CraftingStation":
                    CraftingStation = value;
                    break;
                case "Empty":
                    Empty = value == "1";
                    break;
                case "Closeable":
                    Closeable = value == "1";
                    break;
                case "Exp":
                    Exp = value == "1";
                    break;
                case "Fill":
                    Fill = value == "1";
                    break;
                case "Broke":
                case "Reason":
                    break;
                default:
                    Jotunn.Logger.LogInfo($"Failed to load load value ({key}) for ${Name}");
                    Enabled = false;
                    break;
            }
        }
    }
}
