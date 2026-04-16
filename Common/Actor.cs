using System.Collections.Generic;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.States;
using Godot;

namespace AarpgTutorial.Common;

/// <summary>
/// Abstract base for all actors (player and enemies).
/// Handles physics movement, sprite direction flipping, and animation keying.
/// </summary>
public abstract partial class Actor : CharacterBody2D
{
    #region Signals

    [Signal]
    public delegate void DirectionChangedEventHandler(Vector2 direction);

    #endregion

    #region Exports

    [Export]
    public AnimationPlayer AnimationPlayer = null!;
    [Export]
    public Sprite2D Sprite = null!;

    #endregion

    #region Fields

    public Vector2 Direction = Vector2.Zero;
    public bool IsInvulnerable;

    private Vector2 _cardinalDirection = Vector2.Down;

    public static readonly Vector2[] CardinalDirections =
        [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];

    private static readonly Dictionary<Vector2, AnimationDirection> DirectionAnimations = new()
    {
        { Vector2.Up,    AnimationDirection.Up   },
        { Vector2.Down,  AnimationDirection.Down },
        { Vector2.Left,  AnimationDirection.Side },
        { Vector2.Right, AnimationDirection.Side },
    };

    #endregion

    #region Lifecycle

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Snaps <paramref name="direction"/> to the nearest cardinal direction, flips the sprite
    /// for left-facing movement, and emits <see cref="DirectionChanged"/> if the cardinal changed.
    /// Returns <c>true</c> if the cardinal direction actually changed, <c>false</c> otherwise.
    /// </summary>
    public bool SetDirection(Vector2 direction)
    {
        Direction = direction;
        if (Direction == Vector2.Zero) return false;

        var newDirection = Direction.Abs().X > Direction.Abs().Y
            ? Direction.X > 0 ? Vector2.Right : Vector2.Left
            : Direction.Y > 0 ? Vector2.Down   : Vector2.Up;

        if (_cardinalDirection == newDirection) return false;

        _cardinalDirection = newDirection;
        EmitSignalDirectionChanged(newDirection);
        Sprite.Scale = new Vector2(_cardinalDirection == Vector2.Left ? -1 : 1, 1);
        return true;
    }

    /// <summary>
    /// Plays the animation for <paramref name="stateType"/> in the current facing direction,
    /// using the combined key format <c>"{StateType}{AnimationDirection}"</c>.
    /// </summary>
    public void UpdateAnimation(StateType stateType)
    {
        GD.Print($"{stateType}{GetAnimDirection()}");
        AnimationPlayer.Play($"{stateType}{GetAnimDirection()}");
    }

    /// <summary>
    /// Returns the <see cref="AnimationDirection"/> that corresponds to the current cardinal direction,
    /// used to build animation keys in <see cref="UpdateAnimation"/>.
    /// </summary>
    public AnimationDirection GetAnimDirection() =>
        DirectionAnimations.GetValueOrDefault(_cardinalDirection, AnimationDirection.Down);

    #endregion
}
