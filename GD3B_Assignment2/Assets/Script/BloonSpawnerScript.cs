using UnityEngine;
using UnityEngine.Splines;

public class BloonSpawnerScript : MonoBehaviour
{
    public GameObject bloonPrefab;
    public SplineContainer splineContainer;
    public float delayInSeconds = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnRedBloon();    
    }

    public void SpawnRedBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.red;
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }

        LeanTween.delayedCall(delayInSeconds, SpawnBlueBloon);
    }

    public void SpawnBlueBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.blue;
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }

        LeanTween.delayedCall(delayInSeconds, SpawnGreenBloon);
    }

    public void SpawnGreenBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.green;
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }

        LeanTween.delayedCall(delayInSeconds, SpawnRedBloon);
    }
}
