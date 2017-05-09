using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject levelCompletePanel;
    public GameObject deathPanel;
    public GameObject hud;
    public Text message;

    public RectTransform healthbar;
    private float healthbarWidth;
    private int maxHealth;
    private int currentHealth;

    public Image screenFlash;
    float screenFlashVisibility = 0f;
    bool menuShown;
    

    // Use this for initialization
    void Start ()
    {
        screenFlash.enabled = false;
        menuShown = false;
        pausePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        deathPanel.SetActive(false);

        healthbarWidth = healthbar.rect.width;

        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, ShowLevelCompleteMenu);
        Messenger.AddListener(GameEvent.PLAYER_DEATH, ShowDeathMenu);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, ShowLevelCompleteMenu);
        Messenger.RemoveListener(GameEvent.PLAYER_DEATH, ShowDeathMenu);
    }
    
    // Update is called once per frame
    void Update () {
        if(menuShown)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetButtonDown("Cancel") && pausePanel.activeInHierarchy)
            {
                HideMenu();
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Input.GetButtonDown("Cancel"))
            {
                ShowPauseMenu();
            }
        }
        
    }

    public void UpdateMessage(string msg)
    {
        message.gameObject.SetActive(true);
        message.text = msg;
    }

    public void UpdateHealthbar(int current, int max)
    {
        currentHealth = current;
        maxHealth = max;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthbar.sizeDelta = new Vector2(currentHealth * healthbarWidth / max, healthbar.sizeDelta.y);
    }

    public void Restart()
    {
        FindObjectOfType<GameManager>().LoadSceneWithSplashScreen(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string scene)
    {
        Debug.Log("Loading Scene " + scene);
        SceneManager.LoadScene(scene);
        HideMenu();
    }

    public void LoadSceneWithSplashScreen(string scene)
    {
        FindObjectOfType<GameManager>().LoadSceneWithSplashScreen(scene);
        HideMenu();
    }

    public void HideMenu()
    {
        StopAllCoroutines(); // so timeScale isn't messed with later
        pausePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        menuShown = false;
        hud.SetActive(true);
        Time.timeScale = 1.0f;
        Input.ResetInputAxes();
        Messenger.Broadcast(GameEvent.GAME_RESUMED);
    }

    public void ShowPauseMenu()
    {
        pausePanel.SetActive(true);
        menuShown = true;
        hud.SetActive(false);
        Messenger.Broadcast(GameEvent.GAME_PAUSED);
    }

    public void ShowLevelCompleteMenu()
    {
        levelCompletePanel.SetActive(true);
        menuShown = true;
        hud.SetActive(false);
        Messenger.Broadcast(GameEvent.GAME_PAUSED);
    }

    public void ShowDeathMenu()
    {
        deathPanel.SetActive(true);
        hud.SetActive(false);
        menuShown = true;
        Messenger.Broadcast(GameEvent.GAME_PAUSED);
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting not supported in editor");
    }
    
    public void DamageFlash()
    {
        screenFlashVisibility = 1f;
        screenFlash.enabled = true;
    }
}
