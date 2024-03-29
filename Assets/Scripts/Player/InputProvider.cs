using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class InputProvider : ScriptableObject
{
    [SerializeField]
    private Vector3 rightDirection = Vector3.right;
    [SerializeField]
    private Vector3 forwardDirection = Vector3.forward;


    public void SetCorrectedDirections(Vector3 right, Vector3 forward)
    {
        rightDirection = right;
        forwardDirection = forward;
    }

    public Vector3 GetMovementInput()
    {
        var forward = Input.GetAxisRaw("Vertical") * forwardDirection;
        var right = Input.GetAxisRaw("Horizontal") * rightDirection;
        return (forward + right).normalized;
    }

}
