using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortItem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food")){
            Debug.Log("Player apanhou a comida.");
        }
    }
}
