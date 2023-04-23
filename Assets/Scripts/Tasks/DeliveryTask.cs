using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : Task
{
    [SerializeField]
    EscortItem item;
    bool itemInTransit = false;
    public void itemCollected(EscortItem item)
    {
        itemInTransit = true;
    }

    public void itemDelivered()
    {
        TaskFinished();
    }
    public void DestinationReached()
    {
        if (itemInTransit)
        {
            GetComponent<Player>().SetCarriedItem(null);
            itemDelivered();

        }
    }
    public override void SetupTask()
    {
        item.gameObject.SetActive(true);
    }
}
