using System.Collections.Generic;
using System.Linq;
using Godot;
namespace AARPGtutorial.Player.Scripts;

public partial class PlayerStateMachine : Node
{
    List<State> states;
    State _previousState;
    State _currentState;
    
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public override void _Process(double delta)
    {
        ChangeState(_currentState.Process(delta));
    }

    public override void _PhysicsProcess(double delta)
    {
        ChangeState(_currentState.Physics(delta));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        ChangeState(_currentState.HandleInput(@event));
    }

    public void Initialize(Player player)
    {
        states = new List<State>();

        foreach (var child in GetChildren())
        {
            if (child is State state)
            {
                state.Player = player;
                state.StateMachine = this;
                states.Add(state);
            }
        }

        if (states.Any())
        {
            ChangeState(states.First());
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }
    
    public T GetState<T>() where T : State => states.OfType<T>().FirstOrDefault();

    public void ChangeState(State newState)
    {
        if (newState == null || newState == _currentState)
        {
            return;
        }
        
        if (_currentState != null) _currentState.Exit();
        
        _previousState = _currentState;
        _currentState = newState;
        _currentState.Enter();
    }
}