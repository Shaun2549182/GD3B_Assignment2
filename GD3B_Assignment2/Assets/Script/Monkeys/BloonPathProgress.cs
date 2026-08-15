using UnityEngine;
/*
    Keeps track of how far a bloon has travelled in its lifetime
*/
public class BloonPathProgress : MonoBehaviour
{
    public float DistanceTraveled { get; private set; }

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        DistanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
    }

    public void ResetProgress()
    {
        DistanceTraveled = 0f;
        lastPosition = transform.position;
    }
}