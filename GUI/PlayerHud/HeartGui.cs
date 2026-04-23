using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PlayerHud;

/// <summary>A single heart in the player HUD, supporting empty, half, and full states.</summary>
public partial class HeartGui : Control
{
	#region Exports

	[Export]
	public Sprite2D Sprite { get; set; } = null!;

	#endregion

	#region Fields

	private int _fillLevel = 2;

	#endregion

	#region Accessors

	/// <summary>Heart fill level. Sets <see cref="Sprite2D.Frame"/> on assignment.</summary>
	/// <value>0 = empty, 1 = half, 2 = full.</value>
	public int FillLevel
	{
		get => _fillLevel;
		set
		{
			_fillLevel = value;
			Sprite.Frame = _fillLevel;
		}
	}

	#endregion

	#region Lifecycle Methods

	public override void _Ready()
	{
		Sprite.Require();
	}

	#endregion
}
