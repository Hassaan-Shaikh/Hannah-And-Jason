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
    [Export] public float jumpForce;
    [ExportSubgroup("Lerp Tools")]
    [Export] public float acceleration;
    [Export] public float rotationAcceleration;
    [ExportSubgroup("Controls")]
	[Export] public bool isUserControlled;
	[Export] public bool canControl = true;
	[Export] public bool canJump = true;
    [Export] public bool disableSwitch = false;
    [ExportSubgroup("Camera")]
    [Export] public float sensitivity = 4f;
    [Export] public float clampAngle = 60.0f;
    [ExportGroup("References")]
	[Export] public Node3D camHolder;
    [Export] public SpringArm3D camSpring;
    [Export] public Camera3D camera;
    [Export] public RayCast3D rayCast;
    [Export] public NavigationAgent3D navAgent;
    
    public Vector3 p_Velocity;
    public Vector3 direction;
    public float currentSpeed;

    public const string forwardKey = "forward";
    public const string backKey = "back";
    public const string leftKey = "left";
    public const string rightKey = "right";
    public const string sprintKey = "sprint";
    public const string jumpKey = "jump";
    public const string interactKey = "interact";
    public const string switchKey = "switch";
    public const string pauseKey = "pause";
    public const string repathKey = "repath";

    public override void _Ready()
    {
        base._Ready();

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
	{
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed(switchKey))
        {
            if (!disableSwitch)
            {
                isUserControlled = !isUserControlled;
            }
        }
    }

    public void HandleUserControl(float delta)
    {
        p_Velocity = Velocity;

        if (IsOnFloor() && Input.IsActionJustPressed(jumpKey) && isUserControlled)
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

        if (isUserControlled)
        {
            direction = Input.GetAxis(leftKey, rightKey) * Vector3.Right + Input.GetAxis(backKey, forwardKey) * Vector3.Forward;
            direction = Transform.Basis * direction.Normalized();

            if (direction != Vector3.Zero)
            {
                if (Input.IsActionPressed(forwardKey) && Input.IsActionPressed(sprintKey))
                {
                    currentSpeed = sprintSpeed;
                }
                else
                {
                    currentSpeed = moveSpeed;
                }
            }
        }
        else if (IsOnFloor())
        {
            direction = Vector3.Zero;
        }

        p_Velocity = p_Velocity.Lerp(direction * currentSpeed + p_Velocity.Y * Vector3.Up, acceleration * delta);

        Velocity = p_Velocity;
        MoveAndSlide();
    }

    public void HandleAIControl(float delta)
    {
        if(navAgent.IsNavigationFinished() || isUserControlled)
        {
            navAgent.TargetPosition = GlobalPosition;
            currentSpeed = 0;
            Velocity = Vector3.Zero;
            return; 
        }
        Vector3 nextPos, direction;
        nextPos = navAgent.GetNextPathPosition();
        direction = GlobalPosition.DirectionTo(nextPos);
        TurnToDirection(delta);
        Velocity = direction * currentSpeed;
        MoveAndSlide();
    }

    public void TurnToDirection(float delta)
    {
        Rotation = new Vector3(Rotation.X, Mathf.LerpAngle(Rotation.Y, Mathf.Atan2(-Velocity.X, -Velocity.Z), delta * rotationAcceleration), Rotation.Z);
    }

    public void HandleInteraction()
    {
        if (Input.IsActionJustPressed(interactKey) && isUserControlled)
        {
            if (rayCast.IsColliding())
            {
                GodotObject detected = rayCast.GetCollider();
                switch(detected)
                {
                    case PhysicalButton:
                    case TimedButton:
                        detected.Call("ToggleButton");
                        break;
                    case Lever:
                        detected.Call("FlipLever");
                        break;
                    default:
                        break;
                }
            }
        }
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
            camHolder.Rotation = new Vector3(Mathf.Clamp(camHolder.Rotation.X - mouseMotion.Relative.Y / 1000 * sensitivity, Mathf.DegToRad((clampAngle * -1.0f)), Mathf.DegToRad(clampAngle)), 0, 0);
        }
    }
}
