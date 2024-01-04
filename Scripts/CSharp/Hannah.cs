using Godot;
using System;

public partial class Hannah : Player
{
    [Export] public Jason jason;

    public override void _Ready()
    {
        base._Ready();

        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed(repathKey) && isUserControlled)
        {
            jason.navAgent.TargetPosition = GlobalPosition;
            jason.currentSpeed = moveSpeed;
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
