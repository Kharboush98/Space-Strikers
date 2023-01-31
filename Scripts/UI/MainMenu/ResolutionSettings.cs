using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown ResDropdown;

    private bool FullScreen;
    [SerializeField] private int ResIndex;
    [SerializeField] private Toggle _Toggle;

    private bool Resolution_Set;

    private void Awake()
    {
        _Toggle.onValueChanged.AddListener(SetFullScreen);

        PlayerPrefs.GetInt("FullScreen", BoolToInt(FullScreen));
        
        ResIndex = PlayerPrefs.GetInt("Resolution");

        if (PlayerPrefs.GetInt("ResSet", 0) == 0) Resolution_Set = false;
        else
            Resolution_Set = true;
    }

    void Start()
    {
        ReturnToggle();
        Res();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("FullScreen", BoolToInt(FullScreen));
    }

    public void SetFullScreen(bool isFullScreen)
    {
        FullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    //public void SetRes(int ResolutionIndex)
    //{
    //    Resolution res = resolutions[ResolutionIndex];
    //    Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    //    ResIndex = ResolutionIndex;
    //    PlayerPrefs.SetInt("Resolution", ResIndex);
    //}

    public void SetRes(int ResolutionIndex)
    {
        if(ResDropdown.value == 0)
        {
            ResIndex = 0;
            Screen.SetResolution(1920 , 1080 , Screen.fullScreen);
            Debug.Log("0");
            PlayerPrefs.SetInt("Resolution", ResIndex);
        }
        else if (ResDropdown.value == 1)
        {
            ResIndex = 1;
            Screen.SetResolution(1600, 900, Screen.fullScreen);
            Debug.Log("01");
            PlayerPrefs.SetInt("Resolution", ResIndex);
        }
        else if (ResDropdown.value == 2)
        {
            ResIndex = 2;
            Screen.SetResolution(1360, 768, Screen.fullScreen);
            Debug.Log("02");
            PlayerPrefs.SetInt("Resolution", ResIndex);
        }
        else if (ResDropdown.value == 3)
        {
            ResIndex = 3;
            Screen.SetResolution(1280, 720, Screen.fullScreen);
            Debug.Log("03");
            PlayerPrefs.SetInt("Resolution", ResIndex);
        }
    }

    private int BoolToInt(bool X)
    {
        if (X) return 1;
        else return 0;
    }

    void ReturnToggle()
    {
        if (PlayerPrefs.GetInt("FullScreen", BoolToInt(FullScreen)) == 1)
        {
            _Toggle.isOn = true;
        }
        else
        {
            _Toggle.isOn = false;
        }
    }

    void Res()
    {
        if (!Resolution_Set)
        {
            PlayerPrefs.SetInt("ResSet", 2);
        }

        ResDropdown.value = ResIndex;
    }

}
