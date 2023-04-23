using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField]
    protected string goal;
    public bool active=false;
    protected GoalNotification goalNotification;
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.Instance.possibleTasks.Add(this);
    }
    public abstract void SetupTask();
    public void activateTask()
    {
        GameManager.Instance.AddActiveTask(this);
        SetupTask();
    }
    public void TaskFinished()
    {
        Destroy(goalNotification.gameObject);
        GameManager.Instance.RemoveTask(this);
        //gameObject.SetActive(false);
    }
    public void SetupGoalNotification(GoalNotification goal)
    {
        goalNotification = goal;
        goalNotification.name = gameObject.name;
        goalNotification.description.text = this.goal;
    }
    public string TaskGoal()
    {
        return goal;
    }
}
