using AarpgTutorial.Common.States;
using AarpgTutorial.Enemies.Scripts;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy-specific state machine. All behavior is inherited from
/// <see cref="StateMachine{TActor,TState}"/>; this class exists to provide
/// concrete type parameters and a node type Godot can instantiate.
/// </summary>
public partial class EnemyStateMachine : StateMachine<Enemy, EnemyState>;
