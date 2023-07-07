using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void OnMove(Vector2 direction)
    {
        movement = new Vector3(direction.x, 0, direction.y) * moveSpeed;
    }

    public void OnLookHorizontal(float rotateDirection)
    {
        transform.Rotate(0, rotateDirection * rotationSpeed, 0);
    }
}
