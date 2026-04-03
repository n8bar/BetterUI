using System.Collections.Generic;
using BepInEx.Configuration;
using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;
using MessageType = MessageHud.MessageType;
using Object = UnityEngine.Object;

namespace BetterUI.GameClasses;

[HarmonyPatch]
public static class BetterHud
{
	private static Player _player = null;

	private static Vector3 lastMousePos = Vector3.zero;

	private static float lastScrollPos = 0f;

	private static string currentlyDragging = "";

	private static bool isEditing = false;

	private static int activeLayer = 0;

	private static bool enablePlayerHudEditing;

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "Awake")]
	private static void Awake(ref Hud __instance)
	{
		enablePlayerHudEditing = Main.enablePlayerHudEditing.Value;
		if (Main.showCharacterXP.Value && Main.showCharacterXpBar.Value)
		{
			XPBar.Create(__instance);
		}
		if (Main.customHealthBar.Value != Main.CustomBarState.off)
		{
			CustomBars.HealthBar.Create();
		}
		if (Main.customStaminaBar.Value != Main.CustomBarState.off)
		{
			CustomBars.StaminaBar.Create();
		}
		if (Main.customEitrBar.Value != Main.CustomBarState.off)
		{
			CustomBars.EitrBar.Create();
		}
		if (Main.customFoodBar.Value != Main.CustomBarState.off)
		{
			CustomBars.FoodBar.Create();
		}
		if (enablePlayerHudEditing)
		{
			Compatibility.QuickSlotsHotkeyBar.Unanchor(__instance);
			CustomHud.Load(__instance);
			CustomHud.PositionTemplates();
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "Update")]
	private static void Update(Hud __instance)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if (Main.showCharacterXP.Value && (Object)(object)_player == (Object)null && (Object)(object)localPlayer != (Object)null)
		{
			_player = localPlayer;
			XP.Awake(localPlayer);
			XPBar.UpdateLevelProgressPercentage();
		}
		if (!enablePlayerHudEditing || (Object)(object)localPlayer == (Object)null)
		{
			return;
		}
		if (Input.GetKeyDown(Main.togglePlayerHudEditModeKey.Value))
		{
			isEditing = !isEditing;
			CustomHud.ShowTemplates(isEditing, activeLayer);
			((Character)Player.m_localPlayer).Message((MessageType)2, "HUD editing is turned " + (isEditing ? "ON" : "OFF"), 0, (Sprite)null);
		}
		if (!isEditing)
		{
			return;
		}
		if (Input.GetKeyDown((KeyCode)324))
		{
			activeLayer = ((activeLayer != CustomHud.roots.Count) ? (activeLayer + 1) : 0);
			Helpers.DebugLine($"Layer changed to: {(Groups)activeLayer}");
			CustomHud.ShowTemplates(isEditing, activeLayer);
			((Character)Player.m_localPlayer).Message((MessageType)2, $"Now editing: {(Groups)activeLayer}", 0, (Sprite)null);
		}
		float largeGuiScale = GuiScaler.m_largeGuiScale;
		Vector3 mousePosition = Input.mousePosition;
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (lastMousePos == Vector3.zero)
		{
			lastMousePos = mousePosition;
		}
		if (lastScrollPos == 0f)
		{
			lastScrollPos = axis;
		}
		List<KeyValuePair<string, RectTransform>> list = new List<KeyValuePair<string, RectTransform>>();
		foreach (HudElement element in CustomHud.elements)
		{
			try
			{
				RectTransform val = CustomHud.LocateTemplateRect(element.Name);
				if ((Object)(object)val != (Object)null && element.Group == (Groups)activeLayer)
				{
					list.Add(new KeyValuePair<string, RectTransform>(element.Name, val));
				}
			}
			catch
			{
				Helpers.DebugLine("Issues while locating UI templates. Your uiData might be corrupted.\nIssue on: " + element.Name + " (" + element.DisplayName + ")");
			}
		}
		Vector3 val2 = (mousePosition - lastMousePos) / largeGuiScale;
		if (Helpers.CheckHeldKey(Main.modKeyPrimary.Value) && list.Count > 0)
		{
			if (currentlyDragging != string.Empty)
			{
				if (Helpers.CheckHeldKey(Main.modKeySecondary.Value))
				{
					Vector2 val3 = new Vector2(val2.x, val2.y);
					Resolution currentResolution = Screen.currentResolution;
					float num = currentResolution.width;
					currentResolution = Screen.currentResolution;
					Vector2 dimensionChanges = val3 / (new Vector2(num, (float)currentResolution.height) / 10f);
					CustomHud.UpdateScaleAndDimensions(currentlyDragging, dimensionChanges, axis);
				}
				else
				{
					CustomHud.UpdatePosition(currentlyDragging, new Vector2(val2.x, val2.y));
					ConfigEntry<Main.CustomBarState> val4 = null;
					switch (currentlyDragging)
					{
					case "BetterUI_FoodBar":
						val4 = Main.customFoodBar;
						break;
					case "BetterUI_HPBar":
						val4 = Main.customHealthBar;
						break;
					case "BetterUI_StaminaBar":
						val4 = Main.customStaminaBar;
						break;
					case "BetterUI_EitrBar":
						val4 = Main.customEitrBar;
						break;
					}
					if (val4 != null)
					{
						if (axis > 0f)
						{
							val4.Value = CustomBars.BarHelper.IncrementRotation(val4.Value);
						}
						else if (axis < 0f)
						{
							val4.Value = CustomBars.BarHelper.DecrementRotation(val4.Value);
						}
					}
				}
			}
			else
			{
				foreach (KeyValuePair<string, RectTransform> item in list)
				{
					if (RectTransformUtility.RectangleContainsScreenPoint(item.Value, new Vector2(mousePosition.x, mousePosition.y)))
					{
						currentlyDragging = item.Key;
						break;
					}
				}
			}
		}
		else
		{
			currentlyDragging = string.Empty;
		}
		lastMousePos = mousePosition;
		lastScrollPos = axis;
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "UpdateHealth")]
	private static void UpdateHealth(Player player)
	{
		CustomBars.HealthBar.Update(((Character)player).GetMaxHealth(), ((Character)player).GetHealth());
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "UpdateStamina")]
	private static void UpdateStamina(Player player)
	{
		CustomBars.StaminaBar.Update(((Character)player).GetMaxStamina(), player.GetStamina());
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "UpdateEitr")]
	private static void UpdateEitr(Player player)
	{
		CustomBars.EitrBar.Update(((Character)player).GetMaxEitr(), player.GetEitr());
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Hud), "UpdateFood")]
	private static void UpdateFood(Player player)
	{
		CustomBars.FoodBar.Update(player);
	}
}
