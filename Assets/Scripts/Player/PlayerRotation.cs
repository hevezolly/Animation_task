using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] 
    private InputProvider input;

    [SerializeField] 
    private Rigidbody rb;

    [Min(0)]
    [SerializeField] 
    private float rotationVelocity;

    private float targetRotation;
    

    private void Update()
    {
        targetRotation = rb.rotation.eulerAngles.y;
        var inputDir = input.GetMovementInput();
        if (inputDir.sqrMagnitude > Mathf.Epsilon)
            targetRotation = Quaternion.LookRotation(inputDir, Vector3.up).eulerAngles.y;
    }

    private void FixedUpdate()
    {
        var rotationDelta = Mathf.DeltaAngle(rb.rotation.eulerAngles.y, targetRotation);
        var velocity = Mathf.Min(Mathf.Abs(rotationDelta), rotationVelocity);

        rb.angularVelocity = Vector3.up * velocity * Mathf.Sign(rotationDelta);
    }
}
