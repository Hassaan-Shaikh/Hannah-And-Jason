using Godot;
using System;

public partial class MainMenu : Control
{
    public LevelLoader levelLoader;
    Label title;
    MarginContainer mainMenu;
    PanelContainer optionsMenu;
    OptionButton resolution;

    const string gameScene = "res://Scenes/MainGame.tscn";
    const string demoScene = "res://Scenes/DemoLevel1.tscn";

    public override void _Ready()
    {
        base._Ready();
        levelLoader = GetNode<LevelLoader>("LevelLoader");
        title = GetNode<Label>("Title");
        mainMenu = GetNode<MarginContainer>("MainMenu");
        optionsMenu = GetNode<PanelContainer>("Options");
        resolution = GetNode<OptionButton>("Options/MarginContainer/VBoxContainer/Resolution/ResolutionOption");

        optionsMenu.Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        switch (resolution.Selected)
        {
            case 0:
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                break;
            case 1:
                DisplayServer.WindowSetSize(new Vector2I(1366, 768));
                break;
            case 2:
                DisplayServer.WindowSetSize(new Vector2I(1440, 810));
                break;
            case 3:
                DisplayServer.WindowSetSize(new Vector2I(1600, 900));
                break;
            case 4:
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
                break;
            case 5:
                DisplayServer.WindowSetSize(new Vector2I(3840, 2160));
                break;
        }
    }

    private void OnPlayButtonPressed()
    {
        levelLoader.SwitchScene(gameScene);
    }

    private void OnPuzzleButtonPressed()
    {
        //levelLoader.SwitchScene(demoScene);
        title.Visible = false;
        mainMenu.Visible = false;
        optionsMenu.Visible = true;
    }

    private void OnOptionsBackPressed()
    {
        optionsMenu.Visible = false;
        title.Visible = true;
        mainMenu.Visible = true;
    }

    private void OnResolutionOptionItemSelected(int index)
    {
        switch (index)
        {
            case 0:
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                break;
            case 1:
                DisplayServer.WindowSetSize(new Vector2I(1366, 768));
                break;
            case 2:
                DisplayServer.WindowSetSize(new Vector2I(1440, 810));
                break;
            case 3:
                DisplayServer.WindowSetSize(new Vector2I(1600, 900));
                break;
            case 4:
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
                break;
            case 5:
                DisplayServer.WindowSetSize(new Vector2I(3840, 2160));
                break;
            default:
                DisplayServer.WindowSetSize(DisplayServer.WindowGetSize());
                break;
        }
    }

    private void OnResolutionOptionToggled(bool toggledOn)
    {
        if (toggledOn)
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
