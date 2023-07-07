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
}
