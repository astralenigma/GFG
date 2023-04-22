using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField]
    string goal;
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
