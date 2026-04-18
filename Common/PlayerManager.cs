using Godot;
using AarpgTutorial.PlayerCharacter.Scripts;

namespace AarpgTutorial.Common;

/// <summary>
/// Singleton that holds a reference to the active <see cref="PlayerCharacter.Scripts.Player"/>,
/// so other systems (enemies, UI) can locate the player without scene-tree traversal.
/// </summary>
public partial class PlayerManager : Node
{
    #region Fields

    public PackedScene PlayerScene = GD.Load<PackedScene>("res://PlayerCharacter/player.tscn");
    public Player Player = null!;
    public bool IsPlayerSpawned;

    public static PlayerManager Instance { get; private set; } = null!;

    #endregion

    #region Lifecycle

    public override void _Ready()
    {
        Instance = this;
        AddPlayerInstance();
    }

    #endregion

    public void SetPlayerPosition(Vector2 position)
    {
        Player.GlobalPosition = position;
    }

    public void SetParent(Node2D node)
    {
        if (Player.GetParent() is not null)
        {
            Player.GetParent().RemoveChild(Player);
        }
        node.AddChild(Player);    
    }
    
    private void AddPlayerInstance()
    {
        Player = PlayerScene.Instantiate<Player>();
        AddChild(Player);
    }
}
