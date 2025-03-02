using Godot;
using System;
using System.Threading.Tasks;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler(); //Emite sinal de jogo iniciado

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message"); // Obtém o nó "Message" do tipo Label
		message.Text = text; // Define o texto da mensagem
		message.Show(); // Exibe o texto na tela

		GetNode<Timer>("MessageTimer").Start(); // Inicia o timer "MessageTimer"
	}	


	public async Task ShowGameOver()
	{
		ShowMessage("Gamer Over!");

		var messagerTimer = GetNode<Timer>("MessagerTimer");
		await ToSignal(messagerTimer, "timeout");

		var message = GetNode<Label>("Message");
		message.Text = "Dodge the\nCreeps!";
		message.Show();

		await ToSignal(GetTree().CreateTimer(1), "timeout");
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
}
