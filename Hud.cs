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
	async public void ShowGameOver()
	{
		ShowMessage("Gamer Over!"); //Envia a mensagem de Game Over

		var messagerTimer = GetNode<Timer>("MessageTimer"); //Seleciona nó com o tempo da mensagem
		await ToSignal(messagerTimer, "timeout"); //Aguarda o sinal "timeout" de MessagerTimer definido em "Wait Timer" para 
		
		var message = GetNode<Label>("Message"); //Seleciona o nó Message
		message.Text = "Dodge the\nCreeps!"; //Altera a propriedade Text 
		message.Show(); //Mostra a mensagem

		await ToSignal(GetTree().CreateTimer(1), "timeout"); //GetTree() retorna ao nó raiz, enquanto CreateTimer(1) cria temporariamente um timer com Wait Time de 1 segundo, quanto o sinal timeout é acionado o programa continua
		GetNode<Button>("StartButton").Show(); //Mostra o botão de iniciar o jogo
	}

	public void UpdateScore(int score) //Função que será chamada para atualizar score
	{
		GetNode<Label>("Score").Text = score.ToString(); //Transforma o valor int em texto para ser exibido no label
	}

	public void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	public void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
}
