using Godot;

namespace AarpgTutorial.Common;

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

	public void ChangeTileMapBounds(Bounds bounds)
	{
		CurrentTileMapBounds = bounds;
		EmitSignalTileMapBoundsChanged(bounds);
	}

	#endregion
}
