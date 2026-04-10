using Godot;
using System.Collections.Generic;

namespace AARPGtutorial.Player.Scripts;

public partial class Player : CharacterBody2D
{
	private const string AnimPlayerNode = "AnimationPlayer";
	private const string SpriteNode     = "Sprite2D";
	private const string StateMachineNode = "StateMachine";

	private const string ActionRight = "right";
	private const string ActionLeft  = "left";
	private const string ActionDown  = "down";
	private const string ActionUp    = "up";

	private Vector2 _cardinalDirection = Vector2.Down;
	public Vector2 Direction = Vector2.Zero;
	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;
	private PlayerStateMachine _stateMachine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>(AnimPlayerNode);
		_sprite = GetNode<Sprite2D>(SpriteNode);
		_stateMachine = GetNode<PlayerStateMachine>(StateMachineNode);

		_stateMachine.Initialize(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		Direction = new Vector2(
			Input.GetAxis(ActionLeft, ActionRight),
			Input.GetAxis(ActionUp, ActionDown)
		).Normalized();
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
		_sprite.Scale = new Vector2(_cardinalDirection == Vector2.Left ? -1 : 1, 1);
		return true;
	}
	
	public void UpdateAnimation(PlayerState state)
	{
		_animationPlayer.Play($"{state}{GetAnimDirection()}");
	}

	private static readonly Dictionary<Vector2, AnimationDirection> DirectionAnimations = new()
	{
		{ Vector2.Up,    AnimationDirection.Up   },
		{ Vector2.Down,  AnimationDirection.Down },
		{ Vector2.Left,  AnimationDirection.Side },
		{ Vector2.Right, AnimationDirection.Side },
	};

	public AnimationDirection GetAnimDirection() =>
		DirectionAnimations.GetValueOrDefault(_cardinalDirection, AnimationDirection.Down);
}
