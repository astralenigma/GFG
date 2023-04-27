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
            movedir = InputCorrectionByCamera(movedir);
            controller.Move(movedir.normalized * speed * Time.deltaTime);
            float anglemove = HandleRotation(movedir);
            //float angle = Vector3.Angle( lastRotationY * Vector3.up, transform.rotation.eulerAngles);
            animator.SetFloat("Turn", movedir.x * anglemove / 90);
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
    /// <summary>
    /// Method to handle player rotation based on player movement.
    /// </summary>
    /// <param name="movement">Player movement this update.</param>
    /// <returns>Returns new Y angle for the player.</returns>
    private float HandleRotation(Vector3 movement)
    {
        float anglemove = Vector3.Angle(transform.rotation.eulerAngles, movement);
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        return anglemove;
    }
    /// <summary>
    /// Method that corrects input based on the camera position.
    /// </summary>
    /// <param name="inputMoveDir">Vector3 representing the input movement.</param>
    /// <returns>Corrected Vector3 based on camera position.</returns>
    private Vector3 InputCorrectionByCamera(Vector3 inputMoveDir)
    {
        Vector3 right = cam.transform.right;
        Vector3 forward = cam.transform.forward;
        right.y = forward.y = 0;
        right.Normalize();
        forward.Normalize();
        inputMoveDir = right * inputMoveDir.x + forward * inputMoveDir.z;
        return inputMoveDir;
    }
    /// <summary>
    /// Procedure to ground the player.
    /// </summary>
    void Ground()
    {
        Ray ray = new Ray(transform.position+Vector3.up*0.1f, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 2);
        if (Physics.Raycast(ray,out RaycastHit hit, 100f, groundLayer))
        {
            transform.position = hit.point;
        }
    }
    /// <summary>
    /// Function to read the input related with movement.
    /// </summary>
    /// <returns></returns>
    Vector3 movementInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")); ;
    }
}
