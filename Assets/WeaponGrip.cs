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
    ItemGun itemGun;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Gun gun = GetComponent<Gun>();
        gun.OnShotFired += OnShotFired;
        itemGun = GetComponentInChildren<ItemGun>();
    }

    Quaternion previousIKRotation;

    // void Update()
    // {
    //     rightHand.rotation =
    //         Quaternion.LookRotation((lookAt - rightHand.transform.position).normalized)
    //         * Quaternion.Euler(new Vector3(0, 90, 90));
    // }

    public Vector3 testOffsetRotation;

    void OnAnimatorIK()
    {
        // animator.enabled = false;

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKPosition(
            AvatarIKGoal.RightHand,
            getRecoilPosition(weaponRestingLocation.position)
        );

        float rotationLerpSpeed = 10f * Time.deltaTime;
        previousIKRotation = Quaternion.LookRotation(
            (lookAt - itemGun.transform.position).normalized
        );
        Debug.Log(
            $"previousIKRotation: {previousIKRotation.eulerAngles} vs {rightHand.rotation.eulerAngles} local {rightHand.localRotation.eulerAngles}"
        );
        // * Quaternion.Inverse(transform.rotation);
        // Quaternion.Lerp(
        //     previousIKRotation,
        //     Quaternion.LookRotation((lookAt - itemGun.transform.position).normalized)
        //         * Quaternion.Inverse(transform.rotation),
        //     rotationLerpSpeed
        // );

        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        // rightHand.rotation = previousIKRotation;
        animator.SetIKRotation(
            AvatarIKGoal.RightHand,
            Quaternion.LookRotation((lookAt - rightHand.transform.position).normalized)
                * Quaternion.Euler(new Vector3(0, 90, 90))
                * Quaternion.Euler(new Vector3(-90, 90, 90))
                * Quaternion.Euler(testOffsetRotation)
        // getRecoilRotation(

        // previousIKRotation //* transform.rotation
        // * Quaternion.Euler(weaponRestingRotation)
        // )
        );

        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(lookAt);
    }

    public void OnShotFired()
    {
        recoilProgress = 0;
        returnProgress = 0;
        recoilMaxRotation =
            weaponRestingLocation.rotation
            * Quaternion.Euler(weaponRestingRotation)
            * Quaternion.Euler(0, -30, 0);
        recoilMaxPosition =
            weaponRestingLocation.position
            + weaponRestingLocation.rotation
                * Quaternion.Euler(weaponRestingRotation)
                * maxRecoilPosition;
    }

    float recoilTime = 0.05f;
    float returnTime = 0.2f;
    public Vector3 lookAt;

    private Quaternion getRecoilRotation(Quaternion currentRotation)
    {
        return currentRotation;
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
        return currentPosition;
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
