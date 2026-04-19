using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.Common;

/// <summary>
/// Singleton that holds a reference to the active <see cref="PlayerCharacter.Scripts.Player"/>,
/// so other systems (enemies, UI) can locate the player without scene-tree traversal.
/// </summary>
public partial class PlayerManager : Node
{
	#region Fields

	public bool IsPlayerSpawned;
	public Player Player = null!;
	public PackedScene PlayerScene = GD.Load<PackedScene>("res://PlayerCharacter/player.tscn");

	#endregion

	#region Accessors

	public static PlayerManager Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle Methods

	/// <summary>Called when the node enters the scene tree.</summary>
	public override void _Ready()
	{
		Instance = this;
		AddPlayerInstance();
	}

	#endregion

	#region Public Methods

	/// <summary>Removes the player from the scene tree without freeing it.</summary>
	/// <param name="node">The node currently parenting the player.</param>
	public void OrphanPlayer(Node2D node)
	{
		node.RemoveChild(Player);
	}

	/// <summary>Reparents the player to the given node.</summary>
	/// <param name="node">The node to parent the player under.</param>
	public void SetParent(Node2D node)
	{
		if (Player.GetParent() is not null)
			Player.GetParent().RemoveChild(Player);
		node.AddChild(Player);
	}

	/// <summary>Sets the player's global position.</summary>
	/// <param name="position">The target world position.</param>
	public void SetPlayerPosition(Vector2 position)
	{
		Player.GlobalPosition = position;
	}

	#endregion

	#region Private Methods

	/// <summary>Instantiates the player scene and adds it as a child of this manager.</summary>
	private void AddPlayerInstance()
	{
		Player = PlayerScene.Instantiate<Player>();
		AddChild(Player);
	}

	#endregion
}