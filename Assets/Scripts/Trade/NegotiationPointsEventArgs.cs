using System;

public class NegotiationPointsEventArgs : EventArgs
{
    public float NegotiationPoints;

    public NegotiationPointsEventArgs(float negotiationPoints)
    {
        NegotiationPoints = negotiationPoints;
    }
}