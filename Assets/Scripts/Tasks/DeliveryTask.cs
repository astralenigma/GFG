using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : Task
{
    [SerializeField]
    EscortItem item;
    [SerializeField]
    Transform destiny;
    bool itemInTransit = false;
    public void itemCollected()
    {
        itemInTransit = true;
    }

    public void itemDelivered()
    {
        TaskFinished();
    }
    public void DestinationReached(Player player)
    {
        if (itemInTransit)
        {
            player.RemoveCarriedItem();
            itemDelivered();

        }
        if(destiny)
        {
            item.transform.SetParent(destiny, false);
        }
        else
        {
            item.gameObject.SetActive(false);
        }
        
        //item.transform.position=destiny.position;
    }
    public override void SetupTask()
    {
        item.gameObject.SetActive(true);
    }
}
