using System;
using Godot;

namespace TowerDefence.characters;

public partial class Enemy : Node2D
{
    [Export] public float MoveSpeed = 100;
    private int _hp = 10;
    private int Hp 
    { 
        get => _hp; 
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                KillEnemy();
            }
        }
    }
    private int Reward { get; set; }
    private PathFollow2D PathFollow { get; set; }
    public event EventHandler<bool> EnemyReachedGoal;
    public event EventHandler<int> EnemyDefeated;
    
    public override void _Ready()
    {
        PathFollow = GetNode<PathFollow2D>("Path2D/PathFollow2D");
        Reward = new Random().Next(1, 11);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("debug"))
        {
            Hp = 0;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        PathFollow.Progress += MoveSpeed * (float) delta;
        
        if (PathFollow.ProgressRatio >= 1)
        {
            EnemyReachedGoal?.Invoke(this, true);
            QueueFree();
        }
    }

    private void KillEnemy()
    {
        EnemyDefeated?.Invoke(this, Reward);
        QueueFree();
    }
}