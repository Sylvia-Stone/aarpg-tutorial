using System.Linq;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

/// <summary>GridContainer that spawns and syncs inventory slot buttons to match the current <see cref="InventoryData"/>.</summary>
public partial class InventoryUI : Control
{
    #region Exports

    [Export]
    public InventoryData InventoryData = null!;

    [Export]
    public PackedScene InventorySlotButtonScene { get; set; } = null!;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        InventoryData.Require();
        InventorySlotButtonScene.Require();

        CreateSlotButtons();
        PauseMenu.Instance.PauseMenuShown += OnPauseMenuShown;
        InventoryData.Changed += UpdateInventorySlots;
        UpdateInventorySlots();
    }

    public override void _ExitTree()
    {
        PauseMenu.Instance.PauseMenuShown -= OnPauseMenuShown;
        InventoryData.Changed -= UpdateInventorySlots;
    }

    #endregion

    #region Private Methods

    /// <summary>Spawns one <see cref="ItemSlotButton"/> per slot in <see cref="Scripts.InventoryData.Slots"/>.</summary>
    private void CreateSlotButtons()
    {
        for (var i = 0; i < InventoryData.Slots.Count; i++)
            AddChild(InventorySlotButtonScene.Instantiate<ItemSlotButton>());
    }

    /// <summary>Refreshes slot data and grabs focus on the first slot so gamepad navigation has an entry point.</summary>
    private void OnPauseMenuShown()
    {
        UpdateInventorySlots();
        GetChildren().Cast<ItemSlotButton>().FirstOrDefault()?.GrabFocus();
    }

    /// <summary>Pushes current <see cref="Scripts.InventoryData.Slots"/> into each slot button, clearing buttons with no matching data.</summary>
    private void UpdateInventorySlots()
    {
        var slotButtons = GetChildren().Cast<ItemSlotButton>().ToArray();
        for (var i = 0; i < slotButtons.Length; i++)
            slotButtons[i].ItemStack = i < InventoryData.Slots.Count ? InventoryData.Slots[i] : null;
    }

    #endregion
}