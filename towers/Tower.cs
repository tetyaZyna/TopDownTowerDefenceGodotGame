using System;
using System.Collections.Generic;
using Godot;

namespace TowerDefence.towers;

public partial class Tower : Node2D
{
    [Export] private int Level { get; set; }
    private Area2D Area2D { get; set; }
    private List<Node2D> _targets = new();
    private PackedScene _arrow = ResourceLoader.Load<PackedScene>("res://towers/arrow.tscn");
    private Node Arrows { get; set; }
    private int _arrowSpeed = 1000;

    public void SetLevel(int level)
    {
        Level = level;
    }
    
    public Tower(Vector2 position)
    {
        Position = position;
        Level = 1;
        Area2D= new Area2D();
        AddChild(Area2D);
        CollisionShape2D collisionShape2D = new CollisionShape2D();
        Shape2D shape2D = new CircleShape2D();
        shape2D.Set("radius", 70);
        collisionShape2D.Shape = shape2D;
        collisionShape2D.Visible = true;
        Area2D.AddChild(collisionShape2D);
        Area2D.BodyEntered += OnBodyEntered;
        Area2D.BodyExited += OnBodyExited;
        Arrows = new Node();
        AddChild(Arrows);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.Name == "Demon")
        {
            _targets.Add(body);
        }
    }
    
    private void OnBodyExited(Node2D body)
    {
        _targets.Remove(body);
    }

    public override void _Process(double delta)
    {
        if (_targets.Count > 0)
        {
            if (Arrows.GetChildren().Count < 30)
            {
                var currTarget = (PathFollow2D) _targets[0].GetParent();
                GD.Print(currTarget.Position);
                GD.Print(currTarget.Name);
                var arrow = (CharacterBody2D) _arrow.Instantiate();
                arrow.ZIndex = 1;
                arrow.Position = Position;
                Vector2 direction = (currTarget.Position - Position).Normalized();
                arrow.Rotation = (float)Math.Atan2(direction.Y, direction.X); 
                arrow.Position = Position;
                arrow.Velocity = direction * _arrowSpeed;
                Arrows.AddChild(arrow);
            }
            

        }
    }
    
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("debug"))
        {
            foreach (var i in Arrows.GetChildren())
            {
                Arrows.RemoveChild(i);
            }
        }
    }
}