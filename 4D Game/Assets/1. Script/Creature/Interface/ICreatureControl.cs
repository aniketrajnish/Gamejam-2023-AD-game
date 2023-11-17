using UnityEngine;

public interface ICreatureControl
{
    void ReadInput();
    Vector3 TurnDirection { get; }
    Vector3 Direction { get; }
}
