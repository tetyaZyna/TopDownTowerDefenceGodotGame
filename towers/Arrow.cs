using System;
using Godot;
using TowerDefence.characters;

namespace TowerDefence.towers;

public partial class Arrow : CharacterBody2D
{
    private int _arrowSpeed = 100;
    private PackedScene _arrow = ResourceLoader.Load<PackedScene>("res://towers/arrow.tscn");
    private int Level { get; set; }
    //private Area2D Area2D { get; set; }
    

    public Arrow(int level, Vector2 startPosition, Vector2 targetPosition)
    {
        var arrow = (CharacterBody2D) _arrow.Instantiate();
        AddChild(arrow);
        arrow.GetNode<Area2D>("Area2D").BodyEntered += HitEnemy;
        // arrow.ZIndex = 1;
        // arrow.Position = startPosition;
        // Vector2 direction = (targetPosition - Position).Normalized();
        // arrow.Rotation = (float)Math.Atan2(direction.Y, direction.X); 
        // arrow.Position = Position;
        // arrow.Velocity = direction * _arrowSpeed;
        // Level = level;
        // // Area2D = GetNode<Area2D>("Area2D");
        // // Area2D.BodyEntered += HitEnemy;

        // ZIndex = 1;
        // Sprite2D sprite2D = new Sprite2D();
        // sprite2D.Texture = ResourceLoader.Load<Texture2D>("res://towers/arrow.tres");
        // AddChild(sprite2D);
        // CollisionShape2D collisionShape2D = new CollisionShape2D();
        // Shape2D shape2D = new CapsuleShape2D();
        // shape2D.Set("radius", 5);
        // shape2D.Set("height", 15);
        // shape2D.Set("rotation", 90);
        // collisionShape2D.Shape = shape2D;
        // AddChild(collisionShape2D);
        // CollisionLayer = 2;
        // CollisionMask = 2;
        // Area2D area2D = arrow.GetNode<Area2D>("Area2D");
        // AddChild(area2D);
        // area2D.BodyEntered += HitEnemy;
    }
   
    public Arrow()
    {
    }

    private void HitEnemy(Node2D body)
    {
        // var enemy = (Enemy)body.GetParent().GetParent().GetParent();
        //
        // enemy.Hp -= 1;
    }

    public override void _Process(double delta)
    {
        MoveAndSlide();
    }
}