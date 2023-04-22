using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject menu;
    public GameObject hud;
    [Header("Time")]
    public TextMeshProUGUI clock;
    public Light sun;
    public float timeRate=180;
    
    [Header("Game")]
    public float energy = 0;
    
    public static GameManager Instance { get; private set; }
    
    float time = 0;
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
        if (Input.GetButton("Cancel"))
        {
            PauseGame();
        }
        TickTime();
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void EndGame()
    {

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
        int day = ((int)time / 24 / 60)+1;
        int hour = (int)time / 60%24;
        int minute = (int)time%60;
        clock.text=parseTime(hour)+":"+parseTime(minute);
    }
    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler(Vector3.right * (time -420)* 0.25f);
    }
    string parseTime(int time)
    {
        return time.ToString("00");
    }

}
