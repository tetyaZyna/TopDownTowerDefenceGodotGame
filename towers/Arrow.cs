using System;
using Godot;
using TowerDefence.characters;

namespace TowerDefence.towers;

public partial class Arrow : CharacterBody2D
{
    private int _arrowSpeed = 100;
    private int _baseArrowDamage = 2;
    private PackedScene _arrow = ResourceLoader.Load<PackedScene>("res://towers/arrow.tscn");
    private int Level { get; set; }
    private readonly Timer _timer;
    

    public Arrow(int level, Vector2 startPosition, Vector2 targetPosition)
    {
        var arrow = (CharacterBody2D) _arrow.Instantiate();
        AddChild(arrow);
        arrow.GetNode<Area2D>("Area2D").BodyEntered += HitEnemy;
        arrow.Position = startPosition;
        Vector2 direction = (targetPosition - arrow.Position).Normalized();
        arrow.Rotation = (float)Math.Atan2(direction.Y, direction.X); 
        arrow.Position = Position;
        arrow.Velocity = direction * _arrowSpeed;
        Level = level;
        
        _timer = new Timer();
        _timer.WaitTime = 0.8f;
        _timer.OneShot = true;
        _timer.Autostart = true;
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
    }
   
    public Arrow()
    {
    }

    private void HitEnemy(Node2D body)
    {
        var enemy = (Enemy)body.GetParent().GetParent().GetParent();
        enemy.Hp -= _baseArrowDamage * Level;
        QueueFree();
    }
    
    private void OnTimerTimeout()
    {
        QueueFree();
    }

    public override void _Process(double delta)
    {
        MoveAndSlide();
    }
}