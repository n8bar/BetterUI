using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;

namespace BetterUI.GameClasses;

[HarmonyPatch]
internal static class BetterFermenter
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(Fermenter), "GetHoverText")]
	private static bool GetHoverText(Fermenter __instance, ref string __result)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Main.timeLeftHoverTextFermenter.Value == Main.TimeLeftStyle.Disabled)
		{
			return true;
		}
		if (!PrivateArea.CheckAccess(((Component)__instance).transform.position, 0f, false, false))
		{
			__result = Localization.instance.Localize(__instance.m_name + "\n$piece_noaccess");
			return false;
		}
		return BetterUI.Patches.HoverText.PatchFermenter(__instance, ref __result);
	}
}
