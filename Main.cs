using Godot;
using System;
using System.ComponentModel;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;

	private void NewGame()
	{
		_score = 0;
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

		var startPosition = GetNode<Marker2D>("StartPosition");
		GetNode<Player>("Player").Start(startPosition.Position);
		GetNode<AudioStreamPlayer>("Music").Play();

		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		GetNode<Timer>("StartTimer").Start();
	}

	private void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		mob.Position = mobSpawnLocation.Position;
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		AddChild(mob);
		Console.WriteLine($"Added mob at position {mob.Position}");
	}
}
