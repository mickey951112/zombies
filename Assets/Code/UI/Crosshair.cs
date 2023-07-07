using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    float maxVerticalOffset;

    [SerializeField]
    float minVerticalOffset;

    public void OnCameraMove(float percentVertical)
    {
        var verticalOffset = Mathf.Lerp(maxVerticalOffset, minVerticalOffset, percentVertical);
        transform.localPosition = new Vector3(0, verticalOffset, 0);
    }
}
