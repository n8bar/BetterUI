using BetterUI.Patches;
using HarmonyLib;
using Skill = Skills.Skill;
using SkillType = Skills.SkillType;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterSkills
{
	[HarmonyPatch]
	private static class SkillPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Skill), "Raise")]
		private static void CalculateXP(ref Skills __instance, float factor)
		{
			XP.RaiseXP();
			XPBar.UpdateLevelProgressPercentage();
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Skills), "RaiseSkill")]
	private static void Notification(Skills __instance, SkillType skillType, float factor = 1f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if ((int)skillType != 0 && Main.showXPNotifications.Value)
		{
			XPNotification.Show(__instance.GetSkill(skillType), factor);
		}
	}
}
