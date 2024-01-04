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

        if (Input.IsActionJustPressed(repathKey) && isUserControlled)
        {
            hannah.navAgent.TargetPosition = GlobalPosition;
            hannah.currentSpeed = moveSpeed;
        }

        if (isUserControlled)
        {
            HandleUserControl((float)delta);
            navAgent.TargetPosition = GlobalPosition;
        }
        else
        {
            HandleAIControl((float)delta);
        }

        HandleInteraction();

        camera.Current = isUserControlled;
    }
}
