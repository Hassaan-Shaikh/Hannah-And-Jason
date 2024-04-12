using Godot;
using System;

public partial class Hannah : Player
{
    [Export] public Jason jason;

    public override void _Ready()
    {
        base._Ready();

        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;
        isUserControlled = Globals.isHannahControlled;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        HandleUserControl((float)delta);

        //HandleInteraction();

        camera.Current = isUserControlled;
    }
}
