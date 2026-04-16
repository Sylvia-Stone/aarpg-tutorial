using AarpgTutorial.Common;
using AarpgTutorial.Common.HitBox;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.Enemies.States;
using Godot;

namespace AarpgTutorial.Enemies.Scripts;

/// <summary>
/// Base enemy actor. Manages health and routes incoming damage to the state machine
/// via <see cref="EnemyDamaged"/> and <see cref="EnemyDestroyed"/> signals.
/// </summary>
public partial class Enemy : Actor
{
    #region Signals

    [Signal]
    public delegate void EnemyDamagedEventHandler(HurtBox hurtBox);
    [Signal]
    public delegate void EnemyDestroyedEventHandler(HurtBox hurtBox);

    #endregion

    #region Exports

    [Export]
    public int CurrentHealth = 3;
    [Export]
    public EnemyStateMachine StateMachine = null!;
    [Export]
    public HitBox HitBox = null!;

    #endregion

    #region Fields

    public PlayerCharacter.Scripts.Player Player = null!;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        StateMachine.Initialize(this);
        Player = PlayerManager.Instance.Player;
        HitBox.Damaged += OnTakeDamage;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Applies damage from <paramref name="hurtBox"/> if not currently invulnerable.
    /// Emits <see cref="EnemyDamaged"/> while health remains, or <see cref="EnemyDestroyed"/>
    /// when health reaches zero.
    /// </summary>
    public void OnTakeDamage(HurtBox hurtBox)
    {
        if (IsInvulnerable) return;
        CurrentHealth -= hurtBox.Damage;
        if (CurrentHealth > 0)
        {
            EmitSignalEnemyDamaged(hurtBox);
        }
        else
        {
            EmitSignalEnemyDestroyed(hurtBox);
        }
    }

    #endregion
}
