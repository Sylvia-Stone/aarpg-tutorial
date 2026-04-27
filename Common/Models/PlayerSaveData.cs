namespace AarpgTutorial.Common.Models;

/// <summary>Serializable snapshot of the player's health and world position written to the save file.</summary>
public class PlayerSaveData
{
    #region Properties

    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    #endregion
}
