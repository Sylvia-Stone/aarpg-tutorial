using Godot;

namespace AarpgTutorial.Common.Utilities;

/// <summary>Extension methods on <see cref="Vector2"/> for common game math conversions.</summary>
public static class Vector2Extensions
{
    /// <summary>Converts a cardinal direction vector to the rotation degrees used by interaction areas.</summary>
    public static float ToRotationDegrees(this Vector2 direction) =>
        direction switch
        {
            var d when d == Vector2.Down  =>   0,
            var d when d == Vector2.Up    => 180,
            var d when d == Vector2.Left  =>  90,
            var d when d == Vector2.Right => -90,
            _                             =>   0
        };
}
