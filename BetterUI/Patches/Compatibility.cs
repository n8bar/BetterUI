using UnityEngine;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal static class Compatibility
{
	public static class QuickSlotsHotkeyBar
	{
		private static readonly string originalPath = "QuickSlotsHotkeyBar";

		private static readonly string parent = "hudroot";

		public static bool isUsing = false;

		public static void Unanchor(Hud hud)
		{
			Transform val = ((Component)Hud.instance).transform.Find(parent).Find(originalPath);
			if ((Object)(object)val != (Object)null)
			{
				isUsing = true;
				Object.Destroy((Object)(object)((Component)val).GetComponent("ConfigPositionedElement"));
			}
		}
	}
}
