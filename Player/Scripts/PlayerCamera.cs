using System.Linq;
using AarpgTutorial.Common;
using Godot;
using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>; 

namespace AarpgTutorial.Player.Scripts;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LevelManager.Instance.TileMapBoundsChanged += UpdateLimits;
		UpdateLimits(LevelManager.Instance.CurrentTileMapBounds);
	}

	public void UpdateLimits(GodotVector2Array bounds)
	{
		if (bounds?.Count is null or 0) return;
		LimitLeft = (int)bounds[0].X;
		LimitTop = (int)bounds[0].Y;
		LimitRight = (int)bounds[1].X;
		LimitBottom = (int)bounds[1].Y;
	}
}