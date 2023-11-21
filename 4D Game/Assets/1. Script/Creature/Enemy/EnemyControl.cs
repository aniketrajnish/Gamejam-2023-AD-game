using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : ICreatureControl
{
    public Vector3 TurnDirection { get; private set; }
    public Vector3 Direction { get; private set; }

    private Vector3 currentDirection = new Vector3(0, 0, 0);

    //Pathfinding
    private Transform currentTransform;
    private NodeTile currentNode;
    private NodeTile targetNode;
    private GameObject targetObject;
    private PathFinder pathFinder;
    private List<NodeTile> path;

    public EnemyControl(Transform transform)
    {
        currentTransform = transform;
    }

    public void Init()
    {
        targetObject = LevelManager.Instance.GetPlayer();
        pathFinder = new PathFinder();
        path = new List<NodeTile>();

        UpdateCurrentNode();
        FindTargetNode();
    }

    public void ReadInput()
    {
        if(path.Count > 0)
        {
            UpdateCurrentNode();

            Vector3 currentPosition = currentNode.transform.position;
            Vector3 nextNodePosition = path[0].transform.position;
            currentDirection =
                new Vector3(nextNodePosition.x - currentPosition.x, 0, nextNodePosition.z - currentPosition.z).normalized;

            if (Vector3.Distance(currentPosition, nextNodePosition) < 0.00001f)
            {
                path.RemoveAt(0);
                FindTargetNode();
            }
        }
        else
        {
            currentDirection = Vector3.zero;
            FindTargetNode();
        }

        Direction = currentDirection;
        TurnDirection = Direction;
    }

    private void UpdateCurrentNode()
    {
        currentNode = MapManager.Instance.GetNearestNodeTile(currentTransform.position);
    }

    private void FindTargetNode()
    {
        if (targetObject != null)
        {
            targetNode = MapManager.Instance.GetNearestNodeTile(targetObject.transform.position);
            path = pathFinder.FindPath(currentNode, targetNode, true, true);
        }
    }
}
