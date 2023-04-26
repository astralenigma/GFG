using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupTask : Task
{
    [SerializeField]
    List<CollectItem> items;

    /// <summary>
    /// Sets the information that the item(s) have been collected and the task has been 
    /// finished when all have been collected.
    /// </summary>
    /// <param name="item">Item that has been collected.</param>
    public void itemCollected(CollectItem item)
    {
        items.Remove(item);
        if (items.Count <= 0 )
        {
            TaskFinished();
            gameObject.SetActive(false);
        }
    }

    public override void SetupTask()
    {
        foreach (CollectItem item in items)
        {
            item.gameObject.SetActive(true);
        }
    }
}
