using System;
using UnityEngine;

namespace BetterUI.Patches;

internal static class DurabilityBar
{
	private static readonly Color[] normal = (Color[])(object)new Color[4]
	{
		new Color(0.11765f, 0.72941f, 0.03529f, 1f),
		new Color(0.72941f, 0.72941f, 0.03529f, 1f),
		new Color(0.72941f, 0.34902f, 0.03529f, 1f),
		new Color(0.72941f, 0.03529f, 0.03529f, 1f)
	};

	private static readonly Color[] protanopia = (Color[])(object)new Color[4]
	{
		new Color(1f, 1f, 1f, 1f),
		new Color(0.878f, 1f, 0f, 1f),
		new Color(0.192f, 0.859f, 0.573f, 1f),
		new Color(0.11f, 0.435f, 0.973f, 1f)
	};

	private static readonly Array[] colorArray = new Array[2] { normal, protanopia };

	private static readonly Color[] activeColor = colorArray[(int)Main.durabilityBarColorPalette.Value] as Color[];

	public static void UpdateColor(GuiBar durabilityBar, float durability)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		durabilityBar.SetValue(durability);
		durabilityBar.ResetColor();
		if (durability >= 0.75f)
		{
			durabilityBar.SetColor(activeColor[0]);
		}
		else if (durability >= 0.5f)
		{
			durabilityBar.SetColor(activeColor[1]);
		}
		else if (durability >= 0.25f)
		{
			durabilityBar.SetColor(activeColor[2]);
		}
		else if (durability >= 0f)
		{
			durabilityBar.SetColor(activeColor[3]);
		}
	}
}
