using System;
using UnityEngine;
using MessageType = MessageHud.MessageType;
using Skill = Skills.Skill;
using SkillType = Skills.SkillType;

namespace BetterUI.Patches;

internal class XPNotification
{
	public static void Show(Skill skill, float factor)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		if (!(skill.m_level >= 100f) && (!Main.skipRunningSkillNotifications.Value || (int)skill.m_info.m_skill != 102))
		{
			string text = $"$skill_{skill.m_info.m_skill.ToString().ToLower()}: {skill.GetLevelPercentage():P2}";
			if (Main.extendedXPNotification.Value)
			{
				float num = (float)Math.Round(skill.m_accumulator * 100f) / 100f;
				float num2 = (float)Math.Round(skill.GetNextLevelRequirement() * 100f) / 100f;
				text += $" (+{skill.m_info.m_increseStep * factor})\n[{num}/{num2}]";
			}
			string arg = Localization.instance.Localize(text);
			MessageHud.instance.ShowMessage(MessageType.TopLeft, $"<size={Main.notificationTextSizeXP.Value}>{arg}</size>", 0, (Sprite)null, false);
		}
	}
}
