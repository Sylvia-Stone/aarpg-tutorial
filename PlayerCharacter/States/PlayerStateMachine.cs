using AarpgTutorial.Common.States;
using Godot;

namespace AarpgTutorial.PlayerCharacter.States;

/// <summary>
/// Player-specific state machine. Adds <c>_UnhandledInput</c> routing so states can
/// respond to input immediately rather than waiting for the next process tick.
/// </summary>
public partial class PlayerStateMachine : StateMachine<Scripts.Player, PlayerState>
{
    #region Lifecycle

    /// <summary>
    /// Forwards unhandled input to the current state. If the state returns a transition,
    /// changes state immediately rather than waiting for the next process tick.
    /// </summary>
    public override void _UnhandledInput(InputEvent @event)
    {
        if (CurrentState?.HandleInput(@event) is { } next)
            ChangeState(next);
    }

    #endregion
    
}
