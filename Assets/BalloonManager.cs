using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for moving Balloons around, as the player flies around and bumps into them
public class BalloonManager : MonoBehaviour
{
    private LinkedList<Balloon> balloons;

    // public variables are managed in Unity editor:
    public float validAngle; // the angle which balloons have to stay within, in regards to plane's facing direction
    public float minSpawnDistance; // minimum spawn distance for a balloon (from the plane)
    public float maxSpawnDistance; // max spawn distance for a balloon
    public float minSize; // minimum and maximum size multipliers for resized balloons:
    public float maxSize;
    public GameObject[] list; // list of GameObjects that contain the Balloon scripts (C# classes)
    public GameObject plane;

    // Start is called by Unity engine before the first frame update
    void Start()
    {
        balloons = new LinkedList<Balloon>();
        foreach(GameObject g in list)
        {
            balloons.AddLast(g.GetComponent<Balloon>());
        }
    }

    // Update is called once per frame by Unity
    void Update()
    {
        CheckBalloons();
    }

    

    // private methods: --------------------------------------------------------------------------

    // Check if balloons should be moved ("popped")
    private void CheckBalloons()
    {
        foreach(Balloon b in balloons)
        {

            if (OutOfBounds(b))
                Pop(b);
        }
    }

    // Tells if the balloon is out of bounds
    private bool OutOfBounds(Balloon b)
    {
        // a position behind the plane, to ultimately reduce visible despawning of balloons, namely when the player just slightly misses one
        Vector3 behindPlane = plane.transform.position + plane.transform.forward * 1000;
        // angle between vector that connects behindPlane and balloon, and plane's facing direction
        float diff = Vector3.Angle((behindPlane - b.transform.position), plane.transform.forward);
        if (diff > validAngle)
        {
            return true;
        }
        return false;
    }

    // Finds a new location for the balloon in the player's view.
    private void Relocate(Balloon b)
    {
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance); // how far the new location will be
        float f = validAngle / 3; // angle split in three to make balloons more easily reachable before going out of bounds 
        (float xRot, float yRot) = (Random.Range(-f, f), Random.Range(-f, f)); // rotations for randomness in spawning
        Vector3 newPos = plane.transform.position + -plane.transform.forward *  distance; // new position calculated (no rotations)
        newPos = Quaternion.Euler(xRot, yRot, 0) * newPos; // new position rotated
        b.transform.position = newPos;
    }

    // Picks a random value to slightly change the balloon's size
    private void Resize(Balloon b)
    {
        b.transform.localScale = b.transform.localScale * Random.Range(minSize, maxSize);
    }



    // public methods: ---------------------------------------------------------------------------

    // Moves the balloon and resizes it.
    // For the player it looks like a new balloon was created and the old destroyed.
    // As a note, it is much more practical and cost-effective to move and manipulate existing GameObjects in Unity, than to destroy and create new ones.
    public void Pop(Balloon b)
    {
        Relocate(b);
        Resize(b);
    }
}
