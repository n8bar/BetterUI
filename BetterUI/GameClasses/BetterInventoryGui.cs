using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterInventoryGui
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(InventoryGui), "UpdateCharacterStats")]
	public static void PatchArmor(ref InventoryGui __instance, ref Player player)
	{
		if (Main.showCombinedItemStats.Value)
		{
			if ((Object)(object)InventoryArmorTooltip.tooltip == (Object)null)
			{
				InventoryArmorTooltip.Awake(__instance);
			}
			float bodyArmor = ((Character)player).GetBodyArmor();
			InventoryArmorTooltip.m_armor.text = bodyArmor.ToString();
			InventoryArmorTooltip.Update(player);
		}
	}
}
