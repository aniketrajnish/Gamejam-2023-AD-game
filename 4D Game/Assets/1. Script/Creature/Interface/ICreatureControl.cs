using UnityEngine;

public interface ICreatureControl
{
    void ReadInput();
    float Rotation { get; }
    Vector3 Direction { get; }
}
