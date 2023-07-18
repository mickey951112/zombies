using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    float timeToLive;

    void Start()
    {
        Destroy(gameObject, timeToLive);
    }
}
