using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] Image hintImage;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;

    private int currentScore = 0;

    void Start()
    {
        EventCenter.RegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.RegisterEvent<OnGainScore>(OnGainScore);
        EventCenter.RegisterEvent<OnNearMachine>(OnNearMachine);
        EventCenter.RegisterEvent<OnGameStateChange>(OnGameStateChange);
        messageText.color = Color.red;
        currentScore = 0;
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.UnRegisterEvent<OnGainScore>(OnGainScore);
        EventCenter.UnRegisterEvent<OnNearMachine>(OnNearMachine);
        EventCenter.UnRegisterEvent<OnGameStateChange>(OnGameStateChange);
    }

    private void OnPlayerAttackMode(OnPlayerAttackMode data)
    {
        if(data.TimeLeft > 0)
        {
            messageText.text = "Attack Mode: " + data.TimeLeft;
            if(data.TimeLeft <= 3)
            {
                messageText.color = Color.red;
            }
            else
            {
                messageText.color = Color.green;
            }
        }
        else
        {
            messageText.text = "Your are being hunted!";
            messageText.color = Color.red;
        }
    }

    private void OnGainScore(OnGainScore data)
    {
        currentScore += data.Delta;
        scoreText.text = "Score: " + currentScore;
    }

    private void OnNearMachine(OnNearMachine data)
    {
        hintImage.gameObject.SetActive(data.IsNear);
    }

    private void OnGameStateChange(OnGameStateChange data)
    {
        switch (data.State)
        {
            case GameState.End:
                winScreen.SetActive(false);
                gameOverScreen.SetActive(true);
                break;
            case GameState.Win:
                winScreen.SetActive(true);
                gameOverScreen.SetActive(false);
                break;
            default:
                break;
        }
    }
}
