using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    Bounds LevelBounds;
    float LevelDistance;
    LineRenderer LineRend;
    EdgeCollider2D EdgeCollider;
    
    public int MaxHeight = 0;
    public int MinHeight = -3;
    float MaxWidth;
    float MinWidth;
    int WidthMultiplier = 4;
    float MaxYAddition = 1f;
    float MinYAddition = 0.5f;

    public Collider2D Player;
    
    int PointCount = 0;
    List<Vector2> Points;
    float XPosition;
    float YPosition;
    float PrevYPosition;
    int NotLandingPlatformPoints;
    public int MaxNotLandingPlatformPoints;
    public int MinNotLandingPlatformPoints;

    void Start()
    {
        Points = new List<Vector2>();
        LevelBounds = CameraUtils.OrthographicBounds();
        LineRend = GetComponent<LineRenderer>();
        EdgeCollider = GetComponent<EdgeCollider2D>();
        MinWidth = Player.bounds.size.x;
        MaxWidth = MinWidth * WidthMultiplier;
        NotLandingPlatformPoints = 0;
        LineRend.positionCount = 1;
        Points.Add(GetFirstPoint());
        LineRend.SetPosition(PointCount, Points[PointCount]);
        LevelDistance = LevelBounds.size.x * 2;
        GenerateLevel();
    }

    void GenerateLevel()
    {
        while(Vector2.Distance(Points[0], Points[PointCount]) < LevelDistance)
        {
            Points.Add(GetPoint());
            PointCount++;
            LineRend.positionCount++;
            LineRend.SetPosition(PointCount, Points[PointCount]);
        }
        EdgeCollider.points = Points.ToArray();
    }

    Vector2 GetPoint()
    {
        Vector2 newPoint;
        XPosition += Random.Range(MaxWidth, MinWidth);

        if (!IsLandingPlatform())
        {
            float randY = Random.Range(MinYAddition, MaxYAddition) * SignMultiplier();
            YPosition += randY;
            YPosition = Mathf.Clamp(YPosition,MinHeight, MaxHeight);
            if(PrevYPosition == YPosition)
            {
                YPosition -= randY;
            }
            NotLandingPlatformPoints++;
            PrevYPosition = YPosition;
        }
        newPoint = new Vector2(XPosition, YPosition);

        return newPoint;
    }

    int SignMultiplier()
    {
        int sign = Random.Range(0, 2);
        if(sign==0)
        {
            return -1;
        }
        return 1;
    }

    bool IsLandingPlatform()
    {
        float rand = Random.Range(0f, 1f);
        if(rand < GetExponentialProbability(NotLandingPlatformPoints))
        {
            NotLandingPlatformPoints = 0;
            return true;
        }
        return false;
    }

    float GetExponentialProbability(int currentValue)
    {
        float valuesCount = MaxNotLandingPlatformPoints - MinNotLandingPlatformPoints;
        float maxChance = Mathf.Pow(2, valuesCount);
        float currentValueChance = Mathf.Pow(2, currentValue - MinNotLandingPlatformPoints);

        return currentValueChance / maxChance;
    }

    Vector2 GetFirstPoint()
    {
        XPosition = LevelBounds.min.x - LevelBounds.extents.x;
        float randHeight = Random.Range(MinHeight, MaxHeight);
        return new Vector2(XPosition, randHeight);
    }
}
