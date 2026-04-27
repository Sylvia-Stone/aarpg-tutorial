using System.Linq;
using Godot;
using Godot.Collections;

namespace AarpgTutorial.Items.Scripts;

/// <summary>Godot resource defining an item's name, description, texture, and list of use effects.</summary>
[GlobalClass]
public partial class ItemData : Resource
{
    #region Exports
    
    [Export]
    public string? Name { get; set; }
    
    [Export(PropertyHint.MultilineText)]
    public string? Description { get; set; }
    
    [Export]
    public Texture2D? Texture { get; set; }

    [ExportCategory("Item Use Effects")]
    [Export]
    public Array<ItemEffect.ItemEffect> Effects { get; set; } = new();
    
    #endregion
    
    #region Public Methods

    /// <summary>Runs all <see cref="Effects"/> in order.</summary>
    /// <returns><c>true</c> if at least one effect was applied; <c>false</c> if the item has no effects.</returns>
    public bool Use()
    {
        if (!Effects.Any()) return false;

        foreach (var effect in Effects)
        {
            effect.Use();
        }
        return true;
    }
    
    #endregion
}