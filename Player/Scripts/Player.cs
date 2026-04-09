using Godot;
using System.Collections.Generic;

namespace AARPGtutorial.Player.Scripts;

public partial class Player : CharacterBody2D
{
	private Vector2 _cardinalDirection = Vector2.Down;
	public Vector2 Direction = Vector2.Zero;
	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;
	private PlayerStateMachine _stateMachine;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer"); 
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_stateMachine = GetNode<PlayerStateMachine>("StateMachine");
		
		_stateMachine.Initialize(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		Direction.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		Direction.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		MoveAndSlide();
	}

	public bool SetDirection()
	{
		if (Direction == Vector2.Zero) return false;
		var newDirection = Direction.Abs().X > Direction.Abs().Y
			? (Direction.X > 0 ? Vector2.Right : Vector2.Left)
			: (Direction.Y > 0 ? Vector2.Down  : Vector2.Up);
		if (_cardinalDirection == newDirection) return false;
		_cardinalDirection = newDirection;
		return true;
	}
	
	public void UpdateAnimation(PlayerState state)
	{
		_sprite.FlipH = _cardinalDirection == Vector2.Left;
		_animationPlayer.Play($"{state}{AnimDirection()}");
	}

	private static readonly Dictionary<Vector2, AnimationDirection> DirectionAnimations = new()
	{
		{ Vector2.Up,    AnimationDirection.Up   },
		{ Vector2.Down,  AnimationDirection.Down },
		{ Vector2.Left,  AnimationDirection.Side },
		{ Vector2.Right, AnimationDirection.Side },
	};

	private AnimationDirection AnimDirection() =>
		DirectionAnimations.GetValueOrDefault(_cardinalDirection, AnimationDirection.Down);
}
