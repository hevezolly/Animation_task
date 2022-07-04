using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RagdollInitiator : MonoBehaviour
{
    [SerializeField]
    private float pushForce;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private List<Rigidbody> bodyParts;

    public UnityEvent ragdollStartedEvent;
    public UnityEvent ragdollEndedEvent;

    private bool isRagdoll = false;

    public void InitiateRagdoll(HitData finishingHit)
    {
        if (isRagdoll)
            return;
        isRagdoll = true;
        Rigidbody closest = null;
        var minDistance = float.MaxValue;
        foreach (var part in bodyParts)
        {
            part.isKinematic = false;
            var distance = Vector3.Distance(finishingHit.position, part.position);
            if (distance < minDistance)
            {
                closest = part;
                minDistance = distance;
            }
        }
        ragdollStartedEvent?.Invoke();
        animator.enabled = false;
        closest.AddForce(-finishingHit.dirrectionToAttacker * pushForce, ForceMode.Impulse);
    }

    public void FinishRagdoll()
    {
        if (!isRagdoll)
            return;
        isRagdoll = false;
        foreach (var part in bodyParts)
        {
            part.isKinematic = true;
        }
        ragdollEndedEvent?.Invoke();
        animator.enabled = true;
    }

    private void Update()
    {
    }
}
