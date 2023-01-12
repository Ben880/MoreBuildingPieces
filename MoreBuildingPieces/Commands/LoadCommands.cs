using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreBuildingPieces.Commands {
    public static class LoadCommands {

        public static void Load() {
            CommandManager.Instance.AddConsoleCommand(new DeleteInstanceRadius());
            CommandManager.Instance.AddConsoleCommand(new DeleteInstanceLooking());
        }
    }
}
