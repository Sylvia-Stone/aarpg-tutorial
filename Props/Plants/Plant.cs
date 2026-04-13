using AarpgTutorial.Common.HitBox;
using Godot;

namespace AarpgTutorial.Props.Plants;

public partial class Plant : Node2D
{
    #region Exports

    [Export]
    private HitBox _hitBox;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        _hitBox.Damaged += TakeDamage;
    }

    #endregion

    #region Private Methods

    private void TakeDamage(int _)
    {
        QueueFree();
    }

    #endregion
}
