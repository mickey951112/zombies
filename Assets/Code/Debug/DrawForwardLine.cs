using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForwardLine : MonoBehaviour
{
    void Update()
    {
        Debug.Log("DrawForwardLine");
        Debug.DrawLine(
            transform.position,
            transform.position + transform.forward * 10000,
            Color.yellow
        );
    }
}
