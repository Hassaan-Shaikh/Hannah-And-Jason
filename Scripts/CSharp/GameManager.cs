using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node3D
{
	[Export] private int levelId;
	[Export] Array<Node3D> affectorList;
	[Export] Array<Node3D> affectedList;

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
}
