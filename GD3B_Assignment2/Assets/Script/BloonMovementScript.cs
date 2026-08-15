using UnityEngine;
using UnityEngine.Splines;

public class BloonMovementScript : MonoBehaviour
{
    private float travelTime = 10f;

    private SplineContainer targetSpline;
    private float progress = 0f;

    public void Initialize(SplineContainer container)
    {
        targetSpline = container;
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime / travelTime;

        Vector3 localPos = targetSpline.EvaluatePosition(progress);
        transform.position = targetSpline.transform.TransformPoint(localPos);
    }
}
