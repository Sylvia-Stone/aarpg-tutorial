using System.Collections.Generic;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.Common;

public abstract partial class Actor : CharacterBody2D
{
    #region Signals

    [Signal]
    public delegate void DirectionChangedEventHandler(Vector2 direction);

    #endregion

    #region Exports

    [Export]
    private AnimationPlayer _animationPlayer;
    public AnimationPlayer AnimationPlayer => _animationPlayer;

    [Export]
    private Sprite2D _sprite;

    #endregion

    #region Fields

    private Vector2 _cardinalDirection = Vector2.Down;
    public Vector2 Direction = Vector2.Zero;

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
        _sprite.Scale = new Vector2(_cardinalDirection == Vector2.Left ? -1 : 1, 1);
        return true;
    }

    public void UpdateAnimation(StateType stateType)
    {
        GD.Print($"{stateType}{GetAnimDirection()}");
        _animationPlayer.Play($"{stateType}{GetAnimDirection()}");
    }

    public AnimationDirection GetAnimDirection() =>
        DirectionAnimations.GetValueOrDefault(_cardinalDirection, AnimationDirection.Down);

    #endregion
}
