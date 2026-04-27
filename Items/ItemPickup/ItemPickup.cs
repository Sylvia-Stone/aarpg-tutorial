using System.Threading.Tasks;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Items.Scripts;
using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.Items.ItemPickup;

public partial class ItemPickup : Node2D
{
    #region Exports

    [Export]
    public Area2D Area { get; set; } = null!;

    [Export]
    public AudioStreamPlayer2D AudioStreamPlayer { get; set; } = null!;

    [Export]
    private ItemData ItemData
    {
        get => _itemData;
        set => SetItemData(value);
    }

    [Export]
    public Sprite2D Sprite { get; set; } = null!;
    
    #endregion
    
    #region Fields
    
    private ItemData _itemData = null!;
    
    #endregion
    
    #region Lifecycle Methods

    public override void _Ready()
    {
        Area.Require();
        AudioStreamPlayer.Require();
        ItemData.Require();
        Sprite.Require();
        
        UpdateTexture();

        if (Engine.IsEditorHint()) return;
        Area.BodyEntered += OnBodyEntered;
    }
    
    #endregion
    
    #region Private Methods

    private async Task ItemPickedUp()
    {
        Area.BodyEntered -= OnBodyEntered;
        AudioStreamPlayer.Play();
        Visible = false;
        await ToSignal(AudioStreamPlayer, AudioStreamPlayer2D.SignalName.Finished);
    }
    private void SetItemData(ItemData itemData)
    {
        _itemData = itemData;
        UpdateTexture();
    }

    private void OnBodyEntered(Node2D node)
    {
        if (node is Player && PlayerManager.Instance.InventoryData.AddItem(_itemData))
        { 
            _ = ItemPickedUp();
        }
    }

    private void UpdateTexture()
    {
        Sprite.Texture = ItemData.Texture;
    }
    
    #endregion
}