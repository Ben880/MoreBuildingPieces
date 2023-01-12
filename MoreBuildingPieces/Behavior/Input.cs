using MoreBuildingPieces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreBuildingPieces.Behavior {
    public static class Input {

        public static void Update() {

            UpdateInWorld();
        }

        private static void UpdateInWorld() {
            if (Validate.InLoadingScreen())
                return;

            // in world
            if (PluginConfigs.SendShoutButton.Value.IsDown()) {
                Jotunn.Logger.LogInfo("MoreBuildingPieces: SendShoutButton");
                Player localPlayer = Player.m_localPlayer;
                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", localPlayer.GetHeadPoint(), 2, localPlayer.GetPlayerName(), PluginConfigs.ShoutMessage.Value, PrivilegeManager.GetNetworkUserId());
            }

            if (PluginConfigs.DeleteLookingAt.Value.IsDown() && PluginConfigs.DevTools.Value) {
                Building.DeleteLookingAt();
            }

            if (PluginConfigs.PrintLookingAt.Value.IsDown() && PluginConfigs.DevTools.Value) {
                Building.PrintLookingAt();
            }
        }
    }
}
