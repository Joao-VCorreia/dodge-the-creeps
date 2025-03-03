using Godot;
using System;

public partial class Main : Node
{
	#pragma warning disable 649

	[Export]
	public PackedScene MobScene;

	#pragma warning disable 649

	public int Score;

	private Hud _hud;

    public override void _Ready()
    {
		GD.Randomize();

		//Obtem referencia de HUD
		_hud = GetNode<Hud>("HUD");

		//Conecta o sinal diretamente
		_hud.StartGame += NewGame;

		//Conecta o sinal de Hit do player
		var player = GetNode<Player>("Player");
		player.Hit += GameOver;

		GetNode<Timer>("ScoreTimer").Timeout += OnScoreTimerTimeout;
    }

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		var musica = GetNode<AudioStreamPlayer2D>("Musica");
		musica.Stop();
		var musicaMorte = GetNode<AudioStreamPlayer2D>("MorteSom");
		musicaMorte.Play();

		GetNode<Hud>("HUD").ShowGameOver();
	}

	public void NewGame()
	{
		Score = 0;
		
		var player = GetNode<Player>("Player");
		var starPosition = GetNode<Marker2D>("StartPosition");
		player.Start(starPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(Score);
		hud.ShowMessage("Get Ready!");

		var musica = GetNode<AudioStreamPlayer2D>("Musica");
		musica.Play();
	}

	public void OnScoreTimerTimeout()
	{
		Score++;
		GetNode<Hud>("HUD").UpdateScore(Score);
		GD.Print("ScoreTimer disparado!");
	}

	public void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	public void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = GetNode<PathFollow2D>("CaminhoMob/LocalGeracaoMob");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		float direcao = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		mob.GlobalPosition = mobSpawnLocation.GlobalPosition;

		direcao += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direcao;

		var velocidade = new Vector2((float)GD.RandRange(150.0, 250.0), 0);	
		mob.LinearVelocity = velocidade.Rotated(direcao);

		AddChild(mob);
	}
}
