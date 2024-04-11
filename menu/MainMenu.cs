using Godot;

namespace TowerDefence.menu;

public partial class MainMenu : Control
{
    private Button Start { get; set; }
    private Button InfoMenu { get; set; }
    private Button Exit { get; set; }

    public override void _Ready()
    {
        Start = GetNode<Button>("Panel/VBoxContainer/StartButton");
        InfoMenu = GetNode<Button>("Panel/VBoxContainer/InfoButton");
        Exit = GetNode<Button>("Panel/VBoxContainer/ExitButton");

        Start.Pressed += OnStartButtonPressed;
        InfoMenu.Pressed += OnInfoMenuButtonPressed;
        Exit.Pressed += OnExitButtonPressed;
    }
    
    private void OnStartButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://levels/tower_defence_demo_level.tscn");
        if (nextScene != null)
        {
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }

    private void OnInfoMenuButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://menu/info_menu.tscn");
        if (nextScene != null)
        {
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}