using System.ComponentModel.DataAnnotations;
using Godot;

namespace AarpgTutorial.Common;

/// <summary>
/// Singleton that holds a reference to the active <see cref="PlayerCharacter.Scripts.Player"/>,
/// so other systems (enemies, UI) can locate the player without scene-tree traversal.
/// </summary>
public partial class PlayerManager : Node
{
    #region Fields

    public PlayerCharacter.Scripts.Player Player = null!;

    public static PlayerManager Instance { get; private set; } = null!;

    #endregion

    #region Lifecycle

    public override void _Ready() => Instance = this;

    #endregion
}
