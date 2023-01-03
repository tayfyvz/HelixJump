using System;
using Managers.Sound;
using Managers.Vibration;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [Header("Sound"), Space]
    [SerializeField] private GameObject soundOnImage;
    [SerializeField] private GameObject soundOffImage;

    [Header("Vibration"), Space] 
    [SerializeField] private GameObject vibrationOnImage;
    [SerializeField] private GameObject vibrationOffImage;

    private void Awake()
    {
        SoundEnabledCheck();
        VibrationEnabledCheck();
    }

    public void SoundButton()
    {
        SoundManager.ControlSound();
    }

    public void VibrationButton()
    {
        VibrationManager.ControlVibration();
    }

    private void SoundEnabledCheck()
    {
        if (SoundManager.IsSoundsEnabled == 1)
        {
            soundOnImage.SetActive(true);
            soundOffImage.SetActive(false);
        }
        else
        {
            soundOffImage.SetActive(true);
            soundOnImage.SetActive(false);
        }
    }

    private void VibrationEnabledCheck()
    {
        if (VibrationManager.IsVibrationEnabled == 1)
        {
            vibrationOnImage.SetActive(true);
            vibrationOffImage.SetActive(false);
        }
        else
        {
            vibrationOffImage.SetActive(true);
            vibrationOnImage.SetActive(false);
        }
    }
}
