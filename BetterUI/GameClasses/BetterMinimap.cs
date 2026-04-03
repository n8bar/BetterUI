using HarmonyLib;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterMinimap
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(Minimap), "Awake")]
	private static void ScalePins(Minimap __instance)
	{
		__instance.m_pinSizeLarge *= Main.mapPinScaleSize.Value;
		__instance.m_pinSizeSmall *= Main.mapPinScaleSize.Value;
	}
}
