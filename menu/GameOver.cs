using Godot;

namespace TowerDefence.menu;

public partial class GameOver : Control
{
    private Button Restart { get; set; }
    private Button MainMenu { get; set; }
    private Button Exit { get; set; }

    public override void _Ready()
    {
        Visible = false;
        Restart = GetNode<Button>("Panel/VBoxContainer/RestartButton");
        MainMenu = GetNode<Button>("Panel/VBoxContainer/MainMenuButton");
        Exit = GetNode<Button>("Panel/VBoxContainer/ExitButton");

        Restart.Pressed += OnRestartButtonPressed;
        MainMenu.Pressed += OnMainMenuButtonPressed;
        Exit.Pressed += OnExitButtonPressed;
    }
    
    private void OnRestartButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://levels/tower_defence_demo_level.tscn");
        if (nextScene != null)
        {
            GetTree().Paused = false;
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }

    private void OnMainMenuButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://menu/main_menu.tscn");
        if (nextScene != null)
        {
            GetTree().Paused = false;
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}