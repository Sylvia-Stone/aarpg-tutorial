using Godot;

namespace AarpgTutorial.Common;

/// <summary>
/// Singleton that manages level-wide state. Currently tracks the active tile map bounds
/// so systems like the camera can respond to level changes without direct scene-tree coupling.
/// </summary>
public partial class LevelManager : Node
{
	#region Signals

	[Signal]
	public delegate void TileMapBoundsChangedEventHandler(Bounds bounds);

	#endregion

	#region Fields

	public Bounds CurrentTileMapBounds;

	public static LevelManager Instance { get; private set; }

	#endregion

	#region Lifecycle

	public override void _Ready() => Instance = this;

	#endregion

	#region Public Methods

	/// <summary>
	/// Updates <see cref="CurrentTileMapBounds"/> and broadcasts the new value
	/// via <see cref="TileMapBoundsChanged"/> so subscribers can react.
	/// </summary>
	public void ChangeTileMapBounds(Bounds bounds)
	{
		CurrentTileMapBounds = bounds;
		EmitSignalTileMapBoundsChanged(bounds);
	}

	#endregion
}
