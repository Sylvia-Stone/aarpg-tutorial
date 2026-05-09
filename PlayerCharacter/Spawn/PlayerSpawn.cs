using Godot;
using PlayerManager = AarpgTutorial.PlayerCharacter.Managers.PlayerManager;

namespace AarpgTutorial.Common;

/// <summary>Sets the player's initial world position on first scene load.</summary>
public partial class PlayerSpawn : Node2D
{
	#region Lifecycle Methods

	public override void _Ready()
	{
		Visible = false;

		if (!PlayerManager.Instance.IsPlayerSpawned)
		{
			PlayerManager.Instance.SetPlayerPosition(GlobalPosition);
			PlayerManager.Instance.IsPlayerSpawned = true;
		}
	}

	#endregion
}