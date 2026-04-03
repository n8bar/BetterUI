namespace BetterUI.Patches;

internal static class CustomWatermark
{
	private static readonly int fontSize = 8;

	private static readonly string colorCode = "#ffff00ff";

	public static void Apply(FejdStartup menu)
	{
		menu.m_versionLabel.text = string.Format("{0}\n<size={1}><color={2}>{3}: ", menu.m_versionLabel.text, fontSize, colorCode, "BetterUI") + "2.5.9</color></size>";
	}
}
