using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class AffectorInstance : Resource
{
    private bool _isAffector;
    [Export] private bool isAffector 
    { 
        get => _isAffector;
        set
        {
            _isAffector = value;
            NotifyPropertyListChanged();
        }
    }
    [Export] private Array<Node3D> affectedOnes {  get; set; }

    public void SetIsAffector(bool newVal)
    {
        isAffector = newVal;
    }

    public bool IsAffector()
    {
        return isAffector;
    }

    public Array<Node3D> GetAffectedList()
    {
        return affectedOnes;
    }

    public override void _ValidateProperty(Dictionary property)
    {
        base._ValidateProperty(property);
        if (property["name"].AsStringName().Equals(PropertyName.affectedOnes))
        {
            if (isAffector)
            {
                property["usage"] = (int)PropertyUsageFlags.Editor;
            }
            else
            {
                property["usage"] = (int)PropertyUsageFlags.NoEditor;
            }
        }
    }
}
