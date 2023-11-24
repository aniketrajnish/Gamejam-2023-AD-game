using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple health bar.
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] private List<Image> healthIcons;

    public void OnHealthChanged(object sender, HealthChangeEvent e)
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (e.CurrentHealth)
        {
            case 0:
                healthIcons[0].gameObject.SetActive(false);
                healthIcons[1].gameObject.SetActive(false);
                healthIcons[2].gameObject.SetActive(false);
                break;
            case 1:
                healthIcons[0].gameObject.SetActive(true);
                healthIcons[1].gameObject.SetActive(false);
                healthIcons[2].gameObject.SetActive(false);
                break;
            case 2:
                healthIcons[0].gameObject.SetActive(true);
                healthIcons[1].gameObject.SetActive(true);
                healthIcons[2].gameObject.SetActive(false);
                break;
            case 3:
                healthIcons[0].gameObject.SetActive(true);
                healthIcons[1].gameObject.SetActive(true);
                healthIcons[2].gameObject.SetActive(true);
                break;
            default:
                healthIcons[0].gameObject.SetActive(true);
                healthIcons[1].gameObject.SetActive(true);
                healthIcons[2].gameObject.SetActive(true);
                break;
        }
    }
}
