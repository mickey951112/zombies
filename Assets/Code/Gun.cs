using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    Crosshair crosshair;

    [SerializeField]
    float bulletDamage;

    [SerializeField]
    float fireRate;

    float lastFireTime;

    [SerializeField]
    AudioClip fireSound;

    public void OnFire()
    {
        if (Time.time - lastFireTime < fireRate)
        {
            return;
        }
        lastFireTime = Time.time;

        var ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        if (Physics.Raycast(ray, out var hit))
        {
            var zombie = hit.collider.GetComponentInParent<Zombie>();
            if (zombie)
            {
                zombie.OnHit(bulletDamage, hit.collider, ray.direction, hit.point);
            }
        }
    }
}
