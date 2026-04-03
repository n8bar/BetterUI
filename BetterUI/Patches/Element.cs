namespace BetterUI.Patches;

public readonly struct Element
{
	private readonly string name;

	private readonly string displayName;

	private readonly Groups group;

	private readonly string locationPath;

	public string Name => name;

	public string DisplayName => displayName;

	public Groups Group => group;

	public string LocationPath => locationPath;

	public Element(string name, Groups group, string locationPath = "", string displayName = "")
	{
		this.name = name;
		this.group = group;
		this.locationPath = ((locationPath == "") ? name : locationPath);
		this.displayName = ((displayName == "") ? name : displayName);
	}
}
