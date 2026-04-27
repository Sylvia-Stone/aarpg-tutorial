using AarpgTutorial.Items.Scripts;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export] 
    public ItemData? Item { get; set; }

    [Export]
    public int Quantity
    {
        get => _quantity; 
        set => SetQuantity(value);
    }

    private int _quantity;

    public void SetQuantity(int quantity)
    {
        _quantity = quantity;
        if (_quantity <= 0) EmitChanged();
    }
}