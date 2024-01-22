using Godot;
using System;

public partial class TimedButton : AnimatableBody3D
{
    [Signal] public delegate void ButtonPushedEventHandler(int affectorId, bool pushed);

    [Export] private int affectorId;
    [ExportGroup("References")]
    [Export] public AnimationPlayer buttonAnim;
    [Export] public Timer buttonTimer;

    public bool canInteract;
    public bool isPushed;

    public const string interactKey = "interact";

    public override void _Ready()
    {
        base._Ready();

        buttonAnim = GetNode<AnimationPlayer>("ButtonAnim");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canInteract)
        {
            return;
        }

        base._PhysicsProcess(delta);
    }

    public void ToggleButton()
    {
        if (canInteract)
        {
            if (!isPushed)
            {
                buttonAnim.Play("Push");
                isPushed = true;
                EmitSignal(SignalName.ButtonPushed, affectorId, isPushed);
                buttonTimer.Start();
                //GD.Print("The timed button has been pushed.");
            }
        }
    }

    public void OnTimedInteractionZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has entered " + Name + "\'s interaction zone.");
            canInteract = true;
        }
    }

    public void OnTimedInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
        }
    }

    public void OnButtonTimerTimeout()
    {
        buttonAnim.Play("Unpush");
        isPushed = false;
        EmitSignal(SignalName.ButtonPushed, affectorId, isPushed);
    }
}
