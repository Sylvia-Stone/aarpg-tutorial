using System.Linq;
using Godot;
using Godot.Collections;

namespace AarpgTutorial.Items.Scripts;

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