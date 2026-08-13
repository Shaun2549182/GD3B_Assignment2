using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class BloonSpawnerScript : MonoBehaviour
{
    public GameObject bloonPrefab;
    public SplineContainer splineContainer;
    public float delayInSeconds = 1.0f;
    public Button startButton;

    public void FirstRound()
    {
        SpawnRedBloon();
        LeanTween.delayedCall(0.2f, SpawnRedBloon);
        LeanTween.delayedCall(0.4f, SpawnRedBloon);
        LeanTween.delayedCall(0.6f, SpawnRedBloon);
        LeanTween.delayedCall(1.35f, SpawnBlueBloon);
        LeanTween.delayedCall(1.55f, SpawnBlueBloon);
        LeanTween.delayedCall(1.75f, SpawnBlueBloon);
        LeanTween.delayedCall(2.5f, SpawnGreenBloon);
        LeanTween.delayedCall(2.7f, SpawnGreenBloon);
        LeanTween.delayedCall(2.9f, SpawnGreenBloon);
        LeanTween.delayedCall(4.9f, FirstRound);
    }

    public void DisableButton()
    {
        startButton.interactable = false;
    }

    public void SpawnRedBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.red;
        }

        if (instance.TryGetComponent<BloonHealthController>(out BloonHealthController bloonHealth))
        {
            bloonHealth.SetBloonHealth(1);
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }
    }

    public void SpawnBlueBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.blue;
        }

        if (instance.TryGetComponent<BloonHealthController>(out BloonHealthController bloonHealth))
        {
            bloonHealth.SetBloonHealth(2);
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }
    }

    public void SpawnGreenBloon()
    {
        GameObject instance = Instantiate(bloonPrefab, transform.position, Quaternion.identity);
        SpriteRenderer[] childRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = Color.green;
        }

        if (instance.TryGetComponent<BloonHealthController>(out BloonHealthController bloonHealth))
        {
            bloonHealth.SetBloonHealth(3);
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }
    }
}
