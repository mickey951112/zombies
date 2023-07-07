using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    float impuleForce;

    public void OnHit(float damage, Collider collider, Vector3 bulletDirection, Vector3 hitPoint)
    {
        OnDeath(collider.gameObject, bulletDirection, hitPoint);
    }

    void OnDeath(GameObject hitComponent, Vector3 bulletDirection, Vector3 hitPoint)
    {
        Debug.Log($"Zombie died from {hitComponent.name} shot");

        var animator = GetComponent<Animator>();
        animator.enabled = false;
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var body in bodies)
        {
            body.isKinematic = false;
            if (body.gameObject == hitComponent)
            {
                body.AddForceAtPosition(bulletDirection * impuleForce, hitPoint, ForceMode.Impulse);
            }
        }
    }
}
