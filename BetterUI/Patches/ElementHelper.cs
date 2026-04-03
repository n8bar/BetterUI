using UnityEngine.UI;
using ItemData = ItemDrop.ItemData;

namespace BetterUI.Patches;

public static class ElementHelper
{
	public static void UpdateElement(GuiBar durabilityBar, Image icon, ItemData item)
	{
		if (Main.durabilityBarColorPalette.Value != Main.DurabilityBarStyle.Disabled && item.m_shared.m_useDurability && !(item.m_durability <= 0f))
		{
			DurabilityBar.UpdateColor(durabilityBar, item.GetDurabilityPercentage());
		}
	}
}
