using Godot;
using System;

[GlobalClass]
public partial class PhysicalButton : AnimatableBody3D
{
    [Signal] public delegate void ButtonPushedEventHandler(int affectorId);

    [Export] private int affectorId;
    [Export] public AnimationPlayer buttonAnim;
    [Export] public Label3D promptLabel;
    [Export] public Hannah hannah;
    [Export] public Jason jason;

    public bool canInteract;
    public bool isPushed;
    public Player currentlyControlledCharacter;

    public const string interactKey = "interact";

    public override void _Ready()
    {
        base._Ready();

        buttonAnim = GetNode<AnimationPlayer>("ButtonAnim");
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
                ToggleButton();
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

    public void ToggleButton()
    {
        if(canInteract)
        {
            if (!isPushed)
            {
                buttonAnim.Play("Push");
                isPushed = true;
                EmitSignal(SignalName.ButtonPushed, affectorId);
            }
        }
    }

    public void OnInteractionZoneBodyEntered(Node3D body)
    {
        if(body.IsInGroup("Character"))
        {
            currentlyControlledCharacter = body as Player;
            GD.Print(body.Name + " has entered " + Name + "\'s interaction zone.");
            canInteract = true;
        }
    }

    public void OnInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            //GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
            currentlyControlledCharacter = null;
        }
    }
}
