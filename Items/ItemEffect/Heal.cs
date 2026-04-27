using AarpgTutorial.Common.Managers;
using AarpgTutorial.GUI.PauseMenu;
using Godot;

namespace AarpgTutorial.Items.ItemEffect;

/// <summary>Item effect that restores player health by <see cref="HealAmount"/> and plays an optional sound.</summary>
[GlobalClass]
public partial class Heal : ItemEffect
{
    #region Exports
    
    [Export]
    public int HealAmount { get; set; } = 1;

    [Export]
    public AudioStream? Sound { get; set; }
    
    #endregion
    
    #region Public Methods
    
    /// <summary>Heals the player by <see cref="HealAmount"/> and plays <see cref="Sound"/> through the pause menu's audio player if set.</summary>
    public override void Use()
    {
        PlayerManager.Instance.Player.UpdateHealth(HealAmount);
        if (Sound is not null) PauseMenu.Instance.PlayAudio(Sound);
    }
    
    #endregion


}