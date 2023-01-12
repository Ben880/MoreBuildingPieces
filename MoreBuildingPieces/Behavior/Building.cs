using MoreBuildingPieces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MoreBuildingPieces.Behavior {
    internal static class Building {

        

        public static GameObject GetLookingAt () {
            Validate.HasLocalPLayer(Validate.FailLevel.Throw);

            float origionalDistance = Player.m_localPlayer.m_maxInteractDistance;
            Player.m_localPlayer.m_maxInteractDistance = PluginConfigs.InteractionRange.Value;

            GameObject hoverObject;
            if (!Validate.TryGetHovered(out hoverObject, Validate.FailLevel.Print))
                return null;

            Player.m_localPlayer.m_maxInteractDistance = origionalDistance;

            return hoverObject;
        }

        public static void DeleteLookingAt() { 
            GameObject hoverObject = GetLookingAt();

            ZNetView zNetView;
            if (!Validate.TryGetZNetView(hoverObject, out zNetView, Validate.FailLevel.Print))
                return;

            Jotunn.Logger.LogInfo($"Destroying: {hoverObject.name}");
            zNetView.GetZDO().SetOwner(ZDOMan.instance.GetMyID());
            ZNetScene.instance.Destroy(zNetView.gameObject);
        }

        public static void PrintLookingAt() {
            GameObject hoverObject = GetLookingAt();
            Jotunn.Logger.LogInfo($"Looking at: {hoverObject.name}");
            if (hoverObject.transform.parent)
                Jotunn.Logger.LogInfo($"    | Parent: {hoverObject.transform.parent.gameObject.name}");
            Jotunn.Logger.LogInfo($"    | Layer: {hoverObject.layer}");
            Jotunn.Logger.LogInfo($"    | Tag: {hoverObject.tag}");
            Jotunn.Logger.LogInfo($"    | Components:");
            Component[] components = hoverObject.GetComponents(typeof(Component));
            foreach (Component component in components) {
                Jotunn.Logger.LogInfo($"        | {component.ToString()}");
            }
            Jotunn.Logger.LogInfo($"    | Children:");
            foreach (Transform tr in hoverObject.GetComponentsInChildren<Transform>()) {
                Jotunn.Logger.LogInfo($"        | {tr.gameObject.name}");
            }

        }
    }
}
