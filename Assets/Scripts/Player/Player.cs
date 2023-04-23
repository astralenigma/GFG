using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField]
    Transform itemHold;
    EscortItem carriedItem;
    internal bool SetCarriedItem(EscortItem escortItem)
    {
        if (carriedItem != null)
        {
            return false;
        }
        carriedItem = escortItem;
        if (carriedItem != null ) {
            carriedItem.transform.SetParent(itemHold);
        }
        else
        {
            Destroy(carriedItem.gameObject);
            carriedItem = null;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GetComponent<PlayerMovement>().DebugPlayerMovement();
        }
        else
        {
            GameManager.Instance.SetPlayer(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
