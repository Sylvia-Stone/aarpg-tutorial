using Godot;

namespace AarpgTutorial.Common.HitBox;

public partial class HitBox : Area2D
{
    #region Signals

    [Signal] public delegate void DamagedEventHandler(int damage);

    #endregion

    #region Public Methods

    public void TakeDamage(int damage)
    {
        GD.Print($"Damage Taken: {damage}");
        EmitSignalDamaged(damage);
    }

    #endregion
}
