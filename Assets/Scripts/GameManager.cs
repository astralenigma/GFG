using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject menu;
    public GameObject loadingScreen;
    public GameObject hud;
    public GameObject endGame;
    [Header("HUD")]
    public GoalNotification prefabNotification;
    public VerticalLayoutGroup verticalLayout;
    public Animator phone;
    public Slider energyBar;
    public TextMeshProUGUI taskCounter;
    public TextMeshProUGUI taskDoneCounter;
    [Header("End Game")]
    //public GameObject endGameScreen;
    public TextMeshProUGUI endText;
    public GameObject[] endGameBackground;
    public string[] endGameTextOptions;
    public string generalEndText;
    public static GameManager Instance { get; private set; }
    [Header("Time")]
    public TextMeshProUGUI clock;
    public Light sun;
    public float timeRate = 180;
    public int startHour = 0;
    public int startMinute = 0;

    [Header("Game")]
    public float maxEnergy = 1;
    private float _energy;
    public float Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            if (maxEnergy >= value)
            {
                _energy = value;
                if (_energy < 0)
                {
                    _energy = 0;
                }
            }
        }
    }
    [Range(0f, 1f)]
    public float energyDrain = .1f;

    [Header("Tasks")]
    float tasks;
    float tasksDone;
    public float timeBetweenTasks = 30;
    public List<Task> possibleTasks;
    public List<Task> activeTasks;
    public List<EnergyRestoreLocation> restoreLocations;

    AsyncOperation asyncLoad;
    Player activePlayer;
    bool loadDone;
    bool paused = false;
    bool gameStarted = false;
    float time = 0;
    private bool gameEnded;
    string endGameMessage;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && gameStarted)
        {
            TogglePause();
        }
        if (gameStarted)
        {
            TickTime();
            CheckGameOver();
            UpdateCanvas();
            EnergyDrain();
            energyCanvasUpdate();
        }
    }

    /// <summary>
    /// Updates general game information.
    /// </summary>
    private void UpdateCanvas()
    {

        phone.SetInteger("notif", activeTasks.Count);
        taskDoneCounter.text = tasksDone.ToString("0000");
        //Canvas Update procedure
    }
    #region Energy Functions
    /// <summary>
    /// Updates the energy bar in the UI.
    /// </summary>
    private void energyCanvasUpdate()
    {
        energyBar.value = Energy / maxEnergy;
    }

    /// <summary>
    /// Energy Drain tick game logic.
    /// </summary>
    private void EnergyDrain()
    {
        Energy -= energyDrain * Time.deltaTime;
        if (Energy <= 0)
        {
            Knockout();
        }
    }

    /// <summary>
    /// Complete loss of energy game logic.
    /// </summary>
    private void Knockout()
    {
        activePlayer.GetComponent<CharacterController>().enabled = false;
        activePlayer.transform.position = FindClosestRest();
        time += 60;
        Energy = maxEnergy * 0.5f;
        activePlayer.GetComponent<CharacterController>().enabled = true;
    }

    /// <summary>
    /// Search for closest energy restore position from the player.
    /// </summary>
    /// <returns>Vector3 position of the closest restore position.</returns>
    private Vector3 FindClosestRest()
    {
        Vector3 position = activePlayer.transform.position;
        GameObject[] restLocations = GameObject.FindGameObjectsWithTag("Rest");
        if (restLocations.Length <= 0) { return Vector3.zero; }
        GameObject closestLocation = restLocations[0];
        foreach (GameObject local in restLocations)
        {
            float closest1 = Vector3.Distance(position, closestLocation.transform.position);
            if (closest1 > Vector3.Distance(position, local.transform.position))
            {
                closestLocation = local;
            }
        }
        return closestLocation.transform.position;

    }

    
    /// <summary>
    /// Code to restore energy based on energy drain and a rate.
    /// </summary>
    /// <param name="rate">Recovery rate percentage compared to Energy drain.</param>
    public void RestoreEnergy(float rate)
    {
        Energy += energyDrain * rate * Time.deltaTime;
    }

    #endregion

    #region Task Functions
    /// <summary>
    /// Routine to generate tasks.
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateTaskRoutine()
    {
        while (!gameEnded)
        {
            CreateTask();
            yield return new WaitForSeconds(timeBetweenTasks);
        }
    }
    /// <summary>
    /// Game logic to Create Tasks.
    /// </summary>
    void CreateTask()
    {
        if (possibleTasks.Count <= 0) { return; }
        int t = Random.Range(0, possibleTasks.Count);
        Task activatedTask = possibleTasks.ElementAt<Task>(t);
        activatedTask.activateTask();
        possibleTasks.Remove(activatedTask);
        tasks++;
    }
    /// <summary>
    /// Remove a task from the active list.
    /// </summary>
    /// <param name="task">Task to be removed from the active list.</param>
    internal void RemoveTask(Task task)
    {
        activeTasks.Remove(task);
        tasksDone++;
    }
    /// <summary>
    /// Add a task to the active list.
    /// </summary>
    /// <param name="task">Task to be added to the active list.</param>
    internal void AddActiveTask(Task task)
    {
        task.SetupGoalNotification(Instantiate(prefabNotification, verticalLayout.transform));
        activeTasks.Add(task);
    }
    #endregion

    #region EndGame
    /// <summary>
    /// Checks for gameover conditions then triggers the appropriate one if one was reached.
    /// </summary>
    private void CheckGameOver()
    {
        if (tasks - tasksDone > 4)
        {
            GameOver(0);
        }
        if (time > (startHour * 60 + startMinute) + 1440)
        {
            if (tasks == tasksDone)
            {
                GameOver(2);
                return;
            }
            GameOver(1);
        }
    }

    /// <summary>
    /// Starts the Game Over procedures.
    /// </summary>
    /// <param name="victory">End Game victory/defeat type.</param>
    void GameOver(int victory)
    {
        if (gameEnded)
        {
            return;
        }
        endGameMessage = GenerateEndGameMessage(victory);
        //endGameBackground[victory].SetActive(true);
        endText.text = endGameMessage;
        StartCoroutine(LoadAsyncScene(2 + victory));
        EndGame();
    }

    /// <summary>
    /// Returns an End Game Message.
    /// </summary>
    /// <param name="victory">End Game victory/defeat type.</param>
    /// <returns>End Game text to be shown.</returns>
    private string GenerateEndGameMessage(int victory)
    {
        try
        {
            return endGameTextOptions[victory];
        }
        catch (Exception)
        {

            return generalEndText;
        }
    }
    /// <summary>
    /// Handles the End Game variables.
    /// </summary>
    void EndGame()
    {
        gameEnded = true;
        endGame.SetActive(true);
        hud.SetActive(false);
        menu.SetActive(true);
        endText.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    #endregion
    /// <summary>
    /// Resets all game variables before the game starts.
    /// </summary>
    void ResetVariables()
    {
        paused = false;
        Energy = maxEnergy;
        tasks = 0;
        tasksDone = 0;
        time = startHour * 60 + startMinute;
        gameStarted = false;
        gameEnded = false;
    }
    /// <summary>
    /// Loads the scene through a coroutine.
    /// </summary>
    /// <param name="i">Build Index of the secene to be loaded.</param>
    /// <returns></returns>
    IEnumerator LoadAsyncScene(int i)
    {
        asyncLoad = SceneManager.LoadSceneAsync(i, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
            loadDone = asyncLoad.isDone;
        }
        loadingScreen.SetActive(false);
        //if scene was the first game scene.
        if (i==1)
        {
            gameStarted = true;
            ResumeGame();
        }
    }
    
    #region MenuControls

    /// <summary>
    /// Resumes game time.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        hud.SetActive(true);
        paused = false;
    }
    /// <summary>
    /// Switches paused gamestate.
    /// </summary>
    void TogglePause()
    {
        if (paused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    /// <summary>
    /// Sets the game to paused gamestate.
    /// </summary>
    void PauseGame()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        hud.SetActive(false);
        paused = true;
    }
    /// <summary>
    /// Game Start logic.
    /// </summary>
    public void StartGame()
    {
        loadDone = false;
        ResetVariables();
        StartCoroutine(LoadAsyncScene(1));
        menu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(CreateTaskRoutine());
    }
    /// <summary>
    /// Closes the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Time Functions
    /// <summary>
    /// Updates time and time related game objects.
    /// </summary>
    private void TickTime()
    {
        time += Time.deltaTime * timeRate;
        UpdateClock();
        UpdateSun();
    }
    /// <summary>
    /// Updates the UI clock.
    /// </summary>
    private void UpdateClock()
    {
        int day = ((int)time / 1440) + 1;
        int hour = (int)time / 60 % 24;
        int minute = (int)(time % 60) / 10;
        clock.text = parseTime(hour) + ":" + parseTime(minute * 10);
    }

    /// <summary>
    /// Updates the sun position based on time.
    /// </summary>
    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler(Vector3.right * (time - 420) * 0.25f);
    }

    /// <summary>
    /// Converts time for a clock based string. Makes sure it has zeros where there's no numbers.
    /// </summary>
    /// <param name="time">Variable that should be shown on the clock part.</param>
    /// <returns>String with the number padded by 0.</returns>
    string parseTime(int time)
    {
        return time.ToString("00");
    }

    #endregion
    /// <summary>
    /// Sets the active player.
    /// </summary>
    /// <param name="player">Player component to be set as active Player.</param>
    internal void SetPlayer(Player player)
    {
        activePlayer = player;
    }

}
