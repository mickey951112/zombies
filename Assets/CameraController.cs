using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform mainCharacter;

    [SerializeField]
    float distanceFromCharacter;

    [SerializeField]
    float defaultCameraHeight;

    [SerializeField]
    Vector2 characterOffset;

    [SerializeField]
    float maxCameraHeight;

    [SerializeField]
    float minCameraHeight;

    [SerializeField]
    float verticalChangeSpeed;

    [SerializeField]
    Crosshair crosshair;

    void Update()
    {
        transform.position =
            mainCharacter.position
            - mainCharacter.forward * distanceFromCharacter
            + Vector3.up * defaultCameraHeight;
        var lookAtTarget =
            mainCharacter.position
            + mainCharacter.rotation * new Vector3(characterOffset.x, characterOffset.y, 0);
        transform.LookAt(lookAtTarget);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        var verticalDirection = direction.y;
        defaultCameraHeight += verticalDirection * verticalChangeSpeed;
        defaultCameraHeight = Mathf.Clamp(defaultCameraHeight, minCameraHeight, maxCameraHeight);
        crosshair.OnCameraMove(
            (defaultCameraHeight - minCameraHeight) / (maxCameraHeight - minCameraHeight)
        );
    }
}
