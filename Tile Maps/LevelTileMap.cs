using AarpgTutorial.Common;
using Godot;

namespace AarpgTutorial.Tile_Maps;

public partial class LevelTileMap : TileMapLayer
{
	#region Lifecycle

	public override void _Ready()
	{
		LevelManager.Instance.ChangeTileMapBounds(GetTileMapBounds());
	}

	#endregion

	#region Private Methods

	private Bounds GetTileMapBounds()
	{
		var position = GetUsedRect().Position * RenderingQuadrantSize;
		var end = GetUsedRect().End * RenderingQuadrantSize;
		return new Bounds
		{
			Left   = (int)position.X,
			Top    = (int)position.Y,
			Right  = (int)end.X,
			Bottom = (int)end.Y
		};
	}

	#endregion
}
