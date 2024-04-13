using Godot;
using System;

public partial class TutorialTrigger : Area3D
{
    [Signal]
    public delegate void TriggeredEventHandler(int id);

    [Export] public int triggerId;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += (Node3D body) =>
        {
            if (body.IsInGroup("Character"))
            {
                EmitSignal(SignalName.Triggered, triggerId);
                GD.Print("Entered a tutorial trigger");
            }
        };
    }
}
