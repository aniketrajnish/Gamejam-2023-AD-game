using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple sigleton pattern.
/// </summary>
public class SimpleSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    static bool isDestroyed;

    public static T Instance
    {
        get
        {
            //if the instance is destroyed then return null.
            if (isDestroyed)
            {
                instance = null;
                return null;
            }

            //if the instance is null for some reason, try to find one. 
            //if we can't find one, we create a new one in current scene.
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject newObject = new GameObject();
                    instance = newObject.AddComponent<T>();
                    newObject.transform.name = typeof(T).ToString();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
        Application.quitting += QuitApp;
    }

    private void QuitApp()
    {
        Application.quitting -= QuitApp;
        isDestroyed = true;
    }
}
