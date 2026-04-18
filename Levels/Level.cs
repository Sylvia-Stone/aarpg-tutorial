using AarpgTutorial.Common;
using Godot;

namespace AarpgTutorial.Levels;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		YSortEnabled = true;
		PlayerManager.Instance.SetParent(this);
	}
}