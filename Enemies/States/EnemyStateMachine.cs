using System.Linq;
using AarpgTutorial.Common.States;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.States;

public partial class EnemyStateMachine : StateMachine<Enemy, EnemyState>
{
    #region Public Methods

    public override void Initialize(Enemy enemy)
    {
        foreach (var state in States)
        {
            state.Enemy = enemy;
            state.StateMachine = this;
            state.Init();
        }

        if (States.FirstOrDefault() is { } initial)
        {
            ChangeState(initial);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    #endregion
}
