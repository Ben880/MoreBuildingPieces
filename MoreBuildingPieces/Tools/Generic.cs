using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace MoreBuildingPieces.Tools {

    /// <summary>
    ///     Assortment of generic tools
    ///     Should only have context of valheim UnityEngine and self
    /// </summary>
    static class Generic {


        private static string pathToDLL;


        public static string GetPathToMod() {
            if (pathToDLL == null) {
                pathToDLL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            return pathToDLL;
        }

    }
}
