using System.Linq;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Enemies.Interfaces;
using AarpgTutorial.Enemies.Scripts;
using AarpgTutorial.PlayerCharacter.Managers;
using Godot;

namespace AarpgTutorial.Enemies.States;

/// <summary>Chases the player while they're visible, and lingers briefly after losing sight before giving up.</summary>
public partial class Chase : EnemyState
{
    #region Exports

    [Export]
    public State AnimationState = State.Chase;

    [Export]
    public double Duration = .5;

    [Export]
    public EnemyState? NextState;

    [Export]
    public double Speed = 40.0;

    [Export]
    public double TurnRate = .25;

    #endregion

    #region Fields

    private IHasAttack? _attackingEnemy;
    private Vector2 _direction;
    private IHasVision? _seeingEnemy;
    private Stun? _stunState;
    private double _timer;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        NextState.Require();
        _stunState = GetParent().GetChildren().OfType<Stun>().FirstOrDefault();
        _stunState?.StunFinished += OnStunFinished;
    }

    public override void Init(Enemy enemy)
    {
        _attackingEnemy = enemy as IHasAttack;
        _seeingEnemy = enemy as IHasVision;

        _seeingEnemy?.VisionArea.PlayerEntered += OnPlayerEntered;
        _seeingEnemy?.VisionArea.PlayerExited += OnPlayerExited;
    }

    /// <summary>Starts the chase animation and resets the lose-sight timer.</summary>
    public override void Enter()
    {
        _timer = Duration;
        Enemy.UpdateAnimation(AnimationState);
        _seeingEnemy?.IsPlayerVisible = true;
        _attackingEnemy?.AttackArea.Monitoring = false;
    }

    /// <summary>Clears player visibility and disables the attack area on exit.</summary>
    public override void Exit()
    {
        _attackingEnemy?.AttackArea.Monitoring = false;
        _seeingEnemy?.IsPlayerVisible = false;
    }

    /// <summary>
    /// Steers toward the player while visible. Counts down a lose-sight timer when the
    /// player leaves view, transitioning to <see cref="NextState"/> when it expires.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        if (_seeingEnemy is null) return null;

        var newDirection = Enemy.GlobalPosition.DirectionTo(PlayerManager.Instance.Player.GlobalPosition);
        _direction = _direction.Lerp(newDirection, (float)TurnRate);
        Enemy.Velocity = _direction * (float)Speed;

        if (Enemy.SetDirection(_direction))
            Enemy.UpdateAnimation(AnimationState);

        if (!_seeingEnemy.IsPlayerVisible)
        {
            _timer -= delta;
            if (_timer < 0) return NextState;
        }
        else
        {
            _timer = Duration;
        }

        return null;
    }

    public override void _ExitTree()
    {
        _seeingEnemy?.VisionArea.PlayerEntered -= OnPlayerEntered;
        _seeingEnemy?.VisionArea.PlayerExited -= OnPlayerExited;
        _stunState?.StunFinished -= OnStunFinished;
    }

    #endregion

    #region Private Methods

    /// <summary>Marks the player visible and transitions into this state unless a higher-priority state is active.</summary>
    private void OnPlayerEntered()
    {
        _seeingEnemy?.IsPlayerVisible = true;
        if (StateMachine.CurrentState is Stun or Destroy) return;
        StateMachine.ChangeState(this);
    }

    /// <summary>Clears player visibility so the lose-sight timer begins counting down.</summary>
    private void OnPlayerExited()
    {
        _seeingEnemy?.IsPlayerVisible = false;
    }

    /// <summary>Transitions into this state once the enemy recovers from stun.</summary>
    private void OnStunFinished() =>
        Callable.From(() => StateMachine.ChangeState(this)).CallDeferred();

    #endregion
}
