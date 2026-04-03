using BetterUI.Patches;
using HarmonyLib;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterFejdStartup
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(FejdStartup), "SetupGui")]
	private static void AddWaterMark(ref FejdStartup __instance)
	{
		CustomWatermark.Apply(__instance);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(FejdStartup), "UpdateCharacterList")]
	private static void ShowCharacterStats(ref FejdStartup __instance)
	{
		if (Main.showCustomCharInfo.Value)
		{
			CharacterStats.Show(__instance);
		}
	}
}
