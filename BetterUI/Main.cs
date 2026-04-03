using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BetterUI.GameClasses;
using BetterUI.Patches;
using HarmonyLib;
using UnityEngine;

namespace BetterUI;

[BepInPlugin("MK_BetterUI", "BetterUI", "2.5.10")]
public class Main : BaseUnityPlugin
{
	public enum CustomBarState
	{
		off = -1,
		on0Degrees = 0,
		on90Degrees = 90,
		on180Degrees = 180,
		on270Degrees = 270
	}

	public enum DurabilityBarStyle
	{
		Disabled = -1,
		GreenYellowOrangeRed,
		WhiteLightYellowLightCyanBlue
	}

	public enum TimeLeftStyle
	{
		Disabled,
		PercentageDone,
		MinutesSecondsLeft
	}

	public enum ChestHasRoomStyle
	{
		Disabled,
		Percentage,
		ItemsSlashMaxRoom,
		AmountOfFreeSlots
	}

	public enum EnemyLevelStyle
	{
		DefaultStars,
		PrefixLevelNumber,
		Both
	}

	public const string MODNAME = "BetterUI";

	public const string AUTHOR = "MK";

	public const string GUID = "MK_BetterUI";

	public const string VERSION = "2.5.9";

	internal static ManualLogSource log;

	internal readonly Harmony harmony;

	internal readonly Assembly assembly;

	public static ConfigEntry<bool> enablePlayerHudEditing;

	public static ConfigEntry<KeyCode> togglePlayerHudEditModeKey;

	public static ConfigEntry<KeyCode> modKeyPrimary;

	public static ConfigEntry<KeyCode> modKeySecondary;

	public static ConfigEntry<CustomBarState> customHealthBar;

	public static ConfigEntry<CustomBarState> customStaminaBar;

	public static ConfigEntry<CustomBarState> customEitrBar;

	public static ConfigEntry<CustomBarState> customFoodBar;

	public static ConfigEntry<int> customBarTextSize;

	public static ConfigEntry<int> customFoodBarTextSize;

	public static ConfigEntry<DurabilityBarStyle> durabilityBarColorPalette;

	public static ConfigEntry<bool> showItemStars;

	public static ConfigEntry<bool> showCustomCharInfo;

	public static ConfigEntry<bool> showCustomTooltips;

	public static ConfigEntry<bool> showCombinedItemStats;

	public static ConfigEntry<float> iconScaleSize;

	public static ConfigEntry<bool> customSkillUI;

	public static ConfigEntry<int> skillUITextSize;

	public static ConfigEntry<TimeLeftStyle> timeLeftHoverTextFermenter;

	public static ConfigEntry<TimeLeftStyle> timeLeftHoverTextPlant;

	public static ConfigEntry<TimeLeftStyle> timeLeftHoverTextCookingStation;

	public static ConfigEntry<TimeLeftStyle> timeLeftHoverTextBeeHive;

	public static ConfigEntry<ChestHasRoomStyle> chestHasRoomHoverText;

	public static ConfigEntry<bool> showCharacterXP;

	public static ConfigEntry<bool> showCharacterXpBar;

	public static ConfigEntry<bool> showXPNotifications;

	public static ConfigEntry<bool> extendedXPNotification;

	public static ConfigEntry<bool> skipRunningSkillNotifications;

	public static ConfigEntry<int> notificationTextSizeXP;

	public static ConfigEntry<bool> customEnemyHud;

	public static ConfigEntry<bool> showEnemyHPText;

	public static ConfigEntry<EnemyLevelStyle> enemyLevelStyle;

	public static ConfigEntry<int> enemyNameTextSize;

	public static ConfigEntry<int> enemyHPTextSize;

	public static ConfigEntry<int> playerHPTextSize;

	public static ConfigEntry<bool> showPlayerHPText;

	public static ConfigEntry<bool> showLocalPlayerEnemyHud;

	public static ConfigEntry<int> bossHPTextSize;

	public static ConfigEntry<bool> makeTamedHPGreen;

	public static ConfigEntry<float> maxShowDistance;

	public static ConfigEntry<bool> useCustomAlertedStatus;

	public static ConfigEntry<float> mapPinScaleSize;

	public static ConfigEntry<string> uiData;

	public static ConfigEntry<bool> isDebug;

