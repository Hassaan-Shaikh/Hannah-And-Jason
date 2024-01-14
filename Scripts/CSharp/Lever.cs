using Godot;
using Godot.Collections;
using System;

public partial class Lever : AnimatableBody3D
{
    [Signal] public delegate void LeverFlippedEventHandler(bool flippedOn);

    [Export] private bool isConflicting;
    [Export] public Array<Lever> conflictingLevers;
    [Export] public bool flippedOn;
    [Export] public Node3D leverPivot;
    [Export] public AnimationPlayer leverAnim;
    [Export] public bool canInteract;

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
                if (isConflicting)
                {
                    foreach(Lever conflictLever in conflictingLevers) 
                    {
                        if (conflictLever.flippedOn)
                        {
                            conflictLever.leverAnim.Play("FlipOff");
                            conflictLever.flippedOn = false;
                        }
                    }
                }
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
