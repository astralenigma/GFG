using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 10;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movedir = movementInput();
        controller.Move(movedir*speed*Time.deltaTime);
        
    }

    Vector3 movementInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")); ;
    }
}
