using Godot;

namespace AarpgTutorial.Items.ItemEffect;

[GlobalClass]
public abstract partial class ItemEffect : Resource
{
    #region Public Methods

    public abstract void Use();

    #endregion
}