	public Main()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		log = Logger;
		harmony = new Harmony("MK_BetterUI");
		assembly = Assembly.GetExecutingAssembly();
	}

	public void Awake()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Expected O, but got Unknown
		string text = "1 - Player HUD";
		togglePlayerHudEditModeKey = ((BaseUnityPlugin)this).Config.Bind<KeyCode>(text, "togglePlayerHudEditModeKey", (KeyCode)288, "Key used to toggle Player HUD editing mode. Accepted values: https://docs.unity3d.com/ScriptReference/KeyCode.html");
		modKeyPrimary = ((BaseUnityPlugin)this).Config.Bind<KeyCode>(text, "modKeyPrimary", (KeyCode)323, "Key needed to be held down to change an elements position by moving the mouse, as well as its rotation with the mouse wheel if supported. Accepted values: https://docs.unity3d.com/ScriptReference/KeyCode.html");
		modKeySecondary = ((BaseUnityPlugin)this).Config.Bind<KeyCode>(text, "modKeySecondary", (KeyCode)306, "Key needed to be held down to change an elements scale with the mouse wheel, as well as its X and Y dimensions by moving the mouse. Accepted Values: https://docs.unity3d.com/ScriptReference/KeyCode.html");
		customBarTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "customBarTextSize", 15, "Font size of the text on the custom bars.");
		customFoodBarTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "customFoodBarTextSize", 15, "Font size of the duration text of food items in the custom food bar.");
		text = "1 - Player HUD (Requires Logout)";
		bool oldOrDefaultConfigValue = GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD", "enablePlayerHudEditing"), defaultValue: true);
		enablePlayerHudEditing = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "enablePlayerHudEditing", oldOrDefaultConfigValue, "Enable the ability to edit the player HUD by pressing a hotkey.");
		bool oldOrDefaultConfigValue2 = GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD", "useCustomHealthBar"), defaultValue: false);
		oldOrDefaultConfigValue2 |= GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD (Requires Logout)", "useCustomHealthBar"), defaultValue: false);
		customHealthBar = ((BaseUnityPlugin)this).Config.Bind<CustomBarState>(text, "customHealthBar", (!oldOrDefaultConfigValue2) ? CustomBarState.off : CustomBarState.on0Degrees, "Resizable, rotatable HP bar. This bar will always be the same size and will not get longer when you eat. Will also disable the default food bar, so customFoodBar will be enabled automatically.");
		customHealthBar.SettingChanged += delegate
		{
			CustomHealthBar_SettingChanged();
		};
		RemoveOldConfigValue<int>(new ConfigDefinition("1 - Player HUD", "healthBarRotation"));
		RemoveOldConfigValue<int>(new ConfigDefinition(text, "customHealthBarRotation"));
		bool oldOrDefaultConfigValue3 = GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD", "useCustomStaminaBar"), defaultValue: false);
		oldOrDefaultConfigValue3 |= GetOldOrDefaultConfigValue(new ConfigDefinition(text, "useCustomStaminaBar"), defaultValue: false);
		customStaminaBar = ((BaseUnityPlugin)this).Config.Bind<CustomBarState>(text, "customStaminaBar", (!oldOrDefaultConfigValue3) ? CustomBarState.off : CustomBarState.on0Degrees, "Resizable, rotatable stamina bar. This bar will always be visible and will not get longer when you eat.");
		customStaminaBar.SettingChanged += delegate
		{
			CustomStaminaBar_SettingChanged();
		};
		RemoveOldConfigValue<int>(new ConfigDefinition("1 - Player HUD", "staminaBarRotation"));
		RemoveOldConfigValue<int>(new ConfigDefinition(text, "customStaminaBarRotation"));
		bool oldOrDefaultConfigValue4 = GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD", "useCustomFoodBar"), defaultValue: false);
		oldOrDefaultConfigValue4 |= GetOldOrDefaultConfigValue(new ConfigDefinition(text, "useCustomFoodBar"), defaultValue: false);
		customFoodBar = ((BaseUnityPlugin)this).Config.Bind<CustomBarState>(text, "customFoodBar", (!oldOrDefaultConfigValue4) ? CustomBarState.off : CustomBarState.on0Degrees, "Resizable, rotatable food bar. Requires customHealthBar.");
		if (customHealthBar.Value != CustomBarState.off && customFoodBar.Value == CustomBarState.off)
		{
			customFoodBar.Value = CustomBarState.on0Degrees;
		}
		customFoodBar.SettingChanged += delegate
		{
			CustomFoodBar_SettingChanged();
		};
		RemoveOldConfigValue<int>(new ConfigDefinition("1 - Player HUD", "foodBarRotation"));
		RemoveOldConfigValue<int>(new ConfigDefinition(text, "customFoodBarRotation"));
		bool oldOrDefaultConfigValue5 = GetOldOrDefaultConfigValue(new ConfigDefinition("1 - Player HUD", "useCustomEitrBar"), defaultValue: false);
		customEitrBar = ((BaseUnityPlugin)this).Config.Bind<CustomBarState>(text, "customEitrBar", (!oldOrDefaultConfigValue5) ? CustomBarState.off : CustomBarState.on0Degrees, "Resizable, rotatable eitr bar. If you don't know what this is yet, just keep it disabled. This bar will always be visible and will not get longer when you eat.");
		customEitrBar.SettingChanged += delegate
		{
			CustomEitrBar_SettingChanged();
		};
		RemoveOldConfigValue<int>(new ConfigDefinition(text, "customSpoilerBarRotation"));
		text = "2 - Character Inventory";
		bool oldOrDefaultConfigValue6 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "showDurabilityColor"), defaultValue: true);
		int oldOrDefaultConfigValue7 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "durabilityColorPalette"), 0);
		durabilityBarColorPalette = ((BaseUnityPlugin)this).Config.Bind<DurabilityBarStyle>(text, "durabilityBarColorPalette", IntToDurabilityBarStyle(oldOrDefaultConfigValue6, oldOrDefaultConfigValue7), "Change durability bar colors. Options: 0 = Green, Yellow, Orange, Red, 1 = White, Light Yellow, Light Cyan, Blue.");
		showItemStars = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showItemStars", true, "Show item quality as stars.");
		showCustomCharInfo = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showCustomCharInfo", true, "Show Deaths, Builds, and Crafts stats on character selection screen. Also shows the Kills stat if something increases it (the base game doesn't).");
		showCustomTooltips = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showCustomTooltips", true, "Show more info on inventory item tooltips. Automatically disabled this if using Epic Loot for compatibility.");
		showCombinedItemStats = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showCombinedItemStats", true, "Show all item stats when mouse is hovered over armor amount.");
		iconScaleSize = ((BaseUnityPlugin)this).Config.Bind<float>(text, "iconScaleSize", 1f, "Scale item icon by this factor. Ex. 0.75 makes them 75% of their original size.");
		text = "3 - Character Skills";
		customSkillUI = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "customSkillUI", false, "Toggle the use of the custom skills UI.");
		skillUITextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "skillUITextSize", 14, "Select text size of the skills UI.");
		text = "4 - Hover Text";
		int oldOrDefaultConfigValue8 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "timeLeftStyleFermenter"), 2);
		timeLeftHoverTextFermenter = ((BaseUnityPlugin)this).Config.Bind<TimeLeftStyle>(text, "timeLeftHoverTextFermenter", IntToTimeLeftStyle(oldOrDefaultConfigValue8), "Select duration display. Disabled = Default, PercentageDone = % Done, MinutesSecondsLeft = min:sec left.");
		oldOrDefaultConfigValue8 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "timeLeftStylePlant"), 2);
		timeLeftHoverTextPlant = ((BaseUnityPlugin)this).Config.Bind<TimeLeftStyle>(text, "timeLeftHoverTextPlant", IntToTimeLeftStyle(oldOrDefaultConfigValue8), "Select duration display. Disabled = Default, PercentageDone = % Done, MinutesSecondsLeft = min:sec left.");
		oldOrDefaultConfigValue8 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "timeLeftStyleCookingStation"), 2);
		timeLeftHoverTextCookingStation = ((BaseUnityPlugin)this).Config.Bind<TimeLeftStyle>(text, "timeLeftHoverTextCookingStation", IntToTimeLeftStyle(oldOrDefaultConfigValue8), "Select duration display. Disabled = Default, PercentageDone = % Done, MinutesSecondsLeft = min:sec left.");
		timeLeftHoverTextBeeHive = ((BaseUnityPlugin)this).Config.Bind<TimeLeftStyle>(text, "timeLeftHoverTextBeeHive", TimeLeftStyle.MinutesSecondsLeft, "Select duration display. Disabled = Default, PercentageDone = % Done, MinutesSecondsLeft = min:sec left.");
		int oldOrDefaultConfigValue9 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "chestHasRoomStyle"), 2);
		chestHasRoomHoverText = ((BaseUnityPlugin)this).Config.Bind<ChestHasRoomStyle>(text, "chestHasRoomHoverText", IntToChestHasRoomStyle(oldOrDefaultConfigValue9), "Select how chest emptiness is displayed. Disabled = Default | Percentage = % | ItemsSlashMaxRoom= used / total slots. | AmountOfFreeSlots = count of free slots.");
		text = "5 - Character XP";
		showCharacterXP = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showCharacterXP", true, "Enable character XP. This combines all skill levels to show overall character progress.");
		showXPNotifications = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showXPNotifications", true, "Show whenever you gain xp from actions.");
		notificationTextSizeXP = ((BaseUnityPlugin)this).Config.Bind<int>(text, "notificationTextSizeXP", 14, "XP notification font size.");
		extendedXPNotification = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "extendedXPNotification", false, "Extend notification with: (xp gained) [current/overall xp].");
		skipRunningSkillNotifications = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "skipRunningSkillNotifications", true, "Whether to ignore xp gain notifications for the running skill.");
		text = "5 - Character XP (Requires Logout)";
		bool oldOrDefaultConfigValue10 = GetOldOrDefaultConfigValue(new ConfigDefinition("5 - Character XP", "showCharacterXpBar"), defaultValue: true);
		showCharacterXpBar = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showCharacterXpBar", oldOrDefaultConfigValue10, "Show Character XP bar on the bottom of the screen. Character XP must be enabled.");
		text = "6 - Enemy HUD";
		customEnemyHud = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "customEnemyHud", true, "Enable custom enemy HUD changes. If this is set to false, all options in this section will be disabled.");
		useCustomAlertedStatus = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "useCustomAlertedStatus", true, "Hide the vanilla alerted icons above the enemy health bar and instead change the color of the name based on the alerted status.");
		showEnemyHPText = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showEnemyHPText", true, "Show the text with HP amount on enemy health bars.");
		int oldOrDefaultConfigValue11 = GetOldOrDefaultConfigValue(new ConfigDefinition(text, "enemyLvlStyle"), 0);
		enemyLevelStyle = ((BaseUnityPlugin)this).Config.Bind<EnemyLevelStyle>(text, "enemyLevelStyle", IntToEnemyLevelStyle(oldOrDefaultConfigValue11), "Choose how enemy level is shown.");
		enemyNameTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "enemyNameTextSize", 14, "Font size of the name on the enemy.");
		enemyHPTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "enemyHPTextSize", 10, "Font size of the HP text on the enemy health bar.");
		showPlayerHPText = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showPlayerHPText", true, "Show the health numbers on other player's health bar in multiplayer.");
		showLocalPlayerEnemyHud = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "showLocalPlayerEnemyHud", false, "Show the enemy HUD/ health Bar for your player.");
		showLocalPlayerEnemyHud.SettingChanged += delegate
		{
			BetterEnemyHud.ShowLocalPlayerEnemyHudConfigChanged();
		};
		playerHPTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "playerHPTextSize", 10, "The size of the font to display on other player's health bar in multiplayer.");
		bossHPTextSize = ((BaseUnityPlugin)this).Config.Bind<int>(text, "bossHPTextSize", 14, "The size of the font to display on the boss's health bar.");
		makeTamedHPGreen = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "makeTamedHPGreen", true, "Make the health bar for tamed creatures green instead of red.");
		maxShowDistance = ((BaseUnityPlugin)this).Config.Bind<float>(text, "maxShowDistance", 1f, "How far you will see enemy HP Bar. This is a multiplier, 1 is game default, 2 is twice as far (valid range: 0 to 3).");
		text = "7 - Map";
		mapPinScaleSize = ((BaseUnityPlugin)this).Config.Bind<float>(text, "mapPinScaleSize", 1f, "Scale map pins by this factor. Ex. 1.5 makes them 150% of their original size.");
		text = "8 - Debug";
		isDebug = ((BaseUnityPlugin)this).Config.Bind<bool>(text, "isDebug", false, "Enable debug logging.");
		text = "9 - xDataUI";
		uiData = ((BaseUnityPlugin)this).Config.Bind<string>(text, "uiData", "none", "This is your customized UI info. Edit to none, if having issues or wanting to reset positions.");
		if (isDebug.Value)
		{
			PrintOrphanedEntries();
		}
		Logger.LogInfo((object)"BetterUI (Forever Maintained Version) loaded");
	}

	private DurabilityBarStyle IntToDurabilityBarStyle(bool wasColorPaletteOn, int selectedColorPalette)
	{
		if (!wasColorPaletteOn)
		{
			return DurabilityBarStyle.Disabled;
		}
		if (selectedColorPalette != 0)
		{
			return DurabilityBarStyle.WhiteLightYellowLightCyanBlue;
		}
		return DurabilityBarStyle.GreenYellowOrangeRed;
	}

	private ChestHasRoomStyle IntToChestHasRoomStyle(int value)
	{
		if (value > 0 && value <= 3)
		{
			return (ChestHasRoomStyle)value;
		}
		return ChestHasRoomStyle.Disabled;
	}

	private TimeLeftStyle IntToTimeLeftStyle(int value)
	{
		return value switch
		{
			1 => TimeLeftStyle.PercentageDone, 
			2 => TimeLeftStyle.MinutesSecondsLeft, 
			_ => TimeLeftStyle.Disabled, 
		};
	}

	private EnemyLevelStyle IntToEnemyLevelStyle(int value)
	{
		return value switch
		{
			1 => EnemyLevelStyle.PrefixLevelNumber, 
			2 => EnemyLevelStyle.Both, 
			_ => EnemyLevelStyle.DefaultStars, 
		};
	}

	private void CustomFoodBar_SettingChanged()
	{
		CustomBars.FoodBar.UpdateRotation();
	}

	private void CustomStaminaBar_SettingChanged()
	{
		CustomBars.StaminaBar.UpdateRotation();
	}

	private void CustomHealthBar_SettingChanged()
	{
		CustomBars.HealthBar.UpdateRotation();
	}

	private void CustomEitrBar_SettingChanged()
	{
		CustomBars.EitrBar.UpdateRotation();
	}

	public void Start()
	{
		harmony.PatchAll(assembly);
	}

	public void RemoveOldConfigValue<T>(ConfigDefinition configDefinition)
	{
		GetOldOrDefaultConfigValue(configDefinition, default(T));
	}

	public T GetOldOrDefaultConfigValue<T>(ConfigDefinition configDefinition, T defaultValue)
	{
		T value = ((BaseUnityPlugin)this).Config.Bind<T>(configDefinition, defaultValue, (ConfigDescription)null).Value;
		((BaseUnityPlugin)this).Config.Remove(configDefinition);
		if (((BaseUnityPlugin)this).Config.SaveOnConfigSet)
		{
			((BaseUnityPlugin)this).Config.Save();
		}
		return value;
	}

	public bool TryGetOldConfigValue<T>(ConfigDefinition configDefinition, ref T oldValue, bool removeIfFound = true)
	{
		if (!TomlTypeConverter.CanConvert(typeof(T)))
		{
			throw new ArgumentException(string.Format("Type {0} is not supported by the config system. Supported types: {1}", typeof(T), string.Join(", ", (from x in TomlTypeConverter.GetSupportedTypes()
				select x.Name).ToArray())));
		}
		try
		{
			object obj = AccessTools.FieldRefAccess<ConfigFile, object>("_ioLock").Invoke(((BaseUnityPlugin)this).Config);
			Dictionary<ConfigDefinition, string> dictionary = (Dictionary<ConfigDefinition, string>)AccessTools.PropertyGetter(typeof(ConfigFile), "OrphanedEntries").Invoke(((BaseUnityPlugin)this).Config, new object[0]);
			lock (obj)
			{
				if (dictionary.TryGetValue(configDefinition, out var value))
				{
					oldValue = (T)TomlTypeConverter.ConvertToValue(value, typeof(T));
					if (removeIfFound)
					{
						dictionary.Remove(configDefinition);
					}
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			log.LogWarning((object)("Error getting orphaned entry: " + ex.StackTrace));
		}
		return false;
	}

	public void PrintOrphanedEntries()
	{
		try
		{
			object obj = AccessTools.FieldRefAccess<ConfigFile, object>("_ioLock").Invoke(((BaseUnityPlugin)this).Config);
			Dictionary<ConfigDefinition, string> dictionary = (Dictionary<ConfigDefinition, string>)AccessTools.PropertyGetter(typeof(ConfigFile), "OrphanedEntries").Invoke(((BaseUnityPlugin)this).Config, new object[0]);
			if (dictionary.Count == 0)
			{
				return;
			}
			lock (obj)
			{
				log.LogInfo((object)"printing orphaned config values");
				foreach (KeyValuePair<ConfigDefinition, string> item in dictionary)
				{
					log.LogInfo((object)(item.Key.Section + "," + item.Key.Key + ": " + item.Value));
				}
			}
		}
		catch (Exception ex)
		{
			log.LogWarning((object)("Error logging orphaned entries: " + ex.StackTrace));
		}
	}
}

