using AarpgTutorial.Common;
using Godot;
using LevelManager = AarpgTutorial.Common.Managers.LevelManager;

namespace AarpgTutorial.PlayerCharacter.Scripts;

/// <summary>
/// Follows the player and constrains the viewport to the current tile map's bounds,
/// preventing the camera from showing areas outside the level.
/// </summary>
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

	/// <summary>
	/// Applies the four edges of <paramref name="bounds"/> to the camera's limit properties,
	/// constraining the viewport to the current tile map area.
	/// </summary>
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
