using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MoreBuildingPieces {
    [HarmonyPatch]
    static class Patches {

        [HarmonyPatch(typeof(Chat), "SendText")]
        private static class Chat_SendText_Patch {
            private static void Prefix(ref string text) {
                if (text.ToLower().Contains("i have")) {
                    text = PluginConfigs.ArriveMessage.Value;
                }

            }
        }

        // Hook just before Jotunn registers the Pieces
        // TODO: Rework Jotunn to be able to add pieces later
        [HarmonyPatch(typeof(ObjectDB), "Awake"), HarmonyPrefix]
        static void ObjectDBAwakePrefix() {
            if (SceneManager.GetActiveScene().name == "main") {
                Behavior.PieceManager.LoadPieces();
            }
        }

        [HarmonyPatch(typeof(ZNetView), "Awake"), HarmonyPrefix]
        static bool ZNetViewAwakePrefix(ZNetView __instance) {
            if (ZNetView.m_useInitZDO && ZNetView.m_initZDO == null) {
                Jotunn.Logger.LogWarning($"Double ZNetview when initializing object {__instance.name}; OVERRIDE: Deleting the {__instance.name} gameobject");
                ZNetScene.instance.Destroy(__instance.gameObject);
                return false;
            }
            return true;
        }

        // Called when piece is just placed
        [HarmonyPatch(typeof(Piece), "SetCreator"), HarmonyPrefix]
        static void PieceSetCreatorPrefix(long uid, Piece __instance) {
            string name = __instance.name.Replace("(Clone)", "");

            if (Behavior.PieceManager.ContainsPiece(name)) {
                Jotunn.Logger.LogWarning($"MBP Managed Item Placed");

                var container = __instance.GetComponent<Container>();
                if (Behavior.PieceManager.GetPiece(name).Empty && container && !PluginConfigs.FillLoot.Value) {
                    container.GetInventory().RemoveAll();
                    //container.m_defaultItems = null;
                    Jotunn.Logger.LogWarning($"Removing Items");
                }

                if (Behavior.PieceManager.GetPiece(name).Fill && container) {
                    container.GetInventory().AddItem(PrefabManager.Instance.GetPrefab("Wood"), 1);
                    //container.m_defaultItems = null;
                    Jotunn.Logger.LogWarning($"Removing Items");
                }

                var dropOnDestroyed = (__instance.GetComponent<DropOnDestroyed>());
                if (dropOnDestroyed) {
                    if (PluginConfigs.DropLoot.Value) {
                        __instance.GetComponent<Piece>().m_resources = new Piece.Requirement[0];
                    } else {
                        dropOnDestroyed.m_dropWhenDestroyed = new DropTable();


                    }
                    
                }
            }
        }

    }
}