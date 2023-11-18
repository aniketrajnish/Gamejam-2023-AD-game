using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple object pool.
/// </summary>
public class SimpleObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPool;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private bool canExpand = true;
    [SerializeField] private Transform parent;

    private List<GameObject> pooledObjectList;
    private GameObject poolContainer;

    private void Awake()
    {
        CreatePool();
        FillObjectPool();
    }

    private void CreatePool()
    {
        if (prefabToPool == null)
        {
            return;
        }

        poolContainer = new GameObject("[ObjectPool]" + gameObject.name);

        if (parent != null)
        {
            poolContainer.transform.SetParent(parent);
            poolContainer.transform.localPosition = Vector3.zero;
        }

        pooledObjectList = new List<GameObject>();
    }

    private void FillObjectPool()
    {
        if (prefabToPool == null)
        {
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            AddObjectToPool();
        }
    }

    private GameObject AddObjectToPool()
    {
        if (prefabToPool == null)
        {
            Debug.LogWarning("The pool " + gameObject.name + " doesn't have a object prefab to pool.", gameObject);
            return null;
        }

        GameObject newObject = (GameObject)Instantiate(prefabToPool);
        newObject.SetActive(false);
        newObject.transform.SetParent(poolContainer.transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.name = newObject.name + "(" + pooledObjectList.Count + ")";
        pooledObjectList.Add(newObject);

        return newObject;
    }

    public GameObject GetPooledObject()
    {
        if (pooledObjectList == null)
            return null;

        for (int i = 0; i < pooledObjectList.Count; i++)
        {
            GameObject pooledObject = pooledObjectList[i];
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }

        if (canExpand)
        {
            return AddObjectToPool();
        }

        return null;
    }

    public int GetPooledActiveCount()
    {
        int count = 0;
        foreach (GameObject g in pooledObjectList)
        {
            if (g.activeInHierarchy)
                count++;
        }
        return count;
    }
}
