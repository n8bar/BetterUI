using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BetterUI.Patches;
using HudData = EnemyHud.HudData;
using Object = UnityEngine.Object;

namespace BetterUI.GameClasses;

[HarmonyPatch(typeof(EnemyHud))]
public static class BetterEnemyHud
{
	private const string BossHpPrefix = "BU_bossHPText";

	private const string EnemyHpPrefix = "BU_enemyHpText";

	private const string EnemyStarsPrefix = "BU_enemyStarsText";

	private const string PlayerHpPrefix = "BU_playerHPText";

	private const float EnemyStarsLift = 5f;

	public static readonly float maxDrawDistance = 3f;

	private static readonly ConditionalWeakTable<HudData, TextMeshProUGUI> _hpTextCache = new ConditionalWeakTable<HudData, TextMeshProUGUI>();

	private static readonly ConditionalWeakTable<HudData, TextMeshProUGUI> _starTextCache = new ConditionalWeakTable<HudData, TextMeshProUGUI>();

	private static int GetDisplayedEnemyLevel(Character character)
	{
		return Mathf.Max(character.m_level - 1, 0);
	}

	private static bool UseCustomEnemyStars()
	{
		return Main.enemyLevelStyle.Value == Main.EnemyLevelStyle.Both;
	}

	private static string FormatEnemyName(Character character)
	{
		string text = Localization.instance.Localize(character.GetHoverName());
		if (Main.enemyLevelStyle.Value != Main.EnemyLevelStyle.PrefixLevelNumber)
		{
			return text;
		}
		int num = GetDisplayedEnemyLevel(character);
		if (num <= 0)
		{
			return text;
		}
		return $"<size={Main.enemyNameTextSize.Value}><color=#ffffffff>Lv.{num} </color></size>{text}";
	}

	private static string FormatEnemyStars(Character character)
	{
		if (!UseCustomEnemyStars())
		{
			return string.Empty;
		}
		int displayedEnemyLevel = GetDisplayedEnemyLevel(character);
		if (displayedEnemyLevel <= 0)
		{
			return string.Empty;
		}
		return $"<size={Main.enemyNameTextSize.Value}><color=#ffff00>{Helpers.Repeat("\u2605", displayedEnemyLevel)}</color></size>";
	}

	private static void UpdateEnemyStars(HudData hudData, RectTransform healthRect)
	{
		if (!_starTextCache.TryGetValue(hudData, out TextMeshProUGUI value))
		{
			return;
		}
		string text = FormatEnemyStars(hudData.m_character);
		bool active = !string.IsNullOrEmpty(text);
		((Component)value).gameObject.SetActive(active);
		if (!active)
		{
			return;
		}
		((TMP_Text)value).text = text;
		((TMP_Text)value).rectTransform.anchoredPosition = new Vector2(healthRect.anchoredPosition.x, healthRect.anchoredPosition.y - (healthRect.sizeDelta.y * 0.5f - EnemyStarsLift));
	}

	[HarmonyPostfix]
	[HarmonyPatch("Awake")]
	private static void AwakePostfix(ref EnemyHud __instance)
	{
		float num = Mathf.Min(Mathf.Abs(Main.maxShowDistance.Value), maxDrawDistance);
		EnemyHud obj = __instance;
		obj.m_maxShowDistance *= num;
	}

	[HarmonyPrefix]
	[HarmonyPatch("ShowHud")]
	private static void ShowHudPrefix(ref EnemyHud __instance, ref Character c, ref bool __state)
	{
		__state = __instance.m_huds.ContainsKey(c);
	}

