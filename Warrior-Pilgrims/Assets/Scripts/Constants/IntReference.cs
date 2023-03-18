using Sirenix.OdinInspector;
using System;

[Serializable]
public class IntReference
{
    public bool UseConstant = true;
    [ShowIf("UseConstant")] public int ConstantValue;
    [HideIf("UseConstant")] public IntVariable Variable;

    public IntReference()
    { }

    public IntReference(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator int(IntReference reference)
    {
        return reference.Value;
    }
}
