using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu;

public partial class PauseMenu : CanvasLayer
{
	#region Exports

	[Export]
	public Button LoadButton = null!;

	[Export]
	public Button SaveButton = null!;

	#endregion

	#region Fields

	private bool _isPaused;

	#endregion

	#region Accessors

	public static PauseMenu? Instance { get; private set; }

	#endregion

	#region Lifecycle

	public override void _Ready()
	{
		LoadButton.Require();
		SaveButton.Require();

		Instance = this;
		HidePauseMenu();

		LoadButton.Pressed += OnLoadPressed;
		SaveButton.Pressed += OnSavePressed;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(InputActions.Pause))
		{
			if (_isPaused)
				HidePauseMenu();
			else
				ShowPauseMenu();
		}
		GetViewport().SetInputAsHandled();
	}

	#endregion

	#region Public Methods

	/// <summary>Hides the pause menu and resumes the game tree.</summary>
	public void HidePauseMenu()
	{
		GetTree().Paused = false;
		Visible = false;
		_isPaused = false;
	}

	/// <summary>Shows the pause menu, pauses the game tree, and focuses the save button.</summary>
	public void ShowPauseMenu()
	{
		GetTree().Paused = true;
		Visible = true;
		_isPaused = true;
		SaveButton.GrabFocus();
	}

	#endregion

	#region Private Methods

	private async void OnLoadPressed()
	{
		if (!_isPaused) return;
		await SaveManager.Instance.Load();
		HidePauseMenu();
	}

	private void OnSavePressed()
	{
		if (!_isPaused) return;
		SaveManager.Instance.Save();
		HidePauseMenu();
	}

	#endregion
}
