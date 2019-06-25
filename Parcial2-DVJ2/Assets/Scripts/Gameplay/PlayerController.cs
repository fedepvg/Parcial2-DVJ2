using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void FinishLevelAction(PlayerController player);
    public static FinishLevelAction OnFinishedLevel;

    public delegate void CloseToTerrainAction(Transform player);
    public static CloseToTerrainAction OnCloseToTerrain;

    public delegate void FarFromTerrainAction();
    public static FarFromTerrainAction OnFarFromTerrain;

    public bool Dead;

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
    Bounds LevelBounds;

    float MinHeight;
    float FuelTimer;
    float TimeToLoseFuel = 0.1f;

    float ZoomRayDistance = 1.5f;

    private void Start()
    {
        PlayerRigidody = GetComponent<Rigidbody2D>();
        Particles.Stop();
        RayDistance = GetComponent<Collider2D>().bounds.extents.y;
        RayDistance += RayDistance / 4;
        MinHeight = Terrain.MinHeight;
        FuelTimer = 0;
        Fuel = MaxFuel;
        Dead = false;
        LevelBounds = CameraUtils.OrthographicBounds();
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
        Altitude = transform.position.y - MinHeight;
        VerticalSpeed = PlayerRigidody.velocity.y;
        HorizontalSpeed = PlayerRigidody.velocity.x;
        Altitude *= 100;
        VerticalSpeed *= 100;
        HorizontalSpeed *= 100;

        CheckTerrainDistance();
        CheckLevelBounds();
    }

    void CheckLevelBounds()
    {
        Vector3 pos = transform.position;
        Vector2 playerVelocity = PlayerRigidody.velocity;
        float playerHalfSize = GetComponent<Collider2D>().bounds.extents.x;
        
        if(pos.x <= LevelBounds.min.x || pos.x >= LevelBounds.max.x)
        {
            pos.x = LevelBounds.min.x;
            playerVelocity = new Vector2(0f, playerVelocity.y);
        }
        else if(pos.y >= LevelBounds.max.y)
        {
            pos.y = LevelBounds.max.y;
            playerVelocity = new Vector2(playerVelocity.x, 0f);
        }
        PlayerRigidody.velocity = playerVelocity;
        transform.position = pos;
    }

    void CheckTerrainDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, ZoomRayDistance, RaycastLayer);
        if (hit)
            ActivateZoomCamera();
        else
            DeactivateZoomCamera();
    }

    void ActivateZoomCamera()
    {
        if (OnCloseToTerrain != null)
            OnCloseToTerrain(transform);
    }

    void DeactivateZoomCamera()
    {
        if (OnFarFromTerrain != null)
            OnFarFromTerrain();
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

    void FinishLevel()
    {
        if (OnFinishedLevel != null)
            OnFinishedLevel(this);
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
            {
                Dead = false;
                FinishLevel();
            }
            else
            {
                Dead = true;
                FinishLevel();
            }
        }
        else
        {
            Dead = true;
            FinishLevel();
        }
        if (Dead)
            gameObject.SetActive(false);
    }
}
