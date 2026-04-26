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
    }

    #endregion

    #region Public Methods

    /// <summary>Populates the slot with item data, updating the icon and quantity label.</summary>
    /// <param name="slotData">The slot data to display. If null, the call is a no-op.</param>
    public void SetSlotData(SlotData? slotData)
    {
        if (slotData is null) return;

        TextureRect.Texture = slotData.Item.Require().Texture;
        Label.Text = slotData.Quantity.ToString();
        _slotData = slotData;
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

    #endregion
}