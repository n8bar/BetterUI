using System;
using BepInEx.Bootstrap;
using BetterUI.Patches;
using HarmonyLib;
using ItemData = ItemDrop.ItemData;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterItemDrop
{
	[HarmonyPatch]
	private static class ItemDataPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ItemData), "GetTooltip", new Type[]
		{
			typeof(ItemData),
			typeof(int),
			typeof(bool),
			typeof(float),
			typeof(int)
		})]
		public static bool PatchTooltip(ref string __result, ItemData item, int qualityLevel, bool crafting)
		{
			if (!Main.showCustomTooltips.Value || Chainloader.PluginInfos.ContainsKey("randyknapp.mods.epicloot"))
			{
				return true;
			}
			__result = BetterTooltip.Create(item, qualityLevel, crafting);
			return false;
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ItemDrop), "GetHoverText")]
	private static bool PatchHoverText(ref ItemDrop __instance, ref string __result)
	{
		string text = __instance.m_itemData.m_shared.m_name;
		if (__instance.m_itemData.m_quality > 1)
		{
			text = string.Concat(new object[4]
			{
				text,
				" (<color=#ffff00ff>",
				Stars.HoverText(__instance.m_itemData.m_quality),
				"</color>) "
			});
		}
		if (__instance.m_itemData.m_stack > 1)
		{
			text = text + " x" + __instance.m_itemData.m_stack;
		}
		__result = Localization.instance.Localize(text + "\n[<color=#ffff00ff><b>$KEY_Use</b></color>] $inventory_pickup");
		return false;
	}
}
