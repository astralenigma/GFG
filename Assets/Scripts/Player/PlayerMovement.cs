using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 10;
    public float rotationSpeed = 5;
    public GameObject gfx;
    public LayerMask groundLayer;
    CharacterController controller;
    Animator animator;
    Camera cam;
    float lastRotationY = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        lastRotationY = transform.rotation.y;
        
    }
    public void DebugPlayerMovement()
    {
            Time.timeScale = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 movedir = movementInput();
        
        if (movedir!=Vector3.zero)
        {
            //movedir= cam.transform.right*movedir.x+cam.transform.forward*movedir.z;
            controller.Move(movedir.normalized * speed * Time.deltaTime);
            float anglemove = Vector3.Angle(transform.rotation.eulerAngles, movedir);
            Quaternion toRotation = Quaternion.LookRotation(movedir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //float angle = Vector3.Angle( lastRotationY * Vector3.up, transform.rotation.eulerAngles);
            animator.SetFloat("Turn", movedir.x* anglemove/90);
        }
        else
        {
            animator.SetFloat("Turn", 0);
        }
        animator.SetFloat("Forward", movedir.magnitude);
        Ground();
        //gfx.transform.Rotate(Vector3.up * anglemove);
        //lastRotationY = transform.rotation.eulerAngles.y;

    }

    void Ground()
    {
        Ray ray = new Ray(transform.position+Vector3.up*0.1f, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 2);
        if (Physics.Raycast(ray,out RaycastHit hit, 100f, groundLayer))
        {
            transform.position = hit.point;
        }
    }
    Vector3 movementInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")); ;
    }
}
