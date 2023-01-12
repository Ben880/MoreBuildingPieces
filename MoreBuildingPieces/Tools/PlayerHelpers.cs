using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreBuildingPieces.Tools {

	/// <summary>
	///     Assortment of helpers related to the local player.
	///     Should only have context of valheim Player UnityEngine and self
	/// </summary>
	static class PlayerHelpers {

		private static Player localPlayer;

		/// <summary>
		///     Get the local player
		/// </summary>
		public static Player GetLocalPlayer() {
			if (localPlayer == null) {
				localPlayer = Player.m_localPlayer;
				if (localPlayer == null)
					Console.instance.Print("failed to get local player");
			}
			return Player.m_localPlayer;
		}

		/// <summary>
		///     Create new undo data for ZDO removal operations. Clones all ZDO data to prevent NREs.
		/// </summary>
		/// <param name="distance"> flaot how far for the raycast to look
		public static GameObject GetLookingAt(float distance) {

			int layerMask = GetLocalPlayer().m_placeRayMask;
			if (Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, out var hitInfo, 50f, layerMask) && (bool)hitInfo.collider && !hitInfo.collider.attachedRigidbody && Vector3.Distance(GetLocalPlayer().m_eye.position, hitInfo.point) < distance) {
				return hitInfo.collider.gameObject;
			}
			return null;
		}

    }
}
