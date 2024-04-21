using Godot;
namespace TowerDefence.menu;

public partial class RewardContainer : HBoxContainer
{
    private readonly Texture2D _texture2D = (Texture2D)GD.Load("res://assets/user interface/Icons-Essentials.png");
    private readonly Rect2 _texture2DRegion = new Rect2(0, 0, 16, 16);
    private readonly Vector2 _speed = new Vector2(0, -50);
    private readonly Timer _timer;
    
    public RewardContainer(int reward, Vector2 startPos)
    {
        Position = startPos;
        var atlasTexture = new AtlasTexture();
        atlasTexture.Atlas = _texture2D;
        atlasTexture.Region = _texture2DRegion;
        var icon = new TextureRect();
        icon.Texture = atlasTexture;
        var label = new Label();
        label.Text = "+ " + reward;
        AddChild(icon);
        AddChild(label);
        
        _timer = new Timer();
        _timer.WaitTime = 3.0f;
        _timer.OneShot = true;
        _timer.Autostart = true;
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
        
    }

    public override void _Process(double delta)
    {
        Position += _speed * (float) delta;
    }
    
    private void OnTimerTimeout()
    {
        QueueFree();
    }
}