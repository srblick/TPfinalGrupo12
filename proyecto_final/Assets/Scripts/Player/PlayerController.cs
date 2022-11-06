using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]// para que al poner el script automaticamente ponga un CharacterController
public class PlayerController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform playerCamera;
    [Header ("General")]
    [SerializeField] private float gravityScale = -20f;

    [Header ("Movement")]
    [SerializeField] private float walkSpeed = 4f; 
    [SerializeField] private float runSpeed = 7f;
    
    [Header ("Jump")]
    [SerializeField] private float jumpHeight = 1.9f;

    [Header ("Rotation")]
    public float rotationSensibility;// variable que sirve para manipular la sensibilidad de la mouse

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
        if( characterController.isGrounded){// Condicion para saber si toca el piso
            moveInput = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
            if(Input.GetKey(KeyCode.LeftShift)){// Condicion para saber si toca la tecla Shift y cambiar la velocidad
                moveInput = transform.TransformDirection(moveInput) * runSpeed;
            }else{
                moveInput = transform.TransformDirection(moveInput) * walkSpeed;
            }
            

            if(Input.GetButtonDown("Jump")){
                moveInput.y = Mathf.Sqrt(jumpHeight* -2f * gravityScale);// formula para calcular el salto del jugador 

            }
        }
        moveInput.y += gravityScale * Time.deltaTime; // Se le suma la gravedad en y

        characterController.Move(moveInput*Time.deltaTime); // Se le añade movimiento al character controller
    }

    private void look(){
        // se almacena los moviento del mouse.
        rotationInput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle += rotationInput.y;// se suma la rotacion en y
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle,-70,85);// le limita la rotacion a 85° y -70°

        transform.Rotate(Vector3.up * rotationInput.x);// se rota el persona en el eje x
        playerCamera.localRotation=Quaternion.Euler(-cameraVerticalAngle,0f,0f);// se rota la camara en el eje y
    }
}
