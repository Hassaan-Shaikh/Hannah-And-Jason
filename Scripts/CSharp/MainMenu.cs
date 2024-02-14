using Godot;
using System;

public partial class MainMenu : Control
{
    const string gameScene = "res://Scenes/DemoLevel1.tscn";

    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile(gameScene);
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
