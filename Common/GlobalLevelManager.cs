using Godot;
using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>; 

namespace AARPGtutorial.Common;

public partial class GlobalLevelManager : Node
{
	private GodotVector2Array _currentTileMapBounds;
	
	[Signal] public delegate void TileMapBoundsChangedEventHandler(GodotVector2Array tileMapBounds);
	
	public static GlobalLevelManager Instance { get; private set; }

	public override void _Ready() => Instance = this;

	public void ChangeTileMapBounds(GodotVector2Array tileMapBounds)
	{
		_currentTileMapBounds = tileMapBounds;
		EmitSignal(SignalName.TileMapBoundsChanged, tileMapBounds);
	}

	public GodotVector2Array GetCurrentTileMapBounds()
	{
		return _currentTileMapBounds;
	}
}