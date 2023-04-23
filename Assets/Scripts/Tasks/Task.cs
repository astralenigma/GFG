using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField]
    protected string goal;
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.Instance.AddActiveTask(this);
    }

    protected void TaskFinished()
    {
        GameManager.Instance.RemoveTask(this);
        gameObject.SetActive(false);
    }

    public string TaskGoal()
    {
        return goal;
    }
    void Update()
    {
        
    }
}
