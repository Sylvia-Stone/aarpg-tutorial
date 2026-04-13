using System.Linq;
using AARPGtutorial.Common;
using Godot;
using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>; 

namespace AARPGtutorial.Player.Scripts;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalLevelManager.Instance.TileMapBoundsChanged += UpdateLimits;
		UpdateLimits(GlobalLevelManager.Instance.GetCurrentTileMapBounds());
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