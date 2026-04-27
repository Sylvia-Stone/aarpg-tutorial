using AarpgTutorial.Common.Managers;
using AarpgTutorial.GUI.PauseMenu;
using Godot;

namespace AarpgTutorial.Items.ItemEffect;

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
    
    public override void Use()
    {
        PlayerManager.Instance.Player.UpdateHealth(HealAmount);
        if (Sound is not null) PauseMenu.Instance.PlayAudio(Sound);
    }
    
    #endregion


}