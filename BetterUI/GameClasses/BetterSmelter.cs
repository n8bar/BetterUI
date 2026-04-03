using HarmonyLib;

namespace BetterUI.GameClasses;

internal static class BetterSmelter
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(Smelter), "UpdateHoverTexts")]
	private static void UpdateHoverTexts(Smelter __instance)
	{
	}
}
