using UnityEngine;

// Controls the camera
public class CameraController : MonoBehaviour
{
    // public variables are set in Unity editor:
    public float minFOV;
    public float practicalMaxFOV; // with normally capped speed
    public float realMaxFOV; // hard limit
    public Camera cam;
    public FlightModel fm;

    // Update is called once per frame by Unity engine
    void Update()
    {
        UpdateFOV(fm.GetRelativeSpeed());
    }

    // Updates the camera's FOV (field of view) relative to the min and max FOVs and the relative speed of the plane.
    // In normal flight the value ranges from minFOV to practicalMaxFOV, with boosted speed hard limit is realMaxFOV
    private void UpdateFOV(float relativeSpeed)
    {
        float fov = minFOV + (practicalMaxFOV - minFOV) * relativeSpeed;
        if (fov > realMaxFOV)
            fov = realMaxFOV;
        cam.fieldOfView = fov;
    }

}
