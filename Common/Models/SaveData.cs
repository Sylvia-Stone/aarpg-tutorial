namespace AarpgTutorial.Common.Models;

/// <summary>Root save file model containing the active scene path and player state.</summary>
public class SaveData
{
    #region Properties

    public PlayerSaveData? Player { get; set; }
    public string? ScenePath { get; set; }

    #endregion
}
