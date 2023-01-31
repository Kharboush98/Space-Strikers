using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] LevelUnlocked;
    [SerializeField] private GameObject[] LevelLocked;

    private int LevelReached;

    // Start is called before the first frame update
    void Start()
    {
        SetInteractable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInteractable()
    {
        LevelReached = PlayerPrefs.GetInt("LevelReached", 1);

        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (i + 1 <= LevelReached)
            {
                LevelLocked[i].gameObject.SetActive(false);
                LevelUnlocked[i].gameObject.SetActive(true);
                //LevelButtons[i].interactable = false;
                //Color color = ButtonBackground[i].color;
                //color.a = 0.5f;
                //ButtonBackground[i].color = color;

            }
        }
    }

}
