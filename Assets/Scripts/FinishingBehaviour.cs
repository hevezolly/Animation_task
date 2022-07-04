using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishingBehaviour : MonoBehaviour
{
    [SerializeField]
    private ScriptableValue<float> searchHeight;

    [SerializeField]
    private ScriptableValue<FinishingTarget> finishTargetSelector;

    [SerializeField]
    private ScriptableValue<bool> isFinishing;

    [SerializeField]
    private float finishTeleportationDistance;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private List<MonoBehaviour> onFinishDisable;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float dashTime;

    public UnityEvent finishingStartedEvent;
    public UnityEvent finishingEndedEvent;

    private FinishingTarget currentTarget;
    private int finishingTriggerId;
    private Vector3 finishingDirection;
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        finishingTriggerId = Animator.StringToHash("Finish");
        wait = new WaitForFixedUpdate();
    }

    private void StartFinishing(FinishingTarget currentTarget)
    {
        if (currentTarget == null || isFinishing.Value)
            return;
        isFinishing.Value = true;
        this.currentTarget = currentTarget;
        animator.SetTrigger(finishingTriggerId);
        foreach (var c in onFinishDisable)
        {
            c.enabled = false;
        }
        var center = new Vector3(transform.position.x, 
            transform.position.y + searchHeight.Value,
            transform.position.z);
        var target = new Vector3(currentTarget.transform.position.x, center.y, currentTarget.transform.position.z);

        var dirToCenter = center - target;
        finishingDirection = dirToCenter.normalized;

        
        rb.MoveRotation(Quaternion.LookRotation(-dirToCenter.normalized, Vector3.up));
        finishingStartedEvent?.Invoke();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void Update()
    {
        if (!isFinishing.Value && finishTargetSelector.Value != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartFinishing(finishTargetSelector.Value);
        }
        if (isFinishing.Value)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void AttackPrepared()
    {
        var center = new Vector3(transform.position.x, 
            transform.position.y + searchHeight.Value,
            transform.position.z);
        var target = new Vector3(currentTarget.transform.position.x, center.y, currentTarget.transform.position.z);
        var dirToCenter = center - target;

        StartCoroutine(DashToTarget(target + dirToCenter.normalized * finishTeleportationDistance - Vector3.up * searchHeight.Value));
    }

    private IEnumerator DashToTarget(Vector3 target)
    {
        var distance = target - rb.position;
        var numOfSteps = Mathf.Max(dashTime / Time.fixedDeltaTime, 1);
        var step = distance / numOfSteps;
        for (var i = 0; i < numOfSteps; i++)
        {
            rb.MovePosition(rb.position + step);
            yield return wait;
        }
    }

    public void EnemyPinned()
    {
        if (!isFinishing.Value)
            return;
        currentTarget.StartFinishing(new HitData(){
            position = attackPoint.position,
            dirrectionToAttacker = finishingDirection,
            attackDirection = attackPoint.forward
        });
    }

    public void EnemyDead()
    {
        if (!isFinishing.Value)
            return;
        currentTarget.EndFinishing(new HitData(){
            position = attackPoint.position,
            dirrectionToAttacker = finishingDirection,
            attackDirection = -attackPoint.forward
        });
    }

    public void FinishingEnded()
    {
        if (!isFinishing.Value)
            return;
        isFinishing.Value = false;
        currentTarget = null;
        finishingEndedEvent?.Invoke();
        foreach (var c in onFinishDisable)
        {
            c.enabled = true;
        }
    }

    private void OnDestroy()
    {
        isFinishing.Value = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (searchHeight == null)
            return;
        Gizmos.color = Color.yellow;
        var target =transform.position + transform.up * searchHeight.Value + transform.forward * finishTeleportationDistance;
        Gizmos.DrawLine(transform.position + Vector3.up * searchHeight.Value, target);
        Gizmos.DrawSphere(target, 0.1f);
    }
}
