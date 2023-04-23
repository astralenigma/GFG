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
    public Slider energyBar;
    public TextMeshProUGUI taskCounter;
    public TextMeshProUGUI taskDoneCounter;
    [Header("Time")]
    public TextMeshProUGUI clock;
    public Light sun;
    public float timeRate=180;
    public int startHour =0;
    public int startMinute =0;
    [Header("Game")]
    public float maxEnergy = 1;
    public float energy;
    [Range(0f, 1f)]
    public float energyDrain = .1f;
    public float tasks;
    public float tasksDone;
    public float timeBetweenTasks = 30;
    public List<Task> possibleTasks;
    public List<Task> activeTasks;
    public static GameManager Instance { get; private set; }
    AsyncOperation asyncLoad;
    Player activePlayer;
    bool loadDone;
    bool paused = false;
    bool gameStarted = false;
    float time = 0;
    private bool gameEnded;

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
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
        TickTime();
        CheckGameOver();
        UpdateText();
        EnergyDrain();
    }

    private void UpdateText()
    {
        //Canvas Update procedure
    }
    private void EnergyDrain()
    {
        energy-=energyDrain;
    }
    IEnumerator CreateTaskRoutine()
    {
        while (!gameEnded)
        {
            CreateTask();
            yield return new WaitForSeconds(timeBetweenTasks);
        }
    }
    void CreateTask()
    {
        if (possibleTasks.Count<=0) { return; }
        int t = Random.Range(0, possibleTasks.Count);
        Instantiate(possibleTasks.ElementAt<Task>(t).gameObject);
        tasks++;
    }
    private void CheckGameOver()
    {
        if (tasks-tasksDone>4)
        {
            GameOver();
        }
        if (time>(startHour*60+startMinute)-24*60) {
            gameEnded = true;
        }
    }
    void GameOver()
    {
        Debug.Log("You Lose");
    }
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
    void PauseGame()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        paused= true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        paused= false;
    }
    void ResetVariables()
    {
        paused = false;
        energy = maxEnergy;
        tasks=0;
        tasksDone = 0;
        time = startHour*60+startMinute;
        gameStarted = false;
        gameEnded= false;
    }
    public void StartGame()
    {
        loadDone = false;
        ResetVariables();
        StartCoroutine(LoadAsyncScene(1));
        menu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine( CreateTaskRoutine());
    }

    IEnumerator LoadAsyncScene(int i)
    {
        asyncLoad =SceneManager.LoadSceneAsync(i,LoadSceneMode.Single);
        
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress>=0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
            loadDone = asyncLoad.isDone;
        }
        loadingScreen.SetActive(false);
        gameStarted = true;
        ResumeGame();
    }
    void EndGame()
    {
        gameEnded = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void TickTime()
    {
        time += Time.deltaTime*timeRate;
        UpdateClock();
        UpdateSun();
    }

    private void UpdateClock()
    {
        int day = ((int)time / 1440)+1;
        int hour = (int)time / 60%24;
        int minute = (int)(time%60)/10;
        clock.text=parseTime(hour)+":"+parseTime(minute*10);
    }
    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler(Vector3.right * (time -420)* 0.25f);
    }
    string parseTime(int time)
    {
        return time.ToString("00");
    }

    internal void SetPlayer(Player player)
    {
        activePlayer= player;
    }

    internal void RemoveTask(Task task)
    {
        activeTasks.Remove(task);
    }

    internal void AddActiveTask(Task task)
    {
        activeTasks.Add(task);
    }
}
