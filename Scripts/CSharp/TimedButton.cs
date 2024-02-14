using Godot;
using System;

public partial class TimedButton : AnimatableBody3D
{
    [Signal] public delegate void ButtonPushedEventHandler(int affectorId, bool pushed);

    [Export] private int affectorId;
    [Export(PropertyHint.Range, "3, 15")] private float timeLimit;
    [ExportGroup("References")]
    [Export] public AnimationPlayer buttonAnim;
    [Export] public Timer buttonTimer;
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
        buttonTimer = GetNode<Timer>("ButtonTimer");
        promptLabel = GetNode<Label3D>("PromptLabel");
        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;

        promptLabel.GlobalPosition = GlobalPosition - Vector3.Down * -0.25f;
        buttonTimer.WaitTime = timeLimit;
    }

    public override void _PhysicsProcess(double delta)
    {
        promptLabel.Visible = canInteract ? true : false;
        if (!canInteract)
        {
            return;
        }

        base._PhysicsProcess(delta);

        if (canInteract && currentlyControlledCharacter == GetCurrentUserControlledCharacter())
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
