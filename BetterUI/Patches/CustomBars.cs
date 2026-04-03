using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Axis = UnityEngine.RectTransform.Axis;
using Food = Player.Food;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal class CustomBars
{
	public static class BarHelper
	{
		public const float scalingFactor = 0.6f;

		public const int padding = 0;

		public static float StepSize => Hud.instance.m_healthPanel.sizeDelta.x;

		public static void BaseCreate(string objectName, string origBarName, ref RectTransform root, ref GuiBar slowBar, ref GuiBar fastBar, ref TMP_Text barText)
		{
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)root != (Object)null))
			{
				((Component)((Component)Hud.instance).transform.Find("hudroot").Find(origBarName)).gameObject.SetActive(false);
				root = Object.Instantiate<RectTransform>(Hud.instance.m_healthBarRoot, ((Component)Hud.instance).transform.Find("hudroot"));
				((Object)((Component)root).gameObject).name = objectName;
				fastBar = ((Component)((Transform)root).Find("fast")).GetComponent<GuiBar>();
				slowBar = ((Component)((Transform)root).Find("slow")).GetComponent<GuiBar>();
				Transform val = ((Component)fastBar).transform.Find("bar").Find("HealthText");
				((Component)val).gameObject.SetActive(false);
				barText = Object.Instantiate<TMP_Text>(((Component)val).GetComponent<TMP_Text>(), (Transform)(object)root);
				((Component)barText).gameObject.AddComponent<TextScaler>();
				((Component)barText).gameObject.SetActive(true);
				((Transform)root).Find("border").localScale = new Vector3(1f, 0.6f, 1f);
				((Transform)root).Find("bkg").localScale = new Vector3(1f, 0.6f, 1f);
				((Component)fastBar).transform.localScale = new Vector3(1f, 0.6f, 1f);
				((Component)slowBar).transform.localScale = new Vector3(1f, 0.6f, 1f);
			}
		}

		public static void UpdateRotation(int configRotation, ref RectTransform root, ref TMP_Text barText)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			((Transform)root).localEulerAngles = new Vector3(0f, 0f, (float)configRotation);
			barText.transform.localEulerAngles = new Vector3(0f, 0f, (float)(-configRotation));
		}

		public static void HealthStyleUpdate(float max, float current, ref GuiBar slowBar, ref GuiBar fastBar, ref TMP_Text barText)
		{
			fastBar.SetMaxValue(max);
			fastBar.SetValue(current);
			slowBar.SetMaxValue(max);
			slowBar.SetValue(current);
			barText.fontSize = Main.customBarTextSize.Value;
			barText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
		}

		public static void StaminaStyleUpdate(float max, float current, ref GuiBar slowBar, ref GuiBar fastBar, ref TMP_Text barText)
		{
			fastBar.SetValue(current / max);
			slowBar.SetValue(current / max);
			barText.fontSize = Main.customBarTextSize.Value;
			barText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
		}

		public static Main.CustomBarState IncrementRotation(Main.CustomBarState state)
		{
			return state switch
			{
				Main.CustomBarState.on0Degrees => Main.CustomBarState.on90Degrees, 
				Main.CustomBarState.on90Degrees => Main.CustomBarState.on180Degrees, 
				Main.CustomBarState.on180Degrees => Main.CustomBarState.on270Degrees, 
				Main.CustomBarState.on270Degrees => Main.CustomBarState.on0Degrees, 
				_ => state, 
			};
		}

		public static Main.CustomBarState DecrementRotation(Main.CustomBarState state)
		{
			return state switch
			{
				Main.CustomBarState.on0Degrees => Main.CustomBarState.on270Degrees, 
				Main.CustomBarState.on90Degrees => Main.CustomBarState.on0Degrees, 
				Main.CustomBarState.on180Degrees => Main.CustomBarState.on0Degrees, 
				Main.CustomBarState.on270Degrees => Main.CustomBarState.on180Degrees, 
				_ => state, 
			};
		}
	}

	public static class HealthBar
	{
		public const string objectName = "BetterUI_HPBar";

		internal static RectTransform root;

		internal static GuiBar slowBar;

		internal static GuiBar fastBar;

		internal static TMP_Text barText;

		public static void UpdateRotation()
		{
			if (!((Object)(object)root == (Object)null) && Main.customHealthBar.Value != Main.CustomBarState.off)
			{
				BarHelper.UpdateRotation((int)Main.customHealthBar.Value, ref root, ref barText);
			}
		}

		public static void Create()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				BarHelper.BaseCreate("BetterUI_HPBar", "healthpanel", ref root, ref slowBar, ref fastBar, ref barText);
				UpdateRotation();
				RectTransform obj = root;
				((Transform)obj).position = ((Transform)obj).position + new Vector3((0f - BarHelper.StepSize) / 4f, BarHelper.StepSize / 4f);
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("HealthBar.Create() " + ex.Message + " " + ex.StackTrace));
			}
		}

		public static void Update(float max, float current)
		{
			try
			{
				if ((Object)(object)root != (Object)null)
				{
					BarHelper.HealthStyleUpdate(max, current, ref slowBar, ref fastBar, ref barText);
				}
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("HealthBar.Update() " + ex.Message + " " + ex.StackTrace));
			}
		}
	}

	public static class StaminaBar
	{
		public const string objectName = "BetterUI_StaminaBar";

		internal static RectTransform root;

		internal static GuiBar slowBar;

		internal static GuiBar fastBar;

		internal static TMP_Text barText;

		public static void UpdateRotation()
		{
			if (!((Object)(object)root == (Object)null) && Main.customStaminaBar.Value != Main.CustomBarState.off)
			{
				BarHelper.UpdateRotation((int)Main.customStaminaBar.Value, ref root, ref barText);
			}
		}

		public static void Create()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				BarHelper.BaseCreate("BetterUI_StaminaBar", "staminapanel", ref root, ref slowBar, ref fastBar, ref barText);
				UpdateRotation();
				fastBar.m_originalColor = ((Graphic)((Component)Hud.instance.m_staminaBar2Fast.m_bar).GetComponent<Image>()).color;
				slowBar.m_originalColor = ((Graphic)((Component)Hud.instance.m_staminaBar2Slow.m_bar).GetComponent<Image>()).color;
				fastBar.ResetColor();
				slowBar.ResetColor();
				fastBar.m_smoothDrain = Hud.instance.m_staminaBar2Fast.m_smoothDrain;
				fastBar.m_changeDelay = Hud.instance.m_staminaBar2Fast.m_changeDelay;
				fastBar.m_smoothSpeed = Hud.instance.m_staminaBar2Fast.m_smoothSpeed;
				RectTransform obj = root;
				((Transform)obj).position = ((Transform)obj).position - new Vector3(BarHelper.StepSize / 4f, 0f);
				if (Main.customHealthBar.Value != Main.CustomBarState.off)
				{
					RectTransform obj2 = root;
					((Transform)obj2).position = ((Transform)obj2).position - new Vector3(0f, 0f);
				}
				else
				{
					RectTransform obj3 = root;
					((Transform)obj3).position = ((Transform)obj3).position + new Vector3(0f, BarHelper.StepSize / 4f);
				}
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("StaminaBar.Create() " + ex.Message + " " + ex.StackTrace));
			}
		}

		public static void Update(float max, float current)
		{
			try
			{
				if ((Object)(object)root != (Object)null)
				{
					BarHelper.StaminaStyleUpdate(max, current, ref slowBar, ref fastBar, ref barText);
				}
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("StaminaBar.Update() " + ex.Message + " " + ex.StackTrace));
			}
		}
	}

	public static class EitrBar
	{
		public const string objectName = "BetterUI_EitrBar";

		internal static RectTransform root;

		internal static GuiBar slowBar;

		internal static GuiBar fastBar;

		internal static TMP_Text barText;

		public static void UpdateRotation()
		{
			if (!((Object)(object)root == (Object)null) && Main.customEitrBar.Value != Main.CustomBarState.off)
			{
				BarHelper.UpdateRotation((int)Main.customEitrBar.Value, ref root, ref barText);
			}
		}

		public static void Create()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				BarHelper.BaseCreate("BetterUI_EitrBar", "eitrpanel", ref root, ref slowBar, ref fastBar, ref barText);
				UpdateRotation();
				fastBar.m_originalColor = ((Graphic)((Component)Hud.instance.m_eitrBarFast.m_bar).GetComponent<Image>()).color;
				slowBar.m_originalColor = ((Graphic)((Component)Hud.instance.m_eitrBarSlow.m_bar).GetComponent<Image>()).color;
				fastBar.ResetColor();
				slowBar.ResetColor();
				fastBar.m_smoothDrain = Hud.instance.m_eitrBarFast.m_smoothDrain;
				fastBar.m_changeDelay = Hud.instance.m_eitrBarFast.m_changeDelay;
				fastBar.m_smoothSpeed = Hud.instance.m_eitrBarFast.m_smoothSpeed;
				RectTransform obj = root;
				((Transform)obj).position = ((Transform)obj).position - new Vector3(BarHelper.StepSize / 4f, 0f);
				if (Main.customHealthBar.Value != Main.CustomBarState.off)
				{
					RectTransform obj2 = root;
					((Transform)obj2).position = ((Transform)obj2).position - new Vector3(0f, 0f);
					if (Main.customStaminaBar.Value != Main.CustomBarState.off)
					{
						RectTransform obj3 = root;
						((Transform)obj3).position = ((Transform)obj3).position - new Vector3(0f, BarHelper.StepSize / 4f + 0f);
					}
				}
				else
				{
					RectTransform obj4 = root;
					((Transform)obj4).position = ((Transform)obj4).position + new Vector3(0f, BarHelper.StepSize / 4f);
				}
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("EitrBar.Create() " + ex.Message + " " + ex.StackTrace));
			}
		}

		public static void Update(float max, float current)
		{
			try
			{
				if ((Object)(object)root != (Object)null)
				{
					BarHelper.StaminaStyleUpdate(max, current, ref slowBar, ref fastBar, ref barText);
				}
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("EitrBar.Update() " + ex.Message + " " + ex.StackTrace));
			}
		}
	}

	public static class FoodBar
	{
		public const string objectName = "BetterUI_FoodBar";

		private static RectTransform foodPanel;

		private static RectTransform foodBarRoot;

		private static RectTransform foodBaseBar;

		private static Image[] foodBars;

		private static Image[] foodIcons;

		private static TMP_Text[] foodTimes;

		private static Transform[] foodTransforms;

		public static void UpdateRotation()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)foodPanel == (Object)null) && Main.customFoodBar.Value != Main.CustomBarState.off)
			{
				int num = (int)(Main.customFoodBar.Value + 270) % 360;
				((Transform)foodPanel).localEulerAngles = new Vector3(0f, 0f, (float)num);
				Transform[] array = foodTransforms;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].localEulerAngles = new Vector3(0f, 0f, (float)(-num));
				}
			}
		}

		public static void Create()
		{
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((Object)(object)foodPanel != (Object)null)
				{
					return;
				}
				if (Main.customHealthBar.Value == Main.CustomBarState.off)
				{
					Main.log.LogWarning((object)"customFoodBar requires customHealthBar. No custom food bar will be created. Activate customHealthBar and log out and back in to use customFoodBar.");
					return;
				}
				foodPanel = Object.Instantiate<RectTransform>(Hud.instance.m_healthPanel, ((Component)Hud.instance).transform.Find("hudroot"));
				((Object)((Component)foodPanel).gameObject).name = "BetterUI_FoodBar";
				((Component)foodPanel).gameObject.SetActive(true);
				foodBarRoot = ((Component)((Transform)foodPanel).Find("Food")).GetComponent<RectTransform>();
				foodBaseBar = ((Component)((Transform)foodBarRoot).Find("baseBar")).GetComponent<RectTransform>();
				foodBars = (Image[])(object)new Image[Hud.instance.m_foodBars.Length];
				foodIcons = (Image[])(object)new Image[Hud.instance.m_foodIcons.Length];
				foodTimes = (TMP_Text[])(object)new TMP_Text[Hud.instance.m_foodTime.Length];
				foodTransforms = (Transform[])(object)new Transform[Hud.instance.m_foodTime.Length];
				for (int i = 0; i < Hud.instance.m_foodBars.Length; i++)
				{
					foodBars[i] = ((Component)((Transform)foodBarRoot).Find(((Object)Hud.instance.m_foodBars[i]).name)).GetComponent<Image>();
					foodTransforms[i] = ((Transform)foodPanel).Find($"food{i}");
					foodIcons[i] = ((Component)foodTransforms[i].Find($"foodicon{i}")).GetComponent<Image>();
					foodTimes[i] = ((Component)foodTransforms[i].Find("time")).GetComponent<TMP_Text>();
				}
				UpdateRotation();
				((Component)((Transform)foodPanel).Find("Health")).gameObject.SetActive(false);
				((Component)((Transform)foodPanel).Find("darken")).gameObject.SetActive(false);
				((Component)((Transform)foodPanel).Find("healthicon")).gameObject.SetActive(false);
				((Component)((Transform)foodPanel).Find("foodicon (1)")).gameObject.SetActive(false);
				((Transform)foodPanel).position = ((Transform)Hud.instance.m_gpRoot).position;
				RectTransform obj = foodPanel;
				((Transform)obj).position = ((Transform)obj).position + new Vector3((0f - foodPanel.sizeDelta.x) / 4f, foodPanel.sizeDelta.x / 2f);
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("FoodBar.Create() " + ex.Message + " " + ex.StackTrace));
			}
		}

		public static void Update(Player player)
		{
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!((Object)(object)foodPanel != (Object)null))
				{
					return;
				}
				List<Food> foods = player.GetFoods();
				float num = player.GetBaseFoodHP() / 25f * 32f;
				foodBaseBar.SetSizeWithCurrentAnchors((Axis)0, num);
				for (int i = 0; i < Hud.instance.m_foodBars.Length; i++)
				{
					Image val = foodBars[i];
					Image val2 = foodIcons[i];
					TMP_Text val3 = foodTimes[i];
					if (i < foods.Count)
					{
						((Component)val).gameObject.SetActive(true);
						Food val4 = foods[i];
						((Component)val2).gameObject.SetActive(true);
						val2.sprite = val4.m_item.GetIcon();
						if (val4.CanEatAgain())
						{
							((Graphic)val2).color = new Color(1f, 1f, 1f, 0.7f + Mathf.Sin(Time.time * 5f) * 0.3f);
						}
						else
						{
							((Graphic)val2).color = Color.white;
						}
						((Component)val3).gameObject.SetActive(true);
						val3.fontSize = Main.customFoodBarTextSize.Value;
						if (val4.m_time >= 60f)
						{
							val3.text = Mathf.CeilToInt(val4.m_time / 60f) + "m";
							((Graphic)val3).color = Color.white;
						}
						else
						{
							val3.text = Mathf.FloorToInt(val4.m_time) + "s";
							((Graphic)val3).color = new Color(1f, 1f, 1f, 0.4f + Mathf.Sin(Time.time * 10f) * 0.6f);
						}
					}
					else
					{
						((Component)val).gameObject.SetActive(false);
						((Component)val2).gameObject.SetActive(false);
						((Component)val3).gameObject.SetActive(false);
					}
				}
				float num2 = Mathf.Ceil(((Character)player).GetMaxHealth() / 25f * 32f);
				foodBarRoot.SetSizeWithCurrentAnchors((Axis)0, num2);
			}
			catch (Exception ex)
			{
				Main.log.LogError((object)("FoodBar.Update() " + ex.Message + " " + ex.StackTrace));
			}
		}
	}
}
