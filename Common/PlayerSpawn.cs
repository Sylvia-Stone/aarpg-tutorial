using Godot;

namespace AarpgTutorial.Common;

public partial class PlayerSpawn : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;

		if (!PlayerManager.Instance.IsPlayerSpawned)
		{
			PlayerManager.Instance.SetPlayerPosition(GlobalPosition);
			PlayerManager.Instance.IsPlayerSpawned = true;
		}
	}
}