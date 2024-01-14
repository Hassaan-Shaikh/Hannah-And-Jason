using Godot;
using Godot.Collections;
using System;


public partial class PressurePlate : AnimatableBody3D
{
    [Signal] public delegate void PlatePushedEventHandler();

    //public enum PlateType
    //{
    //    Light,
    //    Heavy
    //}

    //private PlateType _plateType;
    //[Export]
    //private PlateType plateType
    //{
    //    get => _plateType;
    //    set
    //    {
    //        _plateType = value;
    //        NotifyPropertyListChanged();
    //    }
    //}
    //[Export] private float weightThreshold;
    [Export] public PressurePlateConfig plateConfiguration; 
    [Export] private AnimationPlayer plateAnim;
    [Export] private MeshInstance3D plateMesh;

    private bool isActive;
    private float weight;

    public override void _Ready()
    {
        base._Ready();
        plateMesh = GetNode<MeshInstance3D>("PlateBaseMesh/PlateMesh");
        Material material = plateMesh.Mesh.SurfaceGetMaterial(0);
        if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Light)
        {
            material.Set("albedo_color", Color.Color8(0, 255, 0, 255));
        }
        else if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Heavy)
        {
            material.Set("albedo_color", Color.Color8(255, 0, 0, 255));
            GD.Print(Name + "\'s Weight Threshold = " + plateConfiguration.GetWeightThreshold());
        }
    }

    

    private void OnDetectionZoneBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Character") && !isActive)
        {
            switch (plateConfiguration.plateType)
            {
                case PressurePlateConfig.PlateType.Light:
                    GD.Print(body.Name + " has entered " + Name + "\'s detection zone");
                    plateAnim.Play("PushIn");
                    isActive = true;
                    break;
                case PressurePlateConfig.PlateType.Heavy:
                    float bodyMass = (float)body.Call("GetMass");
                    GD.Print(body.Name + "\'s Mass = " + bodyMass);
                    if (bodyMass < plateConfiguration.GetWeightThreshold())
                    {
                        return;
                    }
                    plateAnim.Play("PushIn");
                    isActive = true;
                    break;
            }
        }
    }

    private void OnDetectionZoneBodyExited(Node3D body)
    {
        if (body.IsInGroup("Character") && isActive)
        {
            GD.Print(body.Name + " has exited " + Name + "\'s detection zone");
            plateAnim.Play("PushOut");
            isActive = false;
            Material material = plateMesh.Mesh.SurfaceGetMaterial(0);
            if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Light)
            {
                material.Set("emission", Color.Color8(0, 255, 0));
                material.Set("emission_energy_multiplier", 0f);
            }
            else if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Heavy)
            {
                material.Set("albedo_color", Color.Color8(255, 0, 0));
                material.Set("emission_energy_multiplier", 0f);
            }
        }
    }

    private void OnPlateAnimAnimationFinished(StringName animName)
    {
        if(animName.Equals("PushIn"))
        {
            Material material = plateMesh.Mesh.SurfaceGetMaterial(0);
            if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Light)
            {
                material.Set("emission", Color.Color8(0, 255, 0));
                material.Set("emission_energy_multiplier", 1.5f);
            }
            else if (plateConfiguration.plateType == PressurePlateConfig.PlateType.Heavy)
            {
                material.Set("albedo_color", Color.Color8(255, 0, 0));
                material.Set("emission_energy_multiplier", 5f);
            }
        }
    }
}
