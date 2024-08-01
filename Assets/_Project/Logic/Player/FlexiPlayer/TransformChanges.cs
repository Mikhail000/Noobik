using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanges : MonoBehaviour
{
    public Vector3 Position { get; }
    //public Vector3 Scale { get; }

    public TransformChanges(Vector3 position /*Vector3 scale*/) 
    {
        Position = position;
        //Scale = scale;
    }
}
