using UnityEngine;

public struct MoveMessage
{
    public Vector3 Delta;

    public MoveMessage(Vector3 delta)
    {
        Delta = delta;
    }
}