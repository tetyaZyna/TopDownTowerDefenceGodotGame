using System;
using Godot;
using TowerDefence.characters;

namespace TowerDefence.towers;

public partial class Arrow : CharacterBody2D
{
    private int _arrowSpeed = 100;
    private PackedScene _arrow = ResourceLoader.Load<PackedScene>("res://towers/arrow.tscn");
    private int Level { get; set; }
    

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
    }
   
    public Arrow()
    {
    }

    private void HitEnemy(Node2D body)
    {
        var enemy = (Enemy)body.GetParent().GetParent().GetParent();
        enemy.Hp -= 1;
        QueueFree();
    }

    public override void _Process(double delta)
    {
        MoveAndSlide();
    }
}