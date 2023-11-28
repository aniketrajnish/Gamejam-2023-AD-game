using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    [SerializeField] private RaymarchRenderer machineShape;
    [SerializeField] private List<RaymarchRenderer> controlledShapes;
    [SerializeField] private GameObject colliderObject;

    [SerializeField] private float currentValue = 0;
    [SerializeField] private float maxValue = 90;
    [SerializeField] private float minValue = 0;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private bool rotateX, rotateY, rotateZ;
    [SerializeField] private bool canInteract = true;

    [SerializeField] private List<Transform> nodeGroup1Transform;
    [SerializeField] private List<Transform> nodeGroup2Transform;
    [SerializeField] private List<NodeTile> nodeGroup1;
    [SerializeField] private List<NodeTile> nodeGroup2;
    

    private void Awake()
    {
        EventCenter.RegisterEvent<OnUsingMachine>(OnUsingMachine);
    }

    private void Start()
    {
        RotateShapes();
        CheckGroupStatus();
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnUsingMachine>(OnUsingMachine);
    }

    private void Update()
    {
        Raymarcher raymarcher = Camera.main.GetComponent<Raymarcher>();
        
        if(raymarcher.wPos != machineShape.posW)
        {
            canInteract = false;
            colliderObject.SetActive(false);
        }
        else
        {
            canInteract = true;
            colliderObject.SetActive(true);
        }
    }

    private void CheckNodeGroup()
    {
        if(nodeGroup1Transform != null)
        {
            if (nodeGroup1 == null || nodeGroup1.Count == 0)
            {
                nodeGroup1 = new List<NodeTile>();
                foreach (Transform t in nodeGroup1Transform)
                {
                    NodeTile node = MapManager.Instance.GetNearestNodeTile(t.position);
                    nodeGroup1.Add(node);
                }
            }
        }
        
        if(nodeGroup2Transform != null)
        {
            if (nodeGroup2 == null || nodeGroup2.Count == 0)
            {
                nodeGroup2 = new List<NodeTile>();
                foreach (Transform t in nodeGroup2Transform)
                {
                    NodeTile node = MapManager.Instance.GetNearestNodeTile(t.position);
                    nodeGroup2.Add(node);
                }
            }
        }  
    }

    private void RotateShapes() 
    { 
        foreach(RaymarchRenderer shape in controlledShapes)
        {
            if (rotateX)
            {
                shape.rotW.x = currentValue;
            }
            if (rotateY)
            {
                shape.rotW.y = currentValue;
            }
            if (rotateZ)
            {
                shape.rotW.z = currentValue;
            }
        }

        if (rotateX)
        {
            machineShape.rotW.x = currentValue;
        }
        if (rotateY)
        {
            machineShape.rotW.y = currentValue;
        }
        if (rotateZ)
        {
            machineShape.rotW.z = currentValue;
        }
    }

    private void CheckGroupStatus()
    {
        if (currentValue >= maxValue)
        {
            if (nodeGroup1 != null && nodeGroup1.Count > 0)
            {
                foreach (NodeTile node in nodeGroup1)
                {
                    node.IsBlocked = false;
                }
            }

            if (nodeGroup2 != null && nodeGroup2.Count > 0)
            {
                foreach (NodeTile node in nodeGroup2)
                {
                    node.IsBlocked = true;
                }
            }
        }
        else if(currentValue <= minValue)
        {
            if (nodeGroup1 != null && nodeGroup1.Count > 0)
            {
                foreach (NodeTile node in nodeGroup1)
                {
                    node.IsBlocked = true;
                }
            }

            if (nodeGroup2 != null && nodeGroup2.Count>0)
            {
                foreach (NodeTile node in nodeGroup2)
                {
                    node.IsBlocked = false;
                }
            }
        }
    }

    private void OnUsingMachine(OnUsingMachine data)
    {
        if (!canInteract)
            return;

        currentValue += data.amount * rotateSpeed;

        if(currentValue >= maxValue)
        {
            currentValue = maxValue;
        }
        else if(currentValue <= minValue)
        {
            currentValue = minValue;
        }
        CheckNodeGroup();
        CheckGroupStatus();
        RotateShapes();
    }
}
