using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private InputProvider input;

    [SerializeField] 
    private Rigidbody rb;
    
    [SerializeField] 
    [Min(0)]
    private float moveAcceleration;
    
    [SerializeField] 
    private ScriptableValueField<float> maxVelocity;
    
    private Vector3 targetVelocity;


    private void Update()
    {
        targetVelocity = input.GetMovementInput() * maxVelocity.Value;
    }

    

    private void FixedUpdate()
    {
        var currentVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        var velocityDelta = targetVelocity - currentVel;
        var acceleration = Mathf.Min(velocityDelta.magnitude / Time.deltaTime, moveAcceleration);
        rb.AddForce(velocityDelta.normalized * acceleration, ForceMode.Acceleration);
    }
}
