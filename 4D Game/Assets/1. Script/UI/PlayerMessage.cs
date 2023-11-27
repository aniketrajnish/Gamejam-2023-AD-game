using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI scroeText;

    void Start()
    {
        EventCenter.RegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.RegisterEvent<OnGainScore>(OnGainScore);
        messageText.color = Color.red;
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
        EventCenter.UnRegisterEvent<OnGainScore>(OnGainScore);
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
        scroeText.text = "Score: " + data.Score;
    }
}
