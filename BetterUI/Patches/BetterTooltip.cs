using System.Text;
using UnityEngine;
using ItemData = ItemDrop.ItemData;
using DamageTypes = HitData.DamageTypes;
using ItemType = ItemDrop.ItemData.ItemType;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal static class BetterTooltip
{
	private static readonly int starsSize = 22;

	private static readonly char arrow = '→';

	private static StringBuilder _sb;

	private static ItemData _item;

	private static bool _crafting;

	private static int _quality;

	private static void Crafted()
	{
		_sb.AppendFormat("\n$item_crafter: <color=#ffa500ff>{0}</color>", _item.m_crafterName);
	}

	private static void CustomDamageCalculations(int newLvl, int oldLvl)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		DamageTypes damage = _item.GetDamage(oldLvl, (float)Game.m_worldLevel);
		DamageTypes damage2 = _item.GetDamage(newLvl, (float)Game.m_worldLevel);
		if (damage2.m_damage > damage.m_damage)
		{
			_sb.AppendFormat("\n$inventory_damage: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_damage, arrow, damage2.m_damage);
		}
		if (damage2.m_blunt > damage.m_blunt)
		{
			_sb.AppendFormat("\n$inventory_blunt: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_blunt, arrow, damage2.m_blunt);
		}
		if (damage2.m_slash > damage.m_slash)
		{
			_sb.AppendFormat("\n$inventory_slash: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_slash, arrow, damage2.m_slash);
		}
		if (damage2.m_pierce > damage.m_pierce)
		{
			_sb.AppendFormat("\n$inventory_pierce: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_pierce, arrow, damage2.m_pierce);
		}
		if (damage2.m_fire > damage.m_fire)
		{
			_sb.AppendFormat("\n$inventory_fire: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_fire, arrow, damage2.m_fire);
		}
		if (damage2.m_frost > damage.m_frost)
		{
			_sb.AppendFormat("\n$inventory_frost: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_frost, arrow, damage2.m_frost);
		}
		if (damage2.m_lightning > damage.m_lightning)
		{
			_sb.AppendFormat("\n$inventory_lightning: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_lightning, arrow, damage2.m_lightning);
		}
		if (damage2.m_poison > damage.m_poison)
		{
			_sb.AppendFormat("\n$inventory_poison: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_poison, arrow, damage2.m_poison);
		}
		if (damage2.m_spirit > damage.m_spirit)
		{
			_sb.AppendFormat("\n$inventory_spirit: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", damage.m_spirit, arrow, damage2.m_spirit);
		}
	}

	private static void Description()
	{
		_sb.Append(_item.m_shared.m_description + "\n");
	}

	private static void DamageModifiers()
	{
		string damageModifiersTooltipString = SE_Stats.GetDamageModifiersTooltipString(_item.m_shared.m_damageModifiers);
		if (damageModifiersTooltipString.Length > 0)
		{
			_sb.Append(damageModifiersTooltipString);
		}
	}

	private static void DLC()
	{
		_sb.Append("\n<color=#00ffffff>$item_dlc</color>");
	}

	private static void Durability(int qualityLevel, bool crafting)
	{
		if (crafting)
		{
			float maxDurability = _item.GetMaxDurability(qualityLevel);
			if (qualityLevel <= 1)
			{
				_sb.AppendFormat("\n$item_durability: <color=#ffa500ff>{0}</color>", maxDurability);
			}
			else if (qualityLevel > _item.m_shared.m_maxQuality)
			{
				float maxDurability2 = _item.GetMaxDurability(qualityLevel - 1);
				_sb.AppendFormat("\n$item_durability: <color=#ffa500ff>{0}</color>", maxDurability2);
			}
			else
			{
				float maxDurability3 = _item.GetMaxDurability(qualityLevel - 1);
				_sb.AppendFormat("\n$item_durability: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", maxDurability3, arrow, maxDurability);
			}
		}
		else
		{
			float maxDurability4 = _item.GetMaxDurability(qualityLevel);
			float durability = _item.m_durability;
			_sb.AppendFormat("\n$item_durability: {0} / {1}", durability.ToString("0"), maxDurability4.ToString("0"));
		}
	}

	private static void ItemType(int qualityLevel, float skillLevel)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected I4, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		ItemType itemType = _item.m_shared.m_itemType;
		DamageTypes damage;
		switch ((int)itemType - 2)
		{
		case 0:
		{
			if (_item.m_shared.m_food > 0f)
			{
				_sb.AppendFormat("\n$item_food_health: <color=#ff0000ff>{0}</color>", _item.m_shared.m_food);
				_sb.AppendFormat("\n$item_food_stamina: <color=#ffff00ff>{0}</color>", _item.m_shared.m_foodStamina);
				if (_item.m_shared.m_foodEitr > 0f)
				{
					_sb.AppendFormat("\n$item_food_eitr: <color=#00ffffff>{0}</color>", _item.m_shared.m_foodEitr);
				}
				_sb.AppendFormat("\n$item_food_duration: <color=#ffa500ff>{0}s ({1}m)</color>", _item.m_shared.m_foodBurnTime, _item.m_shared.m_foodBurnTime / 60f);
				_sb.AppendFormat("\n$item_food_regen: <color=#ffa500ff>{0} hp/tick</color>", _item.m_shared.m_foodRegen);
			}
			string statusEffectTooltip3 = _item.GetStatusEffectTooltip(qualityLevel, skillLevel);
			if (statusEffectTooltip3.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(statusEffectTooltip3);
			}
			break;
		}
		case 1:
		case 2:
		case 12:
		case 13:
		{
			StringBuilder sb2 = _sb;
			damage = _item.GetDamage(qualityLevel, (float)Game.m_worldLevel);
			sb2.Append(damage.GetTooltipString(_item.m_shared.m_skillType));
			_sb.AppendFormat("\n$item_knockback: <color=#ffa500ff>{0}</color>", _item.m_shared.m_attackForce);
			_sb.AppendFormat("\n$item_backstab: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_backstabBonus);
			_sb.AppendFormat("\n\n$item_blockpower: <color=#ffa500ff>{0}</color> <color=#ffff00ff>({1})</color>", _item.GetBaseBlockPower(qualityLevel), _item.GetBlockPowerTooltip(qualityLevel).ToString("0"));
			if (_item.m_shared.m_timedBlockBonus > 1f)
			{
				_sb.AppendFormat("\n$item_deflection: <color=#ffa500ff>{0}</color>", _item.GetDeflectionForce(qualityLevel));
				_sb.AppendFormat("\n$item_parrybonus: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_timedBlockBonus);
			}
			string projectileTooltip = _item.GetProjectileTooltip(qualityLevel);
			if (projectileTooltip.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(projectileTooltip);
			}
			string statusEffectTooltip2 = _item.GetStatusEffectTooltip(qualityLevel, skillLevel);
			if (statusEffectTooltip2.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(statusEffectTooltip2);
			}
			_sb.Append("\n");
			break;
		}
		case 3:
			_sb.AppendFormat("\n$item_blockpower: <color=#ffa500ff>{0}</color> <color=#ffff00ff>({1})</color>", _item.GetBaseBlockPower(qualityLevel), _item.GetBlockPowerTooltip(qualityLevel).ToString("0"));
			if (_item.m_shared.m_timedBlockBonus > 1f)
			{
				_sb.AppendFormat("\n$item_deflection: <color=#ffa500ff>{0}</color>", _item.GetDeflectionForce(qualityLevel));
				_sb.AppendFormat("\n$item_parrybonus: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_timedBlockBonus);
				_sb.Append("\n");
			}
			break;
		case 4:
		case 5:
		case 9:
		case 15:
		{
			_sb.AppendFormat("\n$item_armor: <color=#ffa500ff>{0}</color>", _item.GetArmor(qualityLevel, (float)Game.m_worldLevel));
			string statusEffectTooltip = _item.GetStatusEffectTooltip(qualityLevel, skillLevel);
			if (statusEffectTooltip.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(statusEffectTooltip);
			}
			break;
		}
		case 7:
		{
			StringBuilder sb = _sb;
			damage = _item.GetDamage(qualityLevel, (float)Game.m_worldLevel);
			sb.Append(damage.GetTooltipString(_item.m_shared.m_skillType));
			_sb.AppendFormat("\n$item_knockback: <color=#ffa500ff>{0}</color>", _item.m_shared.m_attackForce);
			break;
		}
		case 6:
		case 8:
		case 10:
		case 11:
		case 14:
			break;
		}
	}

	private static void Movement()
	{
		string arg = ((_item.m_shared.m_movementModifier >= 0f) ? "green" : "red");
		_sb.AppendFormat("\n$item_movement_modifier: <color={0}>{1}%</color>", arg, (_item.m_shared.m_movementModifier * 100f).ToString("+0;-0"));
	}

	private static void Quality(int qualityLevel)
	{
		_sb.AppendFormat("\n$item_quality: <color=#ffa500ff>{0}</color>", qualityLevel);
	}

	private static void RepairStation()
	{
		Recipe recipe = ObjectDB.instance.GetRecipe(_item);
		if ((Object)(object)recipe != (Object)null)
		{
			int minStationLevel = recipe.m_minStationLevel;
			_sb.AppendFormat("\n$item_repairlevel: <color=#ffa500ff>{0}</color>", minStationLevel.ToString());
		}
	}

	private static void Stars(int qualityLevel)
	{
		string arg = Helpers.Repeat("★", qualityLevel);
		_sb.AppendFormat("\n<size={0}><color=#ffff00ff>{1}</color></size>", starsSize, arg);
	}

	private static void StatusEffect(string effectText)
	{
		string arg = $"$item_seteffect ({_item.m_shared.m_setSize})";
		_sb.AppendFormat("\n\n<color=#c0c0c0ff>{0}</color>", arg);
		_sb.AppendFormat("\n<color=#ffa500ff>{0}</color>", effectText);
	}

	private static void Teleport()
	{
		_sb.Append("\n<color=#ff0000ff>$item_noteleport</color>");
	}

	private static void UpgradeStats(Player localPlayer)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected I4, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		int quality = _quality;
		int num = _quality - 1;
		ItemType itemType = _item.m_shared.m_itemType;
		DamageTypes damage;
		switch ((int)itemType - 3)
		{
		case 0:
		case 1:
		case 11:
		case 12:
		{
			damage = _item.GetDamage(quality, (float)Game.m_worldLevel);
			float totalDamage = damage.GetTotalDamage();
			damage = _item.GetDamage(num, (float)Game.m_worldLevel);
			if (totalDamage > damage.GetTotalDamage())
			{
				CustomDamageCalculations(quality, num);
			}
			else
			{
				StringBuilder sb2 = _sb;
				damage = _item.GetDamage(quality, (float)Game.m_worldLevel);
				sb2.Append(damage.GetTooltipString(_item.m_shared.m_skillType));
			}
			_sb.AppendFormat("\n$item_knockback: <color=#ffa500ff>{0}</color>", _item.m_shared.m_attackForce);
			_sb.AppendFormat("\n$item_backstab: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_backstabBonus);
			if (_item.GetBaseBlockPower(quality) > _item.GetBaseBlockPower(num))
			{
				_sb.AppendFormat("\n\n$item_blockpower: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", _item.GetBaseBlockPower(num), arrow, _item.GetBaseBlockPower(quality));
			}
			else
			{
				_sb.AppendFormat("\n\n$item_blockpower: <color=#ffa500ff>{0}</color> <color=#ffff00ff>({1})</color>", _item.GetBaseBlockPower(quality), _item.GetBlockPowerTooltip(quality).ToString("0"));
			}
			if (_item.m_shared.m_timedBlockBonus > 1f)
			{
				if (_item.GetDeflectionForce(quality) > _item.GetDeflectionForce(num))
				{
					_sb.AppendFormat("\n$item_deflection: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", _item.GetDeflectionForce(num), arrow, _item.GetDeflectionForce(quality));
				}
				else
				{
					_sb.AppendFormat("\n$item_deflection: <color=#ffa500ff>{0}</color>", _item.GetDeflectionForce(quality));
				}
				_sb.AppendFormat("\n$item_parrybonus: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_timedBlockBonus);
			}
			string projectileTooltip = _item.GetProjectileTooltip(quality);
			if (projectileTooltip.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(projectileTooltip);
			}
			string statusEffectTooltip2 = _item.GetStatusEffectTooltip(quality, ((Character)localPlayer).GetSkillLevel(_item.m_shared.m_skillType));
			if (statusEffectTooltip2.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(statusEffectTooltip2);
			}
			_sb.Append("\n");
			break;
		}
		case 2:
			if (_item.GetBaseBlockPower(quality) > _item.GetBaseBlockPower(num))
			{
				_sb.AppendFormat("\n$item_blockpower: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", _item.GetBaseBlockPower(num), arrow, _item.GetBaseBlockPower(quality));
			}
			else
			{
				_sb.AppendFormat("\n$item_blockpower: <color=#ffa500ff>{0}</color> <color=#ffff00ff>({1})</color>", _item.GetBaseBlockPower(quality), _item.GetBlockPowerTooltip(quality).ToString("0"));
			}
			if (_item.m_shared.m_timedBlockBonus > 1f)
			{
				if (_item.GetDeflectionForce(quality) > _item.GetDeflectionForce(num))
				{
					_sb.AppendFormat("\n$item_deflection: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", _item.GetDeflectionForce(num), arrow, _item.GetDeflectionForce(quality));
				}
				else
				{
					_sb.AppendFormat("\n$item_deflection: <color=#ffa500ff>{0}</color>", _item.GetDeflectionForce(quality));
				}
				_sb.AppendFormat("\n$item_parrybonus: <color=#ffa500ff>{0}x</color>", _item.m_shared.m_timedBlockBonus);
				_sb.Append("\n");
			}
			break;
		case 3:
		case 4:
		case 8:
		case 14:
		{
			if (_item.GetArmor(quality, (float)Game.m_worldLevel) > _item.GetArmor(num, (float)Game.m_worldLevel))
			{
				_sb.AppendFormat("\n$item_armor: <color=#c0c0c0ff>{0}</color> {1} <color=#ffa500ff>{2}</color>", _item.GetArmor(num, (float)Game.m_worldLevel), arrow, _item.GetArmor(quality, (float)Game.m_worldLevel));
			}
			else
			{
				_sb.AppendFormat("\n$item_armor: <color=#ffa500ff>{0}</color>", _item.GetArmor(quality, (float)Game.m_worldLevel));
			}
			string statusEffectTooltip = _item.GetStatusEffectTooltip(quality, ((Character)localPlayer).GetSkillLevel(_item.m_shared.m_skillType));
			if (statusEffectTooltip.Length > 0)
			{
				_sb.Append("\n\n");
				_sb.Append(statusEffectTooltip);
			}
			break;
		}
		case 6:
		{
			StringBuilder sb = _sb;
			damage = _item.GetDamage(quality, (float)Game.m_worldLevel);
			sb.Append(damage.GetTooltipString(_item.m_shared.m_skillType));
			_sb.AppendFormat("\n$item_knockback: <color=#ffa500ff>{0}</color>", _item.m_shared.m_attackForce);
			break;
		}
		case 5:
		case 7:
		case 9:
		case 10:
		case 13:
			break;
		}
	}

	private static void Value()
	{
		_sb.AppendFormat("\n$item_value: <color=#ffa500ff>{0}  ({1})</color>", _item.GetValue(), _item.m_shared.m_value);
	}

	private static void Weight()
	{
		_sb.AppendFormat("\n\n$item_weight: <color=#ffa500ff>{0}</color>", _item.GetWeight(-1).ToString("F1"));
	}

	private static void WieldType()
	{
		StringBuilder stringBuilder = new StringBuilder();
		ItemData.AddHandedTip(_item, stringBuilder);
		_sb.AppendFormat("<color=#c0c0c0ff>{0}</color>", stringBuilder);
	}

	public static string Create(ItemData item, int qualityLevel, bool crafting)
	{
		Player localPlayer = Player.m_localPlayer;
		_sb = new StringBuilder(256);
		_item = item;
		_quality = qualityLevel;
		_crafting = crafting;
		Description();
		if (crafting && _item.m_shared.m_maxQuality > 1)
		{
			if (qualityLevel <= 1)
			{
				BasicTooltip(localPlayer);
			}
			else if (qualityLevel > _item.m_shared.m_maxQuality)
			{
				CraftingTooltip(localPlayer, isMax: true);
			}
			else
			{
				CraftingTooltip(localPlayer);
			}
		}
		else
		{
			BasicTooltip(localPlayer);
		}
		Weight();
		if (_item.m_crafterID != 0L)
		{
			Crafted();
		}
		return _sb.ToString();
	}

	private static void BasicTooltip(Player localPlayer, bool isMax = false)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (_item.m_shared.m_dlc.Length > 0)
		{
			DLC();
		}
		WieldType();
		if (!_item.m_shared.m_teleportable)
		{
			Teleport();
		}
		if (_item.m_shared.m_value > 0)
		{
			Value();
		}
		ItemType(_quality, localPlayer.m_skills.GetSkillLevel(_item.m_shared.m_skillType));
		if (_item.m_shared.m_useDurability)
		{
			if (isMax)
			{
				Durability(_quality + 1, _crafting);
			}
			else
			{
				Durability(_quality, _crafting);
			}
			if (_item.m_shared.m_canBeReparied)
			{
				RepairStation();
			}
			_sb.Append("\n");
		}
		localPlayer.AppendEquipmentModifierTooltips(_item, _sb);
		DamageModifiers();
		string setStatusEffectTooltip = _item.GetSetStatusEffectTooltip(_quality, ((Character)localPlayer).GetSkillLevel(_item.m_shared.m_skillType));
		if (setStatusEffectTooltip.Length > 0)
		{
			StatusEffect(setStatusEffectTooltip);
		}
	}

	private static void CraftingTooltip(Player localPlayer, bool isMax = false)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (isMax)
		{
			_quality--;
			BasicTooltip(localPlayer, isMax: true);
			return;
		}
		if (_item.m_shared.m_dlc.Length > 0)
		{
			DLC();
		}
		WieldType();
		if (!_item.m_shared.m_teleportable)
		{
			Teleport();
		}
		if (_item.m_shared.m_value > 0)
		{
			Value();
		}
		UpgradeStats(localPlayer);
		if (_item.m_shared.m_useDurability)
		{
			Durability(_quality, _crafting);
			if (_item.m_shared.m_canBeReparied)
			{
				RepairStation();
			}
			_sb.Append("\n");
		}
		localPlayer.AppendEquipmentModifierTooltips(_item, _sb);
		DamageModifiers();
		string setStatusEffectTooltip = _item.GetSetStatusEffectTooltip(_quality, ((Character)localPlayer).GetSkillLevel(_item.m_shared.m_skillType));
		if (setStatusEffectTooltip.Length > 0)
		{
			StatusEffect(setStatusEffectTooltip);
		}
	}
}
