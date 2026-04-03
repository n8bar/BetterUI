using UnityEngine;
using MessageType = MessageHud.MessageType;
using Skill = Skills.Skill;

namespace BetterUI.Patches;

public static class XP
{
	private static readonly int fXPPerSkillRank = 1;

	private static Player RPG_Player;

	private static double xp_used;

	private static float xp_current;

	private static float xp_accumulator;

	public static int level;

	public static float LevelProgressPercentage;

	public static void Awake(Player player)
	{
		if ((Object)(object)player == (Object)null)
		{
			Main.log.LogWarning((object)"Didn't get a player");
			return;
		}
		RPG_Player = player;
		UpdatePlayerXP();
		UpdatePlayerLevel();
		UpdateUsedXP();
		UpdateLevelProgressPercentage();
	}

	public static void RaiseXP()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		UpdatePlayerXP();
		xp_accumulator = (float)((double)xp_current - xp_used);
		UpdateLevelProgressPercentage();
		if (xp_accumulator >= GetNextLevelRequirement(level))
		{
			UpdatePlayerLevel();
			UpdateUsedXP();
			RPG_Player.m_skillLevelupEffects.Create(((Character)RPG_Player).m_head.position, ((Character)RPG_Player).m_head.rotation, ((Character)RPG_Player).m_head, 1.5f, -1);
			((Character)RPG_Player).Message((MessageType)2, $"<size=60><color=#ffffffff>LEVEL UP</color></size>\n<size=30>You are now Lv. {level} </size>", 0, (Sprite)null);
		}
	}

	private static void UpdatePlayerXP()
	{
		float num = 0f;
		if ((Object)(object)RPG_Player == (Object)null)
		{
			return;
		}
		foreach (Skill value in RPG_Player.m_skills.m_skillData.Values)
		{
			num += (float)CalulateGainedXP(value.m_level);
			num += (float)(int)value.m_level * value.GetLevelPercentage();
		}
		xp_current = num;
	}

	private static void UpdateUsedXP()
	{
		xp_used = 12.5 * (double)(level * level) + 62.5 * (double)level - 75.0;
	}

	private static void UpdatePlayerLevel()
	{
		level = Mathf.FloorToInt(-2.5f + Mathf.Sqrt(8f * xp_current + 1225f) / 10f);
	}

	private static void UpdateLevelProgressPercentage()
	{
		LevelProgressPercentage = (float)((double)xp_current - xp_used) / GetNextLevelRequirement(level);
		Main.log.LogDebug((object)$"Updated level progress percent to {LevelProgressPercentage}");
	}

	private static float GetNextLevelRequirement(int lvl)
	{
		return (lvl + 3) * 25;
	}

	private static int CalulateGainedXP(float lvl)
	{
		return (int)lvl * ((int)lvl + 1) * fXPPerSkillRank / 2;
	}
}
