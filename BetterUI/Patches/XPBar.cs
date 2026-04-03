using UnityEngine;
using UnityEngine.UI;
using IterativeSearchType = Utils.IterativeSearchType;

namespace BetterUI.Patches;

public static class XPBar
{
	private static readonly bool smoothFill = true;

	private static readonly bool smoothDrain = true;

	private static readonly float smoothSpeed = 0.7f;

	private static readonly float maxValue = 1f;

	private static Color barColor = Color.yellow;

	private static GuiBar _xp_bar = null;

	public static void UpdateLevelProgressPercentage()
	{
		if ((Object)(object)_xp_bar != (Object)null)
		{
			_xp_bar.SetValue(XP.LevelProgressPercentage);
		}
	}

	public static void Create(Hud __instance)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_xp_bar != (Object)null))
		{
			Transform val = Utils.FindChild(((Component)__instance).gameObject.transform, "hudroot", (IterativeSearchType)0);
			GuiBar obj = Object.Instantiate<GuiBar>(Hud.instance.m_stealthBar, val, true);
			obj.m_barImage = ((Component)Hud.instance.m_stealthBar.m_bar).GetComponent<Image>();
			obj.m_smoothFill = smoothFill;
			obj.m_smoothDrain = smoothDrain;
			obj.m_smoothSpeed = smoothSpeed;
			obj.Awake();
			((Object)obj).name = "BU_XP_BAR";
			obj.m_originalColor = barColor;
			obj.ResetColor();
			obj.SetMaxValue(maxValue);
			obj.SetValue(0f);
			Transform transform = ((Component)obj).transform;
			RectTransform val2 = (RectTransform)(object)((transform is RectTransform) ? transform : null);
			val2.anchorMin = Vector2.zero;
			val2.anchorMax = new Vector2(1f, 0f);
			val2.anchoredPosition = Vector2.zero;
			val2.offsetMin = Vector2.zero;
			val2.offsetMax = Vector2.zero;
			val2.sizeDelta = new Vector2(0f, 5f);
			obj.m_bar.anchorMin = Vector2.zero;
			obj.m_bar.anchorMax = new Vector2(1f, 0.5f);
			obj.m_bar.offsetMin = Vector2.zero;
			obj.m_bar.offsetMax = Vector2.zero;
			RectTransform bar = obj.m_bar;
			Rect rect = val2.rect;
			bar.sizeDelta = new Vector2(rect.width, 5f);
			((Component)obj).gameObject.SetActive(true);
			_xp_bar = obj;
		}
	}

	public static void UpdatePosition()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_xp_bar != (Object)null)
		{
			Transform transform = ((Component)_xp_bar).transform;
			RectTransform val = (RectTransform)(object)((transform is RectTransform) ? transform : null);
			if ((Object)(object)val != (Object)null)
			{
				RectTransform bar = _xp_bar.m_bar;
				Rect rect = val.rect;
				bar.sizeDelta = rect.size;
			}
		}
	}
}
