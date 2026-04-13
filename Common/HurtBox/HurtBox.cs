using Godot;
using HitBoxArea2D = AarpgTutorial.Common.HitBox.HitBox;

namespace AarpgTutorial.Common.HurtBox;

public partial class HurtBox : Area2D
{
    #region Exports

    [Export]
    private int Damage { get; set; } = 1;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    #endregion

    #region Private Methods

    private void OnAreaEntered(Area2D area)
    {
        if (area is HitBox.HitBox hitBox)
        {
            hitBox.TakeDamage(Damage);
        }
    }

    #endregion
}
