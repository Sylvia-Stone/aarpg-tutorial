using AarpgTutorial.Combat;
using AarpgTutorial.Common.Utilities;
using AarpgTutorial.Enemies.Interfaces;
using AarpgTutorial.Enemies.Scripts;
using Godot;

namespace AarpgTutorial.Enemies.Goblin;

public partial class Goblin : Enemy, IHasAttack, IHasVision
{
    #region Exports

    [Export]
    public HurtBox AttackArea { get; set; } = null!; 
    
    [Export]
    public VisionArea VisionArea { get; set; } = null!;

    #endregion

    #region Fields

    public bool IsPlayerVisible { get; set; }

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        AttackArea.Require();
        VisionArea.Require();
        
        base._Ready();
    }

    #endregion
}