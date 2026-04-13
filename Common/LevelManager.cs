using Godot;
using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>; 

namespace AarpgTutorial.Common;

public partial class LevelManager : Node
{
	public GodotVector2Array CurrentTileMapBounds;
	
	[Signal]
	public delegate void TileMapBoundsChangedEventHandler(GodotVector2Array tileMapBounds);
	
	public static LevelManager Instance { get; private set; }

	public override void _Ready() => Instance = this;

	public void ChangeTileMapBounds(GodotVector2Array tileMapBounds)
	{
		CurrentTileMapBounds = tileMapBounds;
		EmitSignalTileMapBoundsChanged(tileMapBounds);
	}
}