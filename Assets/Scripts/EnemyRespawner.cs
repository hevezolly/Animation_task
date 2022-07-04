using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField]
    private float respawnTime;

    [SerializeField]
    private Vector2 spawnAreaSize;
    [SerializeField]
    private float maxGroundDistance;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private RagdollInitiator enemy;

    private WaitForSeconds respawnWait;
    private void Awake()
    {
        respawnWait = new WaitForSeconds(respawnTime);
    }

    public void StartRespawnProcess()
    {
        StartCoroutine(RespawnProcess());
    }

    private IEnumerator RespawnProcess()
    {
        yield return respawnWait;
        Respawn();
    }

    private void Respawn()
    {
        var pos = SelectSpawnPoint();
        var forward = Random.insideUnitCircle;
        var rotation = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.y), Vector3.up);
        enemy.FinishRagdoll();
        enemy.transform.SetPositionAndRotation(pos, rotation);
    }

    private Vector3 SelectSpawnPoint()
    {
        var x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        var y = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        var position = transform.position + transform.forward * transform.localScale.z * y + transform.right * transform.localScale.x * x;

        if (Physics.Raycast(position, -transform.up, out var hit, maxGroundDistance, groundLayer))
        {
            return hit.point;
        }
        return SelectSpawnPoint();
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        var height = maxGroundDistance / transform.localScale.y; 
        Gizmos.DrawWireCube(Vector3.down * height / 2, new Vector3(spawnAreaSize.x, height, spawnAreaSize.y));
    }
}
