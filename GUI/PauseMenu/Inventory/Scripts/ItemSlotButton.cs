using AarpgTutorial.Common.Utilities;
using AarpgTutorial.GUI.PauseMenu;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

/// <summary>A single inventory slot button that displays an item's icon and stack quantity.</summary>
public partial class ItemSlotButton : Button
{
    #region Exports

    [Export]
    public Label Label { get; set; } = null!;

    [Export]
    public TextureRect TextureRect { get; set; } = null!;

    #endregion

    #region Fields

    private ItemStack? _itemStack;

    #endregion

    #region Accessors

    public ItemStack? ItemStack
    {
        get => _itemStack;
        set => SetSlotData(value);
    }

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        Label.Require();
        TextureRect.Require();

        TextureRect.Texture = null;
        Label.Text = string.Empty;
        FocusEntered += OnItemFocused;
        FocusExited += OnItemUnfocused;
        Pressed += OnPressed;
    }

    #endregion

    #region Private Methods

    /// <summary>Shows the focused item's description in the pause menu when this slot gains focus.</summary>
    private void OnItemFocused()
    {
        if (_itemStack?.Item?.Description is null) return;
        PauseMenu.Instance.UpdateItemDescription(_itemStack.Item.Description);
    }

    /// <summary>Clears the item description label when this slot loses focus.</summary>
    private void OnItemUnfocused()
    {
        PauseMenu.Instance.UpdateItemDescription(string.Empty);
    }

    /// <summary>Uses the item in this slot and decrements the quantity by one on success.</summary>
    private void OnPressed()
    {
        if (ItemStack?.Item is null) return;

        var isUsed = ItemStack.Item.Use();

        if (isUsed) ItemStack.Quantity -= 1;
    }

    /// <summary>Updates the slot display. Passing <c>null</c> clears the icon and label.</summary>
    /// <param name="itemStack">The slot data to display, or <c>null</c> to show an empty slot.</param>
    private void SetSlotData(ItemStack? itemStack)
    {
        _itemStack = itemStack;
        TextureRect.Texture = itemStack?.Item?.Texture;
        Label.Text = itemStack?.Quantity.ToString() ?? string.Empty;
    }

    #endregion
}