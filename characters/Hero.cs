using System;
using Godot;

namespace TowerDefence.characters;

public partial class Hero : CharacterBody2D
{
    [Export] public float MoveSpeed = 100;
    [Export] public Vector2 StartDirection = new(0, 1);
    private Vector2 HeroDirection { get; set; }
    private AnimationTree AnimationTree { get; set; }
    private AnimationNodeStateMachinePlayback StateMachine { get; set; }
    private NavigationAgent2D NavigationAgent { get; set; }
    public event EventHandler<bool> AutoMoveModeChanged;
    private bool _isAutoMoveMode;
    private bool IsAutoMoveMode 
    { 
        get => _isAutoMoveMode; 
        set
        {
            if (_isAutoMoveMode == value) return;
            _isAutoMoveMode = value;
            AutoMoveModeChanged?.Invoke(this, value);
        }
    }
        
        
    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        StateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        UpdateAnimationParameters(StartDirection);
        HeroDirection = StartDirection;
        NavigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
    }

    public Vector2 GetHeroDirection()
    {
        return HeroDirection;
    }

    public void MoveHeroTo(Vector2 targetPos)
    {
        NavigationAgent.TargetPosition = targetPos;
        IsAutoMoveMode = NavigationAgent.IsTargetReachable();
    }

    public override void _PhysicsProcess(double delta)
    {
        var moveDirection = new Vector2(
            Input.GetActionStrength("right") - Input.GetActionStrength("left"),
            Input.GetActionStrength("down") - Input.GetActionStrength("up"));
        if (moveDirection != Vector2.Zero && IsAutoMoveMode)
        {
            IsAutoMoveMode = false;
        }
        if (IsAutoMoveMode)
        {
            if (Position.DistanceTo(NavigationAgent.TargetPosition) > 7)
            {
                moveDirection = ToLocal(NavigationAgent.GetNextPathPosition()).Normalized();
            }
            else
            {
                moveDirection = Vector2.Zero;
                IsAutoMoveMode = false;
            }
        }
        MoveHero(moveDirection);
    }

    private void MoveHero(Vector2 moveDirection)
    {
        UpdateHeroDirection(moveDirection);
        UpdateAnimationParameters(moveDirection);
        Velocity = moveDirection * MoveSpeed;
        MoveAndSlide();
        PickNewState();
    }

    private void UpdateHeroDirection(Vector2 newDirection)
    {
        if (newDirection is { X: 0, Y: 0 }) return;
        const float epsilon = 0.0001f;
        var diffX = Math.Abs(HeroDirection.X - newDirection.X);
        var diffY = Math.Abs(HeroDirection.Y - newDirection.Y);
        if (diffX > epsilon || diffY > epsilon)
        {
            var newX = Math.Abs(HeroDirection.X - newDirection.X) > epsilon ? newDirection.X : HeroDirection.X;
            var newY = Math.Abs(HeroDirection.Y - newDirection.Y) > epsilon ? newDirection.Y : HeroDirection.Y;
            HeroDirection = new Vector2(newX, newY);
        }
    }

    private void UpdateAnimationParameters(Vector2 moveInput)
    {
        if (moveInput == Vector2.Zero) return;
        AnimationTree.Set("parameters/idle/blend_position", moveInput);
        AnimationTree.Set("parameters/walk/blend_position", moveInput);
    }

    private void PickNewState()
    {
        StateMachine.Travel(Velocity != Vector2.Zero ? "walk" : "idle");
    }
}