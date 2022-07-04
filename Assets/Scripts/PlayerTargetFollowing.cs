using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetFollowing : MonoBehaviour
{
    [SerializeField]
    private Transform lookTarget;

    [SerializeField]
    private Transform rotatingBone;

    [SerializeField]
    private Vector3 initialBodyForward;
    [SerializeField]
    private Vector3 initialBodyUp;

    [SerializeField]
    [Tooltip("how fast bone direction moves to look target after OnEnable")]
    private float wakeUpAdjustSpeed;
    
    private float interpolation = 1;
    private Quaternion wordRotationToBoneRotation;

    private Vector3 initialUp;

    private void Start()
    {
        var correctedForward = rotatingBone.TransformDirection(initialBodyForward).normalized;
        var correctedUpward = rotatingBone.TransformDirection(initialBodyUp).normalized;
        wordRotationToBoneRotation = Quaternion.Inverse(Quaternion.LookRotation(correctedForward, correctedUpward)) * rotatingBone.rotation;
    } 

    private void LateUpdate()
    {
        interpolation = Mathf.Min(1, interpolation + wakeUpAdjustSpeed * Time.deltaTime);
        var lookDirection = lookTarget.position - rotatingBone.position;
        var targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up) * wordRotationToBoneRotation;
        rotatingBone.rotation = Quaternion.Lerp(rotatingBone.rotation, targetRotation, interpolation);
    }

    private void OnDisable()
    {
        interpolation = 0;
    }


    private void OnDrawGizmosSelected()
    {
        if (rotatingBone == null)
            return;
        Gizmos.matrix = rotatingBone.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, initialBodyForward.normalized);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, initialBodyUp.normalized);
        var right = Vector3.Cross(initialBodyForward, initialBodyUp).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, right);
    }

}
