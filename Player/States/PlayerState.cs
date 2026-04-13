using AarpgTutorial.Common.States;
using Godot;

namespace AarpgTutorial.Player.States;

public abstract partial class PlayerState : State
{
    public Scripts.PlayerCharacter PlayerCharacter { get; set; }
    public PlayerStateMachine StateMachine { get; set; }

    public override PlayerState? Process(double delta) => null;
    public override PlayerState? Physics(double delta) => null;
    public override PlayerState? HandleInput(InputEvent e) => null;
}
