using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffects : MonoBehaviour
{
    [SerializeField]
    GameObject bloodParticles;

    public void OnHit(Transform hitComponent, Vector3 position, Vector3 hitDirection)
    {
        Instantiate(bloodParticles, position, Quaternion.LookRotation(hitDirection), hitComponent);
    }
}
