using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float rotationSpeed;

    CharacterController characterController;

    Vector3 movement;

    Gun gun;

    Animator animator;

    enum MovementStyle
    {
        Idle,
        SlowWalk,
        Walk,
        Run,
    }

    bool isRunning;
    bool isSlow;
    float walkDirection;
    float sideStepDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gun = GetComponent<Gun>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(Vector2 direction)
    {
        this.walkDirection = direction.y;
        this.sideStepDirection = direction.x;

        UpdateMovementAnimation();
    }

    public void OnLookHorizontal(float rotateDirection)
    {
        transform.Rotate(0, rotateDirection * rotationSpeed, 0);
    }

    public void OnFire()
    {
        gun.OnFire();
    }

    internal void OnRun(bool isRunning)
    {
        this.isRunning = isRunning;
        UpdateMovementAnimation();
    }

    internal void OnSlow(bool isSlow)
    {
        this.isSlow = isSlow;
        UpdateMovementAnimation();
    }

    void UpdateMovementAnimation()
    {
        var movementStyle =
            walkDirection == 0 && sideStepDirection == 0
                ? MovementStyle.Idle
                : isRunning
                    ? MovementStyle.Run
                    : isSlow
                        ? MovementStyle.SlowWalk
                        : MovementStyle.Walk;

        animator.SetInteger("MovementStyle", (int)movementStyle);

        var isSideStepping = sideStepDirection != 0;
        animator.SetBool("IsSideStepping", isSideStepping);
        if (isSideStepping)
        {
            animator.SetBool("MirrorAnimation", sideStepDirection < 0);
            animator.SetFloat("AnimationSpeed", Mathf.Abs(sideStepDirection));
        }
        else
        {
            animator.SetFloat("AnimationSpeed", walkDirection);
        }
    }
}
