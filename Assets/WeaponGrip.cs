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

    float recoilProgress = 100f;
    float returnProgress = 100f;

    Quaternion recoilMaxRotation;

    Vector3 recoilMaxPosition;

    [SerializeField]
    Vector3 maxRecoilPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Gun gun = GetComponent<Gun>();
        gun.OnShotFired += OnShotFired;
    }

    void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(
            AvatarIKGoal.RightHand,
            getRecoilPosition(weaponRestingLocation.position)
        );
        animator.SetIKRotation(
            AvatarIKGoal.RightHand,
            getRecoilRotation(
                weaponRestingLocation.rotation * Quaternion.Euler(weaponRestingRotation)
            )
        );
    }

    public void OnShotFired()
    {
        Debug.Log("OnShotFired()");
        recoilProgress = 0;
        returnProgress = 0;
        recoilMaxRotation =
            weaponRestingLocation.rotation
            * Quaternion.Euler(weaponRestingRotation)
            * Quaternion.Euler(0, -30, 0);
        // Right for up?, left is down? forward crossbody left
        recoilMaxPosition =
            weaponRestingLocation.position
            + weaponRestingLocation.rotation
                * Quaternion.Euler(weaponRestingRotation)
                * maxRecoilPosition;
    }

    float recoilTime = 0.05f;
    float returnTime = 0.2f;

    private Quaternion getRecoilRotation(Quaternion currentRotation)
    {
        recoilProgress += Time.deltaTime;

        var recoilSpeed = recoilTime / 2;
        if (recoilProgress < recoilTime)
        {
            return Quaternion.Lerp(
                currentRotation,
                recoilMaxRotation,
                recoilProgress / recoilSpeed
            );
        }

        returnProgress += Time.deltaTime;

        var returnSpeed = returnTime / 2;
        if (returnProgress < returnTime)
        {
            return Quaternion.Lerp(
                recoilMaxRotation,
                currentRotation,
                returnProgress / returnSpeed
            );
        }
        return currentRotation;
    }

    private Vector3 getRecoilPosition(Vector3 currentPosition)
    {
        if (recoilProgress < recoilTime)
        {
            return Vector3.Lerp(currentPosition, recoilMaxPosition, recoilProgress / recoilTime);
        }

        if (returnProgress < returnTime)
        {
            return Vector3.Lerp(recoilMaxPosition, currentPosition, returnProgress / returnTime);
        }

        return currentPosition;
    }
}
