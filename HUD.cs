using Godot;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");
		GetNode<Timer>("MessageTimer").Start();

		await ToSignal(GetNode<Timer>("MessageTimer"), Timer.SignalName.Timeout);
		ShowTitle();

		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		GetNode<Timer>("MessageTimer").Start();
	}

	public void ShowTitle()
	{
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the Creeps!";
		message.Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
}
