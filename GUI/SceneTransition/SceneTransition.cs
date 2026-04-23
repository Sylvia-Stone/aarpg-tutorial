using System.Threading.Tasks;
using AarpgTutorial.Common.Enums;
using AarpgTutorial.Common.Utilities;
using Godot;

namespace AarpgTutorial.GUI.SceneTransition;

public partial class SceneTransition : CanvasLayer
{
    #region Exports

    [Export]
    public AnimationPlayer AnimationPlayer = null!;

    #endregion

    #region Accessors

    public static SceneTransition Instance { get; private set; } = null!;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        AnimationPlayer.Require();
        Instance = this;
    }

    #endregion

    #region Public Methods

    /// <summary>Fades the screen in by playing the FadeIn animation and awaiting completion.</summary>
    public async Task<bool> FadeIn()
    {
        AnimationPlayer.Play(nameof(AnimationType.FadeIn));
        await ToSignal(AnimationPlayer, "animation_finished");
        return true;
    }

    /// <summary>Fades the screen out by playing the FadeOut animation and awaiting completion.</summary>
    public async Task<bool> FadeOut()
    {
        AnimationPlayer.Play(nameof(AnimationType.FadeOut));
        await ToSignal(AnimationPlayer, "animation_finished");
        return true;
    }

    #endregion
}
