namespace AarpgTutorial.Common.Enums;

/// <summary>Named state types used to build directional animation keys via <see cref="AarpgTutorial.Common.Actor.UpdateAnimation"/>.</summary>
public enum StateType
{
    Idle,
    Walk,
    Attack,
    Destroy,
    Stun,
    Damaged
}
