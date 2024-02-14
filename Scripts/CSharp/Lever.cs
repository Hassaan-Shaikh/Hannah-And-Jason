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
    [Export] public Label3D promptLabel;
    [ExportSubgroup("Characters")]
    [Export] public Hannah hannah;
    [Export] public Jason jason;

    public Player currentlyControlledCharacter;

    public const string interactKey = "interact";

    public override void _Ready()
    {
        base._Ready();

        leverPivot = GetNode<Node3D>("LeverBaseMesh/LeverPivot");
        leverAnim = GetNode<AnimationPlayer>("LeverAnim");
        promptLabel = GetNode<Label3D>("PromptLabel");
        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;

        promptLabel.GlobalPosition = GlobalPosition - Vector3.Down * -0.25f;
    }

    public override void _PhysicsProcess(double delta)
    {
        promptLabel.Visible = canInteract;
        if (!canInteract)
        {
            return;
        }

        base._PhysicsProcess(delta);

        Player userControlledCharacter = GetCurrentUserControlledCharacter();
        if (canInteract && currentlyControlledCharacter == userControlledCharacter)
        {
            promptLabel.Visible = true;
            if (Input.IsActionJustPressed(interactKey))
            {
                FlipLever();
            }
        }
        else if (!hannah.isUserControlled || !jason.isUserControlled)
        {
            promptLabel.Visible = false;
        }
    }

    private Player GetCurrentUserControlledCharacter()
    {
        return hannah.isUserControlled ? hannah : jason;
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
            currentlyControlledCharacter = body as Player;
        }
    }

    private void OnInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
            currentlyControlledCharacter = null;
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
