using System;
using System.Collections.Generic;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PlayerHud;

/// <summary>HUD overlay that displays the player's health as a row of hearts.</summary>
public partial class PlayerHud : CanvasLayer
{
	#region Exports

	[Export]
	public int HealthPerHeart { get; set; } = 2;

	[Export]
	public HFlowContainer HFlowContainer = null!;

	#endregion

	#region Fields

	private List<HeartGui> _hearts = new();

	#endregion

	#region Accessors

	public static PlayerHud Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle Methods

	/// <summary>Collects all <see cref="HeartGui"/> children and hides them until health is set.</summary>
	public override void _Ready()
	{
		HFlowContainer.Require();
		Instance = this;
		foreach (var child in HFlowContainer.GetChildren())
		{
			if (child is HeartGui heart)
			{
				_hearts.Add(heart);
				heart.Visible = false;
			}
		}
	}

	#endregion

	#region Public Methods

	/// <summary>Updates heart visibility and fill levels to reflect current health.</summary>
	/// <param name="currentHealth">Current health value.</param>
	/// <param name="maximumHealth">Maximum health, determines how many hearts are visible.</param>
	public void UpdateHealth(int currentHealth, int maximumHealth)
	{
		var heartCount = Math.Ceiling((float)maximumHealth / HealthPerHeart);

		for (int i = 0; i < _hearts.Count; i++)
		{
			_hearts[i].Visible = i < heartCount;
			_hearts[i].FillLevel = Math.Clamp(currentHealth, 0, HealthPerHeart);
			currentHealth -= HealthPerHeart;
		}
	}

	#endregion
}
