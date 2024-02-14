using Godot;
using Godot.Collections;
using System;


public partial class PressurePlate : AnimatableBody3D
{
    [Signal] public delegate void PlatePushedEventHandler(int affectorId, bool pushed);

    [Export] private int affectorId;
    [Export] public PressurePlateConfig plateConfiguration;
    [Export] private AnimationPlayer plateAnim;
    [Export] private MeshInstance3D plateMesh;

    private bool isActive;
    private float weight;
    private Material material;

    public override void _Ready()
    {
        base._Ready();
        plateMesh = GetNode<MeshInstance3D>("PlateBaseMesh/PlateMesh");
        weight = plateConfiguration.GetWeightThreshold();
        material = plateMesh.Mesh.SurfaceGetMaterial(0);
        if (plateConfiguration.GetPlateType() == PressurePlateConfig.PlateType.Light)
        {
            material.Set("albedo_color", Color.Color8(0, 255, 0, 255));
        }
        else if (plateConfiguration.GetPlateType() == PressurePlateConfig.PlateType.Heavy)
        {
            material.Set("albedo_color", Color.Color8(255, 0, 0, 255));
        }
    }

    

    private void OnDetectionZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character") && !isActive)
        {
            switch (plateConfiguration.GetPlateType())
            {
                case PressurePlateConfig.PlateType.Light:
                    //GD.Print(body.Name + " has entered " + Name + "\'s detection zone");
                    plateAnim.Play("PushIn");
                    isActive = true;
                    break;
                case PressurePlateConfig.PlateType.Heavy:
                    float bodyMass = (float)body.Call("GetMass");
                    //GD.Print(body.Name + "\'s Mass = " + bodyMass);
                    if (bodyMass >= weight)
                    {
                        plateAnim.Play("PushIn");
                        isActive = true;
                    }
                    break;
            }
        }
        else if (body is RigidBody3D && isActive)
        {
            RigidBody3D rigidBody3D = (RigidBody3D)body;
            switch (plateConfiguration.GetPlateType())
            {
                case PressurePlateConfig.PlateType.Light:
                    //GD.Print(body.Name + " has entered " + Name + "\'s detection zone");
                    plateAnim.Play("PushIn");
                    isActive = true;
                    break;
                case PressurePlateConfig.PlateType.Heavy:
                    //float bodyMass = (float)body.Call("GetMass");
                    //GD.Print(body.Name + "\'s Mass = " + bodyMass);
                    if (rigidBody3D.Mass >= weight)
                    {
                        GD.Print("The Rigidbody: ", rigidBody3D.Name, " is on the pressure plate.");
                        plateAnim.Play("PushIn");
                        isActive = true;
                    }
                    break;
            }
        }
    }

    private void OnDetectionZoneBodyExited(Node3D body)
    {
        if ((body.IsInGroup("Character") || body is RigidBody3D) && isActive)
        {
            //GD.Print(body.Name + " has exited " + Name + "\'s detection zone");
            plateAnim.Play("PushOut");
            isActive = false;
            if (plateConfiguration.GetPlateType() == PressurePlateConfig.PlateType.Light)
            {
                material.Set("emission", Color.Color8(0, 255, 0));
                material.Set("emission_energy_multiplier", 0f);
            }
            else
            {
                material.Set("albedo_color", Color.Color8(255, 0, 0));
                material.Set("emission_energy_multiplier", 0f);
            }
            EmitSignal(SignalName.PlatePushed, affectorId, isActive);
        }
    }

    private void OnPlateAnimAnimationFinished(StringName animName)
    {
        if(animName.Equals("PushIn"))
        {
            if (plateConfiguration.GetPlateType() == PressurePlateConfig.PlateType.Light)
            {
                material.Set("emission", Color.Color8(0, 255, 0));
                material.Set("emission_energy_multiplier", 1.5f);
            }
            else if (plateConfiguration.GetPlateType() == PressurePlateConfig.PlateType.Heavy)
            {
                material.Set("albedo_color", Color.Color8(255, 0, 0));
                material.Set("emission_energy_multiplier", 5f);
            }
            EmitSignal(SignalName.PlatePushed, affectorId, isActive);
        }
    }
}
