using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupTask : Task
{
    [SerializeField]
    List<CollectItem> items;
    private void Start()
    {
        
    }
    public void itemCollected(CollectItem item)
    {
        items.Remove(item);
        if (items.Count <= 0 )
        {
            TaskFinished();
        }
    }
}
