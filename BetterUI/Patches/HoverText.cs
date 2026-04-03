using UnityEngine;
using Object = UnityEngine.Object;

namespace BetterUI.Patches;

internal static class HoverText
{
	private static readonly string useKey = "[<color=#ffff00ff><b>$KEY_Use</b></color>]";

	private static readonly string _containerBase = "[<color=#ffff00ff><b>$KEY_Use</b></color>] $piece_container_open";

	private static readonly string _cookItem = "[<color=#ffff00ff><b>$KEY_Use</b></color>] $piece_cstand_cook";

	private static readonly string _selectItem = "[<color=#ffff00ff><b>1-8</b></color>] $piece_cstand_cook";

	private static readonly string overCookColor = "red";

	private static readonly string smelterRoof = "<color=#ffff00ff>$piece_smelter_reqroof</color>";

	public static bool PatchFermenter(Fermenter fermenter, ref string hoverText)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		Fermenter.Status status = fermenter.GetStatus();
		if ((int)status != 1)
		{
			if ((int)status == 3)
			{
				string contentName = fermenter.GetContentName();
				hoverText = Localization.instance.Localize(fermenter.m_name + ", $piece_fermenter_ready \n" + contentName + "\n[<color=#ffff00ff><b>$KEY_Use</b></color>] $piece_fermenter_tap");
				return false;
			}
			return true;
		}
		string contentName2 = fermenter.GetContentName();
		if (fermenter.m_exposed)
		{
			hoverText = Localization.instance.Localize(fermenter.m_name + " ( " + contentName2 + ", $piece_fermenter_exposed )");
			return false;
		}
		string text = ((Main.timeLeftHoverTextFermenter.Value == Main.TimeLeftStyle.PercentageDone) ? $"{fermenter.GetFermentationTime() / (double)fermenter.m_fermentationDuration:P0}" : Helpers.TimeString((double)fermenter.m_fermentationDuration - fermenter.GetFermentationTime()));
		hoverText = Localization.instance.Localize(contentName2 + "\n$piece_fermenter_fermenting: " + text);
		return false;
	}

	public static void PatchBeeHive(Beehive beeHive, ref string hoverText)
	{
		int honeyLevel = beeHive.GetHoneyLevel();
		string text = string.Empty;
		if (honeyLevel < beeHive.m_maxHoney)
		{
			float num = beeHive.m_nview.GetZDO().GetFloat("product", 0f);
			float num2 = beeHive.m_secPerUnit - num;
			text = ((Main.timeLeftHoverTextBeeHive.Value != Main.TimeLeftStyle.PercentageDone) ? (Helpers.TimeString(num2) + ", ") : $"{num / beeHive.m_secPerUnit:P0}, ");
		}
		if (honeyLevel > 0)
		{
			hoverText = Localization.instance.Localize($"{beeHive.m_name} ( {text}{beeHive.m_honeyItem.m_itemData.m_shared.m_name} x {honeyLevel} ) \n[<color=#ffff00ff><b>$KEY_Use</b></color>] $piece_beehive_extract");
		}
		else
		{
			hoverText = Localization.instance.Localize(beeHive.m_name + " ( " + text + "$piece_container_empty ) \n[<color=#ffff00ff><b>$KEY_Use</b></color>] $piece_beehive_check");
		}
	}

	public static bool PatchPlant(Plant plant, ref string hoverText)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)plant.m_status == 0)
		{
			string text = ((Main.timeLeftHoverTextPlant.Value == Main.TimeLeftStyle.PercentageDone) ? $"{plant.TimeSincePlanted() / (double)plant.GetGrowTime():P0}" : Helpers.TimeString((double)plant.GetGrowTime() - plant.TimeSincePlanted()));
			hoverText = Localization.instance.Localize(plant.m_name + "\n" + text);
			return false;
		}
		return true;
	}

	public static string PatchContainer(Container container)
	{
		string text = Main.chestHasRoomHoverText.Value switch
		{
			Main.ChestHasRoomStyle.Percentage => $"{container.m_inventory.SlotsUsedPercentage():F0}%", 
			Main.ChestHasRoomStyle.ItemsSlashMaxRoom => $"{container.m_inventory.NrOfItems()}/{container.m_inventory.GetWidth() * container.m_inventory.GetHeight()}", 
			Main.ChestHasRoomStyle.AmountOfFreeSlots => $"{container.m_inventory.GetEmptySlots()}", 
			_ => $"{container.m_inventory.SlotsUsedPercentage():F0}%", 
		};
		return Localization.instance.Localize(container.m_name + " ( " + text + " )\n" + _containerBase);
	}

	public static bool PatchCookingStation(CookingStation cookingStation, ref string hoverText)
	{
		if (cookingStation.m_nview.IsOwner())
		{
			string text = "";
			int num = 0;
			string text2 = default(string);
			float num2 = default(float);
			CookingStation.Status val = default(CookingStation.Status);
			for (int i = 0; i < cookingStation.m_slots.Length; i++)
			{
				cookingStation.GetSlot(i, out text2, out num2, out val);
				if (!(text2 != "") || !(text2 != ((Object)cookingStation.m_overCookedItem).name))
				{
					continue;
				}
				CookingStation.ItemConversion itemConversion = cookingStation.GetItemConversion(text2);
				if (text2 != null)
				{
					num++;
					if (num2 > itemConversion.m_cookTime)
					{
						string text3 = ((Main.timeLeftHoverTextCookingStation.Value == Main.TimeLeftStyle.PercentageDone) ? $"{num2 / (itemConversion.m_cookTime * 2f):P0}" : Helpers.TimeString(itemConversion.m_cookTime * 2f - num2));
						text = text + "\n" + cookingStation.m_overCookedItem.GetHoverName() + ": <color=" + overCookColor + ">" + text3 + "</color>";
					}
					else
					{
						string text4 = ((Main.timeLeftHoverTextCookingStation.Value == Main.TimeLeftStyle.PercentageDone) ? $"{num2 / itemConversion.m_cookTime:P0}" : Helpers.TimeString(itemConversion.m_cookTime - num2));
						text = text + "\n" + itemConversion.m_to.GetHoverName() + ": " + text4;
					}
				}
			}
			if (num > 0)
			{
				hoverText = ((num >= cookingStation.m_slots.Length) ? Localization.instance.Localize(cookingStation.m_name + text) : Localization.instance.Localize(cookingStation.m_name + "\n" + _cookItem + "\n" + _selectItem + text));
				return false;
			}
		}
		return true;
	}

	public static void PatchSmelter(Smelter smelter)
	{
		if ((Object)(object)smelter.m_emptyOreSwitch != (Object)null && smelter.m_spawnStack)
		{
			int processedQueueSize = smelter.GetProcessedQueueSize();
			smelter.m_emptyOreSwitch.m_hoverText = $"{smelter.m_name} {processedQueueSize} $piece_smelter_ready \n{useKey} {smelter.m_emptyOreTooltip}";
		}
		int queueSize = smelter.GetQueueSize();
		smelter.m_addOreSwitch.m_hoverText = $"{smelter.m_name} ({queueSize}/{smelter.m_maxOre}) ";
		if (queueSize > 0)
		{
			Main.log.LogInfo((object)$"{smelter.GetBakeTimer()}, {smelter.m_secPerProduct}, {queueSize}");
			Switch addOreSwitch = smelter.m_addOreSwitch;
			addOreSwitch.m_hoverText += Helpers.TimeString(smelter.m_secPerProduct * (float)queueSize - smelter.GetBakeTimer());
		}
		if (smelter.m_requiresRoof && !smelter.m_haveRoof && Mathf.Sin(Time.time * 10f) > 0f)
		{
			Switch addOreSwitch2 = smelter.m_addOreSwitch;
			addOreSwitch2.m_hoverText = addOreSwitch2.m_hoverText + " " + smelterRoof;
		}
		Switch addOreSwitch3 = smelter.m_addOreSwitch;
		addOreSwitch3.m_hoverText = addOreSwitch3.m_hoverText + " \n" + useKey + " " + smelter.m_addOreTooltip;
	}

	private static void CalculateSmelterBakeTime(Smelter smelter)
	{
		double deltaTime = smelter.GetDeltaTime();
		float accumulator = smelter.GetAccumulator();
		accumulator += (float)deltaTime;
		float num = (((Object)(object)smelter.m_windmill != (Object)null) ? smelter.m_windmill.GetPowerOutput() : 1f);
		while (accumulator >= 1f)
		{
			accumulator -= 1f;
			float fuel = smelter.GetFuel();
			string queuedOre = smelter.GetQueuedOre();
			if ((smelter.m_maxFuel != 0 && !(fuel > 0f)) || !(queuedOre != "") || !(smelter.m_secPerProduct > 0f) || (smelter.m_requiresRoof && !smelter.m_haveRoof))
			{
				continue;
			}
			float num2 = 1f * num;
			if (smelter.m_maxFuel > 0)
			{
				float num3 = smelter.m_secPerProduct / (float)smelter.m_fuelPerProduct;
				fuel -= num2 / num3;
				if (fuel < 0f)
				{
					fuel = 0f;
				}
				smelter.SetFuel(fuel);
			}
			float bakeTimer = smelter.GetBakeTimer();
			bakeTimer += num2;
			smelter.SetBakeTimer(bakeTimer);
			if (bakeTimer > smelter.m_secPerProduct)
			{
				smelter.SetBakeTimer(0f);
				smelter.RemoveOneOre();
				smelter.QueueProcessed(queuedOre);
			}
		}
		if (smelter.GetQueuedOre() == "" || ((float)smelter.m_maxFuel > 0f && smelter.GetFuel() == 0f))
		{
			smelter.SpawnProcessed();
		}
		smelter.SetAccumulator(accumulator);
	}
}
