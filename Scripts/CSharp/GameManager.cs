using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node3D
{
	[Export] private int levelId;
	[Export] Array<Node3D> affectorList;
	[Export] Array<Node3D> affectedList;
    [Export] Array<Area3D> tutorialTriggers;
    [Export] Array<Area3D> checkpointMarkers;
    [Export] string[] tips = {
        "Use [W][A][S][D] keys to move.",
        "Use the mouse to look around and [SHIFT] to run.",
        "Walk up to the button and press [E] when prompted to press it.",
        "You can press [R] to restart at a saved checkpoint.",
        "Timed Buttons will deactivate after a while.",
        "Press [TAB] to switch between Hannah and Jason.",
        "Hannah can get through small spaces and Jason can reach high places.",
        "Pressure plates can be activated when something is on them.",
        "Heavy pressure plates, however, can only be activated when something heavy is on it."
    };
    [Export] LevelLoader levelLoader;

    Label tutorialTip;
    AnimationPlayer checkpointAnim;
    Hannah hannah;
    Jason jason;

    private bool isGamePaused = false;
    private Vector3 currentCheckpoint;

    const string gameScene = "res://Scenes/MainGame.tscn";
    const string demoScene = "res://Scenes/DemoLevel1.tscn";
    const string pauseKey = "pause";
    const string restartKey = "restart";

    public override void _Ready()
    {
        base._Ready();
        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;
        tutorialTip = GetNode<Label>("GameUI/TutorialTip");
        checkpointAnim = GetNode<AnimationPlayer>("GameUI/CheckpointAnim");
        foreach (Area3D tutorialTrigger in tutorialTriggers)
        {
            tutorialTrigger.BodyExited += (Node3D body) => 
            {
                if (body.IsInGroup("Character"))
                {
                    tutorialTip.Text = "";
                    GD.Print("Not insde any tutorial trigger.");
                }
            };
        }
        for (int i = 0; i < GetTree().GetNodesInGroup("Checkpoints").Count; i++)
        {
            checkpointMarkers[i] = GetTree().GetNodesInGroup("Checkpoints")[i] as Area3D;
        }
        if (Globals.markedCheckpoint == Vector3.Zero)
        {
            Globals.hannahCheckpoint = hannah.GlobalPosition;
            Globals.jasonCheckpoint = jason.GlobalPosition;
        }
        hannah.GlobalPosition = Globals.hannahCheckpoint;
        jason.GlobalPosition = Globals.jasonCheckpoint;
        //Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed(pauseKey))
        {
            isGamePaused = !isGamePaused;
            GetTree().Paused = isGamePaused;
            //PauseGame();
        }

        if (Input.IsActionJustPressed(restartKey))
        {
            levelLoader.SwitchScene(gameScene);
            tutorialTip = GetNode<Label>("TutorialTips/TutorialTip");
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

        void On2BodyEntered(Node3D body)
        {
            if (body.IsInGroup("Character"))
            {
                tutorialTip.Text = tips[2];
            }
        }

        void On3BodyEntered(Node3D body)
        {
            if (body.IsInGroup("Character"))
            {
                tutorialTip.Text = tips[3];
            }
        }

        void On4BodyEntered(Node3D body)
        {
            if (body.IsInGroup("Character"))
            {
                tutorialTip.Text = tips[4];
            }
        }

        void On5BodyEntered(Node3D body)
        {
            if (body.IsInGroup("Character"))
            {
                tutorialTip.Text = tips[5];
            }
        }

        void On6BodyEntered(Node3D body)
        {
            if (body.IsInGroup("Character"))
            {
                tutorialTip.Text = tips[6];
            }
        }

    void On7BodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            tutorialTip.Text = tips[7];
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
        else if (levelId == 1)
        {
            Tween tween = GetTree().CreateTween();
            switch (affectorId)
            {
                case 2:
                    if (flippedOn) 
                    {
                        tween.TweenProperty(affectedList[2], "position", new Vector3(affectedList[2].GlobalPosition.X - 8, affectedList[2].GlobalPosition.Y, affectedList[2].GlobalPosition.Z), 1);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[2], "position", new Vector3(affectedList[2].GlobalPosition.X + 8, affectedList[2].GlobalPosition.Y, affectedList[2].GlobalPosition.Z), 1);
                    }
                break;
                case 3:
                    if (flippedOn)
                        tween.TweenProperty(affectedList[3], "position", new Vector3(affectedList[3].GlobalPosition.X, affectedList[3].GlobalPosition.Y + 3, affectedList[3].GlobalPosition.Z), 0.75);
                    else
                        tween.TweenProperty(affectedList[3], "position", new Vector3(affectedList[3].GlobalPosition.X, affectedList[3].GlobalPosition.Y - 3, affectedList[3].GlobalPosition.Z), 0.75);
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
        else if (levelId == 1)
        {
            switch(affectorId)
            {
                case 4:
                    Tween tween = GetTree().CreateTween();
                    if (pushed)
                        tween.TweenProperty(affectedList[4], "position", new Vector3(affectedList[4].GlobalPosition.X, affectedList[4].GlobalPosition.Y + 3, affectedList[4].GlobalPosition.Z), 0.5);
                    else
                        tween.TweenProperty(affectedList[4], "position", new Vector3(affectedList[4].GlobalPosition.X, affectedList[4].GlobalPosition.Y - 3, affectedList[4].GlobalPosition.Z), 0.5);
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
        else if (levelId == 1)
        {
            switch (affectorId)
            {
                case 1:
                    Tween tween = GetTree().CreateTween();
                    if (pushed)
                    {
                        tween.TweenProperty(affectedList[1], "position", new Vector3(0, affectedList[1].GlobalPosition.Y, affectedList[1].GlobalPosition.Z), 1);
                    }
                    else
                    {
                        tween.TweenProperty(affectedList[1], "position", new Vector3(1.8f, affectedList[1].GlobalPosition.Y, affectedList[1].GlobalPosition.Z), 1);
                    }
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

    void OnCheckpoint0BodyEntered(Player player)
    {
        if (player != null)
        {
            GD.Print("Entered a checkpoint");
            checkpointAnim.Play("CheckpointReached");
            checkpointMarkers[0].GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
            if (levelId == 1) 
            {
                currentCheckpoint = checkpointMarkers[0].GlobalPosition;
                Globals.markedCheckpoint = currentCheckpoint;
                if (!player.disableSwitch)
                {
                    Globals.hannahCheckpoint = currentCheckpoint + Vector3.Right * 1.5f;
                    Globals.jasonCheckpoint = currentCheckpoint + Vector3.Left * 1.5f;
                }
                else
                {
                    switch (player)
                    {
                        case Hannah:
                            Globals.hannahCheckpoint = currentCheckpoint;
                            break;
                        case Jason:
                            Globals.jasonCheckpoint = currentCheckpoint;
                            break;
                    }
                }
            }
        }
    }

    void OnCheckpoint1BodyEntered(Player player)
    {
        if (player != null)
        {
            GD.Print("Entered a checkpoint");
            checkpointAnim.Play("CheckpointReached");
            hannah.disableSwitch = false;
            jason.disableSwitch = false;
            checkpointMarkers[1].GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
            if (levelId == 1)
            {
                currentCheckpoint = checkpointMarkers[1].GlobalPosition;
                Globals.markedCheckpoint = currentCheckpoint;
                if (!player.disableSwitch)
                {
                    Globals.hannahCheckpoint = currentCheckpoint + Vector3.Right * 1.5f;
                    Globals.jasonCheckpoint = currentCheckpoint + Vector3.Left * 1.5f;
                }
                else
                {
                    switch (player)
                    {
                        case Hannah:
                            Globals.hannahCheckpoint = currentCheckpoint;
                            break;
                        case Jason:
                            Globals.jasonCheckpoint = currentCheckpoint;
                            break;
                    }
                }
            }
        }
    }

    void OnDrawerTrapBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(affectedList[1], "position", new Vector3(0, affectedList[1].GlobalPosition.Y, affectedList[1].GlobalPosition.Z), 1);
        }
    }
}
