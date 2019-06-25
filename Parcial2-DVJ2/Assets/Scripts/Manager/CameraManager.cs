using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera LandingCamera;
    public Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.OnCloseToTerrain = ActivateZoom;
        PlayerController.OnFarFromTerrain = DeactivateZoom;
        LandingCamera.enabled = false;
        MainCamera.enabled = true;
    }

    void ActivateZoom(Transform playerTransform)
    {
        Vector3 landingCameraPos = playerTransform.position - Vector3.forward;
        LandingCamera.transform.position = landingCameraPos;
        LandingCamera.enabled = true;
        MainCamera.enabled = false;
    }

    void DeactivateZoom()
    {
        LandingCamera.enabled = false;
        MainCamera.enabled = true;
    }
}
