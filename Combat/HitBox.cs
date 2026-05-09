using Godot;

namespace AarpgTutorial.Combat;

/// <summary>
/// Receives incoming hits from overlapping <see cref="HurtBox"/> nodes and
/// fires the <see cref="HitBox.Damaged"/> signal. Attach this to characters, props, or anything
/// that can be hurt, then subscribe to <see cref="HitBox.Damaged"/> to handle the damage.
/// </summary>
public partial class HitBox : Area2D
{
    #region Signals

    [Signal]
    public delegate void DamagedEventHandler(Combat.HurtBox hurtBox);

    #endregion

    #region Public Methods

    /// <summary>
    /// Fires the <see cref="HitBox.Damaged"/> signal with the <paramref name="hurtBox"/> that landed the hit.
    /// Called by <see cref="HurtBox"/> when it detects an overlap.
    /// </summary>
    public void TakeDamage(HurtBox hurtBox)
    {
        EmitSignalDamaged(hurtBox);
    }

    #endregion
}
