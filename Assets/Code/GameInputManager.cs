using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    [SerializeField]
    MainCharacterController mainCharacterController;

    [SerializeField]
    CameraController cameraController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        mainCharacterController.OnMove(direction);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        var rotateDirection = direction.x;
        mainCharacterController.OnLookHorizontal(rotateDirection);

        var verticalDirection = direction.y;
        cameraController.OnLookVertical(verticalDirection);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        mainCharacterController.OnFire();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        var isRunning = context.ReadValueAsButton();
        mainCharacterController.OnRun(isRunning);
    }

    public void OnSlow(InputAction.CallbackContext context)
    {
        var isSlow = context.ReadValueAsButton();
        mainCharacterController.OnSlow(isSlow);
    }
}
