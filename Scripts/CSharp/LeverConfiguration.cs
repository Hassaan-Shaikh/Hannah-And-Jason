using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class LeverConfiguration : Resource
{
    [Export] private bool isConflicting;
    [Export] public Array<Lever> conflictingLevers;

    public bool IsConflicting()
    {
        return isConflicting;
    }
}
