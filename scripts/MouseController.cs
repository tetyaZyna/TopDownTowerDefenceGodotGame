using Godot;

namespace TowerDefence.scripts;

public partial class MouseController : Node2D
{
    private MouseMovement MouseMovementModule { get; set; }
        
    public override void _Ready()
    {
        MouseMovementModule = new MouseMovement();
        MouseMovementModule._Ready();
    }
    
    public override void _Input(InputEvent @event)
    {
        MouseMovementModule._Input(@event);
    }
    
    public override void _Process(double delta)
    {
        MouseMovementModule._Process(delta);
    }
}