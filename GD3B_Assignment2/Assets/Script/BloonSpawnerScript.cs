using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BloonSpawnerScript : MonoBehaviour
{
    public static BloonSpawnerScript Instance { get; private set; }

    [System.Serializable]
    public struct WaveDefinition
    {
        public int redCount;
        public int blueCount;
        public int greenCount;
    }

    private struct EnemyData
    {
        public Color color;
        public int health;

        public EnemyData(Color color, int health)
        {
            this.color = color;
            this.health = health;
        }
    }

    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button roundStartButton;

    private float spawnInterval = 0.25f;
    private float groupPauseInterval = 1.25f;
    private float waveStartDelay = 3.0f;
    private float waveEndDelay = 3.0f;

    private readonly WaveDefinition[] exponentialWaves = new WaveDefinition[]
     {
        new WaveDefinition { redCount = 20, blueCount = 0,  greenCount = 0  },
        new WaveDefinition { redCount = 15, blueCount = 6,  greenCount = 0  },
        new WaveDefinition { redCount = 18, blueCount = 9,  greenCount = 0  },
        new WaveDefinition { redCount = 11, blueCount = 7,  greenCount = 8  },
        new WaveDefinition { redCount = 15, blueCount = 9,  greenCount = 11 },
        new WaveDefinition { redCount = 19, blueCount = 13, greenCount = 15 },
        new WaveDefinition { redCount = 0,  blueCount = 29, greenCount = 21 },
        new WaveDefinition { redCount = 0,  blueCount = 38, greenCount = 29 },
        new WaveDefinition { redCount = 0,  blueCount = 52, greenCount = 39 },
        new WaveDefinition { redCount = 0,  blueCount = 68, greenCount = 54 }
     };

    private readonly WaveDefinition[] linearWaves = new WaveDefinition[]
    {
        new WaveDefinition { redCount = 20, blueCount = 0,  greenCount = 0  },
        new WaveDefinition { redCount = 20, blueCount = 10, greenCount = 0  },
        new WaveDefinition { redCount = 12, blueCount = 9,  greenCount = 10 },
        new WaveDefinition { redCount = 17, blueCount = 12, greenCount = 13 },
        new WaveDefinition { redCount = 22, blueCount = 15, greenCount = 16 },
        new WaveDefinition { redCount = 24, blueCount = 18, greenCount = 20 },
        new WaveDefinition { redCount = 0,  blueCount = 34, greenCount = 24 },
        new WaveDefinition { redCount = 0,  blueCount = 38, greenCount = 28 },
        new WaveDefinition { redCount = 0,  blueCount = 42, greenCount = 32 },
        new WaveDefinition { redCount = 0,  blueCount = 46, greenCount = 36 }
    };

    private WaveDefinition[] activeWaves;
    private Queue<EnemyData> currentWaveQueue = new Queue<EnemyData>();
    private int currentWaveIndex = -1;
    private int activePrefabsCount = 0;
    private int groupSpawnCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        string activeSceneName = SceneManager.GetActiveScene().name;

        if (activeSceneName == "Exponential")
        {
            activeWaves = exponentialWaves;
        }
        else if (activeSceneName == "Linear")
        {
            activeWaves = linearWaves;
        }
        else
        {
            return;
        }
    }


    public void StartNextWave()
    {
        if (roundStartButton != null)
        {
            roundStartButton.interactable = false;
        }

        currentWaveIndex++;

        if (currentWaveIndex >= activeWaves.Length)
        {
            LeanTween.delayedCall(gameObject, 3.0f, () =>
            {
                SceneManager.LoadScene("Win");
            });
            return;
        }

        if (waveText != null)
        {
            waveText.text = $"Round {currentWaveIndex + 1} of 10";
        }

        BuildWaveQueue(activeWaves[currentWaveIndex]);
        groupSpawnCount = 0;

        LeanTween.delayedCall(gameObject, waveStartDelay, SpawnNextInWave);
    }

    private void BuildWaveQueue(WaveDefinition wave)
    {
        currentWaveQueue.Clear();

        for (int i = 0; i < wave.redCount; i++)
            currentWaveQueue.Enqueue(new EnemyData(Color.red, 1));

        for (int i = 0; i < wave.blueCount; i++)
            currentWaveQueue.Enqueue(new EnemyData(Color.blue, 2));

        for (int i = 0; i < wave.greenCount; i++)
            currentWaveQueue.Enqueue(new EnemyData(Color.green, 3));
    }

    private void SpawnNextInWave()
    {
        if (currentWaveQueue.Count == 0) return;

        EnemyData enemy = currentWaveQueue.Dequeue();


        GameObject instance = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        activePrefabsCount++;
        groupSpawnCount++;

        SpriteRenderer[] renderers = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            sr.color = enemy.color;
        }

        if (instance.TryGetComponent<BloonHealthController>(out BloonHealthController health))
        {
            health.SetBloonHealth(enemy.health);
        }

        if (instance.TryGetComponent<BloonMovementScript>(out BloonMovementScript follower))
        {
            follower.Initialize(splineContainer);
        }

        if (currentWaveQueue.Count > 0)
        {
            float nextDelay = (groupSpawnCount % 10 == 0) ? groupPauseInterval : spawnInterval;
            LeanTween.delayedCall(gameObject, nextDelay, SpawnNextInWave);
        }
    }

    public void OnBloonsDestroyed()
    {
        activePrefabsCount--;

        if (currentWaveQueue.Count == 0 && activePrefabsCount <= 0)
        {
            LeanTween.delayedCall(gameObject, waveEndDelay, StartNextWave);
        }
    }
}