using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Rot W Pos W")]
    [SerializeField] float maxAmplitudeWPos = 1.0f;
    [SerializeField] Vector3 maxAmplitudeWRot;
    [SerializeField] Vector3 maxFrequencyWRot;

    Raymarcher raymarcher;

    float randomAmplitudeWPos, randomFrequencyWPos, timeElapsedWPos;
    Vector3 randomAmplitudeWRot, randomFrequencyWRot, timeElapsedWRot;    
    void Start()
    {        
        raymarcher = Camera.main.GetComponent<Raymarcher>(); 
        ResetRandomValues();
        PostEffectsManager.Instance.TransSlideOut();
    }

    void Update()
    {        
        ApplyRandomWRotWPos();
    }
    void ApplyRandomWRotWPos()
    {
        Vector3 periodWRot = 2 * Mathf.PI * new Vector3(1 / randomFrequencyWRot.x, 1 / randomFrequencyWRot.y, 1 / randomFrequencyWRot.z);
        float periodWPos = 2 * Mathf.PI / randomFrequencyWPos;

        timeElapsedWRot += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
        timeElapsedWPos += Time.deltaTime;
        if (timeElapsedWRot.x > periodWRot.x || timeElapsedWRot.y > periodWRot.y || timeElapsedWRot.z > periodWRot.z)
        {
            timeElapsedWRot = Vector3.zero;
            ResetRandomValuesWRot();
        }

        if (timeElapsedWPos > periodWPos)
        {
            timeElapsedWPos = 0;
            ResetRandomValuesWPos();
        }

        raymarcher.wRot = new Vector3(
            randomAmplitudeWRot.x * Mathf.Sin(timeElapsedWRot.x * randomFrequencyWRot.x),
            randomAmplitudeWRot.y * Mathf.Sin(timeElapsedWRot.y * randomFrequencyWRot.y),
            randomAmplitudeWRot.z * Mathf.Sin(timeElapsedWRot.z * randomFrequencyWRot.z)
        );
        raymarcher.wPos = randomAmplitudeWPos * Mathf.Sin(timeElapsedWPos * randomFrequencyWPos);
    }
    private void ResetRandomValuesWPos()
    {
        randomAmplitudeWPos = Random.Range(0, maxAmplitudeWPos);
        randomFrequencyWPos = Mathf.Max(randomFrequencyWRot.x, randomFrequencyWRot.y, randomFrequencyWRot.z);
    }

    private void ResetRandomValuesWRot()
    {
        randomAmplitudeWRot = new Vector3(
            Random.Range(0, maxAmplitudeWRot.x),
            Random.Range(0, maxAmplitudeWRot.y),
            Random.Range(0, maxAmplitudeWRot.z)
        );
        randomFrequencyWRot = new Vector3(
            Random.Range(0, maxFrequencyWRot.x),
            Random.Range(0, maxFrequencyWRot.y),
            Random.Range(0, maxFrequencyWRot.z)
        );
    }

    private void ResetRandomValues()
    {        
        ResetRandomValuesWRot();
        ResetRandomValuesWPos();
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void PlayButton()
    {
        StartCoroutine(PostEffectsManager.Instance.ChangeScene(1));
    }    
    
}
