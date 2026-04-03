using BetterUI.Patches;
using HarmonyLib;

namespace BetterUI.GameClasses;

[HarmonyPatch]
internal static class BetterCookingStation
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(CookingStation), "GetHoverText")]
	private static bool GetHoverText(CookingStation __instance, ref string __result)
	{
		if (Main.timeLeftHoverTextCookingStation.Value == Main.TimeLeftStyle.Disabled)
		{
			return true;
		}
		return BetterUI.Patches.HoverText.PatchCookingStation(__instance, ref __result);
	}
}
