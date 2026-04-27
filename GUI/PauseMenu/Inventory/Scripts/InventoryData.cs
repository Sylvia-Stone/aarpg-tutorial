using AarpgTutorial.Items.Scripts;
using Godot;
using Godot.Collections;
using System.Linq;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

[GlobalClass]
public partial class InventoryData : Resource
{
    #region Exports

    [Export]
    public Array<SlotData?> Slots { get; set; } = new();

    #endregion

    #region Constructor

    public InventoryData()
    {
        ConnectSlots();
    }

    #endregion

    #region Public Methods

    /// <summary>Adds an item to the inventory, stacking onto an existing slot or filling the first empty one.</summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The quantity to add.</param>
    /// <returns><c>true</c> if added successfully; <c>false</c> if the inventory was full.</returns>
    public bool AddItem(ItemData item, int amount = 1)
    {
        var existing = Slots.FirstOrDefault(s => s?.Item == item);
        if (existing is not null)
        {
            existing.Quantity += amount;
            return true;
        }

        var emptyIndex = Slots.IndexOf(null);
        if (emptyIndex < 0)
        {
            GD.Print("Inventory was full");
            return false;
        }

        Slots[emptyIndex] = new SlotData { Item = item, Quantity = amount };
        Slots[emptyIndex]?.Changed += OnSlotChanged;
        return true;
    }

    /// <summary>Subscribes <see cref="OnSlotChanged"/> to all non-null slots. Call after deserializing a saved inventory.</summary>
    public void ConnectSlots()
    {
        foreach (var slot in Slots)
        {
            if (slot is not null) slot.Changed += OnSlotChanged;
        }
    }

    #endregion

    #region Private Methods

    private void OnSlotChanged()
    {
        foreach (var slot in Slots)
        {
            if (slot is not null && slot.Quantity <= 0)
            {
                slot.Changed -= OnSlotChanged;
                Slots[Slots.IndexOf(slot)] = null;
                EmitChanged();
                break;
            }
        }
    }

    #endregion
}