using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Items.Scripts;
using AarpgTutorial.PlayerCharacter.Managers;
using AarpgTutorial.Save;
using Godot;
using Animation = AarpgTutorial.Interactables.Enums.Animation;


namespace AarpgTutorial.Interactables.TreasureChest;

/// <summary>Interactable chest that grants items when opened and remembers its open state across scene loads.</summary>
public partial class TreasureChest : Node2D
{
    #region Exports

    [Export]
    public AnimationPlayer AnimationPlayer = null!;

    [Export]
    public Area2D InteractArea = null!;

    [Export]
    public Sprite2D ItemSprite = null!;

    [Export]
    public Label Label = null!;

    [Export]
    public Persistence Persistence = null!;

    [Export]
    public ItemData ItemData
    {
        get => _itemData;
        set => SetItemData(value);
    }

    [Export]
    public int Quantity
    {
        get => _quantity;
        set => SetQuantity(value);
    }

    #endregion

    #region Fields

    private bool _isOpen;
    private ItemData _itemData = null!;
    private int _quantity = 1;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        AnimationPlayer.Require();
        InteractArea.Require();
        ItemData.Require();
        ItemSprite.Require();
        Label.Require();
        Persistence.Require();

        UpdateLabel();
        UpdateTexture();

        InteractArea.AreaEntered += OnInteractAreaEntered;
        InteractArea.AreaExited += OnInteractAreaExited;
        Persistence.MarkRestored += SetChestState;
    }

    #endregion

    #region Private Methods

    /// <summary>Subscribes to the player's interact event when they enter the area.</summary>
    private void OnInteractAreaEntered(Area2D area)
    {
        PlayerManager.Instance.InteractPressed += PlayerInteract;
    }

    /// <summary>Unsubscribes from the player's interact event when they exit the area.</summary>
    private void OnInteractAreaExited(Area2D area)
    {
        PlayerManager.Instance.InteractPressed -= PlayerInteract;
    }

    /// <summary>Opens the chest, marks it persistent, plays the open animation, and adds the item to inventory.</summary>
    private void PlayerInteract()
    {
        if (_isOpen) return;
        _isOpen = true;
        Persistence.Mark();
        AnimationPlayer.Play(nameof(Animation.Open));
        // The error handling in the tutorial is not needed due to the custom .Require() extension method. See _Ready().
        PlayerManager.Instance.InventoryData.AddItem(ItemData, Quantity);
    }

    /// <summary>Restores the chest's visual state from the persistence mark and disconnects the event.</summary>
    private void SetChestState()
    {
        _isOpen = Persistence.IsMarked;
        AnimationPlayer.Play(_isOpen ? nameof(Animation.Opened) : nameof(Animation.Closed));
        Persistence.MarkRestored -= SetChestState;
    }

    /// <summary>Sets the item data and updates the displayed texture.</summary>
    /// <param name="itemData">The item to display.</param>
    private void SetItemData(ItemData itemData)
    {
        _itemData = itemData;
        UpdateTexture();
    }

    /// <summary>Sets the item quantity and updates the label.</summary>
    /// <param name="quantity">The new quantity.</param>
    private void SetQuantity(int quantity)
    {
        _quantity = quantity;
        UpdateLabel();
    }

    /// <summary>Updates the quantity label, hiding it when quantity is 1 or less.</summary>
    private void UpdateLabel()
    {
        Label.Text = _quantity <= 1 ? string.Empty : $"x{_quantity}";
    }

    /// <summary>Updates the item sprite texture from the current item data.</summary>
    private void UpdateTexture()
    {
        ItemSprite.Texture = ItemData.Texture;
    }

    #endregion
}