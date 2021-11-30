using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool clicking; // is input active
    private float previousX;
    private float previousY;
    private int roll;
    private int pitch;

    // public variables are set in Unity editor:
    public string inputName; // name of the tracked input
    public float dragToleranceX; // how long the screen has to be dragged horizontally for the input to count
    public float dragToleranceY; // same for vertical movement



    // methods called by Unity engine: --------------------------------------------------------------------------

    // Start is called before the first frame update by Unity
    void Start()
    {
        clicking = false;
    }

    // Update is called once per frame by Unity
    void Update()
    {
        InputStatus();
        Roll();
        Pitch();
    }



    // private methods: ------------------------------------------------------------------------------------------

    // Checks and updates input status.
    // Returns true if the input is down and this isn't the initial click
    private bool InputStatus()
    {
        if (!Input.GetButton(inputName)) // if the input isn't active
        {
            clicking = false;
            return false;
        }
        if (!clicking) // if this is the initial input click
        {
            clicking = true;
            previousX = Input.mousePosition.x;
            previousY = Input.mousePosition.y;
            return false;
        }
        return true;
    }

    // Makes the plane roll, depending on mouse's X axis movement
    private void Roll()
    {
        if (Mathf.Abs(Input.mousePosition.x - previousX) > dragToleranceX)
        {
            if (Input.mousePosition.x > previousX)
                roll = 1;
            else
                roll = -1;
            previousX = Input.mousePosition.x;
        }
        else
            roll = 0;
    }

    // Controls the plane's pitch, depending on mouse's Y axis movement.
    // Dragging down makes the plane turn more upwards.
    private void Pitch()
    {
        if (Mathf.Abs(Input.mousePosition.y - previousY) > dragToleranceY)
        {
            if (Input.mousePosition.y > previousY)
                pitch = -1;
            else
                pitch = 1;
            previousY = Input.mousePosition.y;
        }
        else
            pitch = 0;
    }



    // public methods: ------------------------------------------------------

    public bool IsClicking()
    {
        return clicking;
    }

    public int GetRoll()
    {
        return roll;
    }

    public int GetPitch()
    {
        return pitch;
    }

}
