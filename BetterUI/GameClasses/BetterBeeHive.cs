using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;

namespace BetterUI.GameClasses;

[HarmonyPatch]
internal class BetterBeeHive
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(Beehive), "GetHoverText")]
	private static bool GetHoverText(Beehive __instance, ref string __result)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Main.timeLeftHoverTextBeeHive.Value == Main.TimeLeftStyle.Disabled)
		{
			return true;
		}
		if (!PrivateArea.CheckAccess(((Component)__instance).transform.position, 0f, false, false))
		{
			__result = Localization.instance.Localize(__instance.m_name + "\n$piece_noaccess");
			return false;
		}
		BetterUI.Patches.HoverText.PatchBeeHive(__instance, ref __result);
		return false;
	}
}
