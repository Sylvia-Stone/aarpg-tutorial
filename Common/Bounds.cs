using Godot;

namespace AarpgTutorial.Common;

/// <summary>
/// DTO for bounds, mainly used by camera.
/// using RefCounted to pass as a Godot signal.
/// Check Godot Memory Management in the README!
/// </summary>
public partial class Bounds : RefCounted
{
    #region Properties

    public int Bottom { get; init; }
    public int Left { get; init; }
    public int Right { get; init; }
    public int Top { get; init; }

    #endregion
}
