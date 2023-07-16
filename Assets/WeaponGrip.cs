using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrip : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    Transform rightHand;

    [SerializeField]
    Transform rightFinger;

    [SerializeField]
    Transform weaponRestingLocation;

    float recoilProgress = 100f;
    float returnProgress = 100f;

    [SerializeField]
    Vector3 recoilMaxRotation;

    [SerializeField]
    Vector3 recoilMaxPosition;

    [SerializeField]
    Vector3 maxRecoilPosition;
    ItemGun itemGun;

    [SerializeField]
    Vector3 aimOffsetRotation;

    Quaternion cachedRightFingerRotation;

    float recoilTime = 0.05f;
    float returnTime = 0.2f;
    public Vector3 lookAt;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Gun gun = GetComponent<Gun>();
        gun.OnShotFired += OnShotFired;
        itemGun = GetComponentInChildren<ItemGun>();
    }

    void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        var targetPosition =
            weaponRestingLocation.position + transform.rotation * getRecoilPosition();
        animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);

        float rotationLerpSpeed = 10f * Time.deltaTime;
        var targetRotation = Quaternion.LookRotation((lookAt - targetPosition).normalized);

        animator.SetBoneLocalRotation(
            HumanBodyBones.RightIndexProximal,
            Quaternion.Inverse(cachedRightFingerRotation)
                * targetRotation
                * getRecoilRotation()
                * Quaternion.Euler(aimOffsetRotation)
        );

        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(lookAt);
    }

    void LateUpdate()
    {
        cachedRightFingerRotation = rightHand.rotation;
    }

    public void OnShotFired()
    {
        recoilProgress = 0;
        returnProgress = 0;
    }

    private Quaternion getRecoilRotation()
    {
        recoilProgress += Time.deltaTime;

        var recoilSpeed = recoilTime / 2;
        if (recoilProgress < recoilTime)
        {
            return Quaternion.Lerp(
                Quaternion.identity,
                Quaternion.Euler(recoilMaxRotation),
                recoilProgress / recoilSpeed
            );
        }

        returnProgress += Time.deltaTime;

        var returnSpeed = returnTime / 2;
        if (returnProgress < returnTime)
        {
            return Quaternion.Lerp(
                Quaternion.Euler(recoilMaxRotation),
                Quaternion.identity,
                returnProgress / returnSpeed
            );
        }
        return Quaternion.identity;
    }

    private Vector3 getRecoilPosition()
    {
        if (recoilProgress < recoilTime)
        {
            return Vector3.Lerp(Vector3.zero, recoilMaxPosition, recoilProgress / recoilTime);
        }

        if (returnProgress < returnTime)
        {
            return Vector3.Lerp(recoilMaxPosition, Vector3.zero, returnProgress / returnTime);
        }

        return Vector3.zero;
    }
}
