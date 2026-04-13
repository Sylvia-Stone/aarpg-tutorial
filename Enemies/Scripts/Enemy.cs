using AarpgTutorial.Common;
using AarpgTutorial.Common.HitBox;
using AarpgTutorial.Enemies.States;
using Godot;

namespace AarpgTutorial.Enemies.Scripts;

public partial class Enemy : Actor
{
    [Signal]
    public delegate void EnemyDamagedEventHandler();
    [Signal]
    public delegate void EnemyDestroyedEventHandler();

    [Export]
    private int _hp = 3;
    [Export]
    private EnemyStateMachine _stateMachine;
    [Export]
    private HitBox _hitBox;

    public bool IsInvulnerable;
    public Player.Scripts.PlayerCharacter PlayerCharacter;

    public override void _Ready()
    {
        _stateMachine.Initialize(this);
        PlayerCharacter = PlayerManager.Instance.PlayerCharacter;
        _hitBox.Damaged += OnTakeDamage;
    }

    public void OnTakeDamage(int damage)
    {
        GD.Print($"Damage: {damage}. HP: {_hp}");
        if (IsInvulnerable) return;
        _hp -= damage;
        if (_hp > 0)
        {
            EmitSignalEnemyDamaged();
        }
        else
        {
            EmitSignalEnemyDestroyed();
        }
    }
}
