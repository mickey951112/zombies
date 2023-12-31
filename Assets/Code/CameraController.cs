using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform mainCharacter;

    [SerializeField]
    Vector3 distanceFromCharacter;

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
            - mainCharacter.rotation * distanceFromCharacter
            + Vector3.up * defaultCameraHeight;
        var lookAtTarget =
            mainCharacter.position
            + mainCharacter.rotation * new Vector3(characterOffset.x, characterOffset.y, 0);
        transform.LookAt(lookAtTarget);
    }

    public void OnLookVertical(float verticalDirection)
    {
        defaultCameraHeight += verticalDirection * verticalChangeSpeed;
        defaultCameraHeight = Mathf.Clamp(defaultCameraHeight, minCameraHeight, maxCameraHeight);
        crosshair.OnCameraMove(
            (defaultCameraHeight - minCameraHeight) / (maxCameraHeight - minCameraHeight)
        );
    }
}
