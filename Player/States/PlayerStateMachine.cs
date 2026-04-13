using System.Linq;
using AarpgTutorial.Common.States;
using AarpgTutorial.Player.Scripts;
using Godot;

namespace AarpgTutorial.Player.States;

public partial class PlayerStateMachine : StateMachine<Scripts.PlayerCharacter, PlayerState>
{
    #region Lifecycle

    public override void _UnhandledInput(InputEvent @event)
    {
        if (CurrentState?.HandleInput(@event) is PlayerState next)
            ChangeState(next);
    }

    #endregion

    #region Public Methods

    public override void Initialize(Scripts.PlayerCharacter playerCharacter)
    {
        foreach (var state in States)
        {
            state.PlayerCharacter = playerCharacter;
            state.StateMachine = this;
        }

        if (States.FirstOrDefault() is { } initial)
        {
            ChangeState(initial);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    #endregion
}
