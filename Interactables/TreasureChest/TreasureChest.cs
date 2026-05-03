using System;
using AarpgTutorial.Common.Managers;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Interactables.Constants;
using AarpgTutorial.Items.Scripts;
using Godot;

namespace AarpgTutorial.Interactables.TreasureChest;

public partial class TreasureChest : Node2D
{
    #region Exports

    [Export]
    public AnimationPlayer AnimationPlayer = null!;

    [Export]
    public Area2D InteractArea = null!;
    
    [Export]
    public ItemData ItemData
    {
        get => _itemData;
        set => SetItemData(value);
    }

    [Export]
    public Label Label = null!;

    [Export]
    public Sprite2D ItemSprite = null!;

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
        Label.Require();
        ItemSprite.Require();

        UpdateLabel();
        UpdateTexture();
        
        InteractArea.AreaEntered += OnInteractAreaEntered;
        InteractArea.AreaExited += OnInteractAreaExited;
    }

    #endregion
    
    #region Private Methods

    private void OnInteractAreaEntered(Area2D area)
    {
        PlayerManager.Instance.InteractPressed += PlayerInteract;
    }

    private void OnInteractAreaExited(Area2D area)
    {
        PlayerManager.Instance.InteractPressed -= PlayerInteract;
    }

    private void PlayerInteract()
    {
        if (_isOpen) return;
        _isOpen = true;
        AnimationPlayer.Play(AnimationType.Open);
        //The error handling in the tutorial is not needed due to the custom .Require() extension method. See _Ready();
        PlayerManager.Instance.InventoryData.AddItem(ItemData, Quantity);
    }

    private void SetItemData(ItemData itemData)
    {
        _itemData = itemData;
        UpdateTexture();
    }

    private void SetQuantity(int quantity)
    {
        _quantity = quantity;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        Label.Text = _quantity <= 1 ? string.Empty : $"x{_quantity}";
    }
    
    private void UpdateTexture()
    {
        ItemSprite.Texture = ItemData.Texture;
    }

    #endregion
}