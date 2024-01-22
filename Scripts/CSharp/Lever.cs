using Godot;
using Godot.Collections;
using System;

public partial class Lever : AnimatableBody3D
{
    [Signal] public delegate void LeverFlippedEventHandler(int affectorId, bool flippedOn);

    [Export] private int affectorId;
    [Export] public bool flippedOn;
    [Export] public bool canInteract;
    [ExportGroup("Conflict Handling")]
    [Export] private bool isConflicting;
    [Export] public Array<Lever> conflictingLevers;
    [ExportGroup("References")]
    [Export] public Node3D leverPivot;
    [Export] public AnimationPlayer leverAnim;

    public override void _Ready()
    {
        base._Ready();

        leverPivot = GetNode<Node3D>("LeverBaseMesh/LeverPivot") as Node3D;
        leverAnim = GetNode<AnimationPlayer>("LeverAnim") as AnimationPlayer;
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
            GD.Print("The lever ", affectorId, " has been flipped: ", flippedOn);
        }
    }

    private void OnInteractionZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has entered " + Name + "\'s interaction zone.");
            canInteract = true;
        }
    }

    private void OnInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
        }
    }

    private void OnLeverAnimAnimationFinished(StringName animName)
    {
        if (animName.Equals("FlipOn") || animName.Equals("FlipOff"))
        {
            EmitSignal(SignalName.LeverFlipped, affectorId, flippedOn);
            GD.Print("Is " + Name + " flippedOn: " + flippedOn);
        }
    }
}
