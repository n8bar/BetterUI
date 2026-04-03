using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Skill = Skills.Skill;
using Axis = UnityEngine.RectTransform.Axis;
using IterativeSearchType = Utils.IterativeSearchType;
using Object = UnityEngine.Object;
using SkillType = Skills.SkillType;

namespace BetterUI.Patches;

internal class SkillUI
{
	private static readonly float paddingFix = 0.15f;

	public static void UpdateDialog(SkillsDialog dialog, Player player)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		foreach (GameObject element in dialog.m_elements)
		{
			Object.Destroy((Object)(object)element);
		}
		dialog.m_elements.Clear();
		List<Skill> skillList = ((Character)player).GetSkills().GetSkillList();
		for (int i = 0; i < skillList.Count; i++)
		{
			Skill val = skillList[i];
			float num = (float)Math.Round(val.m_accumulator * 100f) / 100f;
			GameObject val2 = Object.Instantiate<GameObject>(dialog.m_elementPrefab, Vector3.zero, Quaternion.identity, (Transform)(object)dialog.m_listRoot);
			val2.SetActive(true);
			Transform transform = val2.transform;
			((RectTransform)((transform is RectTransform) ? transform : null)).anchoredPosition = new Vector2(0f, (0f - (float)i - paddingFix) * dialog.m_spacing);
			val2.GetComponentInChildren<UITooltip>().m_text = val.m_info.m_description;
			((Component)Utils.FindChild(val2.transform, "icon", (IterativeSearchType)0)).GetComponent<Image>().sprite = val.m_info.m_icon;
			((Component)Utils.FindChild(val2.transform, "name", (IterativeSearchType)0)).GetComponent<TMP_Text>().text = Localization.instance.Localize("$skill_" + val.m_info.m_skill.ToString().ToLower() + $"\n<size={Main.skillUITextSize.Value - 2}>Lvl: {(int)val.m_level}</size>");
			((Component)Utils.FindChild(val2.transform, "name", (IterativeSearchType)0)).GetComponent<TMP_Text>().fontSize = Main.skillUITextSize.Value;
			float skillLevel = ((Character)player).GetSkills().GetSkillLevel(val.m_info.m_skill);
			((Component)Utils.FindChild(val2.transform, "leveltext", (IterativeSearchType)0)).GetComponent<TMP_Text>().text = $"<size={Main.skillUITextSize.Value - 4}>{num} ({val.GetLevelPercentage() * 100f:0.##}%)</size>";
			TMP_Text component = ((Component)Utils.FindChild(val2.transform, "bonustext", (IterativeSearchType)0)).GetComponent<TMP_Text>();
			bool num2 = (double)skillLevel != (double)Mathf.Floor(val.m_level);
			((Component)component).gameObject.SetActive(false);
			if (num2)
			{
				component.text = (skillLevel - val.m_level).ToString("+0");
				TMP_Text component2 = ((Component)Utils.FindChild(val2.transform, "name", (IterativeSearchType)0)).GetComponent<TMP_Text>();
				component2.text += $" <size={Main.skillUITextSize.Value - 2}><color=#00ffffff>{component.text}</color></size>";
			}
			((Component)((Component)Utils.FindChild(val2.transform, "levelbar_total", (IterativeSearchType)0)).GetComponent<GuiBar>()).gameObject.SetActive(false);
			((Component)Utils.FindChild(val2.transform, "levelbar", (IterativeSearchType)0)).GetComponent<GuiBar>().SetValue(val.GetLevelPercentage());
			Transform parent = ((Transform)((Component)Utils.FindChild(val2.transform, "levelbar", (IterativeSearchType)0)).GetComponent<GuiBar>().m_bar).parent;
			RectTransform val3 = (RectTransform)(object)((parent is RectTransform) ? parent : null);
			val3.sizeDelta = new Vector2(val3.sizeDelta.x, val3.sizeDelta.y + 4f);
			val3.anchoredPosition = new Vector2(-4f, 0f);
			RectTransform rectTransform = ((Component)Utils.FindChild(val2.transform, "leveltext", (IterativeSearchType)0)).GetComponent<TMP_Text>().rectTransform;
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 2f);
			((Component)((Component)Utils.FindChild(val2.transform, "currentlevel", (IterativeSearchType)0)).GetComponent<GuiBar>()).gameObject.SetActive(false);
			dialog.m_elements.Add(val2);
		}
		float num3 = Mathf.Max(dialog.m_baseListSize, ((float)skillList.Count + paddingFix) * dialog.m_spacing);
		dialog.m_listRoot.SetSizeWithCurrentAnchors(Axis.Vertical, num3);
		dialog.m_totalSkillText.text = "<color=#ffa500ff>" + ((Character)player).GetSkills().GetTotalSkill().ToString("0") + "</color><color=#ffffffff> / </color><color=#ffa500ff>" + ((Character)player).GetSkills().GetTotalSkillCap().ToString("0") + "</color>";
	}
}
