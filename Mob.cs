  using Godot;
  using System;

  public partial class Mob : RigidBody2D
  {
      public override void _Ready()
      {
        //Obtem o nó de animacao para manipular
        AnimatedSprite2D animacao = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Lista de animações do nosso nó
        string[] tiposMob = animacao.SpriteFrames.GetAnimationNames();

        // seleciona uma animação aleatoriamente entre as animacoes obtidas anteriormente
        animacao.Animation = tiposMob[GD.Randi() % tiposMob.Length];

        //Inicia a animação
        animacao.Play();

        VisibleOnScreenNotifier2D foraDoScreen = GetNode<VisibleOnScreenNotifier2D>("Visibilidade");

        foraDoScreen.ScreenExited += OnScreenExited;
      }

      private void OnScreenExited()
      {
        GD.Print("Mob saiu da tela!");
        QueueFree();
      }
  }
