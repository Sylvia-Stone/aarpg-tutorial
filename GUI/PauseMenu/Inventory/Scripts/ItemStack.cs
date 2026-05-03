using AarpgTutorial.Items.Scripts;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

/// <summary>Godot resource pairing an item definition with a stack quantity.</summary>
[GlobalClass]
public partial class ItemStack : Resource
{
    /// <summary>The item definition for this stack.</summary>
    [Export]
    public ItemData? Item { get; set; }

    /// <summary>Number of items in this stack. Emits <see cref="Godot.Resource.Changed"/> when it drops to zero or below.</summary>
    [Export]
    public int Quantity
    {
        get => _quantity;
        set => SetQuantity(value);
    }

    private int _quantity;

    /// <summary>Sets the quantity and emits <see cref="Godot.Resource.Changed"/> if it drops to zero or below.</summary>
    /// <param name="quantity">The new quantity value.</param>
    public void SetQuantity(int quantity)
    {
        _quantity = quantity;
        EmitChanged();
    }
}