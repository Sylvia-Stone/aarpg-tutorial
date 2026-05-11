using AarpgTutorial.Common.Utilities;
using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.Scripts;

/// <summary>Rotates with the parent enemy and emits signals when the player enters or exits detection range.</summary>
public partial class VisionArea : Area2D
{
    #region Signals

    [Signal]
    public delegate void PlayerEnteredEventHandler();

    [Signal]
    public delegate void PlayerExitedEventHandler();

    #endregion

    #region Fields

    private Enemy? _enemy;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        var parent = GetParent();
        if (parent is Enemy enemy)
        {
            enemy.DirectionChanged += OnDirectionChanged;
            _enemy = enemy;
        }
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
        if (_enemy != null) _enemy.DirectionChanged -= OnDirectionChanged;
    }

    #endregion

    #region Private Methods

    /// <summary>Emits <see cref="PlayerEntered"/> when the player's body enters the area.</summary>
    private void OnBodyEntered(Node2D node)
    {
        if (node is Player) EmitSignalPlayerEntered();
    }

    /// <summary>Emits <see cref="PlayerExited"/> when the player's body exits the area.</summary>
    private void OnBodyExited(Node2D node)
    {
        if (node is Player) EmitSignalPlayerExited();
    }

    /// <summary>Rotates the vision cone to match the enemy's current facing direction.</summary>
    private void OnDirectionChanged(Vector2 direction)
    {
        RotationDegrees = direction.ToRotationDegrees();
    }

    #endregion
}
