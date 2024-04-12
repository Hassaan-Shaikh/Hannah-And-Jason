using Godot;
using System;
using System.Runtime.CompilerServices;

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
    [Export] public float mass;
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
    [Export] public Node3D propHoldPoint;
    [Export] public SpringArm3D camSpring;
    [Export] public Camera3D camera;
    [Export] public RayCast3D rayCast;
    [Export] public AnimationPlayer animPlayer;
    [Export] public Node3D characterVisuals;
    
    //public Vector3 p_Velocity;
    public Vector3 direction;
    public float currentSpeed;

    public const string forwardKey = "forward";
    public const string backKey = "back";
    public const string leftKey = "left";
    public const string rightKey = "right";
    public const string sprintKey = "sprint";
    public const string jumpKey = "jump";
    public const string interactKey = "interact";
    public const string throwKey = "throw";
    public const string switchKey = "switch";
    public const string pauseKey = "pause";
    public const string walkAnim = "walkAnim";

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

        if (Input.IsActionJustPressed(pauseKey))
        {
            GetTree().Quit();
        }
    }

    public void HandleUserControl(float delta)
    {
        Vector3 p_Velocity = Velocity;        

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
                //if (Input.IsActionPressed(forwardKey) && Input.IsActionPressed(sprintKey))
                //{
                //    currentSpeed = sprintSpeed;
                //}
                //else
                //{
                //    currentSpeed = moveSpeed;
                //}
                currentSpeed = Input.IsActionPressed(sprintKey) ? sprintSpeed : moveSpeed;
            }
        }
        else if (IsOnFloor())
        {
            direction = Vector3.Zero;
        }

        p_Velocity = p_Velocity.Lerp(direction * currentSpeed + p_Velocity.Y * Vector3.Up, acceleration * delta);

        HandleAnimation(p_Velocity);

        Velocity = p_Velocity;

        characterVisuals.LookAt(GlobalPosition - direction);
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
                    case RigidBody3D:
                        RigidBody3D rigidBody3D = (RigidBody3D)detected;
                        rigidBody3D.Reparent(propHoldPoint);
                        rigidBody3D.Freeze = true;
                        //rigidBody3D.SetCollisionMaskValue(1, false);
                        //rigidBody3D.SetCollisionMaskValue(2, false);
                        //rigidBody3D.SetCollisionMaskValue(3, false);
                        //SetCollisionMaskValue(4, false);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public float GetMass()
    {
        return mass;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (isUserControlled)
        {
            if (@event is InputEventMouseMotion)
            {
                InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
                Rotation = new Vector3(0, Rotation.Y - mouseMotion.Relative.X / 1000 * sensitivity, 0f);
                camHolder.Rotation = new Vector3(Mathf.Clamp(camHolder.Rotation.X - mouseMotion.Relative.Y / 1000 * sensitivity, Mathf.DegToRad(clampAngle * -1.0f), Mathf.DegToRad(clampAngle)), 0, 0);
                characterVisuals.RotateY(mouseMotion.Relative.X / 1000 * sensitivity);
            }
        }
    }

    private bool IsWalking()
    {
        return (Input.IsActionPressed(forwardKey) || Input.IsActionPressed(leftKey) || Input.IsActionPressed(rightKey));
    }

    private void HandleAnimation(Vector3 p_Velocity)
    {
        animPlayer.SpeedScale = p_Velocity.Y > 0 ? 0.5f : 1;
        if (isUserControlled)
        {
            //RotationalAdjustment();
            if (Input.IsActionPressed(walkAnim) && Input.IsActionPressed(sprintKey) && IsOnFloor())
            {
                if (animPlayer.CurrentAnimation != "Run")                
                    animPlayer.Play("Run");                
            }
            else if (Input.IsActionPressed(walkAnim) && IsOnFloor())
            {
                if (animPlayer.CurrentAnimation != "Walk")                
                    animPlayer.Play("Walk");                
            }
        }
        if (p_Velocity.Y > 0)
        {
            if (animPlayer.CurrentAnimation != "Jump")            
                animPlayer.Play("Jump");            
        }
        else if (p_Velocity.Y < 0)
        {
            if (animPlayer.CurrentAnimation != "Fall")
                animPlayer.Play("Fall");
        }
        else if (!Input.IsActionPressed(walkAnim) && IsOnFloor())
        {
            if (animPlayer.CurrentAnimation != "Idle")
                animPlayer.Play("Idle");
        }
    }

    private void RotationalAdjustment()
    {
        Vector3 currentRotation = Rotation;
        Tween tween = GetTree().CreateTween().SetParallel(false);
        if (Input.IsActionPressed(forwardKey))
        {
            if (currentRotation.Y >= -90)
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(-180), 0), 0.2);
            }
            else if (currentRotation.Y <= -450)
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(180), 0), 0.2);
            }            
        }
        if (Input.IsActionPressed(leftKey))
        {
            if (currentRotation.Y >= Mathf.DegToRad(-365) && currentRotation.Y <= Mathf.DegToRad(-255))
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(-90), 0), 0.2);
            }
            else
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(-450), 0), 0.2);
            }
        }
        if (Input.IsActionPressed(rightKey))
        {
            tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(-270), 0), 0.2);
        }
        if (Input.IsActionPressed(backKey))
        {
            if (currentRotation.Y >= Mathf.DegToRad(-275) && currentRotation.Y <= Mathf.DegToRad(-265))
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(0), 0), 0.2);
            }
            else
            {
                tween.TweenProperty(characterVisuals, "rotation", new Vector3(0, Mathf.DegToRad(-360), 0), 0.2);
            }
        }
    }
}
