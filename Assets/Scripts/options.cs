using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class options : MonoBehaviour
{

    [SerializeField] private AudioMixer AudioMixer;
    public void fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void upgradevolumen(float volumen)
    {
        AudioMixer.SetFloat("Volumen", volumen);
    }

    public void updatequality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
