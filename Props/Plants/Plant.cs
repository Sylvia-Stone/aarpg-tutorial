using AarpgTutorial.Common.HitBox;
using AarpgTutorial.Common.HurtBox;
using Godot;

namespace AarpgTutorial.Props.Plants;

/// <summary>
/// A destroyable prop. Removed from the scene on any hit regardless of damage value.
/// </summary>
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

    /// <summary>
    /// Removes the plant from the scene when hit. Damage value is ignored.
    /// </summary>
    private void TakeDamage(HurtBox hurtBox)
    {
        QueueFree();
    }

    #endregion
}
