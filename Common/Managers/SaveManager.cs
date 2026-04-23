using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AarpgTutorial.Common.Models;
using AarpgTutorial.GUI.PauseMenu;
using AarpgTutorial.Levels.Scripts;
using Godot;

namespace AarpgTutorial.Common.Managers;

public partial class SaveManager : Node
{
	#region Signals

	[Signal]
	public delegate void GameLoadedEventHandler();

	[Signal]
	public delegate void GameSavedEventHandler();

	#endregion

	#region Fields

	private SaveData _saveData = new();
	private string _savePath = ProjectSettings.GlobalizePath("user://SaveFile.sav");

	#endregion

	#region Accessors

	public static SaveManager Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle

	public override void _Ready()
	{
		Instance = this;
	}

	#endregion

	#region Public Methods

	/// <summary>Loads game state from disk and restores the saved level, player position, and health.</summary>
	public async Task Load()
	{
		if (!File.Exists(_savePath))
		{
			GD.PushWarning("No save file found.");
			return;
		}

		var json = File.ReadAllText(_savePath);
		_saveData = JsonSerializer.Deserialize<SaveData>(json);

		if (_saveData is null)
		{
			GD.PushWarning("Save file could not be deserialized.");
			return;
		}

		if (_saveData.Player is null)
		{
			GD.PushWarning("Save file contains no player data.");
			return;
		}

		var saveDataPlayer = _saveData.Player;
		var position = new Vector2((float)saveDataPlayer.X, (float)saveDataPlayer.Y);

		await LevelManager.Instance.LoadNewLevel(_saveData.ScenePath, "", Vector2.Zero, () =>
		{
			PlayerManager.Instance.SetPlayerPosition(position);
			PlayerManager.Instance.SetHealth(saveDataPlayer.Health, saveDataPlayer.MaxHealth);
			PauseMenu.Instance.HidePauseMenu();
		});

		EmitSignalGameLoaded();
	}

	/// <summary>Writes current game state to disk.</summary>
	public void Save()
	{
		UpdateScenePath();
		UpdatePlayerData();
		var json = JsonSerializer.Serialize(_saveData, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(_savePath, json);
	}

	#endregion

	#region Private Methods

	private void UpdatePlayerData()
	{
		var player = PlayerManager.Instance.Player;
		_saveData.Player ??= new();

		_saveData.Player.Health = player.CurrentHealth;
		_saveData.Player.MaxHealth = player.MaxHealth;
		_saveData.Player.X = player.GlobalPosition.X;
		_saveData.Player.Y = player.GlobalPosition.Y;
	}

	private void UpdateScenePath()
	{
		string path = string.Empty;
		foreach (var child in GetTree().Root.GetChildren())
		{
			if (child is Level level)
				path = level.SceneFilePath;
		}

		_saveData.ScenePath = path;
	}

	#endregion
}
