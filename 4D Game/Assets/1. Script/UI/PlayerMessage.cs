using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    void Start()
    {
        EventCenter.RegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
    }

    private void OnDestroy()
    {
        EventCenter.UnRegisterEvent<OnPlayerAttackMode>(OnPlayerAttackMode);
    }

    private void OnPlayerAttackMode(OnPlayerAttackMode data)
    {
        if(data.TimeLeft > 0)
        {
            messageText.text = "Attack Mode: " + data.TimeLeft;
        }
        else
        {
            messageText.text = "Your are being hunted!";
        }
    }
}
