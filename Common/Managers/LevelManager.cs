using System;
using System.Threading.Tasks;
using AarpgTutorial.GUI.SceneTransition;
using Godot;

namespace AarpgTutorial.Common.Managers;

/// <summary>
/// Singleton that manages level-wide state. Currently tracks the active tile map bounds
/// so systems like the camera can respond to level changes without direct scene-tree coupling.
/// </summary>
public partial class LevelManager : Node
{
	#region Signals

	[Signal]
	public delegate void LevelLoadFinishedEventHandler();

	[Signal]
	public delegate void LevelLoadStartedEventHandler();

	[Signal]
	public delegate void TileMapBoundsChangedEventHandler(Bounds bounds);

	#endregion

	#region Fields

	public Vector2 PositionOffset;

	#endregion

	#region Properties

	public Bounds? CurrentTileMapBounds { get; set; }
	public string? TargetTransition { get; set; }

	#endregion

	#region Accessors

	public static LevelManager Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle Methods

	public override void _EnterTree()
	{
		Instance = this;
	}

	/// <summary>Called when the node enters the scene tree. Emits LevelLoadFinished after one frame to signal initial scene readiness.</summary>
	public async override void _Ready()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		EmitSignalLevelLoadFinished();
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Updates <see cref="CurrentTileMapBounds"/> and broadcasts the new value
	/// via <see cref="Common.LevelManager.TileMapBoundsChanged"/> so subscribers can react.
	/// </summary>
	public void ChangeTileMapBounds(Bounds bounds)
	{
		CurrentTileMapBounds = bounds;
		EmitSignalTileMapBoundsChanged(bounds);
	}

	/// <summary>Pauses the tree, transitions to the new level, then resumes.</summary>
	/// <param name="levelPath">Path to the new level scene.</param>
	/// <param name="targetTransition">Name of the transition area to spawn at.</param>
	/// <param name="positionOffset">Offset applied to the spawn position.</param>
	/// <param name="onBlackScreen">Optional action invoked after the scene changes but before fade-in, while the screen is black.</param>
	public async Task LoadNewLevel(string levelPath, string targetTransition, Vector2 positionOffset, Action? onBlackScreen = null)
	{
		GetTree().Paused = true;
		TargetTransition = targetTransition;
		PositionOffset = positionOffset;

		await SceneTransition.Instance.FadeOut();
		EmitSignalLevelLoadStarted();

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		GetTree().ChangeSceneToFile(levelPath);
		onBlackScreen?.Invoke();

		await SceneTransition.Instance.FadeIn();
		GetTree().Paused = false;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		EmitSignalLevelLoadFinished();
	}

	#endregion
}