using AarpgTutorial.Items.Scripts;
using Godot;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export] 
    public ItemData? Item { get; set; }
    
    [Export]
    public int Quantity { get; set; }
}