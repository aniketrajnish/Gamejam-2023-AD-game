using UnityEngine;

public interface ICreatureControl
{
    void ReadInput();
    Vector3 Rotation { get; }
    Vector3 Direction { get; }
}
