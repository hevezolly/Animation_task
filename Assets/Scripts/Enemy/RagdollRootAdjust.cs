using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollRootAdjust : MonoBehaviour
{
    [SerializeField]
    private Transform pelvis;

    private Vector3 pelvisLocalPos;
    private Quaternion pelvisLocalRot;
    private Vector3 pelvisOffset;
    private Quaternion pelvisRotationoffset;
    
    private void OnEnable()
    {
        
        Record();
    }

    private void Record()
    {
        pelvisLocalPos = pelvis.localPosition;
        pelvisLocalRot = pelvis.localRotation;

        var offset = transform.position - pelvis.position;
        pelvisOffset = pelvis.InverseTransformVector(offset);

        pelvisRotationoffset = Quaternion.Inverse(pelvis.rotation) * transform.rotation; 
    }

    private void LateUpdate()
    {
        transform.position = pelvis.position + pelvis.TransformVector(pelvisOffset);
        transform.rotation = pelvis.rotation * pelvisRotationoffset;

        pelvis.localPosition = pelvisLocalPos;
        pelvis.localRotation = pelvisLocalRot;

    }
}
