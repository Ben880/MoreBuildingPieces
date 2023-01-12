using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using MoreBuildingPieces.Behavior;
using MoreBuildingPieces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreBuildingPieces {
    public class DeleteInstanceRadius : ConsoleCommand {
        public override string Name => "mbp_delete_r";
        public override string Help => "deletes an instance of a prefab in radius";

        public override void Run(string[] args) {
            // todo validators
            if (args.Length == 0) {
                return;
            }

            Player localPlayer = Tools.PlayerHelpers.GetLocalPlayer();

            GameObject prefab = PrefabManager.Instance.GetPrefab(args[0]);
            if (!prefab) {
                Console.instance.Print("that doesn't exist: " + args[0]);
                return;
            }

            string matchPattern = "^" + Regex.Escape(prefab.name) + @"(\(Clone\))?$";

            float sqrRadius = 10;
            float.TryParse(args[1], out sqrRadius);

            Dictionary<ZDO, ZNetView> znetSceneInstances = AccessTools.FieldRefAccess<ZNetScene, Dictionary<ZDO, ZNetView>>(ZNetScene.instance, "m_instances");
            List<GameObject> toDelete = znetSceneInstances.Values
              .Where(inst => Regex.IsMatch(inst.name, matchPattern))
              .Where(inst => (inst.transform.position - localPlayer.transform.position).sqrMagnitude <= sqrRadius)
              .Select(inst => inst.gameObject)
              .ToList();


            toDelete.ForEach(ZNetScene.instance.Destroy);
            Console.instance.Print($"Deleted {toDelete.Count} Objects");
        }
    }

    public class DeleteInstanceLooking : ConsoleCommand {
        public override string Name => "mbp_delete_look";
        public override string Help => "deletes an instance of a prefab you are looking at";

        public override void Run(string[] args) {
            Building.DeleteLookingAt();
        }
    }

    public class Reload : ConsoleCommand {
        public override string Name => "mbp_reload";
        public override string Help => "reloads piece data";

        public override void Run(string[] args) {
            Behavior.PieceManager.LoadData();
        }
    }
}
