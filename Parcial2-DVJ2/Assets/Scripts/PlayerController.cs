using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float MaxLeftRotation = 90;
    const float MaxRightRotation = 270;
    const float HalfRotation = 180;

    public float ForceMultiplier;
    public float TorqueMultiplier;
    float MovementInput;
    float RotationInput;
    Rigidbody2D PlayerRigidody;

    private void Start()
    {
        PlayerRigidody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovementInput = Input.GetAxis("Vertical");
        RotationInput = - Input.GetAxisRaw("Horizontal");

        if (MovementInput < 0)
            MovementInput = 0;

        PlayerRigidody.AddRelativeForce(Vector2.up * ForceMultiplier * MovementInput);

        if (RotationInput != 0)
            PlayerRigidody.AddTorque(RotationInput * TorqueMultiplier);
        else
            PlayerRigidody.angularVelocity = 0;
        
        Vector3 EulerRotation = transform.eulerAngles;
        if (EulerRotation.z > MaxLeftRotation && EulerRotation.z < HalfRotation)
            EulerRotation.z = MaxLeftRotation;
        else if (EulerRotation.z < MaxRightRotation && EulerRotation.z > HalfRotation)
            EulerRotation.z = MaxRightRotation;
        transform.eulerAngles = EulerRotation;
    }

    void Update()
    {
        
    }
}
