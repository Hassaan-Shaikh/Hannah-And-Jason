using Godot;
using System;

public partial class Hannah : Player
{
    [Export] public Jason jason;

    public override void _Ready()
    {
        base._Ready();

        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;
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

        if (!isUserControlled)
        {
            return;
        }

        camera.Current = isUserControlled;
    }
}
