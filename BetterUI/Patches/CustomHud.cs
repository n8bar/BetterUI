using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HudData = EnemyHud.HudData;
using MessageType = MessageHud.MessageType;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal static class CustomHud
{
	public static List<HudElement> elements;

	public static Dictionary<Groups, Transform> roots = new Dictionary<Groups, Transform>();

	public static Dictionary<Groups, Transform> templates = new Dictionary<Groups, Transform>();

	private static Transform hudRoot;

	private static Transform invRoot;

	private static Transform baseRoot;

	public static readonly string templateSuffix = "_template";

	private static readonly Element[] supportedElements = new Element[19]
	{
		new Element("HotKeyBar", Groups.HudRoot),
		new Element("BuildHud", Groups.HudRoot, "BuildHud/SelectedInfo"),
		new Element("MiniMap", Groups.HudRoot, "MiniMap/small"),
		new Element("GuardianPower", Groups.HudRoot),
		new Element("StatusEffects", Groups.HudRoot),
		new Element("SaveIcon", Groups.HudRoot),
		new Element("BadConnectionIcon", Groups.HudRoot),
		new Element("BuildHints", Groups.HudRoot, "KeyHints/BuildHints"),
		new Element("CombatHints", Groups.HudRoot, "KeyHints/CombatHints"),
		new Element("Player", Groups.Inventory, "Player", "PlayerInventory"),
		new Element("Container", Groups.Inventory, "Container", "ChestContainer"),
		new Element("Info", Groups.Inventory, "Info", "UITab"),
		new Element("Crafting", Groups.Inventory, "Crafting", "CraftingWindow"),
		new Element("BetterUI_HPBar", Groups.HudRoot, "BetterUI_HPBar", "HP Bar"),
		new Element("BetterUI_FoodBar", Groups.HudRoot, "BetterUI_FoodBar", "Food Bar"),
		new Element("BetterUI_StaminaBar", Groups.HudRoot, "BetterUI_StaminaBar", "Stamina Bar"),
		new Element("BetterUI_EitrBar", Groups.HudRoot, "BetterUI_EitrBar", "Eitr Bar"),
		new Element("QuickSlots", Groups.HudRoot, "QuickSlotsHotkeyBar", "QuickSlots"),
		new Element("BossHud", Groups.HudRoot, "EnemyHud/HudRoot/HudBaseBoss", "Boss Health Bar")
	};

	public static void Load(Hud hud)
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			hudRoot = ((Component)hud).transform.Find("hudroot");
			invRoot = ((Component)InventoryGui.instance).transform.Find("root");
			baseRoot = ((Component)MessageHud.instance).transform;
			roots[Groups.HudRoot] = hudRoot;
			roots[Groups.Inventory] = invRoot;
			if (Main.uiData.Value == "none" || Main.uiData.Value == "")
			{
				Helpers.DebugLine("User has no uiData. Creating basic template.");
				elements = new List<HudElement>();
			}
			else
			{
				try
				{
					elements = (List<HudElement>)Convert.FromBase64String(Main.uiData.Value).DeSerialize();
					Helpers.DebugLine($"User has {elements.Count} ui elements set.");
				}
				catch
				{
					Helpers.DebugLine("FAILED to DeSerialize uiData: " + Main.uiData.Value);
				}
				foreach (HudElement element in elements)
				{
					element.OnAfterDeserialize();
				}
			}
			if (elements.Count < supportedElements.Length)
			{
				Element[] array = supportedElements;
				for (int i = 0; i < array.Length; i++)
				{
					Element e = array[i];
					if (!elements.Exists((HudElement he) => he.Name == e.Name))
					{
						Helpers.DebugLine("Adding to elements: " + e.Name + " with path: " + e.LocationPath);
						elements.Add(new HudElement(e.Name, e.DisplayName, e.Group, e.LocationPath, Vector2.zero));
						if (elements.Count == supportedElements.Length)
						{
							break;
						}
					}
				}
			}
			else if (elements.Count > supportedElements.Length)
			{
				Helpers.DebugLine("Seems that your UI might be corrupted!", pref: true, warn: true);
			}
			CreateTemplates();
		}
		catch (Exception ex)
		{
			Helpers.DebugLine("Issue while CustomHud Load. " + ex.Message, pref: true, warn: true);
		}
	}

	public static void Save()
	{
		try
		{
			elements.RemoveAll((HudElement e) => e.Position == Vector2.zero);
			byte[] array = elements.Serialize();
			Helpers.DebugLine($"uiData bytes: {array.Length}");
			string value = Convert.ToBase64String(array);
			Main.uiData.Value = value;
		}
		catch (Exception ex)
		{
			Helpers.DebugLine("FAILED to Save: " + ex.Message);
		}
	}

	public static void ShowTemplates(bool show, int activeLayer)
	{
		foreach (HudElement element in elements)
		{
			if (activeLayer == (int)element.Group)
			{
				RectTransform val = LocateTemplateRect(element.Name);
				if ((Object)(object)val != (Object)null)
				{
					((Component)val).gameObject.SetActive(show);
				}
			}
			else
			{
				RectTransform val2 = LocateTemplateRect(element.Name);
				if ((Object)(object)val2 != (Object)null)
				{
					((Component)val2).gameObject.SetActive(false);
				}
			}
		}
		if (!show)
		{
			Save();
		}
	}

	public static void UpdatePosition(string name, Vector2 posChange)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		HudElement hudElement = elements.Find((HudElement e) => e.Name == name);
		if (hudElement.Name == name)
		{
			hudElement.Position += posChange;
			if (hudElement.Group == Groups.Inventory)
			{
				Vector3 val3 = Camera.main.ScreenToViewportPoint(new Vector3(posChange.x, posChange.y, 0f));
				Vector2 val = new Vector2(val3.x, val3.y);
				hudElement.AnchorMin += val;
				hudElement.AnchorMax += val;
			}
			PositionTemplate(hudElement);
		}
	}

	public static void UpdateScaleAndDimensions(string name, Vector2 dimensionChanges, float scaleChange)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		HudElement hudElement = elements.Find((HudElement e) => e.Name == name);
		if (hudElement.Name == name)
		{
			if (scaleChange != 0f)
			{
				hudElement.ChangeScale(scaleChange);
				((Character)Player.m_localPlayer).Message((MessageType)2, $"{hudElement.DisplayName} size: {hudElement.Scale}", 0, (Sprite)null);
			}
			if (dimensionChanges != Vector2.zero)
			{
				hudElement.ChangeXDims(dimensionChanges.x);
				hudElement.ChangeYDims(dimensionChanges.y);
				((Character)Player.m_localPlayer).Message((MessageType)2, $"{hudElement.DisplayName} dimensions: ({hudElement.XDimensions},{hudElement.YDimensions})", 0, (Sprite)null);
				PositionTemplate(hudElement);
			}
		}
		else
		{
			Helpers.DebugLine("Invalid call when updating element: " + name, pref: true, warn: true);
		}
	}

	private static void PositionTemplate(HudElement e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			RectTransform val = LocateRectTransform(e.Group, e.Path);
			RectTransform val2 = LocateTemplateRect(e.Name);
			if ((Object)(object)val == (Object)null)
			{
				return;
			}
			if (e.Name == "BossHud")
			{
				val.anchoredPosition = e.Position;
				((Transform)val).localScale = new Vector3(e.Scale * e.XDimensions, e.Scale * e.YDimensions);
				EnemyHud instance = EnemyHud.m_instance;
				if ((Object)(object)instance != (Object)null)
				{
					foreach (KeyValuePair<Character, HudData> hud in instance.m_huds)
					{
						if (hud.Key.IsBoss())
						{
							RectTransform component = hud.Value.m_gui.GetComponent<RectTransform>();
							if ((Object)(object)component != (Object)null)
							{
								component.anchoredPosition = e.Position;
								((Transform)component).localScale = new Vector3(e.Scale * e.XDimensions, e.Scale * e.YDimensions);
							}
						}
					}
				}
			}
			if (e.Group == Groups.Inventory)
			{
				float largeGuiScale = GuiScaler.m_largeGuiScale;
				Camera.main.ViewportToScreenPoint(new Vector3(e.AnchorMax.x, e.AnchorMax.y, 0f));
				_ = e.Position;
				val.anchorMin = e.AnchorMin;
				val.anchorMax = e.AnchorMax;
				val2.anchoredPosition = e.Position / largeGuiScale;
			}
			else
			{
				val.anchoredPosition = e.Position;
				val2.anchoredPosition = e.Position;
			}
			((Transform)val).localScale = new Vector3(e.Scale * e.XDimensions, e.Scale * e.YDimensions);
			((Transform)val2).localScale = ((Transform)val).localScale;
		}
		catch
		{
			Helpers.DebugLine("PositionTemplate Catch: " + e.Name);
		}
	}

	public static RectTransform LocateRectTransform(Groups group, string path)
	{
		try
		{
			roots.TryGetValue(group, out Transform value);
			if (group == Groups.Inventory)
			{
				value = ((Component)InventoryGui.instance).transform.Find("root");
			}
			if (path.StartsWith("EnemyHud/"))
			{
				Transform val = hudRoot.parent.parent.Find(path);
				if ((Object)(object)val != (Object)null)
				{
					return ((Component)val).GetComponent<RectTransform>();
				}
			}
			return ((Component)value.Find(path)).GetComponent<RectTransform>();
		}
		catch
		{
			return null;
		}
	}

	public static RectTransform LocateTemplateRect(string name)
	{
		try
		{
			return ((Component)baseRoot.Find(name + templateSuffix)).GetComponent<RectTransform>();
		}
		catch
		{
			Helpers.DebugLine("Unable to find template for " + name, pref: true, warn: true);
			return null;
		}
	}

	public static void PositionTemplates()
	{
		foreach (HudElement element in elements)
		{
			PositionTemplate(element);
		}
	}

	private static void CreateTemplates()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		List<HudElement> list = new List<HudElement>();
		foreach (HudElement element in elements)
		{
			try
			{
				RectTransform val = LocateRectTransform(element.Group, element.Path);
				if (element.Position == Vector2.zero)
				{
					if (element.Group == Groups.Inventory)
					{
						element.Position = val.anchoredPosition;
						element.AnchorMin = val.anchorMin;
						element.AnchorMax = val.anchorMax;
					}
					else
					{
						element.Position = val.anchoredPosition;
					}
				}
				AddTemplateToHud(element, val);
			}
			catch
			{
				list.Add(element);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		Helpers.DebugLine($"Removing {list.Count} unused elements.");
		foreach (HudElement item in list)
		{
			Helpers.DebugLine("Remove " + item.DisplayName + " as not used.");
			elements.Remove(item);
		}
	}

	private static void AddTemplateToHud(HudElement element, RectTransform rt)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		Transform obj = Object.Instantiate<Transform>(hudRoot.Find("BuildHud/SelectedInfo"), baseRoot);
		((Object)((Component)obj).gameObject).name = element.Name + templateSuffix;
		((Component)obj.Find("selected_piece")).gameObject.SetActive(false);
		((Component)obj.Find("requirements")).gameObject.SetActive(false);
		TextMeshProUGUI val = ((Component)obj).gameObject.AddComponent<TextMeshProUGUI>();
		((TMP_Text)val).text = element.DisplayName ?? "";
		((TMP_Text)val).font = Hud.instance.m_pieceDescription.font;
		((TMP_Text)val).fontSize = 20f;
		((TMP_Text)val).alignment = (TextAlignmentOptions)544;
		((Component)obj).gameObject.SetActive(false);
		RectTransform component = ((Component)obj).GetComponent<RectTransform>();
		component.pivot = rt.pivot;
		component.anchorMin = rt.anchorMin;
		component.anchorMax = rt.anchorMax;
		component.offsetMin = rt.offsetMin;
		component.offsetMax = rt.offsetMax;
		component.sizeDelta = rt.sizeDelta;
		component.anchoredPosition = rt.anchoredPosition;
		((Transform)component).position = ((Transform)rt).position;
		((Transform)component).localEulerAngles = ((Transform)rt).localEulerAngles;
		((TMP_Text)val).enableAutoSizing = true;
	}
}
