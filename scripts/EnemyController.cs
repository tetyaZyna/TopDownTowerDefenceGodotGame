using System;
using Godot;
using TowerDefence.characters;

namespace TowerDefence.scripts;

public partial class EnemyController : Node2D
{
    private Hero Hero { get; set; }
    

    public override void _Ready()
    {
        Hero = GetParent().GetNode<Hero>("Hero");
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("debug"))
        {
            var characterInstance = (Enemy) ResourceLoader.Load<PackedScene>("res://characters/Enemy.tscn").Instantiate();
            AddChild(characterInstance);
            var enemy = ResourceLoader.Load<PackedScene>("res://characters/Demon.tscn").Instantiate();
            characterInstance.GetNode<PathFollow2D>("Path2D/PathFollow2D").AddChild(enemy);
            characterInstance.EnemyReachedGoal += HitHero;
            characterInstance.EnemyDefeated += RewardHero;
            
        }
    }

    private void HitHero(object sender, bool isReached)
    {
        Hero.Hp -= 1;
    }
    
    private void RewardHero(object sender, int reward)
    {
        Hero.Coins += reward;
    }
}