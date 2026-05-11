using AarpgTutorial.Enemies.Scripts;
using Godot.NativeInterop;

namespace AarpgTutorial.Enemies.Interfaces;

public interface IHasVision
{
    VisionArea VisionArea { get; }
    bool IsPlayerVisible { get; set; }
}