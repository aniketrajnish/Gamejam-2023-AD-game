using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Simple persistant sigleton pattern.
/// </summary>
public class PersistantSingleton<T> : MonoBehaviour where T : Component
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
        //If current instance is the first one, make it DontDestroyOnLoad
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            Application.quitting += QuitApp;
        }
        else
        {   
            //If singleton already exists destroy current instance.
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }


    private void QuitApp()
    {
        Application.quitting -= QuitApp;
        isDestroyed = true;
    }
}
