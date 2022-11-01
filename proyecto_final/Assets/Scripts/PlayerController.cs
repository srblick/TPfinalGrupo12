using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform playerCamera;
    [Header ("General")]
    [SerializeField] private float gravityScale = -20f;

    [Header ("Movement")]
    [SerializeField] private float walkSpeed = 5f; 
    
    [Header ("Jump")]
    [SerializeField] private float jumpHeight = 1.9f;

    [Header ("Rotation")]
    public float rotationSensibility;

    private float cameraVerticalAngle;
    private Vector3 moveInput =Vector3.zero;
    private Vector3 rotationInput = Vector3.zero;
    private CharacterController characterController;
    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        move();
        look();
    }

    private void move(){
        if( characterController.isGrounded){
            moveInput = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
            moveInput = transform.TransformDirection(moveInput) * walkSpeed;

            if(Input.GetButtonDown("Jump")){
                moveInput.y = Mathf.Sqrt(jumpHeight* -2f * gravityScale);

            }
        }
        moveInput.y += gravityScale * Time.deltaTime;

        characterController.Move(moveInput*Time.deltaTime);
    }

    private void look(){
        rotationInput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle += rotationInput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle,-50,90);

        transform.Rotate(Vector3.up * rotationInput.x);
        playerCamera.localRotation=Quaternion.Euler(-cameraVerticalAngle,0f,0f);
    }
}
