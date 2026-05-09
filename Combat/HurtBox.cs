using Godot;
using HitBoxArea2D = AarpgTutorial.Combat.HitBox;

namespace AarpgTutorial.Combat;

/// <summary>
/// Deals damage to overlapping <see cref="AarpgTutorial.Combat.HitBox"/> nodes.
/// Attach to attack hitboxes, projectiles, or environmental hazards.
/// </summary>
public partial class HurtBox : Area2D
{
    #region Exports

    [Export]
    public int Damage { get; set; } = 1;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Passes damage to any <see cref="AarpgTutorial.Combat.HitBox"/> that entered this area,
    /// guarded against self-hits by comparing scene-tree owners.
    /// </summary>
    private void OnAreaEntered(Area2D area)
    {
        //Fix for slime hurting itself on startup
        if (area is HitBoxArea2D hitBox && hitBox.GetOwner<Node>() != GetOwner<Node>())
        {
            hitBox.TakeDamage(this);
        }
    }

    #endregion
}
