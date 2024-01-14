using Godot;
using Godot.Collections;
using System;

[Tool]
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
    public PlateType plateType
    {
        get => _plateType;
        set
        {
            _plateType = value;
            NotifyPropertyListChanged();
        }
    }
    [Export] private float weightThreshold;

    public override void _ValidateProperty(Dictionary property)
    {
        base._ValidateProperty(property);
        if (property["name"].AsStringName().Equals(PropertyName.weightThreshold))
        {
            switch (plateType)
            {
                case PlateType.Light:
                    property["usage"] = (int)PropertyUsageFlags.NoEditor;
                    break;
                case PlateType.Heavy:
                    property["usage"] = (int)PropertyUsageFlags.Editor;
                    break;
            }
        }
    }

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
