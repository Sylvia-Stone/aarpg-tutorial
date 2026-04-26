using Godot;

namespace AarpgTutorial.Items.Scripts;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export]
    public string? Name { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string? Description { get; set; }
    [Export]
    public Texture2D? Texture { get; set; }
}