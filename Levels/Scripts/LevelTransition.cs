using System;
using AarpgTutorial.Common;
using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Utilities;
using Godot;
using LevelManager = AarpgTutorial.Common.Managers.LevelManager;
using PlayerManager = AarpgTutorial.Common.Managers.PlayerManager;

namespace AarpgTutorial.Levels.Scripts;

public partial class LevelTransition : Area2D
{
	#region Fields

	private Side _side;
	private int _size = 2;

	#endregion

	#region Exports

	[ExportCategory("Collision Area Settings")]
	[Export]
	public Side Side
	{
		get => _side;
		set { _side = value; UpdateArea(); }
	}

	[Export(PropertyHint.Range, "1,12,1, or_greater")]
	public int Size
	{
		get => _size;
		set { _size = value; UpdateArea(); }
	}

	// I removed this as tool scripts don't seem to play nice with C#
	// It kept unlinking from scenes, and I'd rather just size the areas manually
	// than relink the scripts on all scenes
	//[ExportToolButton("Snap to Grid")]
	//public Callable SnapToGridButton => Callable.From(SnapToGrid);

	[ExportCategory("")]
	[Export]
	public CollisionShape2D CollisionShape { get; set; } = null!;

	[Export(PropertyHint.File, "*.tscn")]
	public string LevelPath { get; set; } = null!;

	[Export]
	public string TargetTransitionArea { get; set; } = "LevelTransition";

	#endregion

	#region Lifecycle Methods

	/// <summary>Called when the node enters the scene tree.</summary>
	public async override void _Ready()
	{
		UpdateArea();
		if (Engine.IsEditorHint()) return;

		LevelPath.Require();
		CollisionShape.Require();

		Monitoring = false;
		PlacePlayer();

		await ToSignal(LevelManager.Instance, LevelManager.SignalName.LevelLoadFinished);
		Monitoring = true;
		BodyEntered += OnPlayerEntered;
	}

	#endregion

	#region Private Methods

	/// <summary>Calculates the player's offset relative to this transition area based on side.</summary>
	private Vector2 GetOffset()
	{
		var offset = Vector2.Zero;
		var playerPosition = PlayerManager.Instance.Player.GlobalPosition;

		if (_side.In(Side.Left, Side.Right))
		{
			offset.Y = playerPosition.Y - GlobalPosition.Y;
			offset.X = _side == Side.Left ? Constants.HalfTileSize * -1 : Constants.HalfTileSize;
		}
		else
		{
			offset.X = playerPosition.X - GlobalPosition.X;
			offset.Y = _side == Side.Top ? Constants.HalfTileSize * -1 : Constants.HalfTileSize;
		}

		return offset;
	}

	/// <summary>Triggers a level load when the player enters this area.</summary>
	private async void OnPlayerEntered(Node2D node)
	{
		await LevelManager.Instance.LoadNewLevel(LevelPath, TargetTransitionArea, GetOffset());
	}

	/// <summary>Places the player at this transition if it matches the target transition name.</summary>
	private void PlacePlayer()
	{
		var lm = LevelManager.Instance;
		if (lm.TargetTransition is null || lm.TargetTransition != Name) return;
		PlayerManager.Instance.SetPlayerPosition(GlobalPosition + lm.PositionOffset);
	}

	/// <summary>Snaps the node's position to the nearest half-tile grid increment.</summary>
	private void SnapToGrid()
	{
		var delta = Constants.HalfTileSize;
		var x = Math.Round(Position.X / delta) * delta;
		var y = Math.Round(Position.Y / delta) * delta;
		Position = new Vector2((float)x, (float)y);
	}

	/// <summary>Recalculates the collision shape size and position based on the current side and size.</summary>
	private void UpdateArea()
	{
		var newRectangle = new Vector2(Constants.TileSize, Constants.TileSize);
		var newPosition = Vector2.Zero;
		var positionAdjust = Constants.HalfTileSize;

		switch (Side)
		{
			case Side.Top:
				newRectangle.X *= Size;
				newPosition.Y -= positionAdjust;
				break;
			case Side.Bottom:
				newRectangle.X *= Size;
				newPosition.Y += positionAdjust;
				break;
			case Side.Left:
				newRectangle.Y *= Size;
				newPosition.X -= positionAdjust;
				break;
			default:
				newRectangle.Y *= Size;
				newPosition.X += positionAdjust;
				break;
		}

		CollisionShape ??= GetNode<CollisionShape2D>("CollisionShape2D");
		((RectangleShape2D)CollisionShape.Shape).Size = newRectangle;
		CollisionShape.Position = newPosition;
	}

	#endregion

}