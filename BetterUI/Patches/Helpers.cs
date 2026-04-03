using System;
using System.Text;
using UnityEngine;

namespace BetterUI.Patches;

internal class Helpers
{
	public static string Repeat(string value, int count)
	{
		return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
	}

	public static string TimeString(double val1, double val2)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(val1 - val2);
		if (timeSpan.Hours <= 0)
		{
			if (timeSpan.Minutes <= 0)
			{
				return $"{timeSpan.Seconds:D2}s";
			}
			return $"{timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
		}
		return $"{timeSpan.Hours:D2}h {timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
	}

	public static string TimeString(double seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		if (timeSpan.Hours <= 0)
		{
			if (timeSpan.Minutes <= 0)
			{
				return $"{timeSpan.Seconds:D2}s";
			}
			return $"{timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
		}
		return $"{timeSpan.Hours:D2}h {timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
	}

	public static void DebugLine(string str = "", bool pref = true, bool warn = false)
	{
		if (Main.isDebug.Value)
		{
			if (warn)
			{
				Main.log.LogWarning((object)((pref ? ("[" + typeof(Main).Namespace + "] ") : "") + str));
			}
			else
			{
				Main.log.LogInfo((object)((pref ? ("[" + typeof(Main).Namespace + "] ") : "") + str));
			}
		}
	}

	public static bool CheckHeldKey(KeyCode key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return Input.GetKey(key);
		}
		catch
		{
			return false;
		}
	}
}
