using AarpgTutorial.Common.Constants;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu;

/// <summary>Pause menu singleton. Toggles game pause state and hosts the inventory grid and save/load controls.</summary>
public partial class PauseMenu : CanvasLayer
{
	#region Exports

	[Export]
	public AudioStreamPlayer AudioStreamPlayer { get; set; } = null!;
	
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
		AudioStreamPlayer.Require();

		HidePauseMenu();

		LoadButton.Pressed += OnLoadPressed;
		SaveButton.Pressed += OnSavePressed;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(InputActions.Pause))                                                                                                                                                                                                                                                                   
		{                                                                                                                                                                                                                                                                                                                 
			if (_isPaused) HidePauseMenu();
			else ShowPauseMenu();                                                                                                                                                                                                                                                                                         
			GetViewport().SetInputAsHandled();
		}
		else if (_isPaused && @event.IsActionPressed("ui_cancel"))
		{                                                                                                                                                                                                                                                                                                                 
			HidePauseMenu();
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

	/// <summary>Plays the given audio stream through the menu's <see cref="AudioStreamPlayer"/>.</summary>
	/// <param name="audio">The audio stream to play.</param>
	public void PlayAudio(AudioStream audio)
	{
		AudioStreamPlayer.Stream = audio;
		AudioStreamPlayer.Play();
	}

	#endregion

	#region Private Methods

	/// <summary>Loads the game from disk and closes the pause menu on success.</summary>
	private async void OnLoadPressed()
	{
		if (!_isPaused) return;
		await SaveManager.Instance.Load();
		HidePauseMenu();
	}

	/// <summary>Saves the game to disk and closes the pause menu.</summary>
	private void OnSavePressed()
	{
		if (!_isPaused) return;
		SaveManager.Instance.Save();
		HidePauseMenu();
	}

	#endregion
}