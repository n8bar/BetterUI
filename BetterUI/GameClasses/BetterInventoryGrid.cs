using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;
using ItemData = ItemDrop.ItemData;
using Element = InventoryGrid.Element;
using Object = UnityEngine.Object;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterInventoryGrid
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]
	private static void PatchInventory(ref InventoryGrid __instance, ref Player player, ItemData dragItem)
	{
		int width = __instance.m_inventory.GetWidth();
		foreach (ItemData allItem in __instance.m_inventory.GetAllItems())
		{
			int num = allItem.m_gridPos.y * width + allItem.m_gridPos.x;
			Element val = null;
			if (num >= 0 && num < __instance.m_elements.Count)
			{
				val = __instance.m_elements[num];
			}
			if (val == null)
			{
				break;
			}
			if ((Object)(object)((Component)val.m_icon).gameObject.GetComponent<ItemIconUpdater>() == (Object)null)
			{
				((Component)val.m_icon).gameObject.AddComponent<ItemIconUpdater>().Setup(val.m_icon);
			}
			ElementHelper.UpdateElement(val.m_durability, val.m_icon, allItem);
			if (Main.showItemStars.Value && allItem.m_shared.m_maxQuality > 1)
			{
				Stars.Draw(val, allItem.m_quality);
			}
		}
	}
}
