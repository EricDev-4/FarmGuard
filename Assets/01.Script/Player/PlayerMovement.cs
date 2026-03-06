using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Transform player;
    Camera mainCam;
    CharacterController characterController;

    Vector2 inputVec;
    Vector2 lookVec;

    float xRotation = 0f; // 상,하
    float yRotation = 0f; // 좌,우
    Vector3 moveDirection;


    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 4f;
    float verticalVelocity = 0f;
    bool isJumpPressed = false;

    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float clampFOV = 80f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Transform>();
        mainCam = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
        Look();
    }

    void ApplyGravity()
    {
        if(characterController.isGrounded)
        {
            verticalVelocity = -1f;

            if(isJumpPressed)
            {
                verticalVelocity = jumpForce;
                isJumpPressed = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            isJumpPressed = false;
        }
    }

    private void Move()
    {
        moveDirection = player.right * inputVec.x + player.forward * inputVec.y;
        moveDirection.y = verticalVelocity;

        characterController.Move(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    private void Look()
    {
        yRotation += lookVec.x * mouseSensitivity * Time.deltaTime;
        xRotation -= lookVec.y * mouseSensitivity * Time.deltaTime;

        xRotation = Math.Clamp(xRotation , -clampFOV, clampFOV);

        player.localRotation = Quaternion.Euler(0, yRotation, 0);
        mainCam.transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
    }


    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookVec = value.Get<Vector2>();
    }
    
    void OnJump(InputValue value)
    {
        if(value.isPressed)
        {
            Debug.Log("jump");
            isJumpPressed = true;
        }
    }
}
