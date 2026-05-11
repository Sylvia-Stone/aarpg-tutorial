using AarpgTutorial.Combat;
using Godot;

namespace AarpgTutorial.Enemies.Interfaces;

public interface IHasAttack
{
    HurtBox AttackArea { get; }
}
