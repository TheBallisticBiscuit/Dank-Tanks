using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string splashSceneName;
    public float splashTime = 3;
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    void Awake()
    {
        // enforce only one object that stays alive thru scene loads, new ones are killed
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }


        DontDestroyOnLoad(this.gameObject);
        isPaused = false;
    }

    // Use this for initialization
    void Start()
    {
        Messenger.AddListener(GameEvent.GAME_PAUSED, PauseGame);
        Messenger.AddListener(GameEvent.GAME_RESUMED, ResumeGame);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_PAUSED, PauseGame);
        Messenger.RemoveListener(GameEvent.GAME_RESUMED, ResumeGame);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ResumeGame();
    }

    public void LoadSceneWithSplashScreen(string scene)
    {
        ResumeGame();
        Debug.Log("Loading Scene " + scene);
        StartCoroutine(ShowSplashScreenWhileLoading(scene));
    }

    public void LoadScene(string scene)
    {
        ResumeGame();
        Debug.Log("Loading Scene " + scene);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting not supported in editor");
    }

    // these are triggered by messages
    private void PauseGame()
    {
        Input.ResetInputAxes();
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Input.ResetInputAxes();
        Time.timeScale = 1f;
        isPaused = false;
    }

    IEnumerator ShowSplashScreenWhileLoading(string resultingScene)
    {
        SceneManager.LoadScene(splashSceneName, LoadSceneMode.Single);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(splashTime);
        SceneManager.LoadScene(resultingScene, LoadSceneMode.Single);
    }
}
