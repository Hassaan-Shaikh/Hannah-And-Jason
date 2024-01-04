using Godot;
using System;

public partial class Lever : AnimatableBody3D
{
    [Signal] public delegate void LeverFlippedEventHandler(bool flippedOn);

    [Export] private bool flippedOn;
    [Export] private Node3D leverPivot;
    [Export] private AnimationPlayer leverAnim;
    [Export] private bool canInteract;

    public override void _Ready()
    {
        base._Ready();
    }

    public void FlipLever()
    {
        if (canInteract)
        {
            if (!flippedOn)
            {
                leverAnim.Play("FlipOn");
            }
            else
            {
                leverAnim.Play("FlipOff");
            }
            flippedOn = !flippedOn;
        }
    }

    private void OnInteractionZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            GD.Print(body.Name + " has entered " + Name + "\'s interaction zone.");
            canInteract = true;
        }
    }

    private void OnInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
        }
    }

    private void OnLeverAnimAnimationFinished(StringName animName)
    {
        if (animName.Equals("FlipOn") || animName.Equals("FlipOff"))
        {
            EmitSignal(SignalName.LeverFlipped, flippedOn);
            GD.Print("Is " + Name + " flippedOn: " + flippedOn);
        }
    }
}
