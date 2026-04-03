using UnityEngine;

namespace BetterUI.Patches;

internal static class CharacterStats
{
	private static readonly float padding = 150f;

	private static readonly int fontSize = 20;

	public static void Show(FejdStartup menu)
	{
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		if (menu.m_profiles == null)
		{
			menu.m_profiles = SaveSystem.GetAllPlayerProfiles();
		}
		if (menu.m_profileIndex >= menu.m_profiles.Count)
		{
			menu.m_profileIndex = menu.m_profiles.Count - 1;
		}
		if (menu.m_profileIndex >= 0 && menu.m_profileIndex < menu.m_profiles.Count)
		{
			PlayerProfile val = menu.m_profiles[menu.m_profileIndex];
			string text = ((val.m_playerStats.m_stats[(PlayerStatType)6] > 0f) ? $"Kills: {val.m_playerStats.m_stats[(PlayerStatType)6]}   " : string.Empty);
			menu.m_csName.text = $"{val.GetName()}\n<size={fontSize}>{text}Deaths: {val.m_playerStats[(PlayerStatType)0]}   Crafts: {val.m_playerStats[(PlayerStatType)13]}   Builds: {val.m_playerStats[(PlayerStatType)2]}</size>";
			((Component)menu.m_csName).gameObject.SetActive(true);
			Transform transform = ((Component)menu.m_csStartButton).transform;
			Vector2 anchoredPosition = ((RectTransform)((transform is RectTransform) ? transform : null)).anchoredPosition;
			menu.m_csName.rectTransform.anchoredPosition = new Vector2(menu.m_csName.rectTransform.anchoredPosition.x, anchoredPosition.y + padding);
			menu.SetupCharacterPreview(val);
		}
		else
		{
			((Component)menu.m_csName).gameObject.SetActive(false);
			menu.ClearCharacterPreview();
		}
	}
}
