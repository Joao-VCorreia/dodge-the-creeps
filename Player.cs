using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Velocidade = 400; // Variavel em export para aparecer no editor
	public Vector2 ScreenSize; // Vector2 armazena valores para X e Y, seja quais forem
	private AnimatedSprite2D animatedSprit;
	public override void _Ready() //_Ready é usado quando queremos garantir que todos os nós filhos estão carregados
	{
		ScreenSize = GetViewportRect().Size; //Get nos traz um retangulo na forma do view port e size extrai o tamanho

		animatedSprit = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // É ideal importar nós filhos dentro de ready()
	}

	public override void _Process(double delta)// atualiza a cada quadro do jogo
	{
		Vector2 movimento = Vector2.Zero; // Inicializa o vetor de movimento como (0, 0) (parado).

		/*Como estamos no terceiro plano cartesiano os valores em X são normais mas Y é o contrário
		(0, 0) ---------------------> (+X, 0)
  		  |                   	        |
  		  |                   	        |
  		  |                   	        |
  		  v                    	        v
		(0, +Y) ------------------> (+X, +Y)
		*/
		if(Input.IsActionPressed("mover_cima"))
		{
			movimento.Y -= 1; 
		}
		if(Input.IsActionPressed("mover_baixo"))
		{
			movimento.Y += 1;
		}
		if(Input.IsActionPressed("mover_direita"))
		{
			movimento.X += 1;
		}
		if(Input.IsActionPressed("mover_esquerda"))
		{
			movimento.X -= 1;
		}

		if(movimento.Length() > 0) //Verifica se o movimento é diferente de (0,0)
		{
			movimento = movimento.Normalized() * Velocidade; //Mantem a magnitude em 1 com normalized para evitar que o movimento diagonal seja mais rapido
			animatedSprit.Play(); //Inicia a Animação
		}
		else
		{
			animatedSprit.Stop();
		}

		Position += movimento * (float)delta; //Atualiza a movimentação no frame com base no vector2 movimento e no tempo delta
		Position = new Vector2 // Mathf.Clamp(valor, min, max)
		(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X), // Limita a posição X dentro da tela
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y) // Limita a posição Y dentro da tela
		); 

		if (movimento.X != 0) // Verifica movimento horizontal (X positivo = direita, X negativo = esquerda)
		{
			animatedSprit.Animation = "walk"; // Define animação de caminhada lateral
			animatedSprit.FlipV = false; // Desativa flip vertical para evitar distorção
			animatedSprit.FlipH = movimento.X < 0; // Inverte horizontalmente o sprite se o movimento for para a esquerda (X < 0)
		}
		else if(movimento.Y != 0) // Se não há movimento horizontal, verifica movimento vertical (Y positivo = baixo, Y negativo = cima)
		{
			animatedSprit.Animation = "up"; // Define animação de movimento vertical 
			animatedSprit.FlipV = movimento.Y > 0; // Inverte verticalmente o sprite se o movimento for para baixo (Y > 0). Útil se a animação "up" originalmente aponta para cima.
		}
	}
}
