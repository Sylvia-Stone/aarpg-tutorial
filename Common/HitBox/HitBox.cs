using Godot;

namespace AarpgTutorial.Common.HitBox;

/// <summary>
/// Receives incoming hits from overlapping <see cref="HurtBox.HurtBox"/> nodes and
/// fires the <see cref="Damaged"/> signal. Attach this to characters, props, or anything
/// that can be hurt, then subscribe to <see cref="Damaged"/> to handle the damage.
/// </summary>
public partial class HitBox : Area2D
{
    #region Signals

    [Signal]
    public delegate void DamagedEventHandler(HurtBox.HurtBox hurtBox);

    #endregion

    #region Public Methods

    /// <summary>
    /// Fires the <see cref="Damaged"/> signal with the <paramref name="hurtBox"/> that landed the hit.
    /// Called by <see cref="HurtBox.HurtBox"/> when it detects an overlap.
    /// </summary>
    public void TakeDamage(HurtBox.HurtBox hurtBox)
    {
        EmitSignalDamaged(hurtBox);
    }

    #endregion
}
