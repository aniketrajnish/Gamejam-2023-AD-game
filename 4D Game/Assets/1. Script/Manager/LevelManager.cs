using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleSingleton<LevelManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private List<GameObject> levelList;
    [SerializeField] private float levelDistance = 40f;

    private Vector3 parentInitPos;
    private int currentLevelIndex = 0;
    private bool isMoving = false;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        parentInitPos = levelParent.transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            var parentPos = levelParent.transform.position;
            var targetOffset = parentInitPos.z + (levelDistance* currentLevelIndex);
            levelParent.transform.position 
                = new Vector3(parentPos.x, parentPos.y, Mathf.Lerp(parentPos.z, targetOffset, 0.1f));

            if (Mathf.Abs(levelParent.transform.position.z - targetOffset) <= 0.1f)
            {
                isMoving = false;
                levelParent.transform.position = new Vector3(parentPos.x, parentPos.y, targetOffset);
            }
        }
    }

    public void ChangeLevel(int index)
    {
        if (index >= levelList.Count) return;

        if (levelList[index] != null)
        {
            currentLevelIndex = index;
            isMoving = true;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
