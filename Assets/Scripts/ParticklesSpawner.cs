using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticklesSpawner : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem longParts;
    [SerializeField]
    private ParticleSystem shortParts;

    public void SpawnEnterPartickles(HitData hit)
    {
        SpawnParts(hit, shortParts);
    }

    public void SpawnExitPartickles(HitData hit)
    {
        SpawnParts(hit, longParts);
    }

    private void SpawnParts(HitData hit, ParticleSystem partsObj)
    {
        var parts = Instantiate(partsObj, transform, true);

        parts.transform.SetPositionAndRotation(hit.position, Quaternion.LookRotation(hit.attackDirection));
        parts.Play();
    }
}
