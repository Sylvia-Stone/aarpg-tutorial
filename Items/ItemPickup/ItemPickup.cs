using System.Threading.Tasks;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Items.Scripts;
using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.Items.ItemPickup;

/// <summary>World item node that the player walks over to pick up, adding it to the inventory.</summary>
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

    /// <summary>Hides the pickup, plays the audio, and waits for it to finish before allowing further interactions.</summary>
    private async Task ItemPickedUp()
    {
        Area.BodyEntered -= OnBodyEntered;
        AudioStreamPlayer.Play();
        Visible = false;
        await ToSignal(AudioStreamPlayer, AudioStreamPlayer2D.SignalName.Finished);
    }
    /// <summary>Sets the backing item data and refreshes the sprite texture.</summary>
    /// <param name="itemData">The item data to assign.</param>
    private void SetItemData(ItemData itemData)
    {
        _itemData = itemData;
        UpdateTexture();
    }

    /// <summary>Triggers pickup when the player enters the area and the inventory has room.</summary>
    /// <param name="node">The body that entered the area.</param>
    private void OnBodyEntered(Node2D node)
    {
        if (node is Player && PlayerManager.Instance.InventoryData.AddItem(_itemData))
        { 
            _ = ItemPickedUp();
        }
    }

    /// <summary>Syncs the sprite texture to the current item's texture.</summary>
    private void UpdateTexture()
    {
        Sprite.Texture = ItemData.Texture;
    }
    
    #endregion
}