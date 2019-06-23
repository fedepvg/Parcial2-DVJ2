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

    public LayerMask RaycastLayer;
    public float RayDistance;
    float LandingRotationLimit = 10;

    private void Start()
    {
        PlayerRigidody = GetComponent<Rigidbody2D>();
        Particles.Stop();
        RayDistance = GetComponent<Collider2D>().bounds.extents.y;
        RayDistance += RayDistance / 4;
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
        Debug.DrawRay(transform.position, -transform.up * RayDistance);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, RayDistance,RaycastLayer);
        if(hit)
        {
            if (transform.eulerAngles.z < LandingRotationLimit && transform.eulerAngles.z > -LandingRotationLimit)
                Debug.Log(hit.transform.name);
            else
                Debug.Log("Lose");
        }
        else
        {
            Debug.Log("Lose");
        }
    }
}
