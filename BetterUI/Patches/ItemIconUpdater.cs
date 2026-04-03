using UnityEngine;
using UnityEngine.UI;

namespace BetterUI.Patches;

internal class ItemIconUpdater : MonoBehaviour
{
	private Vector3 origScale;

	private Image icon;

	public void Setup(Image icon)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		this.icon = icon;
		origScale = ((Component)icon).transform.localScale;
		IconScaleSize_SettingChanged();
		Main.iconScaleSize.SettingChanged += delegate
		{
			IconScaleSize_SettingChanged();
		};
	}

	private void IconScaleSize_SettingChanged()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		((Component)icon).transform.localScale = origScale * Mathf.Max(Main.iconScaleSize.Value, 0.1f);
	}
}
