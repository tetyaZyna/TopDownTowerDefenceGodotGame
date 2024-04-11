using Godot;

namespace TowerDefence.menu;

public partial class InfoMenu : Control
{
    private Button Back { get; set; }
    
    public override void _Ready()
    {
        Back = GetNode<Button>("Panel/BackButton");

        Back.Pressed += OnBackButtonPressed;
    }
    
    private void OnBackButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://menu/main_menu.tscn");
        if (nextScene != null)
        {
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }
}