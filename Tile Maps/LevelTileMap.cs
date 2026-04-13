using AARPGtutorial.Common;
using Godot;
using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>;

namespace AARPGtutorial.Tile_Maps;

public partial class LevelTileMap : TileMapLayer
{
	public override void _Ready()
	{
		GlobalLevelManager.Instance.ChangeTileMapBounds(GetTileMapBounds());
	}

	private GodotVector2Array GetTileMapBounds() =>
	[
		GetUsedRect().Position * RenderingQuadrantSize,
		GetUsedRect().End * RenderingQuadrantSize
	];
}