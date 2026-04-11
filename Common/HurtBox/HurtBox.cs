using Godot;

namespace AARPGtutorial.Common.HurtBox;

public partial class HurtBox : Area2D
{
    [Export] private int Damage { get; set; } = 1;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is HitBox.HitBox hitBox)
        {
            hitBox.TakeDamage(Damage);
        }
    }
}