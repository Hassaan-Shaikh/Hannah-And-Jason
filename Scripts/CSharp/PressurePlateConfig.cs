using Godot;
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
        return this.plateType;
    }

    public void SetPlateType(PlateType type)
    {
        this.plateType = type;
    }

    public float GetWeightThreshold()
    {
        return this.weightThreshold;
    }

    public void SetWeightThreshold(float newThreashold)
    {
        this.weightThreshold = newThreashold;
    }
}
