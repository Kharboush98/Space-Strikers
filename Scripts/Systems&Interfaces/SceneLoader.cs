using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public bool inTransition = false;

    private GameObject Loader;
    private Animator Transition;
    public float TransitionTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Loader = gameObject.transform.GetChild(0).gameObject;
        Transition = Loader.GetComponent<Animator>();
    }

    public void Load(string SceneName)
    {
        inTransition = true;
        StartCoroutine(LoadNextScene(SceneName));
        GamePause.ResumeGame();
        //Debug.Log(Time.timeScale);
        //Time.timeScale = 1f;
        //Debug.Log(Time.timeScale);
    }

    IEnumerator LoadNextScene(string SceneName)
    {
        GamePause.PauseGame();
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(SceneName);
    }
}
