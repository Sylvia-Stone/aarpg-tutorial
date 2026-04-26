using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu;

public partial class PauseMenu : CanvasLayer
{
	#region Exports

	[Export]
	public Label ItemDescription = null!;

	[Export]
	public Button LoadButton = null!;

	[Export]
	public Button SaveButton = null!;

	#endregion

	#region Signals

	[Signal]
	public delegate void PauseMenuHiddenEventHandler();

	[Signal]
	public delegate void PauseMenuShownEventHandler();

	#endregion

	#region Fields

	private bool _isPaused;

	#endregion

	#region Accessors

	public static PauseMenu Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle Methods

	public override void _EnterTree()
	{
		Instance = this;
	}

	public override void _Ready()
	{
		ItemDescription.Require();
		LoadButton.Require();
		SaveButton.Require();

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
			GetViewport().SetInputAsHandled();
		}
	}

	#endregion

	#region Public Methods

	/// <summary>Hides the pause menu, resumes the game tree and emits <see cref="EmitSignalPauseMenuHidden"/> signal.</summary>
	public void HidePauseMenu()
	{
		GetTree().Paused = false;
		Visible = false;
		_isPaused = false;

		EmitSignalPauseMenuHidden();
	}

	/// <summary>Shows the pause menu, pauses the game tree, emits <see cref="EmitSignalPauseMenuShown"/> signal.</summary>
	public void ShowPauseMenu()
	{
		GetTree().Paused = true;
		Visible = true;
		_isPaused = true;

		EmitSignalPauseMenuShown();
	}

	/// <summary>Sets the item description label text.</summary>
	/// <param name="text">The description to display.</param>
	public void UpdateItemDescription(string text)
	{
		ItemDescription.Text = text;
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