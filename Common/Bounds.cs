using Godot;

namespace AarpgTutorial.Common;

public partial class Bounds : RefCounted
{
    public int Left { get; init; }
    public int Top { get; init; }
    public int Right { get; init; }
    public int Bottom { get; init; }
}
