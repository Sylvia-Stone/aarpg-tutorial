using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AarpgTutorial.Common.Models;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.GUI.PauseMenu;
using AarpgTutorial.Levels.Scripts;
using Godot;

namespace AarpgTutorial.Common.Managers;

/// <summary>Singleton that serializes and deserializes game state to and from disk.</summary>
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

	#region Lifecycle Methods

	public override void _EnterTree()
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
		var saveData = JsonSerializer.Deserialize<SaveData>(json).WarnIfNull("Save file could not be deserialized");

		saveData?.Player?.WarnIfNull("Player data could not be loaded");
		if (saveData?.Player is null) return;
		
		_saveData = saveData;

		var saveDataPlayer = _saveData.Player;
		var position = new Vector2((float)saveDataPlayer.X, (float)saveDataPlayer.Y);

		await LevelManager.Instance.LoadNewLevel(_saveData.ScenePath.Require(), "", Vector2.Zero, () =>
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

	/// <summary>Reads the current player's health and position and stores them in <see cref="_saveData"/>.</summary>
	private void UpdatePlayerData()
	{
		var player = PlayerManager.Instance.Player;
		_saveData.Player ??= new();

		_saveData.Player.Health = player.CurrentHealth;
		_saveData.Player.MaxHealth = player.MaxHealth;
		_saveData.Player.X = player.GlobalPosition.X;
		_saveData.Player.Y = player.GlobalPosition.Y;
	}

	/// <summary>Finds the active <see cref="Level"/> in the scene tree and stores its file path in <see cref="_saveData"/>.</summary>
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
