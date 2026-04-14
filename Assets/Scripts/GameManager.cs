using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    // define the different states of the game
    public enum GameState
    {
        GamePlay,
        Paused,
        GameOver,
        LevelUp
    }

    // store the current state of the game
    public GameState currentState;
    // store previous state of game
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Stopwatch")]
    public float timeLimit; // time limit in seconds
    float stopwatchTime; // the current time elapsed since stopwatch started
    public Text stopwatchDisplay;

    // flag to check if game is over
    public bool isGameOver = false;

    // flag to check if player is choosing upgraddes
    public bool choosingUpgrade = false;

    // reference to the player's game object
    public GameObject playerObject;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
        DisableScreens();
    }

    // Update is called once per frame
    void Update()
    {

        // define behavior for each state

        switch(currentState)
        {
            case GameState.GamePlay:
                // code for gameplay state
                CheckForPauseAndResume();
                UpdateStopWatch();
                break;

            case GameState.Paused:
                // code for paused state
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                // code for game over state
                {
                    if(!isGameOver)
                    {
                        isGameOver = true;
                        Time.timeScale = 0f; // stop the game entirely
                        Debug.Log("GAME IS OVER");
                        DisplayResults();
                    }
                }
                break; 

                case GameState.LevelUp:
                    if(!choosingUpgrade)
                    {
                        choosingUpgrade = true;
                        Time.timeScale = 0f; // pause the game for now
                        Debug.Log("Upgrade Screen");
                        levelUpScreen.SetActive(true);
                    }
                break;
            
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
        
    }

    void TestSwitchState()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            currentState++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState--;
        }
    }

    // method to change the state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // stops the game
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // resumes the game
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    // the method for pause and resume input
    void CheckForPauseAndResume()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopWatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopWatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime/60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        // update the stopwatch to display the elapsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // resume the game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.GamePlay);
    }
}
