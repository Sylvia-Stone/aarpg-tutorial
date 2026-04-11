using Godot;
using AARPGtutorial.Common.HitBox;

namespace AARPGtutorial.Props.Plants;

public partial class Plant : Node2D
{
    [Export] private HitBox _hitBox;

    public override void _Ready()
    {
        _hitBox.Damaged += TakeDamage;
    }

    private void TakeDamage(int _)
    {
        QueueFree();
    }
}