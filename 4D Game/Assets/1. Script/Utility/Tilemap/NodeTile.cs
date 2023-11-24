using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overlay object above every passable tiles, serverd as pathfinding node.
/// </summary>
public class NodeTile : MonoBehaviour
{
    public int G = int.MaxValue; //Cost G is the Distance between start point and this tile.
    public int H = 0; //Cost H is the Distance between end point and this tile.

    public int P = 0; //Extra penalty cost  if needed.
    public int F { get { return G + H + P; } } //Cost F is the sum of G and H and extra penatly cost P.

    public bool IsBlocked = false; 
    public NodeTile Previous;
    public Vector3Int GridLocation;

    public GameObject Occupier { get;  private set; }
    public bool IsOccupied { get; private set; }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public void MarkOccupied(GameObject obj, bool mark)
    {
        if (mark && !Occupier)
        {
            Occupier = obj;
            IsOccupied = true;
        }

        if (!mark && Occupier == obj)
        {
            Occupier = null;
            IsOccupied = false;
        }
       
    }

    public bool CheckIsOccupiedByOther(GameObject obj)
    {
        if(IsOccupied && Occupier != obj)
        {
            return true;
        }
        return false;
    }

}
