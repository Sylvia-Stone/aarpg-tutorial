using AarpgTutorial.Combat;

namespace AarpgTutorial.Enemies.Interfaces;

public interface IHasAttack
{
    HurtBox AttackArea { get; }
}
