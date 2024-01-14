using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class PressurePlateConfig : Resource
{
    public enum PlateType
    {
        Light,
        Heavy
    }

    private PlateType _plateType;
    [Export]
    private PlateType plateType
    {
        get => _plateType;
        set
        {
            _plateType = value;
            NotifyPropertyListChanged();
        }
    }
    [Export] private float weightThreshold;

    public PlateType GetPlateType()
    {
        return plateType;
    }

    public void SetPlateType(PlateType type)
    {
        plateType = type;
    }

    public float GetWeightThreshold()
    {
        return weightThreshold;
    }

    public void SetWeightThreshold(float newThreashold)
    {
        weightThreshold = newThreashold;
    }
}
