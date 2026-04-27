using System.Linq;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

public partial class InventoryUI : Control
{
    #region Exports

    [Export]
    public InventoryData InventoryData = null!;

    [Export]
    public PackedScene InventorySlot { get; set; } = null!;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        InventoryData.Require();
        InventorySlot.Require();

        InitializeSlots();
        PauseMenu.Instance.PauseMenuShown += UpdateInventory;
        InventoryData.Changed += UpdateInventory;
        UpdateInventory();
    }

    public override void _ExitTree()
    {
        PauseMenu.Instance.PauseMenuShown -= UpdateInventory;
        InventoryData.Changed -= UpdateInventory;
    }

    #endregion

    #region Private Methods

    private void InitializeSlots()
    {
        for (var i = 0; i < InventoryData.Slots.Count; i++)
            AddChild(InventorySlot.Instantiate<InventorySlotUI>());
    }

    private void UpdateInventory()
    {
        var slots = GetChildren().Cast<InventorySlotUI>().ToArray();
        for (var i = 0; i < slots.Length; i++)
            slots[i].SlotData = i < InventoryData.Slots.Count ? InventoryData.Slots[i] : null;
    }

    #endregion
}