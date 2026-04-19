using System.Threading.Tasks;
using AarpgTutorial.Common.Enums;
using Godot;

namespace AarpgTutorial.GUI.SceneTransition;

public partial class SceneTransition : CanvasLayer
{
    [Export]
    public AnimationPlayer AnimationPlayer = null!;
    
    public static SceneTransition Instance { get; private set; }

    public override void _Ready() => Instance = this;
    
    public async Task<bool> FadeOut()
    {
        AnimationPlayer.Play(nameof(AnimationType.FadeOut));
        await ToSignal(AnimationPlayer, "animation_finished");
        return true;
    }
    
    public async Task<bool> FadeIn()
    {
        AnimationPlayer.Play(nameof(AnimationType.FadeIn));
        await ToSignal(AnimationPlayer, "animation_finished");
        return true;
    }
}