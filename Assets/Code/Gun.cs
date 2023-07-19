using System;
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
    AudioSource shootSound;

    WeaponGrip weaponGrip;

    public event Action OnShotFired;

    ItemGun itemGun;

    void Awake()
    {
        weaponGrip = GetComponent<WeaponGrip>();
        itemGun = GetComponentInChildren<ItemGun>();
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red);
        if (Physics.Raycast(ray, out var hit))
        {
            weaponGrip.lookAt = hit.point;
        }
        else
        {
            weaponGrip.lookAt = ray.origin + ray.direction * 1000;
        }
    }

    public void OnFire()
    {
        if (Time.time - lastFireTime < fireRate)
        {
            return;
        }
        lastFireTime = Time.time;

        shootSound.Play();

        var ray = new Ray(itemGun.transform.position, itemGun.transform.forward);
        DebugDraw.xAtPoint(ray.direction, weaponGrip.lookAt, Color.red);

        if (Physics.Raycast(ray, out var hit))
        {
            DebugDraw.xAtPoint(ray.direction, hit.point, Color.blue);

            // Test custom slicer
            var customSlicer = hit.collider.GetComponentInParent<SkinnedMeshSlicer>();
            if (customSlicer)
            {
                customSlicer.Slice(hit.collider.gameObject, hit.point);
            }

            var zombie = hit.collider.GetComponentInParent<Zombie>();
            if (zombie)
            {
                zombie.OnHit(bulletDamage, hit.collider, ray.direction, hit.point);
            }

            // Testing the slicer
            // var slicer = hit.collider.GetComponentInParent<SkeletonMeshSlicer>();
            // Debug.Log(hit.collider.name);
            // if (slicer)
            // {
            //     slicer.SliceByMeshPlane(
            //         Quaternion.Euler(0, 90, 0) * ray.direction,
            //         hit.point,
            //         null
            //     );
            // }
        }

        OnShotFired();
    }
}
