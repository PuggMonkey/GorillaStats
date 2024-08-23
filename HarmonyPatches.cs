using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace GorillaStats
{
	/// <summary>
	/// This class handles applying harmony patches to the game.
	/// You should not need to modify this class.
	/// </summary>
	public class HarmonyPatches
	{
		private static Harmony instance;

		public static bool IsPatched { get; private set; }
		public const string InstanceId = PluginInfo.GUID;

		internal static void ApplyHarmonyPatches()
		{
			if (!IsPatched)
			{
				if (instance == null)
				{
					instance = new Harmony(InstanceId);
				}

				instance.PatchAll(Assembly.GetExecutingAssembly());
				IsPatched = true;
				Debug.Log("[GorillaStats] Harmony patches applied successfully.");
			}
		}

		internal static void RemoveHarmonyPatches()
		{
			if (instance != null && IsPatched)
			{
				instance.UnpatchSelf();
				IsPatched = false;
			}
		}
	}
}
