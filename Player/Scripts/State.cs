using Godot;

namespace AARPGtutorial.Player.Scripts;

public abstract partial class State : Node, IState
{
    public Player Player { get; set; }
    public PlayerStateMachine StateMachine { get; set; }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual State? Process(double delta) => null;
    public virtual State? Physics(double delta) => null;
    public virtual State? HandleInput(InputEvent e) => null;
}