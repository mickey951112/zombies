using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrip : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    Transform rightHand;

    [SerializeField]
    Transform weaponRestingLocation;

    [SerializeField]
    Vector3 weaponRestingRotation;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        Debug.Log("test");
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, weaponRestingLocation.position);
        animator.SetIKRotation(
            AvatarIKGoal.RightHand,
            weaponRestingLocation.rotation * Quaternion.Euler(weaponRestingRotation)
        );
    }
}
