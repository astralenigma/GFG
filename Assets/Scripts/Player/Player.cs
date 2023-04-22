using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{

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
