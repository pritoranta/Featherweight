using UnityEngine;

// Class for handling the plane's collisions to balloons
public class Balloon : MonoBehaviour
{
    private BalloonManager bm;

    // Start is called before the first frame update
    void Start()
    {
        bm = GetComponentInParent<BalloonManager>();
    }

    // Called by Unity when an applicable collision occurs
    void OnCollisionEnter(Collision collision)
    {
        Pop();
    }

    // Tells BalloonManager to pop this balloon.
    // This is done in this class because 3rd parties (BallonManager) cannot practically listen for collisions between other objects.
    private void Pop()
    {
        bm.Pop(this);
    }

}
