using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    Crosshair crosshair;

    public void OnFire()
    {
        var ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        if (Physics.Raycast(ray, out var hit))
        {
            var zombie = hit.collider.GetComponentInParent<Zombie>();
            if (zombie)
            {
                Destroy(zombie.gameObject);
            }
            Debug.Log($"Hit {hit.collider.gameObject.name}");
        }
    }
}
