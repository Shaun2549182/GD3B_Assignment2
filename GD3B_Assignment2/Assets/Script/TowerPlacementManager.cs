using UnityEngine;

[System.Serializable]
public class TowerDefinition
{
    public string label = "Tower";
    public GameObject towerPrefab;
    public int cost = 200;
}

public class TowerPlacementManager : MonoBehaviour
{
    [Header("Towers (select with 1, 2, 3)")]
    [SerializeField] private TowerDefinition[] towers = new TowerDefinition[3];

    [Header("Placement Rules")]
    [Tooltip("Layers that block placement: your track/path, and existing towers.")]
    [SerializeField] private LayerMask blockingMask;
    [SerializeField] private float placementFootprintRadius = 0.5f;

    [Header("Preview Appearance")]
    [SerializeField] private Color validColor = new Color(0.3f, 1f, 0.3f, 0.6f);
    [SerializeField] private Color invalidColor = new Color(1f, 0.3f, 0.3f, 0.6f);
    [SerializeField] private int rangeCircleSegments = 64;

    private TowerDefinition selectedTower;
    private GameObject previewRoot;
    private SpriteRenderer previewSprite;
    private LineRenderer previewRangeCircle;
    private bool placementValid;

    private void Update()
    {
        HandleSelectionInput();

        if (selectedTower == null)
        {
            return;
        }

        Vector3 worldPos = GetMouseWorldPosition();
        previewRoot.transform.position = worldPos;

        placementValid = EvaluatePlacement(worldPos);
        UpdatePreviewColor();

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceTower(worldPos);
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacement();
        }
    }

    private void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectTower(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectTower(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectTower(2);
    }

    private void SelectTower(int index)
    {
        if (index < 0 || index >= towers.Length || towers[index] == null || towers[index].towerPrefab == null)
        {
            Debug.LogWarning($"No tower configured for slot {index + 1}.");
            return;
        }

        selectedTower = towers[index];
        BuildPreview(selectedTower);
    }

    private void BuildPreview(TowerDefinition definition)
    {
        DestroyPreview();

        previewRoot = new GameObject($"Preview_{definition.label}");

        // reuse the real tower's sprite so the ghost actually looks like the tower.
        previewSprite = previewRoot.AddComponent<SpriteRenderer>();
        SpriteRenderer sourceSprite = definition.towerPrefab.GetComponentInChildren<SpriteRenderer>();
        if (sourceSprite != null)
        {
            previewSprite.sprite = sourceSprite.sprite;
        }
        previewSprite.sortingOrder = 67; // draw above everything else while placing

        // range circle
        GameObject circleGO = new GameObject("RangeCircle");
        circleGO.transform.SetParent(previewRoot.transform, false);
        previewRangeCircle = circleGO.AddComponent<LineRenderer>();
        previewRangeCircle.material = new Material(Shader.Find("Sprites/Default"));
        previewRangeCircle.useWorldSpace = false;
        previewRangeCircle.loop = true;
        previewRangeCircle.widthMultiplier = 0.05f;
        previewRangeCircle.sortingOrder = 99;

        float range = GetTowerRange(definition.towerPrefab);
        DrawCircle(previewRangeCircle, range, rangeCircleSegments);
    }

    private float GetTowerRange(GameObject prefab)
    {
        ITowerRangeProvider rangeProvider = prefab.GetComponent<ITowerRangeProvider>();
        if (rangeProvider != null)
        {
            return rangeProvider.GetRange();
        }

        Debug.LogWarning($"{prefab.name} doesn't implement ITowerRangeProvider - using a fallback range of 3.");
        return 3f;
    }

    private void DrawCircle(LineRenderer lr, float radius, int segments)
    {
        lr.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            lr.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }
    }

    private bool EvaluatePlacement(Vector3 worldPos)
    {
        bool blocked = Physics2D.OverlapCircle(worldPos, placementFootprintRadius, blockingMask);
        bool canAfford = MoneyManager.Instance != null && MoneyManager.Instance.Money >= selectedTower.cost;
        return !blocked && canAfford;
    }

    private void UpdatePreviewColor()
    {
        Color tint = placementValid ? validColor : invalidColor;

        if (previewSprite != null)
        {
            previewSprite.color = tint;
        }

        if (previewRangeCircle != null)
        {
            previewRangeCircle.startColor = tint;
            previewRangeCircle.endColor = tint;
        }
    }

    private void TryPlaceTower(Vector3 worldPos)
    {
        if (!placementValid)
        {
            return;
        }

        if (MoneyManager.Instance == null || !MoneyManager.Instance.TrySpendMoney(selectedTower.cost))
        {
            return;
        }

        Instantiate(selectedTower.towerPrefab, worldPos, Quaternion.identity);
        CancelPlacement();
    }

    private void CancelPlacement()
    {
        selectedTower = null;
        DestroyPreview();
    }

    private void DestroyPreview()
    {
        if (previewRoot != null)
        {
            Destroy(previewRoot);
        }
        previewSprite = null;
        previewRangeCircle = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        return worldPos;
    }
}