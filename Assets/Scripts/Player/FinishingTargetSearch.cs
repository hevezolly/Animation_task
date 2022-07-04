using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinishingTargetSearch : MonoBehaviour, IComparer<Collider>
{
    [SerializeField]
    private ScriptableValueField<float> searchHeight;

    [SerializeField]
    private float canFinishDistance;

    [SerializeField]
    private LayerMask finishTargetLayerMask;

    [SerializeField]
    private LayerMask obstickleLayerMask;

    [SerializeField]
    private ScriptableValue<FinishingTarget> finishTarget;

    [SerializeField]
    private ScriptableValue<bool> isFinishing;
    private int visibilityCheckLayermask;

    private void Start()
    {
        visibilityCheckLayermask = finishTargetLayerMask.value | obstickleLayerMask;
    }

    private void OnDisable()
    {
        finishTarget.Value = null;
    }

    private void Update()
    {
        FinishingTarget target = null;
        if (!isFinishing.Value)
            target = GetPossibleFinishingTarget();
        if (finishTarget.Value != target)
        {
            finishTarget.Value = target;
        }
    }

    private FinishingTarget GetPossibleFinishingTarget()
    {
        var center = transform.position + Vector3.up * searchHeight.Value;
        var possibleTargets = new List<Collider>(Physics.OverlapSphere(center,
            canFinishDistance, finishTargetLayerMask));
        possibleTargets.Sort(0, possibleTargets.Count, this);

        foreach (var target in possibleTargets)
        {
            if (!target.TryGetComponent<FinishingTarget>(out var finishingTarget))
                continue;
            if (!finishingTarget.CanBeFinished)
                continue;
            var rayDirection = new Vector3(target.transform.position.x, center.y, target.transform.position.z) - center;
            if (Physics.Raycast(center, rayDirection.normalized, out var hit, rayDirection.magnitude, visibilityCheckLayermask))
            {
                if (hit.collider.transform == target.transform)
                    return finishingTarget;
            }
        }
        return null;
    }


    private void OnDrawGizmosSelected()
    {
        if (!searchHeight.HasValue)
            return;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        var searchDistance = new Vector3(0, searchHeight.Value, 0);
        Gizmos.DrawSphere(searchDistance, 0.1f);
        Gizmos.DrawWireSphere(searchDistance, canFinishDistance);
    }

    public int Compare(Collider c1, Collider c2)
    {
        var center = transform.position + Vector3.up * searchHeight.Value;
        var c1Center = new Vector3(c1.transform.position.x, center.y, c1.transform.position.z);
        var c2Center = new Vector3(c2.transform.position.x, center.y, c2.transform.position.z);
        return Vector3.Distance(c1Center, center).CompareTo(Vector3.Distance(c2Center, center));
    }
}
