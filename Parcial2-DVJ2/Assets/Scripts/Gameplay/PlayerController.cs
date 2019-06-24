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
    float RayDistance;
    float LandingRotationLimit = 10;
    public float MaxVerticalSpeedOnLanding;
    public float MaxHorizontalSpeedOnLanding;

    public int MaxFuel;
    public int Fuel;
    public float Altitude;
    public float VerticalSpeed;
    public float HorizontalSpeed;

    public LevelGenerator Terrain;

    float MinHeight;
    float FuelTimer;
    float TimeToLoseFuel = 0.1f;

    private void Start()
    {
        PlayerRigidody = GetComponent<Rigidbody2D>();
        Particles.Stop();
        RayDistance = GetComponent<Collider2D>().bounds.extents.y;
        RayDistance += RayDistance / 4;
        MinHeight = Terrain.MinHeight;
        FuelTimer = 0;
        Fuel = MaxFuel;
    }

    private void FixedUpdate()
    {
        MovementInput = Input.GetAxis("Vertical");
        RotationInput = -Input.GetAxisRaw("Horizontal");


        FuelTimer += Time.fixedDeltaTime;
        if (MovementInput <= 0)
        {
            MovementInput = 0;
            Particles.Stop();
        }
        else
        {
            if (FuelTimer >= TimeToLoseFuel)
            {
                FuelTimer = 0;
                Fuel--;
            }
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
        Altitude = transform.position.y - MinHeight;
        VerticalSpeed = PlayerRigidody.velocity.y;
        HorizontalSpeed = PlayerRigidody.velocity.x;
        Altitude *= 100;
        VerticalSpeed *= 100;
        HorizontalSpeed *= 100;
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

    bool IsRotationCorrect()
    {
        int fullAngle = 360;
        float zRotation = transform.eulerAngles.z;
        bool correctLeftRot = zRotation < LandingRotationLimit || zRotation > fullAngle / 2;
        bool correctRightRot = zRotation > fullAngle - LandingRotationLimit && zRotation < fullAngle;

        return correctLeftRot || correctRightRot;
    }

    bool IsSpeedCorect()
    {
        if (VerticalSpeed > -MaxVerticalSpeedOnLanding &&
            HorizontalSpeed < MaxHorizontalSpeedOnLanding &&
            HorizontalSpeed > -MaxHorizontalSpeedOnLanding)
            return true;
        else
            return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bounds spriteBounds = GetComponent<Collider2D>().bounds;
        Vector3 pos = transform.position;

        Vector3 downVector = -transform.up;
        Vector2 leftRayPos = new Vector3(spriteBounds.min.x, pos.y);
        Vector2 rightRayPos = new Vector3(spriteBounds.max.x, pos.y);

        RaycastHit2D hitMiddle = Physics2D.Raycast(pos, downVector, RayDistance, RaycastLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayPos, downVector, RayDistance, RaycastLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayPos, downVector, RayDistance, RaycastLayer);

        if(hitLeft && hitMiddle && hitRight)
        {
            if (IsRotationCorrect() && IsSpeedCorect())
                Debug.Log(hitMiddle.transform.name);
            else
                Debug.Log("Lose");
        }
        else
        {
            Debug.Log("Lose");
        }
    }
}
