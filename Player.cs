using Godot;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;
	
	private Vector2 ScreenSize;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	public override void _Process(double delta)
	{
		var velocity = ReadInputsAndGetSpeed();

		SetAnimation(velocity);
		SetPosition(delta, velocity);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private Vector2 ReadInputsAndGetSpeed()
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		return velocity.Length() > 0 ? velocity.Normalized() * Speed : velocity;
	}

	private void SetAnimation(Vector2 velocity)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
		}
		else
		{
			animatedSprite2D.Animation = "walk";
		}

		animatedSprite2D.FlipH = velocity.X < 0;
		animatedSprite2D.FlipV = velocity.Y > 0;
	}

	private void SetPosition(double delta, Vector2 velocity)
	{
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

}
