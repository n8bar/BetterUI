using UnityEngine;

namespace BetterUI.Patches;

internal class TextScaler : MonoBehaviour
{
	public void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localScale = ((Component)this).transform.localScale;
		float num = 1f / ((Component)this).transform.parent.lossyScale.x;
		float y = ((Component)this).transform.parent.lossyScale.y;
		localScale.x = num * y * localScale.y;
		((Component)this).transform.localScale = localScale;
	}
}
