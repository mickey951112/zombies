using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float rotationSpeed;

    CharacterController characterController;

    Vector3 movement;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    void Update()
    {
        characterController.Move(transform.rotation * movement * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        movement = new Vector3(direction.x, 0, direction.y) * moveSpeed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        var rotateDirection = direction.x;
        transform.Rotate(0, rotateDirection * rotationSpeed, 0);
    }
}
