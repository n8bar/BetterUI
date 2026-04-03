using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ItemData = ItemDrop.ItemData;
using DamageTypes = HitData.DamageTypes;
using IterativeSearchType = Utils.IterativeSearchType;
using Object = UnityEngine.Object;
using SkillType = Skills.SkillType;

namespace BetterUI.Patches;

internal static class InventoryArmorTooltip
{
	public static UITooltip tooltip;

	public static TMP_Text m_armor;

	public static void Awake(InventoryGui ig)
	{
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		Transform parent = ig.m_armor.transform.parent;
		Transform val = Utils.FindChild(parent, "bkg", (IterativeSearchType)0);
		Transform val2 = Utils.FindChild(parent, "armor_icon", (IterativeSearchType)0);
		Transform val3 = Utils.FindChild(parent, "ac_text", (IterativeSearchType)0);
		GameObject elementPrefab = ig.m_containerGrid.m_elementPrefab;
		if (!((Object)(object)elementPrefab == (Object)null))
		{
			GameObject val4 = Object.Instantiate<GameObject>(elementPrefab, parent);
			((Behaviour)((Component)val4.transform.Find("equiped")).GetComponent<Image>()).enabled = false;
			((Behaviour)((Component)val4.transform.Find("queued")).GetComponent<Image>()).enabled = false;
			((Behaviour)((Component)val4.transform.Find("icon")).GetComponent<Image>()).enabled = false;
			((Behaviour)((Component)val4.transform.Find("amount")).GetComponent<TMP_Text>()).enabled = false;
			((Component)val4.transform.Find("durability")).gameObject.SetActive(false);
			((Behaviour)((Component)val4.transform.Find("binding")).GetComponent<TMP_Text>()).enabled = false;
			((Behaviour)((Component)val4.transform.Find("quality")).GetComponent<TMP_Text>()).enabled = false;
			((Component)val4.transform.Find("selected")).gameObject.SetActive(false);
			((Behaviour)((Component)val4.transform.Find("noteleport")).GetComponent<Image>()).enabled = false;
			((Behaviour)((Component)val4.transform.Find("foodicon")).GetComponent<Image>()).enabled = false;
			Object.Destroy((Object)(object)val4.GetComponent<UIInputHandler>());
			Object.Destroy((Object)(object)val4.GetComponent<Button>());
			val4.GetComponent<Image>().sprite = ((Component)val).GetComponent<Image>().sprite;
			((Graphic)val4.GetComponent<Image>()).color = ((Graphic)((Component)val).GetComponent<Image>()).color;
			((Graphic)val4.GetComponent<Image>()).material = ((Graphic)((Component)val).GetComponent<Image>()).material;
			RectTransform val5 = (RectTransform)(object)((val is RectTransform) ? val : null);
			Transform transform = val4.transform;
			Transform obj = ((transform is RectTransform) ? transform : null);
			((RectTransform)obj).anchoredPosition = val5.anchoredPosition;
			((RectTransform)obj).sizeDelta = val5.sizeDelta;
			val2.SetParent(val4.transform);
			val3.SetParent(val4.transform);
			((Behaviour)((Component)val).GetComponent<Image>()).enabled = false;
			UITooltip component = val4.GetComponent<UITooltip>();
			component.m_topic = DisplayXpLevel() + Player.m_localPlayer.GetPlayerName();
			tooltip = component;
			m_armor = ig.m_armor;
		}
	}

	public static void Update(Player player)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		WeaponStats(player, stringBuilder);
		BlockStats(player, stringBuilder);
		stringBuilder.AppendFormat("\n$item_armor: <color=#ffa500ff>{0}</color>", Convert.ToInt32(((Character)player).GetBodyArmor()));
		stringBuilder.Append("\n" + new string('─', 10) + "  Buffs  " + new string('─', 10));
		if (((Character)player).GetEquipmentMovementModifier() != 0f)
		{
			string arg = ((((Character)player).GetEquipmentMovementModifier() >= 0f) ? "green" : "red");
			stringBuilder.AppendFormat("\nMovement: <color={0}>{1}%</color>", arg, ((Character)player).GetEquipmentMovementModifier() * 100f);
		}
		stringBuilder.Append("\n");
		tooltip.m_text = stringBuilder.ToString();
		tooltip.m_topic = DisplayXpLevel() + player.GetPlayerName();
	}

	private static string DisplayXpLevel()
	{
		if (!Main.showCharacterXP.Value)
		{
			return string.Empty;
		}
		return $"Lv.{XP.level} ";
	}

	private static void BlockStats(Player player, StringBuilder sb)
	{
		ItemData currentBlocker = ((Humanoid)player).GetCurrentBlocker();
		if (currentBlocker != null)
		{
			float skillFactor = ((Character)player).GetSkillFactor((SkillType)6);
			sb.AppendFormat("\n\n$item_blockpower: <color=#ffa500ff>{0}</color>", Convert.ToInt32(currentBlocker.GetBlockPower(skillFactor)));
			if (currentBlocker.m_shared.m_timedBlockBonus > 1f)
			{
				sb.AppendFormat("\n$item_deflection: <color=#ffa500ff>{0}</color>", currentBlocker.GetDeflectionForce(currentBlocker.m_quality));
				sb.AppendFormat("\n$item_parrybonus: <color=#ffa500ff>{0}x</color>", currentBlocker.m_shared.m_timedBlockBonus);
			}
		}
	}

	private static void WeaponStats(Player player, StringBuilder sb)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		ItemData hand = GetHand(player, left: true);
		ItemData hand2 = GetHand(player, left: false);
		ItemData ammoItem = ((Humanoid)player).GetAmmoItem();
		if (hand != null && (int)hand.m_shared.m_itemType != 5)
		{
			DamageTypes damage = hand.GetDamage(hand.m_quality, (float)Game.m_worldLevel);
			if ((int)hand.m_shared.m_skillType == 8 && ammoItem != null)
			{
				damage.Add(ammoItem.GetDamage(), 1);
			}
			sb.AppendFormat("{0}", damage.GetTooltipString(hand.m_shared.m_skillType));
			sb.AppendFormat("\n\n$item_knockback: <color=#ffa500ff>{0}</color>", hand.m_shared.m_attackForce);
			sb.AppendFormat("\n$item_backstab: <color=#ffa500ff>{0}x</color>", hand.m_shared.m_backstabBonus);
		}
		if (hand2 != null)
		{
			DamageTypes damage2 = hand2.GetDamage(hand2.m_quality, (float)Game.m_worldLevel);
			sb.AppendFormat("{0}", damage2.GetTooltipString(hand2.m_shared.m_skillType));
			sb.AppendFormat("\n\n$item_knockback: <color=#ffa500ff>{0}</color>", hand2.m_shared.m_attackForce);
			sb.AppendFormat("\n$item_backstab: <color=#ffa500ff>{0}x</color>", hand2.m_shared.m_backstabBonus);
		}
	}

	private static ItemData GetHand(Player player, bool left)
	{
		if (left)
		{
			return ((Humanoid)player).m_leftItem ?? ((Humanoid)player).m_hiddenLeftItem;
		}
		return ((Humanoid)player).m_rightItem ?? ((Humanoid)player).m_hiddenRightItem;
	}
}
