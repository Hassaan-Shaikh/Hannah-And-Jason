using Godot;
using System;

public partial class MainMenu : Control
{
    public LevelLoader levelLoader;

    const string gameScene = "res://Scenes/MainGame.tscn";
    const string demoScene = "res://Scenes/DemoLevel1.tscn";

    public override void _Ready()
    {
        base._Ready();
        levelLoader = GetNode<LevelLoader>("LevelLoader");
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void OnPlayButtonPressed()
    {
        levelLoader.SwitchScene(gameScene);
    }

    private void OnPuzzleButtonPressed()
    {
        levelLoader.SwitchScene(demoScene);
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
