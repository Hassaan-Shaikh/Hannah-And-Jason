using Godot;
using System;

[GlobalClass]
public partial class PhysicalButton : AnimatableBody3D
{
    [Signal] public delegate void ButtonPushedEventHandler();

    [Export] public AnimationPlayer buttonAnim;

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

        //if (Input.IsActionJustPressed(interactKey))
        //{
        //    ToggleButton();
        //}
    }

    public void ToggleButton()
    {
        if(canInteract)
        {
            if (!isPushed)
            {
                buttonAnim.Play("Push");
                isPushed = true;
                EmitSignal(SignalName.ButtonPushed);
            }
        }
    }

    public void OnInteractionZoneBodyEntered(Node3D body)
    {
        if(body.IsInGroup("Character"))
        {
            GD.Print(body.Name + " has entered " + Name + "\'s interaction zone.");
            canInteract = true;
        }
    }

    public void OnInteractionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character"))
        {
            GD.Print(body.Name + " has exited " + Name + "\'s interaction zone.");
            canInteract = false;
        }
    }
}
