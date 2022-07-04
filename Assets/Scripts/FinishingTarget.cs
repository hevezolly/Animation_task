using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishingTarget : MonoBehaviour
{
    [SerializeField]
    private bool canBeFinished;

    public bool CanBeFinished => canBeFinished;

    public UnityEvent<HitData> FinishingStartedEvent;
    public UnityEvent<HitData> FinishingEndedEvent;

    public void SetCanBeFinished(bool state)
    {
        canBeFinished = state;
    }
    
    public void StartFinishing(HitData hitData)
    {
        Debug.DrawRay(hitData.position, hitData.attackDirection, Color.red, 5);
        FinishingStartedEvent?.Invoke(hitData);
    }

    public void EndFinishing(HitData hitData)
    {
        Debug.DrawRay(hitData.position, hitData.attackDirection, Color.yellow, 5);
        FinishingEndedEvent?.Invoke(hitData);
    }
}

public struct HitData 
{
    public Vector3 position;
    public Vector3 dirrectionToAttacker;

    public Vector3 attackDirection;
}
