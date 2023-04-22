using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 10;
    public float rotationSpeed = 5;
    public GameObject gfx;
    CharacterController controller;
    Animator animator;
    float lastRotationY = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        lastRotationY = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movedir = movementInput();
        if (movedir!=Vector3.zero)
        {

            controller.Move(movedir * speed * Time.deltaTime);
            float anglemove = Vector3.Angle(transform.rotation.eulerAngles, movedir);
            animator.SetFloat("Forward", movedir.magnitude);

            Quaternion toRotation = Quaternion.LookRotation(movedir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //float angle = Vector3.Angle( lastRotationY * Vector3.up, transform.rotation.eulerAngles);
            animator.SetFloat("Turn", anglemove);
        }
        else
        {
            animator.SetFloat("Turn", 0);
        }

        //gfx.transform.Rotate(Vector3.up * anglemove);
        //lastRotationY = transform.rotation.eulerAngles.y;

    }

    Vector3 movementInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")); ;
    }
}
