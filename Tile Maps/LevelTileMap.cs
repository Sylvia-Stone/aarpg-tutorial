using AarpgTutorial.Common;
using AarpgTutorial.Levels;
using Godot;

namespace AarpgTutorial.Tile_Maps;

/// <summary>
/// Tile map that publishes its world-space bounds to <see cref="LevelManager"/> on ready,
/// used to constrain the camera to the playable area.
/// </summary>
public partial class LevelTileMap : TileMapLayer
{
	#region Lifecycle Methods

	public override void _Ready()
	{
		LevelManager.Instance.ChangeTileMapBounds(GetTileMapBounds());
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Computes world-space pixel bounds from the tile map's used rect,
	/// accounting for the rendering quadrant size.
	/// </summary>
	private Bounds GetTileMapBounds()
	{
		var position = GetUsedRect().Position * RenderingQuadrantSize;
		var end = GetUsedRect().End * RenderingQuadrantSize;
		return new Bounds
		{
			Left   = position.X,
			Top    = position.Y,
			Right  = end.X,
			Bottom = end.Y
		};
	}

	#endregion
}
