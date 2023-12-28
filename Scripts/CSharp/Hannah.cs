using Godot;
using System;

public partial class Hannah : Player
{
    [Export] public Jason jason;
    //[Export] public bool isUserControlled;

    public override void _Ready()
    {
        base._Ready();

        jason = GetTree().GetFirstNodeInGroup("Jason") as Jason;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionJustPressed(switchKey))
        {
            isUserControlled = !isUserControlled;
            //jason.isUserControlled = true;
        }

        MovePlayer((float)delta);

        if (!isUserControlled)
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
}
