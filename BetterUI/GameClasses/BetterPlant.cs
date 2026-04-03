using BetterUI.Patches;
using HarmonyLib;

namespace BetterUI.GameClasses;

[HarmonyPatch]
internal static class BetterPlant
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(Plant), "GetHoverText")]
	private static bool GetHoverText(Plant __instance, ref string __result)
	{
		if (Main.timeLeftHoverTextPlant.Value == Main.TimeLeftStyle.Disabled)
		{
			return true;
		}
		return BetterUI.Patches.HoverText.PatchPlant(__instance, ref __result);
	}
}
