using BetterUI.Patches;
using HarmonyLib;
using TMPro;
using UnityEngine;
using IterativeSearchType = Utils.IterativeSearchType;

namespace BetterUI.GameClasses;

[HarmonyPatch(typeof(SkillsDialog), "Setup")]
internal static class SkillsDialogSetupPatch
{
	private static void Postfix(SkillsDialog __instance, Player player)
	{
		if ((Object)(object)__instance == (Object)null || (Object)(object)player == (Object)null)
		{
			Main.log.LogError((object)"SkillsDialog or Player instance is null.");
			return;
		}
		try
		{
			GameObject gameObject = ((Component)__instance).gameObject;
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		catch
		{
			Main.log.LogError((object)"Failed to set SkillsDialog active.");
		}
		if (Main.showCharacterXP.Value)
		{
			Transform obj2 = Utils.FindChild(((Component)__instance).transform, "topic", (IterativeSearchType)0);
			TMP_Text val = ((obj2 != null) ? ((Component)obj2).GetComponent<TMP_Text>() : null);
			if ((Object)(object)val != (Object)null)
			{
				val.text = $"Level: {XP.level:0}      Progress: {XP.LevelProgressPercentage:0%}";
			}
			else
			{
				Main.log.LogError((object)"Topic text component is null.");
			}
		}
		if (Main.customSkillUI.Value)
		{
			SkillUI.UpdateDialog(__instance, player);
		}
	}
}
