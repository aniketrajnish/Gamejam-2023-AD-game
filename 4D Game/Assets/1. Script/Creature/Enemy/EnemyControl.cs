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
    private NodeTile previousNode;
    private NodeTile targetNode;
    private NodeTile preTargetNode;
    private GameObject targetObject;
    private PathFinder pathFinder;
    private List<NodeTile> path;
    private Timer waitTimer;
    private bool isWaiting = false;

    public EnemyControl(Transform transform)
    {
        currentTransform = transform;
    }

    public void Init()
    {
        targetObject = LevelManager.Instance.GetPlayer();
        pathFinder = new PathFinder();
        path = new List<NodeTile>();
        waitTimer = TimerManager.Instance.GetTimer();
        waitTimer.gameObject.SetActive(true);

        EventCenter.RegisterEvent<OnEnemyDeath>(OnEnemyDeath);

        UpdateCurrentNode();
        FindTargetNode();
    }

    public void ReadInput()
    {
        if(path.Count > 0 && !isWaiting)
        {
            UpdateCurrentNode();
            if (path[0].CheckIsOccupiedByOther(currentTransform.gameObject))
            {
                FindTargetNode();
                Wait(0.5f);
                return;
            }

            Vector3 currentPosition = currentTransform.position;
            Vector3 nextNodePosition = path[0].transform.position;

            currentDirection =
                new Vector3(nextNodePosition.x - currentPosition.x, 0, nextNodePosition.z - currentPosition.z).normalized;

            if (Vector3.Distance(currentPosition, nextNodePosition) <= 0.18f)
            {
                path.RemoveAt(0);
                FindTargetNode();
            }
        }
        else
        {
            isWaiting = waitTimer.IsRunning();
            currentDirection = Vector3.zero;
            FindTargetNode();
        }

        Direction = currentDirection;
        TurnDirection = Direction;
    }

    private void UpdateCurrentNode()
    {
        if (currentNode != null)
        {
            previousNode = currentNode;
        }
        currentNode = MapManager.Instance.GetNearestNodeTile(currentTransform.position);
        
        if(currentNode == null)
        {
            return;
        }
        currentNode.MarkOccupied(currentTransform.gameObject, true);

        if (previousNode != null && previousNode != currentNode)
        {
            previousNode.MarkOccupied(currentTransform.gameObject, false);
        }
    }

    private void FindTargetNode()
    {
        if (targetObject != null)
        {
            targetNode = MapManager.Instance.GetNearestNodeTile(targetObject.transform.position);

            if (targetNode != null && targetNode.IsOccupied)
            {
                targetNode = FindSpaceNearTarget(targetNode);
            }

            if (preTargetNode == targetNode || targetNode == null)
            {
                return;
            }
            else
            {
                preTargetNode = targetNode;
            }

            path = pathFinder.FindPath(currentNode, targetNode, true, true);
        }
    }

    private NodeTile FindSpaceNearTarget(NodeTile tile)
    {
        var targetNbs = pathFinder.GetNeightbourNodeTiles(tile, true);
        float distance = int.MaxValue;
        float preDistance = 0;
        NodeTile result = null;

        foreach (var nb in targetNbs)
        {
            if (!nb.IsBlocked && !nb.CheckIsOccupiedByOther(currentTransform.gameObject))
            {
                preDistance = distance;
                distance = Vector3Int.Distance(nb.GridLocation, currentNode.GridLocation);

                if (distance < preDistance)
                {
                    result = nb;
                }
            }
        }
        return result;
    }

    private void Wait(float time)
    {
        if (!isWaiting)
        {
            waitTimer.StartTimer(time);
            isWaiting = true;
        }
    }

    private void OnEnemyDeath(OnEnemyDeath data)
    {
        if(data.DeadEnemy == currentTransform.gameObject)
        {
            previousNode.MarkOccupied(currentTransform.gameObject, false);
            currentNode.MarkOccupied(currentTransform.gameObject, false);
            EventCenter.UnRegisterEvent<OnEnemyDeath>(OnEnemyDeath);
        }
    }
}
