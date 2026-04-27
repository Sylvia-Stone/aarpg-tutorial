using Godot;

namespace AarpgTutorial.Items.ItemEffect;

/// <summary>Abstract base for item use effects. Subclass and implement <see cref="Use"/> to define specific behavior.</summary>
[GlobalClass]
public abstract partial class ItemEffect : Resource
{
    #region Public Methods

    /// <summary>Applies this effect. Called when the player uses the owning item.</summary>
    public abstract void Use();

    #endregion
}