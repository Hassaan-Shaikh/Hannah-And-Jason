using Godot;
using System;

public partial class Jason : Player
{
    [Export] public Hannah hannah;
    //[Export] public bool isUserControlled;

    public override void _Ready()
    {
        base._Ready();

        hannah = GetTree().GetFirstNodeInGroup("Hannah") as Hannah;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed(switchKey))
        {
            isUserControlled = !isUserControlled;
            //hannah.isUserControlled = true;
        }

        MovePlayer((float)delta);

        if (!isUserControlled || !canControl)
        {
            return;
        }

        camera.Current = isUserControlled;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (!isUserControlled)
        {
            return;
        }
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
            Rotation = new Vector3(0, Rotation.Y - mouseMotion.Relative.X / 1000 * sensitivity, 0f);
            camSpring.Rotation = new Vector3(Mathf.Clamp(camSpring.Rotation.X - mouseMotion.Relative.Y / 1000 * sensitivity, Mathf.DegToRad(-80), Mathf.DegToRad(80)), 0, 0);
        }
    }

    private void OnSwitchedCharacter(string name)
    {
        if (name.Equals("Jason"))
        {
            isUserControlled = !isUserControlled;
            //GD.Print(Name + " is user controlled: " + isUserControlled);
            hannah.isUserControlled = true;
        }
    }
}
