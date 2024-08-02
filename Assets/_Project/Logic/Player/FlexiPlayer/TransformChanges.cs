using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanges : MonoBehaviour
{
    public Vector3 Position { get; }

    public TransformChanges(Vector3 position) 
    {
        Position = position;
    }
}
