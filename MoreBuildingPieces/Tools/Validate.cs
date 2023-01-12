using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Mono.Security.X509.X520;

namespace MoreBuildingPieces.Tools {
    public static class Validate {

        public enum FailLevel {
            Return,
            Print,
            Throw
        }
        private static bool handleIfFail(bool cond, FailLevel level, string callerName, int callerNumber, [CallerMemberName] string valName = null, [CallerLineNumber] int valNumber = 0) {
            if (level == FailLevel.Throw && !cond) {
                throw new ArgumentException($"Failed validator {valName}");
            }
            if (level == FailLevel.Throw && !cond) {
                Jotunn.Logger.LogWarning($"Failed Validator at\n{valName}:{valNumber}\n{callerName}:{callerNumber}");
            }
            return cond;
        }

        public static bool TryGetZNetView(GameObject gameObject, out ZNetView zNetView, FailLevel level = FailLevel.Return, [CallerMemberName] string callerName = null, [CallerLineNumber] int callerNumber = 0) {
            zNetView = gameObject.GetComponentInParent<ZNetView>();
            return handleIfFail(zNetView != null || zNetView.IsValid(), level, callerName, callerNumber);
        }

        public static bool HasLocalPLayer(FailLevel level = FailLevel.Return, [CallerMemberName] string callerName = null, [CallerLineNumber] int callerNumber = 0) {
            return handleIfFail(Player.m_localPlayer, level, callerName, callerNumber);
        }

        public static bool TryGetHovered(out GameObject gameObject, FailLevel level = FailLevel.Return, [CallerMemberName] string callerName = null, [CallerLineNumber] int callerNumber = 0) {
            gameObject = Player.m_localPlayer.GetHoverObject();
            return handleIfFail(gameObject != null, level, callerName, callerNumber);
        }

        public static bool HasHovered(FailLevel level = FailLevel.Return, [CallerMemberName] string callerName = null, [CallerLineNumber] int callerNumber = 0) {
            return handleIfFail(Player.m_localPlayer.GetHoverObject(), level, callerName, callerNumber);
        }

        public static bool InLoadingScreen(FailLevel level = FailLevel.Return, [CallerMemberName] string callerName = null, [CallerLineNumber] int callerNumber = 0) {
            return handleIfFail(Player.m_localPlayer == null || Player.m_localPlayer.IsTeleporting(), level, callerName, callerNumber);
        }
    }
}
