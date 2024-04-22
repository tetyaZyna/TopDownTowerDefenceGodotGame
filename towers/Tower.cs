using System.Collections.Generic;
using Godot;
using TowerDefence.characters;

namespace TowerDefence.towers;

public partial class Tower : Node2D
{
    [Export] private int Level { get; set; }
    private Area2D Area2D { get; set; }
    private readonly List<Node2D> _targets = new();
    private PackedScene _arrow = ResourceLoader.Load<PackedScene>("res://towers/arrow.tscn");
    private Node Arrows { get; set; }
    private int _arrowSpeed = 100;
    private readonly Timer _timer;
    private bool _isReloaded = true;

    public void SetLevel(int level)
    {
        Level = level;
        _timer.WaitTime = 1.5f / Level;
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
        Arrows = new Node2D();
        AddChild(Arrows);
        _timer = new Timer();
        _timer.WaitTime = 1.5f / Level;
        _timer.OneShot = true;
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
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
            if (Arrows.GetChildren().Count == 0 && _isReloaded)
            {
                var currTarget = (PathFollow2D) _targets[0].GetParent();
                var arrow = new Arrow(Level, Position, currTarget.Position);
                Arrows.AddChild(arrow);
                _isReloaded = false;
                _timer.Start();
            }
        }
    }
    
    private void HitEnemy(Node2D body)
    {
        var enemy = (Enemy)body.GetParent().GetParent().GetParent();
        enemy.Hp -= 1;
        foreach (var i in Arrows.GetChildren())
        {
            i.QueueFree();
        }
    }
    
     
    private void OnTimerTimeout()
    {
        _isReloaded = true;
    }
}