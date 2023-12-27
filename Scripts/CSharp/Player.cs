using Godot;
using System;

[Tool]
[GlobalClass]
public partial class Player : CharacterBody3D
{
    [ExportGroup("Player Configuration")]
    [ExportSubgroup("Movement")]    
	[Export] public float gravity = 9.8f;
    [Export] public float moveSpeed;
    [Export] public float sprintSpeed;
    [Export] public float acceleration;
    [Export] public float jumpForce;
	[ExportSubgroup("Controls")]
	[Export] public bool isUserControlled;
	[Export] public bool canControl = true;
	[Export] public bool canJump = true;
    [ExportSubgroup("Camera")]
    [Export] public float sensitivity = 4f;
    [Export] public float lowerClampAngle = -80.0f;
    [Export] public float upperClampAngle = 80.0f;
    [ExportGroup("References")]
	[Export] public Camera3D camera;
    [Export] public Node3D camHolder;

    public Vector3 p_Velocity;
    public float currentSpeed;

    public const string forwardKey = "forward";
    public const string backKey = "back";
    public const string leftKey = "left";
    public const string rightKey = "right";
    public const string sprintKey = "sprint";
    public const string jumpKey = "jump";
    public const string switchKey = "switch";
    public const string pauseKey = "pause";
    public override void _PhysicsProcess(double delta)
	{
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed(pauseKey))
        {
            GetTree().Quit();
        }
    }

    public void MovePlayer(float delta)
    {
        p_Velocity = Velocity;

        Vector3 direction = Input.GetAxis(leftKey, rightKey) * Vector3.Right + Input.GetAxis(backKey, forwardKey) * Vector3.Forward;
        direction = Transform.Basis * direction.Normalized();

        if(direction != Vector3.Zero)
        {
            if(Input.IsActionPressed(sprintKey))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }

        p_Velocity = p_Velocity.Lerp(direction * currentSpeed + p_Velocity.Y * Vector3.Up, acceleration * delta);

        if (IsOnFloor() && Input.IsActionJustPressed(jumpKey))
        {
            if (canJump)
            {
                p_Velocity.Y = jumpForce;
            }
        }
        else if (!IsOnFloor())
        {
            p_Velocity.Y -= gravity * delta;
        }

        Velocity = p_Velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (!isUserControlled)
        {
            return;
        }
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion) @event;
            Rotation = new Vector3(0, Rotation.Y - mouseMotion.Relative.X / 1000 * sensitivity, 0f);
            camHolder.Rotation = new Vector3(Mathf.Clamp(Rotation.X - mouseMotion.Relative.Y / 1000 * sensitivity, Mathf.DegToRad(lowerClampAngle), Mathf.DegToRad(upperClampAngle)), 0f, 0f);
        }
    }
}
