using System;
using Godot;

namespace TowerDefence.characters;

public partial class Demon : CharacterBody2D
{
    [Export] public Vector2 StartDirection = new(0, 1);
    private Vector2 EnemyDirection { get; set; }
    private AnimationTree AnimationTree { get; set; }
    private AnimationNodeStateMachinePlayback StateMachine { get; set; }
    private Vector2 PreviousPosition { get; set; }
    private PathFollow2D PathFollow { get; set; }
    
    public override void _Ready()
    {
        PathFollow = (PathFollow2D) GetParent();
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        StateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        UpdateAnimationParameters(StartDirection);
        EnemyDirection = StartDirection;
        PreviousPosition = PathFollow.Position;
    }

    public override void _Process(double delta)
    {
        Vector2 currentPosition = PathFollow.Position;
        Vector2 direction = (currentPosition - PreviousPosition).Normalized();
        direction.X *= -1; 
        PreviousPosition = currentPosition;
        StateMachine.Travel( "walk");
        UpdateAnimationParameters(direction);
    }

    private void UpdateAnimationParameters(Vector2 direction)
    {
        if (direction == Vector2.Zero) return;
        AnimationTree.Set("parameters/idle/blend_position", direction);
        AnimationTree.Set("parameters/walk/blend_position", direction);
    }
}