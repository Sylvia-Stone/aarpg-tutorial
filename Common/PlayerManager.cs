using System.ComponentModel.DataAnnotations;
using Godot;

namespace AarpgTutorial.Common;

public partial class PlayerManager : Node
{
    public Player.Scripts.PlayerCharacter PlayerCharacter;
    
    public static PlayerManager Instance { get; private set; }

    public override void _Ready() => Instance = this;
}