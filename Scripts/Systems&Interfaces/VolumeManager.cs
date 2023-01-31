using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _sliders;

    [SerializeField] private VolumeController[] VM;

    void Start()
    {
        SetVol();
    }

    void SetVol()
    {
        for(int i = 0; i < _sliders.Length; i++)
        {
            //VM[i] = _sliders[i].GetComponent<VolumeController>();
            VM[i]._slider.value = PlayerPrefs.GetFloat(VM[i].MixerVolumeName, VM[i]._slider.value);
        
        }
    }
}
