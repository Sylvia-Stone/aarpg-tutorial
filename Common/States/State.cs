using Godot;

namespace AarpgTutorial.Common.States;

public abstract partial class State : Node
{
    #region Lifecycle

    public virtual void Init() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual State? Process(double delta) => null;
    public virtual State? Physics(double delta) => null;
    public virtual State? HandleInput(InputEvent e) => null;

    #endregion
}
