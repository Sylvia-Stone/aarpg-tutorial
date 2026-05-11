namespace AarpgTutorial.Common.Enums;

/// <summary>Named state types used to build directional animation keys via <see cref="AarpgTutorial.Common.Actor.UpdateAnimation"/>.</summary>
public enum State
{
    Attack,
    Chase,
    Destroy,
    Idle,
    Stun,
    Walk,
}