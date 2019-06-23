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
    public ParticleSystem Particles;

    private void Start()
    {
        PlayerRigidody = GetComponent<Rigidbody2D>();
        Particles.Stop();
    }

    private void FixedUpdate()
    {
        MovementInput = Input.GetAxis("Vertical");
        RotationInput = -Input.GetAxisRaw("Horizontal");

        if (MovementInput <= 0)
        {
            MovementInput = 0;
            Particles.Stop();
        }
        else
        {
            Particles.Play();
        }

        PlayerRigidody.AddRelativeForce(Vector2.up * ForceMultiplier * MovementInput);

        if (RotationInput != 0)
            PlayerRigidody.AddTorque(RotationInput * TorqueMultiplier);
        else
            PlayerRigidody.angularVelocity = 0;

        CheckRotation();
    }

    void Update()
    {
        
    }

    void CheckRotation()
    {
        Vector3 EulerRotation = transform.eulerAngles;
        if (EulerRotation.z > MaxLeftRotation && EulerRotation.z < HalfRotation)
        {
            EulerRotation.z = MaxLeftRotation;
            PlayerRigidody.angularVelocity = 0;
        }
        else if (EulerRotation.z < MaxRightRotation && EulerRotation.z > HalfRotation)
        {
            EulerRotation.z = MaxRightRotation;
            PlayerRigidody.angularVelocity = 0;
        }
        transform.eulerAngles = EulerRotation;
    }
}
