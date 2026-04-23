using AarpgTutorial.Common;
using Godot;
using LevelManager = AarpgTutorial.Common.Managers.LevelManager;
using PlayerManager = AarpgTutorial.Common.Managers.PlayerManager;

namespace AarpgTutorial.Levels.Scripts;

/// <summary>Represents an individual level scene. Manages player parenting and tears itself down on level load.</summary>
public partial class Level : Node2D
{
	#region Lifecycle Methods

	public override void _Ready()
	{
		YSortEnabled = true;
		PlayerManager.Instance.SetParent(this);
		LevelManager.Instance.LevelLoadStarted += FreeLevel;
	}

	#endregion

	#region Private Methods

	/// <summary>Unsubscribes from signals, orphans the player, and queues this node for deletion.</summary>
	private void FreeLevel()
	{
		LevelManager.Instance.LevelLoadStarted -= FreeLevel;
		PlayerManager.Instance.OrphanPlayer(this);
		QueueFree();
	}

	#endregion
}