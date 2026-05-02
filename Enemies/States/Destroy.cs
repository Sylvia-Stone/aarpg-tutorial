using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.HurtBox;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Enemies.Scripts;
using AarpgTutorial.Items.ItemSpawn;
using Godot;
using Godot.Collections;
using DropData = AarpgTutorial.Items.Scripts.DropData;

namespace AarpgTutorial.Enemies.States;

/// <summary>
/// Enemy death state. Applies a final knockback away from the damage source,
/// plays the destroy animation, then removes the enemy from the scene.
/// </summary>
public partial class Destroy : EnemyState
{
    #region Exports

    [Export]
    public StateType AnimationStateType = StateType.Destroy;

    [Export]
    public double DecelerateSpeed = 10.0;

    [Export]
    public PackedScene ItemPickupScene = null!;

    [Export]
    public double KnockBackSpeed = 200.0;
    
    [ExportCategory("Item Drops")]
    [Export]
    public Array<DropData?> ItemDrops = new();

    #endregion

    #region Fields

    private Vector2 _damagePosition;
    private Vector2 _direction;

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        ItemPickupScene.Require();
    }
    
    /// <summary>
    /// Marks the enemy invulnerable, applies knockback away from the damage source,
    /// and starts the destroy animation.
    /// </summary>
    public override void Enter()
    {
        Enemy.IsInvulnerable = true;

        _direction = Enemy.GlobalPosition.DirectionTo(_damagePosition);

        Enemy.SetDirection(_direction);
        Enemy.Velocity = _direction * -(float)KnockBackSpeed;

        Enemy.UpdateAnimation(AnimationStateType);
        Enemy.AnimationPlayer.AnimationFinished += OnAnimationFinished;
        DisableHurtBox();
        DropItems();
    }

    /// <summary>
    /// Subscribes to <see cref="Enemy.EnemyDestroyed"/> so this state can intercept
    /// the kill event and queue itself regardless of the currently active state.
    /// </summary>
    public override void Init(Enemy enemy)
    {
        enemy.EnemyDestroyed += OnEnemyDestroyed;
    }

    /// <summary>
    /// Decelerates the knockback velocity each frame. This state has no exit transition;
    /// the enemy is removed at the end of the animation.
    /// </summary>
    public override EnemyState? Process(double delta)
    {
        Enemy.Velocity -= Enemy.Velocity * (float)DecelerateSpeed * (float)delta;
        return null;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Disables <see cref="HurtBox"/> if present and sets monitoring to false
    /// </summary>
    private void DisableHurtBox()
    {
        var hurtBox = GetNodeOrNull<HurtBox>("HurtBox");
        hurtBox?.Monitoring = false;
    }

    private void DropItems() => ItemSpawn.Drop(ItemDrops, ItemPickupScene, Enemy);
    
    /// <summary>
    /// Called when the destroy animation finishes. Removes the enemy from the scene.
    /// </summary>
    private void OnAnimationFinished(StringName animation)
    {
        Enemy.QueueFree();
    }

    /// <summary>
    /// Stores the kill-shot position for knock-back direction, then transitions into this state.
    /// </summary>
    private void OnEnemyDestroyed(HurtBox hurtBox)
    {
        _damagePosition = hurtBox.GlobalPosition;
        StateMachine.ChangeState(this);
    }

    #endregion
}
