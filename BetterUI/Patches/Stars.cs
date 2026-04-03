using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal static class Stars
{
	private static Color starColor = new Color(1f, 0.85882f, 0.23137f, 1f);

	public static void Draw(InventoryGrid.Element element, int quality_lvl)
	{
		element.m_quality.textWrappingMode = (TextWrappingModes)3;
		string text;
		if (quality_lvl >= 5)
		{
			text = $"<color=#ffffffff><size=10>{quality_lvl}</size>x </color>\u2605";
			element.m_quality.alignment = (TextAlignmentOptions)4100;
			element.m_quality.rectTransform.sizeDelta = new Vector2(60f, 20f);
			if (quality_lvl > 99998)
			{
				text = "<color=#ffffffff><size=10>MODDED</size> </color>\u2605";
			}
		}
		else
		{
			text = Helpers.Repeat("\u2605", quality_lvl);
			element.m_quality.rectTransform.sizeDelta = new Vector2((float)quality_lvl * 10f, 20f);
		}
		Object.Destroy(element.m_quality.GetComponent<Outline>());
		element.m_quality.text = "<size=10>" + text + "</size>";
		element.m_quality.color = starColor;
		element.m_quality.rectTransform.anchoredPosition = new Vector2(-4f, -6f);
	}

	public static string HoverText(int quality_lvl)
	{
		if (quality_lvl >= 5)
		{
			return $"{quality_lvl}x\u2605";
		}
		if (quality_lvl > 99999)
		{
			return "Modded";
		}
		return Helpers.Repeat("\u2605", quality_lvl);
	}
}
