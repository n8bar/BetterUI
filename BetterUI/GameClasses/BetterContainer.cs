using BetterUI.Patches;
using HarmonyLib;

namespace BetterUI.GameClasses;

[HarmonyPatch]
internal static class BetterContainer
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(Container), "GetHoverText")]
	private static void GetHoverText(Container __instance, ref string __result)
	{
		if (Main.chestHasRoomHoverText.Value != Main.ChestHasRoomStyle.Disabled && __instance.m_inventory.NrOfItems() != 0)
		{
			__result = BetterUI.Patches.HoverText.PatchContainer(__instance);
		}
	}
}
