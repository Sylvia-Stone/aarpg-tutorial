using AarpgTutorial.Enemies.Scripts;

namespace AarpgTutorial.Enemies.Interfaces;

public interface IHasVision
{
    VisionArea VisionArea { get; }
    bool IsPlayerVisible { get; set; }
}