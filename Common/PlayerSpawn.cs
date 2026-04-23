using Godot;

namespace AarpgTutorial.Common;

/// <summary>Sets the player's initial world position on first scene load.</summary>
public partial class PlayerSpawn : Node2D
{
	#region Lifecycle Methods

	/// <summary>Called when the node enters the scene tree.</summary>
	public override void _Ready()
	{
		Visible = false;

		if (!Managers.PlayerManager.Instance.IsPlayerSpawned)
		{
			Managers.PlayerManager.Instance.SetPlayerPosition(GlobalPosition);
			Managers.PlayerManager.Instance.IsPlayerSpawned = true;
		}
	}

	#endregion
}