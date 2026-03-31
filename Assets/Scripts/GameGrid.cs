using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private int _width = 10;

    [SerializeField]
    private int _height = 10;

    [SerializeField]
    private float _cellSize = 2f;

    [SerializeField]
    private GameObject _basicCellPrefab;

    private ICell[,] grid;

    private void Awake()
    {
        InitializeLogicalGrid();
        RevealAll();
    }

    [ContextMenu("Generate grid")]
    public void GenerateGrid()
    {
        if (_basicCellPrefab == null)
            return;

        ClearGrid();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 spawnPosition = new Vector3(x * _cellSize, 0, y * _cellSize);

                GameObject spawnedCell = Instantiate(_basicCellPrefab, spawnPosition, Quaternion.identity);
                spawnedCell.transform.SetParent(this.transform);

                spawnedCell.name = $"Cell_{x}_{y}";
            }
        }

    }

    [ContextMenu("Clear grid")]
    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void RevealAll()
    {
        foreach (var cell in grid)
        {
            if (cell == null) continue;
            cell.Activate();
        }
    }

    private void InitializeLogicalGrid()
    {
        grid = new ICell[_width, _height];

        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.localPosition.x / _cellSize);
            int y = Mathf.RoundToInt(child.localPosition.z / _cellSize);

            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                grid[x, y] = child.GetComponent<ICell>();
            }
        }
    }
}