using AarpgTutorial.GUI.PauseMenu.Inventory.Scripts;
using AarpgTutorial.PlayerCharacter.Scripts;
using Godot;

namespace AarpgTutorial.Common.Managers;

/// <summary>Singleton that hands out the active player reference to anything that needs it.</summary>
public partial class PlayerManager : Node
{
	#region Signals

	[Signal]
	public delegate void InteractPressedEventHandler();

	#endregion

	#region Fields

	public InventoryData InventoryData = GD.Load<InventoryData>("res://GUI/PauseMenu/Inventory/PlayerInventory.tres");
	public bool IsPlayerSpawned;
	public Player Player = null!;
	public PackedScene PlayerScene = GD.Load<PackedScene>("res://PlayerCharacter/Player.tscn");

	#endregion

	#region Accessors

	public static PlayerManager Instance { get; private set; } = null!;

	#endregion

	#region Lifecycle Methods

	public override void _EnterTree()
	{
		Instance = this;
	}

	public override void _Ready()
	{
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

	/// <summary>Fires the InteractPressed signal. Godot requires this indirection because signals can only be emitted from within the class that declares them.</summary>
	public void RaisePlayerInteracted() => EmitSignalInteractPressed();

	/// <summary>Sets the player's current and max health, then forces a HUD refresh.</summary>
	/// <param name="health">The health value to restore.</param>
	/// <param name="maxHealth">The max health value to restore.</param>
	public void SetHealth(int health, int maxHealth)
	{
		Player.CurrentHealth = health;
		Player.MaxHealth = maxHealth;
		Player.UpdateHealth(0);
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
