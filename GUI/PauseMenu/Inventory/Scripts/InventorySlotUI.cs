using AarpgTutorial.Common.Utilities;
using AarpgTutorial.GUI.PauseMenu;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

public partial class InventorySlotUI : Button
{
    #region Exports

    [Export]
    public Label Label { get; set; } = null!;

    [Export]
    public TextureRect TextureRect { get; set; } = null!;

    #endregion

    #region Fields

    private SlotData? _slotData;

    #endregion

    #region Accessors

    public SlotData? SlotData
    {
        get => _slotData;
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

    private void OnItemFocused()
    {
        if (_slotData?.Item?.Description is null) return;
        PauseMenu.Instance.UpdateItemDescription(_slotData.Item.Description);
    }

    private void OnItemUnfocused()
    {
        PauseMenu.Instance.UpdateItemDescription(string.Empty);
    }

    private void OnPressed()
    {
        if (SlotData?.Item is null) return;

        var isUsed = SlotData.Item.Use();

        if (isUsed) SlotData.Quantity -= 1;
    }

    /// <summary>Updates the slot display. Passing <c>null</c> clears the icon and label.</summary>
    /// <param name="slotData">The slot data to display, or <c>null</c> to show an empty slot.</param>
    private void SetSlotData(SlotData? slotData)
    {
        _slotData = slotData;
        TextureRect.Texture = slotData?.Item?.Texture;
        Label.Text = slotData?.Quantity.ToString() ?? string.Empty;
    }

    #endregion
}