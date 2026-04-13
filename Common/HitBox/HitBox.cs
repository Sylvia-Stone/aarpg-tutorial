using Godot;

namespace AarpgTutorial.Common.HitBox;

public partial class HitBox : Area2D
{
    [Signal] public delegate void DamagedEventHandler(int damage);

    public void TakeDamage(int damage)
    {
        GD.Print($"Damage Taken: {damage}");
        EmitSignalDamaged(damage);
    }
}