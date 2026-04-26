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

        PauseMenu.Instance.PauseMenuShown += UpdateInventory;
        PauseMenu.Instance.PauseMenuHidden += ClearInventory;
        ClearInventory();
    }

    // Unlike GDScript, C# event handlers are not automatically disconnected when a node is freed.
    public override void _ExitTree()
    {
        PauseMenu.Instance.PauseMenuShown -= UpdateInventory;
        PauseMenu.Instance.PauseMenuHidden -= ClearInventory;
    }

    #endregion

    #region Public Methods

    public void ClearInventory()
    {
        foreach (var child in GetChildren())
            child.QueueFree();
    }

    public void UpdateInventory()
    {
        foreach (var slot in InventoryData.Slots)
        {
            var newSlot = InventorySlot.Instantiate<InventorySlotUI>();
            AddChild(newSlot);
            newSlot.SlotData = slot;
        }

        GetChild<Control>(0).GrabFocus();
    }

    #endregion
}