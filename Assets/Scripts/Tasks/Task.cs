using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField]
    protected string goal;
    public bool active=false;
    protected GoalNotification goalNotification;
    bool gameManagerActive=false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (GameManager.Instance)
        {
            gameManagerActive = true;
            GameManager.Instance.possibleTasks.Add(this);
        }
        else
        {
            Debug.LogWarning("Can't set Tasks without a Game Manager.");
            gameManagerActive=false;
        }
        
    }

    /// <summary>
    /// Procedure to setup the task. ex: hide gameobjects.
    /// </summary>
    public abstract void SetupTask();
    public void activateTask()
    {
        if (gameManagerActive)
        {
            GameManager.Instance.AddActiveTask(this);
            SetupTask();
        }
    }

    /// <summary>
    /// Procedure to deal with task being finished.
    /// </summary>
    public void TaskFinished()
    {
        Destroy(goalNotification.gameObject);
        GameManager.Instance.RemoveTask(this);
        //gameObject.SetActive(false);
    }
    /// <summary>
    /// Procedure to setup the GoalNotification to have the proper info and reserve it 
    /// so it can be handled by the task.
    /// </summary>
    /// <param name="goal">GoalNotification generated by the GameManager.</param>
    public void SetupGoalNotification(GoalNotification goal)
    {
        goalNotification = goal;
        goalNotification.name = gameObject.name;
        goalNotification.description.text = this.goal;
    }

    /// <summary>
    /// Function to show the Task Goal information.
    /// </summary>
    /// <returns>Goal text.</returns>
    public string TaskGoal()
    {
        return goal;
    }
}