	[HarmonyPostfix]
	[HarmonyPatch("ShowHud")]
	private static void ShowHudPostfix(ref EnemyHud __instance, ref Character c, ref bool __state)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		if ((!Main.customEnemyHud.Value | __state) || !__instance.m_huds.TryGetValue(c, out var value))
		{
			return;
		}
		Transform obj = value.m_gui.transform.Find("Health");
		RectTransform val = (RectTransform)(object)((obj is RectTransform) ? obj : null);
		if (c.IsPlayer())
		{
			if (Main.showPlayerHPText.Value)
			{
				TextMeshProUGUI val2 = Object.Instantiate<TextMeshProUGUI>(value.m_name, ((TMP_Text)value.m_name).transform.parent);
				((Object)val2).name = "BU_playerHPText";
				((TMP_Text)val2).rectTransform.anchoredPosition = new Vector2(((TMP_Text)val2).rectTransform.anchoredPosition.x, 3.5f);
				((TMP_Text)val2).text = $"<size={Main.playerHPTextSize.Value}>{value.m_character.GetHealth():0}/{value.m_character.GetMaxHealth():0}</size>";
				((Graphic)val2).color = Color.white;
				Object.Destroy((Object)(object)((Component)val2).GetComponent<Outline>());
				_hpTextCache.Add(value, val2);
				val.sizeDelta = new Vector2(val.sizeDelta.x, val.sizeDelta.y * 3f);
				value.m_healthFast.m_bar.sizeDelta = new Vector2(value.m_healthFast.m_width, val.sizeDelta.y);
				value.m_healthSlow.m_bar.sizeDelta = new Vector2(value.m_healthSlow.m_width, val.sizeDelta.y);
			}
			return;
		}
		if (c.IsBoss())
		{
			if (Main.showEnemyHPText.Value)
			{
				TextMeshProUGUI val3 = Object.Instantiate<TextMeshProUGUI>(value.m_name, ((TMP_Text)value.m_name).transform.parent);
				((Object)val3).name = "BU_bossHPText";
				((TMP_Text)val3).rectTransform.anchoredPosition = new Vector2(((TMP_Text)val3).rectTransform.anchoredPosition.x, 0f);
				((TMP_Text)val3).text = $"<size={Main.bossHPTextSize.Value}>{value.m_character.GetHealth():0} / {value.m_character.GetMaxHealth():0}</size>";
				((Graphic)val3).color = Color.white;
				Object.Destroy((Object)(object)((Component)val3).GetComponent<Outline>());
				_hpTextCache.Add(value, val3);
			}
			return;
		}
		((TMP_Text)value.m_name).fontSize = Main.enemyNameTextSize.Value;
		((TMP_Text)value.m_name).text = FormatEnemyName(c);
		if (Main.showEnemyHPText.Value)
		{
			val.sizeDelta = new Vector2(val.sizeDelta.x, val.sizeDelta.y * 3f);
			TextMeshProUGUI val4 = Object.Instantiate<TextMeshProUGUI>(value.m_name, ((TMP_Text)value.m_name).transform.parent);
			((Object)val4).name = "BU_enemyHpText";
			((TMP_Text)val4).rectTransform.anchoredPosition = new Vector2(((TMP_Text)val4).rectTransform.anchoredPosition.x, 7f);
			((TMP_Text)val4).text = $"<size={Main.enemyHPTextSize.Value}>{value.m_character.GetHealth():0}/{value.m_character.GetMaxHealth():0}</size>";
			((Graphic)val4).color = Color.white;
			Object.Destroy((Object)(object)((Component)val4).GetComponent<Outline>());
			_hpTextCache.Add(value, val4);
		}
		if (c.IsTamed() && Main.makeTamedHPGreen.Value)
		{
			value.m_healthFast.SetColor(Color.green);
			value.m_healthSlow.SetColor(Color.green);
		}
		if (Main.useCustomAlertedStatus.Value)
		{
			((Component)value.m_alerted).gameObject.SetActive(false);
			((Component)value.m_aware).gameObject.SetActive(false);
		}
		value.m_healthFast.m_bar.sizeDelta = new Vector2(value.m_healthFast.m_width, val.sizeDelta.y);
		value.m_healthSlow.m_bar.sizeDelta = new Vector2(value.m_healthSlow.m_width, val.sizeDelta.y);
		if (UseCustomEnemyStars())
		{
			TextMeshProUGUI val5 = Object.Instantiate<TextMeshProUGUI>(value.m_name, ((TMP_Text)value.m_name).transform.parent);
			((Object)val5).name = EnemyStarsPrefix;
			((Graphic)val5).color = Color.white;
			Object.Destroy((Object)(object)((Component)val5).GetComponent<Outline>());
			_starTextCache.Add(value, val5);
			UpdateEnemyStars(value, val);
		}
		if (Main.enemyLevelStyle.Value != Main.EnemyLevelStyle.DefaultStars)
		{
			((Component)value.m_level2).gameObject.SetActive(false);
			((Component)value.m_level3).gameObject.SetActive(false);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("UpdateHuds")]
	private static void UpdateHudsPostfix(ref EnemyHud __instance)
	{
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.customEnemyHud.Value)
		{
			return;
		}
		Character val = null;
		foreach (KeyValuePair<Character, HudData> hud in __instance.m_huds)
		{
			HudData value = hud.Value;
			if ((Object)(object)value.m_character == (Object)null || !__instance.TestShow(value.m_character, true))
			{
				if ((Object)(object)val == (Object)null)
				{
					val = value.m_character;
					Object.Destroy((Object)(object)value.m_gui);
				}
				continue;
			}
			if (value.m_character.IsPlayer())
			{
				if (Main.showPlayerHPText.Value && _hpTextCache.TryGetValue(value, out TextMeshProUGUI value2))
				{
					((TMP_Text)value2).text = $"<size={Main.playerHPTextSize.Value}>{value.m_character.GetHealth():0}/{value.m_character.GetMaxHealth():0}</size>";
				}
				continue;
			}
			if (value.m_character.IsBoss())
			{
				if (Main.showEnemyHPText.Value && _hpTextCache.TryGetValue(value, out TextMeshProUGUI value3))
				{
					((TMP_Text)value3).text = $"<size={Main.bossHPTextSize.Value}>{value.m_character.GetHealth():0} / {value.m_character.GetMaxHealth():0}</size>";
				}
				continue;
			}
			if (Main.useCustomAlertedStatus.Value)
			{
				((Component)value.m_alerted).gameObject.SetActive(false);
				((Component)value.m_aware).gameObject.SetActive(false);
				bool flag = value.m_character.GetBaseAI().HaveTarget();
				bool flag2 = value.m_character.GetBaseAI().IsAlerted();
				((Graphic)value.m_name).color = ((!(flag || flag2)) ? Color.white : (flag2 ? Color.red : Color.yellow));
			}
			if (Main.showEnemyHPText.Value && _hpTextCache.TryGetValue(value, out TextMeshProUGUI value4))
			{
				((TMP_Text)value4).text = $"<size={Main.enemyHPTextSize.Value}>{value.m_character.GetHealth():0}/{value.m_character.GetMaxHealth():0}</size>";
			}
			((TMP_Text)value.m_name).text = FormatEnemyName(value.m_character);
			Transform obj = value.m_gui.transform.Find("Health");
			RectTransform val5 = (RectTransform)(object)((obj is RectTransform) ? obj : null);
			if ((Object)(object)val5 != (Object)null)
			{
				UpdateEnemyStars(value, val5);
			}
			if (Main.enemyLevelStyle.Value != Main.EnemyLevelStyle.DefaultStars)
			{
				((Component)value.m_level2).gameObject.SetActive(false);
				((Component)value.m_level3).gameObject.SetActive(false);
			}
		}
		if ((Object)(object)val != (Object)null)
		{
			__instance.m_huds.Remove(val);
		}
	}

	[HarmonyTranspiler]
	[HarmonyPatch("LateUpdate")]
	private static IEnumerable<CodeInstruction> LateUpdateTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[4]
		{
			new CodeMatch((OpCode?)OpCodes.Stloc_3, (object)null, (string)null),
			new CodeMatch((OpCode?)OpCodes.Ldloc_3, (object)null, (string)null),
			new CodeMatch((OpCode?)OpCodes.Ldloc_1, (object)null, (string)null),
			new CodeMatch((OpCode?)OpCodes.Call, (object)null, (string)null)
		}).Advance(3).SetInstructionAndAdvance(Transpilers.EmitDelegate<Func<Character, Player, bool>>((Func<Character, Player, bool>)CharacterLocalPlayerEqualityDelegate))
			.InstructionEnumeration();
	}

	private static bool CharacterLocalPlayerEqualityDelegate(Character character, Player player)
	{
		if (Main.showLocalPlayerEnemyHud.Value)
		{
			return false;
		}
		return (Object)(object)character == (Object)(object)player;
	}

	public static void ShowLocalPlayerEnemyHudConfigChanged()
	{
		if ((Object)(object)Player.m_localPlayer != (Object)null && (Object)(object)EnemyHud.m_instance != (Object)null && EnemyHud.m_instance.m_huds.TryGetValue((Character)(object)Player.m_localPlayer, out var value))
		{
			Object.Destroy((Object)(object)value.m_gui);
			EnemyHud.m_instance.m_huds.Remove((Character)(object)Player.m_localPlayer);
		}
	}
}
