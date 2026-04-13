using AarpgTutorial.Common.States;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.States;

public abstract partial class EnemyState : State
{
    #region Fields

    public Enemy Enemy { get; set; }
    public EnemyStateMachine StateMachine { get; set; }

    #endregion

    #region Lifecycle

    public override EnemyState? Process(double delta) => null;
    public override EnemyState? Physics(double delta) => null;
    public override EnemyState? HandleInput(InputEvent e) => null;

    #endregion
}
