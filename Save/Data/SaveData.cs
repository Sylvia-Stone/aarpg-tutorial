using System.Collections.Generic;

namespace AarpgTutorial.Save.Data;

/// <summary>Root save file model containing the active scene path and player state.</summary>
public class SaveData
{
    #region Properties

    public List<InventorySlotDto?>? InventorySlots { get; set; }
    public List<string>? Persistence { get; set; }
    public PlayerSaveData? Player { get; set; }
    public string? ScenePath { get; set; }

    #endregion
}
