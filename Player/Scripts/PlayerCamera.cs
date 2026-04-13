using AarpgTutorial.Common;
using Godot;

namespace AarpgTutorial.Player.Scripts;

public partial class PlayerCamera : Camera2D
{
	#region Lifecycle

	public override void _Ready()
	{
		LevelManager.Instance.TileMapBoundsChanged += UpdateLimits;
		UpdateLimits(LevelManager.Instance.CurrentTileMapBounds);
	}

	#endregion

	#region Public Methods

	private void UpdateLimits(Bounds? bounds)
	{
		if (bounds is null) return;
		LimitLeft   = bounds.Left;
		LimitTop    = bounds.Top;
		LimitRight  = bounds.Right;
		LimitBottom = bounds.Bottom;
	}

	#endregion
}
