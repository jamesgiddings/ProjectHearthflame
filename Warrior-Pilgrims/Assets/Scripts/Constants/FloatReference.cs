using Sirenix.OdinInspector;
using System;

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    [ShowIf("UseConstant")] public float ConstantValue;
    [HideIf("UseConstant")] public FloatVariable Variable;

    public FloatReference()
    { }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
