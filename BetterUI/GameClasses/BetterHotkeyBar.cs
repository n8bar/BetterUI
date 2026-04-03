using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;
using ItemData = ItemDrop.ItemData;
using ElementData = HotkeyBar.ElementData;
using Object = UnityEngine.Object;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterHotkeyBar
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(HotkeyBar), "UpdateIcons")]
	private static void PatchHotkeyBar(ref HotkeyBar __instance, ref Player player)
	{
		if ((Object)(object)player == (Object)null || ((Character)player).IsDead())
		{
			return;
		}
		foreach (ItemData item in __instance.m_items)
		{
			ElementData val = null;
			float num = item.m_gridPos.x;
			if (num >= 0f && num < (float)__instance.m_elements.Count)
			{
				val = __instance.m_elements[item.m_gridPos.x];
			}
			if (val == null)
			{
				break;
			}
			if ((Object)(object)((Component)val.m_icon).gameObject.GetComponent<ItemIconUpdater>() == (Object)null)
			{
				((Component)val.m_icon).gameObject.AddComponent<ItemIconUpdater>().Setup(val.m_icon);
			}
			ElementHelper.UpdateElement(val.m_durability, val.m_icon, item);
		}
	}
}
