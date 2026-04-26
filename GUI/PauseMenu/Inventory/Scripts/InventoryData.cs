using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;

[GlobalClass]
public partial class InventoryData : Resource
{
    [Export]
    public Array<SlotData> Slots { get; set; } = new();

    public void AddItem()
    {
        
    }
}