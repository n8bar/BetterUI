using System;
using UnityEngine;

namespace BetterUI.Patches;

[Serializable]
public class HudElement
{
	private readonly string name;

	private readonly string displayName;

	private readonly string path;

	private readonly Groups group;

	private float x;

	private float y;

	private float scale;

	private float dimensions;

	private float xDimensions;

	private float anchorMinX;

	private float anchorMinY;

	private float anchorMaxX;

	private float anchorMaxY;

	public string Name => name;

	public string DisplayName => displayName;

	public string Path => path;

	public Groups Group => group;

	public Vector2 Position
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(x, y);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			x = value.x;
			y = value.y;
		}
	}

	public Vector2 AnchorMin
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(anchorMinX, anchorMinY);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			anchorMinX = value.x;
			anchorMinY = value.y;
		}
	}

	public Vector2 AnchorMax
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(anchorMaxX, anchorMaxY);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			anchorMaxX = value.x;
			anchorMaxY = value.y;
		}
	}

	public float Scale => scale;

	public float XDimensions => xDimensions;

	public float YDimensions => dimensions;

	public HudElement(string name, string displayName, Groups group, string path, Vector2 position, float scale = 1f, float xDimensions = 1f, float yDimensions = 1f)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		this.name = name;
		this.displayName = displayName;
		this.path = path;
		this.group = group;
		x = position.x;
		y = position.y;
		this.scale = scale;
		this.xDimensions = xDimensions;
		dimensions = yDimensions;
	}

	public void ChangeScale(float change)
	{
		scale = (float)Math.Round(Mathf.Abs(scale + change), 1);
	}

	public void ChangeXDims(float change)
	{
		xDimensions = Mathf.Max(0.1f, (float)Math.Round(Mathf.Abs(xDimensions + change), 2));
	}

	public void ChangeYDims(float change)
	{
		dimensions = Mathf.Max(0.1f, (float)Math.Round(Mathf.Abs(dimensions + change), 2));
	}

	public void OnAfterDeserialize()
	{
		if (xDimensions < 0.1f)
		{
			xDimensions = 1f;
		}
		if (dimensions < 0.1f)
		{
			dimensions = 1f;
		}
	}
}
