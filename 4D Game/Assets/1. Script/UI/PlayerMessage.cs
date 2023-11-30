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

    void Start()
    {
        EventCenter.RegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.RegisterEvent<OnGainScore>(OnGainScore);
        EventCenter.RegisterEvent<OnNearMachine>(OnNearMachine);
        messageText.color = Color.red;
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.UnRegisterEvent<OnGainScore>(OnGainScore);
        EventCenter.UnRegisterEvent<OnNearMachine>(OnNearMachine);
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
        scoreText.text = "Score: " + data.Score;
    }

    private void OnNearMachine(OnNearMachine data)
    {
        hintImage.gameObject.SetActive(data.IsNear);
    }
}
