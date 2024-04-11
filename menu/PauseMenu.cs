using System;
using Godot;

namespace TowerDefence.menu;

public partial class PauseMenu : Control
{
    public event EventHandler<bool> PauseChanged;
    private bool _isPaused;
    private bool IsPaused 
    { 
        get => _isPaused; 
        set
        {
            if (_isPaused == value) return;
            _isPaused = value;
            PauseChanged?.Invoke(this, value);
        }
    }
    private Button Back { get; set; }
    private Button MainMenu { get; set; }
    private Button Exit { get; set; }

    public override void _Ready()
    {
        IsPaused = false;
        Visible = IsPaused;
        Back = GetNode<Button>("Panel/VBoxContainer/BackButton");
        MainMenu = GetNode<Button>("Panel/VBoxContainer/MainMenuButton");
        Exit = GetNode<Button>("Panel/VBoxContainer/ExitButton");

        Back.Pressed += OnBackButtonPressed;
        MainMenu.Pressed += OnMainMenuButtonPressed;
        Exit.Pressed += OnExitButtonPressed;
    }
    
    private void OnBackButtonPressed()
    {
        IsPaused = !IsPaused;
        Visible = IsPaused;
    }

    private void OnMainMenuButtonPressed()
    {
        var nextScene = (PackedScene)ResourceLoader.Load("res://menu/main_menu.tscn");
        if (nextScene != null)
        {
            GetTree().ChangeSceneToPacked(nextScene);
        }
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }


    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            IsPaused = !IsPaused;
            Visible = IsPaused;
        }
    }
}