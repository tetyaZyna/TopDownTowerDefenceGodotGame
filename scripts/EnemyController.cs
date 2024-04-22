using Godot;
using TowerDefence.characters;

namespace TowerDefence.scripts;

public partial class EnemyController : Node2D
{
    private Hero Hero { get; set; }
    private Button AddEnemyButton { get; set; }
    

    public override void _Ready()
    {
        Hero = GetParent().GetNode<Hero>("Hero");
        AddEnemyButton = Hero.GetNode<Button>("Sprite2D/Camera2D/CanvasLayer/MarginContainer2/Button");
        AddEnemyButton.Pressed += SummonEnemy;
    }
    
    private void SummonEnemy()
    {
        var characterInstance = (Enemy) ResourceLoader.Load<PackedScene>("res://characters/Enemy.tscn").Instantiate();
        AddChild(characterInstance);
        var enemy = ResourceLoader.Load<PackedScene>("res://characters/Demon.tscn").Instantiate();
        characterInstance.GetNode<PathFollow2D>("Path2D/PathFollow2D").AddChild(enemy);
        characterInstance.EnemyReachedGoal += HitHero;
        characterInstance.EnemyDefeated += RewardHero;
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