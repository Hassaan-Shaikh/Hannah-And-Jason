using Godot;
using System;

public partial class Jason : Player
{
    [Export] public Hannah hannah;
    //[Export] public bool isUserControlled;

    public override void _Ready()
    {
        base._Ready();

        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        //if (Input.IsActionJustPressed(switchKey))
        //{
        //    isUserControlled = !isUserControlled;
        //}

        MovePlayer((float)delta);

        if (!isUserControlled || !canControl)
        {
            return;
        }

        camera.Current = isUserControlled;
    }

    private void OnSwitchedCharacter(string name)
    {
        if (name.Equals("Jason"))
        {
            isUserControlled = !isUserControlled;
            //GD.Print(Name + " is user controlled: " + isUserControlled);
            hannah.isUserControlled = true;
        }
    }
}
