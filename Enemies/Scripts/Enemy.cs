using AarpgTutorial.Common;
using AarpgTutorial.Enemies.States;
using Godot;

namespace AarpgTutorial.Enemies.Scripts;

public partial class Enemy : Actor
{
    [Signal]
    public delegate void EnemyDamagedEventHandler();

    [Export]
    private int _hp = 3;
    [Export]
    private EnemyStateMachine _stateMachine;

    private bool _isInvulnerable;
    private Player.Scripts.PlayerCharacter _playerCharacter;

    public override void _Ready()
    {
        _stateMachine.Initialize(this);
        _playerCharacter = PlayerManager.Instance.PlayerCharacter;
    }
}
