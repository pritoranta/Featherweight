using UnityEngine;

public class FlightModel : MonoBehaviour
{
    private float speed; // current speed of the plane
    private bool inControl; // is player actively controlling the plane
    private int ctrlRoll; // player's roll input
    private int ctrlPitch; // player's pitch input
    private float zoomTimer; // a timer for handling speed boosts (after bumping into balloons)

    // public variables are set in Unity editor.
    // the goal is to have most of the important, gameplay-related values modifiable in engine (versus in code)
    public float minSpeed;
    public float maxSpeed;
    public float zoomSpeed; // acceleration amount after bumping into a balloon
    public float zoomTime; // time (in seconds) of speed-boosted flight
    public float defaultPitch;
    public float diveStrength;
    public float accelerationSpeed;
    public InputController controller;
    public Rigidbody rb; // the plane's physics body



    // methods called by Unity engine: --------------------------------------------------------------------------

    // Start is called before the first frame update by Unity
    void Start()
    {
        speed = minSpeed + (maxSpeed - minSpeed) / 2; // initial speed
        zoomTimer = 0;
    }

    // Update is called once per frame by Unity, the "main" method for most Unity scripts
    void Update()
    {
        GetInput();
        Maneuver();
        FlyForward();
        Accelerate();
    }

    // Collision handler called by Unity when applicable
    void OnCollisionEnter(Collision collision)
    {
        zoomTimer = zoomTime;
    }



    // private methods: ------------------------------------------------------------------------------------------

    // gets player input
    private void GetInput()
    {
        if (inControl = controller.IsClicking())
        {
            ctrlRoll = controller.GetRoll();
            ctrlPitch = controller.GetPitch();
        }
    }

    // moves plane forward according to current speed
    private void FlyForward()
    {
        rb.transform.position += -rb.transform.forward * speed * Time.deltaTime; // deltaTime is time passed since last update.
        // Direction rb.transform.forward would be backwards, because of how I set this up in the editor. That's why it's inverted.
    }

    // Turns the plane on x,y, and z axises, depending on current rotations and player's input.
    private void Maneuver()
    {
        float zTilt = FakeSin(rb.transform.eulerAngles.z); // z-axis tilt (roll) of the plane

        float yaw = zTilt * Time.deltaTime; // convert roll into yaw (turn the plane horizontally)
        float pitch;
        float roll;
        if (inControl) // if the player is actively controlling the plane, pitch and roll are in their control
        {
            pitch = ctrlPitch;
            roll = ctrlRoll;
        }
        else
        {
            pitch = (defaultPitch - FakeSin(rb.transform.eulerAngles.x)) * Time.deltaTime; // move pitch towards default
            roll = -zTilt * Time.deltaTime; // move roll towards zero
        }

        rb.transform.Rotate(pitch, yaw, roll); // apply rotations to plane
    }

    // Accelerates or decelerates the plane, depending on if facing upwards or downwards.
    // If going under minimum speed, calls Dive() to make the plane dive downwards (and pick up speed).
    // If currently under the effects of a speed boost, ignores plane's orientation and accelerates.
    private void Accelerate()
    {
        if (zoomTimer > 0)
        {
            zoomTimer -= Time.deltaTime;
            speed += zoomSpeed * Time.deltaTime;
            return;
        }
        if(speed > maxSpeed)
        {
            speed -= Time.deltaTime * speed * 0.2f;
            return;
        }
        float angle = FakeSin(rb.transform.eulerAngles.x); // get facing direction
        speed -= angle * Time.deltaTime * accelerationSpeed;
        if (speed < minSpeed)
            Dive(minSpeed - speed);
    }

    // Rotates the nose of the plane downwards. Higher strength value makes for a stronger rotation.
    // Negative strength value rotates plane's nose upwards.
    private void Dive(float strength)
    {
        rb.transform.Rotate(-strength * diveStrength * Time.deltaTime, 0, 0);
    }

    // Imitates the action of sin function.
    // Unity gives rotations in range of [0,360], but that is not handy to work with.
    // This is a function that increases towards 90, then decreases towards 0 and finally -90, then increases back towards 0:
    // [0,90] -> [0,90]
    // [90,270] -> [90,-90]
    // [270,360] -> [-90,0]
    private static float FakeSin(float angle)
    {
        angle = angle % 360;
        if (angle > 270)
            return angle - 360;
        if (angle > 90)
            return -angle + 180;
        return angle;
    }



    // public methods: ----------------------------------------------------------------------------------------

    // Tells how fast plane is traveling relative to min and max speeds.
    // Returns near 0 when near minimum speed and near 1 when near max speed. Linear progression in between.
    public float GetRelativeSpeed()
    {
        return (speed - minSpeed) / (maxSpeed - minSpeed);
    }
    
}
