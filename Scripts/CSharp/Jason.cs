using Godot;
using System;

public partial class Jason : Player
{
    [Export] public Hannah hannah;

    public override void _Ready()
    {
        base._Ready();

        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        HandleUserControl((float)delta);

        //HandleInteraction();

        camera.Current = isUserControlled;
    }
}
