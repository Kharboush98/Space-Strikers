using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] public Slider _slider;
    [SerializeField] public string MixerVolumeName;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(SetVolume);
    }

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(MixerVolumeName, _slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MixerVolumeName, _slider.value);
    }

    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat(MixerVolumeName, Volume);
    }
}
