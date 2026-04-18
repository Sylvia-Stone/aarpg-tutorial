using System;
using AarpgTutorial.Common;
using AarpgTutorial.Common.HitBox;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.GUI.PlayerHud;
using AarpgTutorial.PlayerCharacter.States;
using Godot;

namespace AarpgTutorial.PlayerCharacter.Scripts;

/// <summary>
/// The player character. Reads directional input, manages health and invulnerability,
/// and coordinates the state machine for movement, attack, and stun behavior.
/// </summary>
public partial class Player : Actor
{
    #region Signals
    
    [Signal]
    public delegate void PlayerDamagedEventHandler(HurtBox hurtBox);
    
    #endregion
    
    #region Exports

    [Export]
    public int CurrentHealth = 6;
    [Export]
    public int MaxHealth = 6;
    [Export]
    public AnimationPlayer EffectAnimationPlayer = null!;
    [Export]
    public HitBox HitBox = null!;
    [Export]
    public PlayerStateMachine StateMachine = null!;

    #endregion
    
    #region Fields
    //used a variable for maxing out health in case we need to refactor later it's easier to modify/find
    private int _maxInt = int.MaxValue;
    
    #endregion
    
    #region Lifecycle

    public override void _Ready()
    {
        
        UpdateHealth(_maxInt);
        StateMachine.Initialize(this);
        HitBox.Damaged += OnTakeDamage;
        UpdateHealth(_maxInt);
    }

    public override void _PhysicsProcess(double delta)
    {
        Direction = new Vector2(
            Input.GetAxis(InputActions.Left, InputActions.Right),
            Input.GetAxis(InputActions.Up, InputActions.Down)
        ).Normalized();
        base._PhysicsProcess(delta);
    }

    #endregion
    
    #region Public Methods

    /// <summary>
    /// Marks the player as invulnerable and disables <see cref="HitBox"/> monitoring for
    /// <paramref name="duration"/> seconds, then restores both via a one-shot scene-tree timer.
    /// </summary>
    public void MakeInvulnerable(double duration = 1.0)
    {
        IsInvulnerable = true;
        HitBox.Monitoring = false;
        
        GetTree().CreateTimer(duration).Timeout += () =>                                                                                                                                                                                                                                                              
        {           
            IsInvulnerable = false;
            HitBox.Monitoring = true;
        };  
    }
    
    /// <summary>                                                                                                                                                                                                                                                                                                   
    /// Adjusts current health by <paramref name="delta"/>, clamped to [0, <see cref="MaxHealth"/>].                                                                                                                                                                                                                  
    /// Pass a negative value to deal damage, positive to heal.                                                                                                                                                                                                                                                       
    /// </summary>                                                                                                                                                                                                                                                                                                    
    /// <param name="delta">The amount to add to current health. Use negative values for damage.</param>
    public void UpdateHealth(int delta)
    {
        CurrentHealth = Math.Clamp(CurrentHealth + delta, 0, MaxHealth);
        PlayerHud.Instance.UpdateHealth(CurrentHealth,  MaxHealth);
    }
    
    #endregion

    #region Private Methods

    /// <summary>                                                                                                                                                                                                                                                                                                   
    /// Called when the player's HitBox receives damage from a HurtBox.                                                                                                                                                                                                                                               
    /// Skipped entirely if the player is invulnerable.                                                                                                                                                                                                                                                               
    /// Applies damage, emits <see cref="PlayerCharacter.Scripts.Player.PlayerDamaged"/>, then resets health to max if the player has died.                                                                                                                                                                                                          
    /// </summary>                                                                                                                                                                                                                                                                                                    
    /// <param name="hurtBox">The HurtBox that dealt the damage. Damage value is read from <see cref="HurtBox.Damage"/>.</param>
    private void OnTakeDamage(HurtBox hurtBox)
    {
        if (IsInvulnerable) return;
        UpdateHealth(-hurtBox.Damage);
        
        EmitSignalPlayerDamaged(hurtBox);
        if (CurrentHealth <= 0) UpdateHealth(_maxInt);
    }
    
    #endregion
}
