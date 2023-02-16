using System;

public class FloatEventArgs : EventArgs
{
    public float FloatValue;

    public FloatEventArgs(float floatValue)
    {
        FloatValue = floatValue;
    }
}