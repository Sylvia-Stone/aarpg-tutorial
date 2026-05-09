using Godot;

namespace AarpgTutorial.Items.Scripts;

[GlobalClass]
public partial class DropData : Resource
{
    #region Exports

    [Export]
    public ItemData ItemData = null!;
    
    [Export(PropertyHint.Range, "0,10,1,suffix:Items")]
    public int MaxAmount;
    
    [Export(PropertyHint.Range, "0,10,1,suffix:Items")]
    public int MinAmount;

    [Export(PropertyHint.Range, "0,100,1,suffix:%")]
    public double Probability = 100;

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates a drop count clamped between <see cref="MinAmount"/> and <see cref="MaxAmount"/>
    /// Pushes a warning and sets MaxAmount to MinAmount if MaxAmount is less than MinAmount
    /// </summary>
    /// <returns></returns>
    public int GetDropCount()
    {
        if (MaxAmount < MinAmount)
        {
            GD.PushWarning($"{nameof(MaxAmount)} drop amount is less than {nameof(MinAmount)} in {nameof(DropData)}");
            MaxAmount = MinAmount;
        }
        return GD.RandRange(0, 100) > Probability ? 0 : GD.RandRange(MinAmount, MaxAmount);
    }

    #endregion
}