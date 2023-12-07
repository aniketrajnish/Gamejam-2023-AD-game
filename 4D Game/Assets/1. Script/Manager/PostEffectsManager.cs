using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostEffectsManager : SimpleSingleton<PostEffectsManager>
{
    [Header("Vignette")]
    [SerializeField] Image vignetteImage;    
    Vector3 originalVignetteScale;

    [Header("FX")]
    [SerializeField] Image bloodImage;
    [SerializeField] ParticleSystem coinPart, enemyDeathPart;

    [Header("Level Image Transition")]
    [SerializeField] GameObject transitionImage;
    [SerializeField] float transitionDuration = 1.0f;

    Vector3 offScreenLeft, offScreenRight, onScreen;

    [Header("Main Menu")]
    bool paused;
    [SerializeField] GameObject pauseMenu;
    private void Start()
    {
        originalVignetteScale = vignetteImage.transform.localScale;

        offScreenLeft = new Vector3(-.925f, 0, -.2f);
        offScreenRight = new Vector3(1.25f, 0, -.2f);
        onScreen = new Vector3(.1f, 0, -.2f);

        TransSlideOut();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.State == GameState.Play)
            TogglePause();
    }
    public void ScaleVignetteUp(float scaleDuration)
    {
        StopAllCoroutines(); 
        StartCoroutine(ScaleVignette(originalVignetteScale, originalVignetteScale, scaleDuration));
    }
    public void ScaleVignetteDown(float scaleDuration)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleVignette(vignetteImage.transform.localScale, originalVignetteScale / 1.25f, scaleDuration));
    }
    IEnumerator ScaleVignette(Vector3 startScale, Vector3 endScale, float scaleDuration)
    {
        float currentTime = 0.0f;

        while (currentTime < scaleDuration)
        {
            float t = currentTime / scaleDuration;
            vignetteImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            currentTime += Time.deltaTime;
            yield return null;
        }

        vignetteImage.transform.localScale = endScale; 
    }
    public void ShowBloodEffect(float duration)
    {
        bloodImage.enabled = true;
        StartCoroutine(HideBloodEffectAfterTime(duration));
    }

    private IEnumerator HideBloodEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        bloodImage.enabled = false;
    }
    public void PlayCoinParticles(Transform target)
    {
        coinPart.transform.position = target.position;
        coinPart.Play();
    }
    public void PlayEnemyDeathParticles(Transform target)
    {
        enemyDeathPart.transform.position = target.position;
        enemyDeathPart.Play();
    }
    public void TransSlideIn()
    {        
        StartCoroutine(SlideImage(offScreenLeft, onScreen));
    }
    public void TransSlideOut()
    {
        StartCoroutine(SlideImage(onScreen, offScreenRight));
    }
    IEnumerator SlideImage(Vector3 startPosition, Vector3 endPosition)
    {
        transitionImage.SetActive(true);
        float time = 0;

        while (time < transitionDuration)
        {
            transitionImage.transform.localPosition = Vector3.Lerp(startPosition, endPosition, time / transitionDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transitionImage.transform.localPosition = endPosition;
        transitionImage.SetActive(false);
    }
    public IEnumerator ChangeScene(int sceneIndex)
    {
        TransSlideIn();
        yield return new WaitForSeconds(transitionDuration - .05f);
        SceneManager.LoadScene(sceneIndex);
    }
    public void TogglePause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);

        if (paused) 
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
    public void ResumeGame()
    {
        TogglePause();
    }
    public void MainMenuTransition()
    {
        Time.timeScale = 1f;
        StartCoroutine(ChangeScene(0));
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
