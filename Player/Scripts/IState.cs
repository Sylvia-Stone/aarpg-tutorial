using Godot;

namespace AARPGtutorial.Player.Scripts;

public interface IState
{
    void Enter();
    void Exit();
    State Process(double delta);
    State Physics(double delta);
    State HandleInput(InputEvent e);
}
