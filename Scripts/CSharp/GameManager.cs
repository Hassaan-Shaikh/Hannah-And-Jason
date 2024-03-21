using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node3D
{
	[Export] private int levelId;
	[Export] Array<Node3D> affectorList;
	[Export] Array<Node3D> affectedList;
    [Export] Array<Area3D> tutorialTriggers;
    [Export] string[] tips;
    [Export] LevelLoader levelLoader;

    Label tutorialTip;

    const string gameScene = "res://Scenes/MainGame.tscn";
    const string demoScene = "res://Scenes/DemoLevel1.tscn";

    public override void _Ready()
    {
        base._Ready();
        tutorialTip = GetNode<Label>("TutorialTips/TutorialTip");
        foreach (Area3D tutorialTrigger in tutorialTriggers)
        {
            tutorialTrigger.BodyExited += OnTutorialTriggerExited;
        }
    }

    private void OnTutorialTriggerEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            for (int i = 0; i < tutorialTriggers.Count; i++)
            {
                if (tutorialTriggers[i].Name.ToString().Equals(i.ToString()))
                {
                    tutorialTip.Text = tips[i];
                    GD.Print(tutorialTriggers[i].Name);
                }
            }
        }
    }

    void On0BodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            tutorialTip.Text = tips[0];
        }
    }

    void On1BodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            tutorialTip.Text = tips[1];
        }
    }

    private void OnTutorialTriggerExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            tutorialTip.Text = "";
            GD.Print("Not insde any tutorial trigger.");
        }
    }

    private void OnKillZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            if (levelId == 0)
            {
                levelLoader.SwitchScene(demoScene);
            }
            else if (levelId == 1)
            {
                levelLoader.SwitchScene(gameScene);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if(Input.IsActionJustPressed("pause"))
        {
            GetTree().Quit(); // Replace this line with appropriate code
        }
    }

    private void OnPhysicalButtonButtonPushed(int affectorId)
	{
		if (levelId == 0)
		{
			switch (affectorId)
			{
				case 0:
					Tween tween = GetTree().CreateTween().SetParallel(true);
					tween.TweenProperty(affectedList[0], "position", new Vector3(affectedList[0].Position.X, affectedList[0].Position.Y, affectedList[0].Position.Z + 3.0f), 2f);
					tween.TweenProperty(affectedList[1], "position", new Vector3(affectedList[1].Position.X, affectedList[1].Position.Y, affectedList[1].Position.Z - 3.0f), 2f);
					break;
				default:
					GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
					break;
			}
		}
        else if (levelId == 1)
        {
            switch (affectorId)
            {
                case 0:
                    Tween tween = GetTree().CreateTween();
                    tween.TweenProperty(affectedList[0], "position", new Vector3(affectedList[0].Position.X, affectedList[0].Position.Y + 3.5f, affectedList[0].Position.Z), 2f);
                    break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }

	private void OnLeverLeverFlipped(int affectorId, bool flippedOn)
	{
        if (levelId == 0)
        {
            switch (affectorId)
            {
                case 1:
                    Tween tween = GetTree().CreateTween().SetParallel(false);
					if (flippedOn)
					{
						tween.TweenProperty(affectedList[2], "position", new Vector3(affectedList[2].Position.X, 7, affectedList[2].Position.Z), 1.0f);
					}
					else
					{
                        tween.TweenProperty(affectedList[2], "position", new Vector3(affectedList[2].Position.X, 3, affectedList[2].Position.Z), 1.0f);
                    }
					break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }

	private void OnLever2LeverFlipped(int affectorId, bool flippedOn)
	{
        if (levelId == 0)
        {
            GD.Print("Source: ", affectorId, "\nFlipped Status: ", flippedOn);
            switch (affectorId)
            {
                case 2:
                    Tween tween = GetTree().CreateTween().SetParallel(false);
                    if (flippedOn)
                    {
                        tween.TweenProperty(affectedList[3], "position", new Vector3(affectedList[3].Position.X, 7, affectedList[3].Position.Z), 1.0f);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[3], "position", new Vector3(affectedList[3].Position.X, 3, affectedList[3].Position.Z), 1.0f);
                    }
                    break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }

    private void OnPressurePlatePlatePushed(int affectorId, bool pushed)
    {
        if(levelId == 0)
        {
            switch(affectorId)
            {
                case 3:
                    Tween tween = GetTree().CreateTween().SetParallel(false);
                    if (pushed)
                    {
                        tween.TweenProperty(affectedList[4], "position", new Vector3(affectedList[4].Position.X, 20, affectedList[4].Position.Z), 3.0f);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[4], "position", new Vector3(affectedList[4].Position.X, 0, affectedList[4].Position.Z), 3.0f);
                    }
                    break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }

    private void OnTimedButtonButtonPushed(int affectorId, bool pushed)
    {
        if (levelId == 0)
        {
            switch (affectorId)
            {
                case 4:
                    Tween tween = GetTree().CreateTween().SetParallel(false);
                    if (pushed)
                    {
                        tween.TweenProperty(affectedList[5], "position", new Vector3(affectedList[5].Position.X, 2.5f, affectedList[5].Position.Z), 0.2f);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[5], "position", new Vector3(affectedList[5].Position.X, 1f, affectedList[5].Position.Z), 0.2f);
                    }
                    break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }

    private void OnPressurePlate2PlatePushed(int affectorId, bool pushed)
    {
        if (levelId == 0)
        {
            switch (affectorId)
            {
                case 5:
                    Tween tween = GetTree().CreateTween().SetParallel(false);
                    if (pushed)
                    {
                        tween.TweenProperty(affectedList[6], "position", new Vector3(affectedList[6].Position.X, 3.2f, affectedList[6].Position.Z), 0.5f);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[6], "position", new Vector3(affectedList[6].Position.X, 1, affectedList[6].Position.Z), 0.5f);
                    }
                    break;
                default:
                    GD.PrintErr("An invalid Affector ID may have been provided. " + affectorId.ToString());
                    break;
            }
        }
    }
}
