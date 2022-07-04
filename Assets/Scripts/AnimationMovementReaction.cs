using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementReaction : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float stopThreshold;

    [SerializeField]
    private ScriptableValue<float> maxVelocity;

    private int isMovingId;
    private int xDirectionId;
    private int yDirectionId;
    private int velocityId;
    private void Awake()
    {
        xDirectionId = Animator.StringToHash("xMoveDir");
        yDirectionId = Animator.StringToHash("yMoveDir");
        velocityId = Animator.StringToHash("velocity");
        isMovingId = Animator.StringToHash("isMoving");
    }

    private void LateUpdate()
    {
        var speed = rb.velocity.magnitude;
        animator.SetFloat(velocityId, Mathf.Min(speed / maxVelocity.Value, 1));
        animator.SetBool(isMovingId, speed > stopThreshold);
        var velocity = rb.velocity.normalized;
        var y = Vector3.Dot(transform.forward, velocity);
        var x = Vector3.Dot(transform.right, velocity);
        animator.SetFloat(xDirectionId, x);
        animator.SetFloat(yDirectionId, y);
    }
}
